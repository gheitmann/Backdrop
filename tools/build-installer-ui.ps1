param(
    [string]$MediaPortalDirectory='C:\Program Files\Team MediaPortal\MediaPortal',
    [Parameter(Mandatory=$true)][string]$CecilPath,
    [Parameter(Mandatory=$true)][string]$OutputDirectory
)
$ErrorActionPreference='Stop'
if(Test-Path -LiteralPath $OutputDirectory){throw 'Use a new output directory.'}
$taskSource=Join-Path (Split-Path $PSScriptRoot -Parent) 'Source/InstallerUi'
$taskOutput=[IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Path $taskOutput | Out-Null
$compiler='C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$helper=Join-Path $taskOutput 'Backdrop.InstallerPresentation.dll'
& $compiler /nologo /target:library /platform:anycpu /r:System.Windows.Forms.dll /r:System.Drawing.dll ("/out:"+$helper) (Join-Path $taskSource 'InstallerPresentation.cs')
if($LASTEXITCODE -ne 0){throw 'Presentation build failed'}
$patcher=Join-Path $taskOutput 'PatchInstaller.exe'
& $compiler /nologo /target:exe /platform:anycpu /r:System.Core.dll ("/r:"+$CecilPath) ("/out:"+$patcher) (Join-Path $taskSource 'PatchInstaller.cs')
if($LASTEXITCODE -ne 0){throw 'Patcher build failed'}
Copy-Item -LiteralPath $CecilPath -Destination (Join-Path $taskOutput 'Mono.Cecil.dll')
& $patcher (Join-Path $MediaPortalDirectory 'MpeCore.dll') $helper (Join-Path $taskOutput 'MpeCore.dll')
if($LASTEXITCODE -ne 0){throw 'Patching failed'}
foreach($architecture in @('x86','x64')){
    & $compiler /nologo /target:winexe ("/platform:"+$architecture) /r:System.Windows.Forms.dll ("/out:"+(Join-Path $taskOutput ('BackdropInstaller.'+$architecture+'.exe'))) (Join-Path $taskSource 'BackdropInstaller.cs')
    if($LASTEXITCODE -ne 0){throw "Launcher build failed: $architecture"}
    & $compiler /nologo /target:exe ("/platform:"+$architecture) /r:System.Windows.Forms.dll /r:System.Drawing.dll ("/r:"+$helper) ("/out:"+(Join-Path $taskOutput ('PresentationProbe.'+$architecture+'.exe'))) (Join-Path $taskSource 'PresentationProbe.cs')
    if($LASTEXITCODE -ne 0){throw "Probe build failed: $architecture"}
}
. (Join-Path $taskSource 'Get-MediaPortalArchitecture.ps1')
$architecture=Get-MediaPortalArchitecture -Directory $MediaPortalDirectory
foreach($name in @('BackdropInstaller','PresentationProbe')){
    Copy-Item -LiteralPath (Join-Path $taskOutput ($name+'.'+$architecture+'.exe')) -Destination (Join-Path $taskOutput ($name+'.exe'))
}
[IO.File]::WriteAllText((Join-Path $taskOutput 'MediaPortalDirectory.txt'),[IO.Path]::GetFullPath($MediaPortalDirectory))
& (Join-Path $taskOutput 'PresentationProbe.exe')
if($LASTEXITCODE -ne 0){throw 'Presentation probe failed; inspect presentation-probe.txt'}
$binding=Start-Process -FilePath (Join-Path $taskOutput 'BackdropInstaller.exe') -ArgumentList '--verify' -WindowStyle Hidden -Wait -PassThru
if($binding.ExitCode -ne 0){throw 'Native assembly binding probe failed'}
Get-FileHash -LiteralPath (Join-Path $MediaPortalDirectory 'MpeCore.dll'),(Join-Path $taskOutput 'MpeCore.dll') -Algorithm SHA256 | Select-Object Path,Hash
Write-Output "Built isolated installer UI in $taskOutput. Installed MediaPortal binaries were not changed."
