param([string]$MediaPortalDirectory='C:\Program Files\Team MediaPortal\MediaPortal')
$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$output=Join-Path $root 'Source\bin'
New-Item -ItemType Directory -Force $output | Out-Null
$exe=Join-Path $output 'PointerSeekGeometry.exe'
$compilerArgs=@('/nologo','/target:exe','/platform:anycpu',('/out:'+$exe),'/r:System.Drawing.dll','/r:System.Windows.Forms.dll')
foreach($file in @('Core.dll','Common.Utils.dll','Utils.dll')) { $compilerArgs+='/r:'+(Join-Path $MediaPortalDirectory $file) }
$compilerArgs+=Join-Path $root 'Source\PointerSeek.cs'
$compilerArgs+=Join-Path $root 'tests\PointerSeekGeometry.cs'
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' @compilerArgs
if($LASTEXITCODE -ne 0){throw 'Pointer seek test compilation failed'}
& $exe
if($LASTEXITCODE -ne 0){throw 'Pointer seek geometry checks failed'}
