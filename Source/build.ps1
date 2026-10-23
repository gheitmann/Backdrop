param([string]$MediaPortalDirectory='C:\Program Files\Team MediaPortal\MediaPortal')
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$src=Join-Path $PSScriptRoot 'BackdropMenuEditor'
 $mp=$MediaPortalDirectory
Add-Type -AssemblyName System.Windows.Forms
$reader=New-Object System.Resources.ResXResourceReader (Join-Path $src 'BackdropMenuEditorConfig.resx')
$writer=New-Object System.Resources.ResourceWriter (Join-Path $PSScriptRoot 'menu.resources')
foreach($entry in $reader.GetEnumerator()){$writer.AddResource($entry.Key,$entry.Value)}
$writer.Generate(); $writer.Dispose(); $reader.Dispose()
$argsList=@('/nologo','/target:library','/platform:anycpu','/optimize+',('/out:'+(Join-Path $PSScriptRoot 'BackdropMenuEditor.dll')))
foreach($ref in @('System.dll','System.Core.dll','System.Data.dll','System.Drawing.dll','System.Windows.Forms.dll','System.Xml.dll')){$argsList+='/r:'+$ref}
foreach($ref in @('Core.dll','Common.Utils.dll','Utils.dll','Databases.dll','plugins\windows\Dialogs.dll','plugins\windows\GUIMusic.dll','plugins\windows\Common.GUIPlugins.dll')){$argsList+='/r:'+(Join-Path $mp $ref)}
$argsList+='/resource:'+(Join-Path $PSScriptRoot 'menu.resources')+',MediaPortal.Plugins.BackdropMenuEditorConfig.resources'
$argsList+='/resource:'+(Join-Path $src 'logo.png')+',BackdropMenuEditor.logo.png'
$argsList+='/resource:'+(Join-Path $src 'logo_disabled.png')+',BackdropMenuEditor.logo_disabled.png'
foreach($file in @('BackdropMenuEditor.cs','BackdropMenuEditorConfig.cs','BackdropMenuEditorConfig.Designer.cs','MusicHandler.cs','Properties\AssemblyInfo.cs')){$argsList+=Join-Path $src $file}
$argsList+=Join-Path $PSScriptRoot 'OptionalPluginAdapters.cs'
$argsList+=Join-Path $PSScriptRoot 'PointerSeek.cs'
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' @argsList
if($LASTEXITCODE -ne 0){throw 'Compilation failed'}



