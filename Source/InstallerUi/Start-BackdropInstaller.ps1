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
    $fallback=Join-Path $runtime 'native-fallback.txt'
    if(-not(Test-Path -LiteralPath $fallback)){
    $probe=Start-Process -FilePath $launcher -ArgumentList '--verify' -WindowStyle Hidden -Wait -PassThru
    if($probe.ExitCode -ne 0){[IO.File]::WriteAllText($fallback,'Native installer binding verification failed.')}
    }
    $useNative=Test-Path -LiteralPath $fallback
    if($VerifyOnly){
        if($useNative){Write-Output 'UNVERIFIED: presentation preparation failed; interactive native fallback is available. No installation performed.'; exit 2}
        Write-Output 'PASS: initialized and verified. No installation performed.'; exit 0
    }
    if($useNative){
        Add-Type -AssemblyName System.Windows.Forms
        $installed=[IO.File]::ReadAllText((Join-Path $runtime 'MediaPortalDirectory.txt')).Trim()
        $version=[Reflection.AssemblyName]::GetAssemblyName((Join-Path $installed 'MpeCore.dll')).Version
        $message="Die angepasste Installer-Oberflaeche konnte fuer MediaPortal (MpeCore $version) nicht vorbereitet werden.`r`n`r`nSie koennen auf eigenes Risiko mit dem originalen MediaPortal Extension Installer fortfahren. Die Kompatibilitaet ist nicht bestaetigt; Flackern und Darstellungsfehler sind moeglich. Sichern Sie vorher Ihre Backdrop-Einstellungen. Die originalen Abhaengigkeitspruefungen bleiben aktiv.`r`n`r`nBis hier wurden keine installierten Dateien geaendert.`r`n`r`nMit dem originalen Installer fortfahren?"
        if($Preview){
            [Windows.Forms.MessageBox]::Show('Fuer diese Version ist keine angepasste Vorschau verfuegbar. Es wurde nichts installiert.','Backdrop Setup',[Windows.Forms.MessageBoxButtons]::OK,[Windows.Forms.MessageBoxIcon]::Information) | Out-Null
            Write-Output 'No preview available; no installation performed.'; exit 0
        }
        $answer=[Windows.Forms.MessageBox]::Show($message,'Backdrop Setup - Kompatibilitaet nicht bestaetigt',[Windows.Forms.MessageBoxButtons]::YesNo,[Windows.Forms.MessageBoxIcon]::Warning,[Windows.Forms.MessageBoxDefaultButton]::Button2)
        if($answer -ne [Windows.Forms.DialogResult]::Yes){Write-Output 'Installation cancelled by user; no installation performed.'; exit 0}
    }
    elseif(-not $Preview -and (Test-Path -LiteralPath (Join-Path $runtime 'compatibility-warning.txt'))){
        Add-Type -AssemblyName System.Windows.Forms
        $detail=[IO.File]::ReadAllText((Join-Path $runtime 'compatibility-warning.txt'))
        $answer=[Windows.Forms.MessageBox]::Show("$detail`r`n`r`nDie benoetigten Installer-Schnittstellen wurden erkannt. Der Skin und der Menueeditor sind auf dieser MediaPortal-Version jedoch nicht vollstaendig getestet. Sichern Sie Ihre Backdrop-Einstellungen.`r`n`r`nAuf eigenes Risiko installieren?",'Backdrop Setup - andere MediaPortal-Version',[Windows.Forms.MessageBoxButtons]::YesNo,[Windows.Forms.MessageBoxIcon]::Warning,[Windows.Forms.MessageBoxDefaultButton]::Button2)
        if($answer -ne [Windows.Forms.DialogResult]::Yes){Write-Output 'Installation cancelled by user; no installation performed.'; exit 0}
    }
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
        if(Test-Path -LiteralPath ($package+'.sha256')){
            $expected=([IO.File]::ReadAllText($package+'.sha256').Trim() -split '\s+')[0]
            if((Get-FileHash -LiteralPath $package -Algorithm SHA256).Hash -ne $expected){throw 'Skin package checksum does not match. Download the setup again.'}
        }
        if(Get-Process MediaPortal -ErrorAction SilentlyContinue){throw 'Close MediaPortal before installing the skin.'}
        $arguments='"'+$package+'"'
    }
    if($useNative){
        $launcher=Join-Path $installed 'MpeInstaller.exe'
        $workingDirectory=$installed
        Write-Output 'User accepted unverified compatibility. Starting original MediaPortal installer; private presentation patch is not used.'
    } else {$workingDirectory=$runtime}
    $process=Start-Process -FilePath $launcher -ArgumentList $arguments -WorkingDirectory $workingDirectory -Wait -PassThru
    if($process.ExitCode -ne 0){
        if($useNative){throw "The original MediaPortal installer exited with code $($process.ExitCode). Check its dependency messages and installation log."}
        throw "Backdrop installer exited with code $($process.ExitCode). Inspect Runtime/preview-error.txt."
    }
} catch {
    Write-Error $_
    exit 1
}
