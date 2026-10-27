param([string]$MediaPortalDirectory='C:\Program Files\Team MediaPortal\MediaPortal')
$ErrorActionPreference='Stop'
$editor=Join-Path (Split-Path $PSScriptRoot -Parent) 'Source\BackdropMenuEditor.dll'
$resolver=[ResolveEventHandler]{param($sender,$eventArgs)
 $name=([Reflection.AssemblyName]$eventArgs.Name).Name+'.dll'
 foreach($dir in @($MediaPortalDirectory,(Join-Path $MediaPortalDirectory 'plugins\windows'))){
  $file=Join-Path $dir $name
  if(Test-Path -LiteralPath $file){return [Reflection.Assembly]::LoadFrom($file)}
 }
 return $null
}
[AppDomain]::CurrentDomain.add_AssemblyResolve($resolver)
[void][Reflection.Assembly]::LoadFrom($editor)
Add-Type -ReferencedAssemblies @($editor,'System.Windows.Forms','System.Drawing',(Join-Path $MediaPortalDirectory 'Core.dll')) -TypeDefinition @"
using System;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using MediaPortal.Plugins;
public class SubmenuProbe : BackdropMenuEditorConfig {
 public string Error;
 protected override void ReportMenuInputError(string message) { Error = message; }
 object Field(string name) { return typeof(BackdropMenuEditorConfig).GetField(name, BindingFlags.Instance|BindingFlags.NonPublic).GetValue(this); }
 void Add() { Error=null; typeof(BackdropMenuEditorConfig).GetMethod("btnAdd_Click", BindingFlags.Instance|BindingFlags.NonPublic).Invoke(this,new object[]{this,EventArgs.Empty}); }
 void Assert(bool condition) { if(!condition) throw new Exception("Submenu assertion failed"); }
 public void Run() {
  ((TextBox)Field("new_windowid")).Text="87950";
  ((TextBox)Field("new_name")).Text="Background lighting";
  Add(); Assert(Error!=null);
  ComboBox menu=(ComboBox)Field("comboBox1"); menu.Items.Add("1"); menu.SelectedIndex=0;
  Add(); Assert(Error!=null);
  ComboBox background=(ComboBox)Field("comboBox5"); background.Items.AddRange(new object[]{"Multiple Images","Fanart Plugin","Single Image"}); background.SelectedIndex=0;
  Add(); Assert(Error!=null);
  background.SelectedIndex=1; Add(); Assert(Error!=null);
  background.SelectedIndex=2;
  ((TextBox)Field("new_imagepath")).Text="settings.jpg";
  Add(); Assert(Error==null);
  IList items=(IList)Field("menuItems1"); Assert(items.Count==1);
  object item=items[0]; Assert((string)item.GetType().GetField("hyperlink").GetValue(item)=="87950");
  Assert((string)item.GetType().GetField("useLatestMedia").GetValue(item)=="No");
  menu.SelectedIndex=-1; background.SelectedIndex=-1;
  Console.WriteLine("PASS: incomplete destination/type/path/fanart; DayNight add; default latest-media; cleared selections.");
 }
}
"@
$probe=New-Object SubmenuProbe
try {$probe.Run()} finally {$probe.Dispose(); [AppDomain]::CurrentDomain.remove_AssemblyResolve($resolver)}

