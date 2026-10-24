param([string]$MediaPortalDirectory, [switch]$SelectDirectory)
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'Find-MediaPortal.ps1')
$installed=Resolve-MediaPortalDirectory -Directory $MediaPortalDirectory -SelectDirectory:$SelectDirectory
. (Join-Path $PSScriptRoot 'Get-MediaPortalArchitecture.ps1')
$architecture=Get-MediaPortalArchitecture -Directory $installed
if($architecture -eq 'x64' -and -not [Environment]::Is64BitOperatingSystem){throw 'This MediaPortal installation requires 64-bit Windows.'}
Write-Output "Selected MediaPortal installer architecture: $architecture"
$runtime=Join-Path $PSScriptRoot 'Runtime'
if(Test-Path -LiteralPath $runtime){throw 'Runtime already exists. Extract a fresh copy before initializing again.'}
foreach($name in @('MpeCore.dll','MpeInstaller.exe')){
    if(-not(Test-Path -LiteralPath (Join-Path $installed $name))){throw "Missing installed MediaPortal file: $name"}
}
New-Item -ItemType Directory -Path $runtime | Out-Null
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'Backdrop.InstallerPresentation.dll') -Destination $runtime
foreach($name in @('BackdropInstaller','PresentationProbe')){
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot ($name+'.'+$architecture+'.exe')) -Destination (Join-Path $runtime ($name+'.exe'))
}
& (Join-Path $PSScriptRoot 'PatchInstaller.exe') (Join-Path $installed 'MpeCore.dll') (Join-Path $runtime 'Backdrop.InstallerPresentation.dll') (Join-Path $runtime 'MpeCore.dll')
if($LASTEXITCODE -ne 0){throw 'Unsupported installer; no installed files were changed.'}
[IO.File]::WriteAllText((Join-Path $runtime 'MediaPortalDirectory.txt'),$installed)
[IO.File]::WriteAllText((Join-Path $runtime 'architecture.txt'),$architecture)
& (Join-Path $runtime 'PresentationProbe.exe')
if($LASTEXITCODE -ne 0){throw 'Presentation probe failed. Inspect Runtime/presentation-probe.txt.'}
$probe=Start-Process -FilePath (Join-Path $runtime 'BackdropInstaller.exe') -ArgumentList '--verify' -WindowStyle Hidden -Wait -PassThru
if($probe.ExitCode -ne 0){throw 'Assembly binding verification failed.'}
Write-Output 'Ready. Open Runtime/BackdropInstaller.exe for a harmless preview, or drag an MPE1 package onto it to install using the native installer.'
Write-Output 'Installed MediaPortal binaries were not changed.'
