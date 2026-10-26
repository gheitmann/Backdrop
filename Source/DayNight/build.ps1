param([string]$MediaPortalDirectory='C:\Program Files\Team MediaPortal\MediaPortal')
$ErrorActionPreference='Stop'
$output=Join-Path (Split-Path $PSScriptRoot -Parent) 'DayNight.Backgrounds.dll'
$compiler='C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$compileArgs=@('/nologo','/target:library','/platform:anycpu','/optimize+',('/out:'+$output),'/r:System.dll','/r:System.Core.dll','/r:System.Xml.dll','/r:System.Drawing.dll','/r:System.Windows.Forms.dll')
foreach($name in @('Core.dll','Common.Utils.dll','Utils.dll','plugins/windows/Dialogs.dll')){$compileArgs+='/r:'+(Join-Path $MediaPortalDirectory $name)}
$compileArgs+='/resource:'+(Join-Path $PSScriptRoot 'Languages.xml')+',DayNight.Languages.xml'
foreach($name in @('DayNightSettings.cs','DayNightPlugin.cs','DayNightConfiguration.cs','DayNightText.cs','DayNightSkinActions.cs','DayNightWindow.cs')){$compileArgs+=Join-Path $PSScriptRoot $name}
& $compiler @compileArgs
if($LASTEXITCODE -ne 0){throw 'Day/Night plugin compilation failed.'}
Write-Output "Built optional AnyCPU plugin: $output"
