param([string]$PackagePath, [string]$MediaPortalDirectory, [switch]$Preview, [switch]$VerifyOnly)
$ErrorActionPreference='Stop'
try {
    if($Preview -and $VerifyOnly){throw 'Choose either -Preview or -VerifyOnly.'}
    $runtime=Join-Path $PSScriptRoot 'Runtime'
    if(-not(Test-Path -LiteralPath $runtime)){
        & (Join-Path $PSScriptRoot 'Initialize.ps1') -MediaPortalDirectory $MediaPortalDirectory -SelectDirectory:(-not $VerifyOnly)
    }
    if($MediaPortalDirectory){
        . (Join-Path $PSScriptRoot 'Find-MediaPortal.ps1')
        $requested=Resolve-MediaPortalDirectory -Directory $MediaPortalDirectory
        $saved=[IO.File]::ReadAllText((Join-Path $runtime 'MediaPortalDirectory.txt')).Trim()
        if($requested -ne $saved){throw 'Runtime belongs to another MediaPortal installation. Extract the bundle into a fresh folder.'}
    }
    $launcher=Join-Path $runtime 'BackdropInstaller.exe'
    $probe=Start-Process -FilePath $launcher -ArgumentList '--verify' -WindowStyle Hidden -Wait -PassThru
    if($probe.ExitCode -ne 0){throw 'Native installer binding verification failed.'}
    if($VerifyOnly){Write-Output 'PASS: initialized and verified. No installation performed.'; exit 0}
    if($Preview){
        $arguments='--preview'
    } else {
        if(-not $PackagePath){
            $packages=@(Get-ChildItem -LiteralPath $PSScriptRoot -Filter '*.mpe1' -File)
            if($packages.Count -ne 1){throw 'Place exactly one MPE1 beside this script, or pass -PackagePath.'}
            $PackagePath=$packages[0].FullName
        }
        $package=(Get-Item -LiteralPath $PackagePath -ErrorAction Stop).FullName
        if([IO.Path]::GetExtension($package) -ne '.mpe1'){throw 'Expected an MPE1 package.'}
        if(Get-Process MediaPortal -ErrorAction SilentlyContinue){throw 'Close MediaPortal before installing the skin.'}
        $arguments='"'+$package+'"'
    }
    $process=Start-Process -FilePath $launcher -ArgumentList $arguments -WorkingDirectory $runtime -Wait -PassThru
    if($process.ExitCode -ne 0){throw "Backdrop installer exited with code $($process.ExitCode). Inspect Runtime/preview-error.txt."}
} catch {
    Write-Error $_
    exit 1
}
