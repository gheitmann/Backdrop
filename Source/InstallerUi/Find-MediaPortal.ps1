function Resolve-MediaPortalDirectory {
    param([string]$Directory, [switch]$SelectDirectory)
    if($Directory){
        $path=[IO.Path]::GetFullPath($Directory)
        foreach($name in @('MpeCore.dll','MpeInstaller.exe')){
            if(-not(Test-Path -LiteralPath (Join-Path $path $name) -PathType Leaf)){
                throw "Missing installed MediaPortal file: $(Join-Path $path $name). Select the MediaPortal client folder, not the TV Server or ZIP folder."
            }
        }
        return $path
    }
    $candidates=@()
    foreach($root in @($env:ProgramW6432,$env:ProgramFiles,${env:ProgramFiles(x86)})){
        if($root){$candidates+=Join-Path $root 'Team MediaPortal\MediaPortal'}
    }
    foreach($key in @('HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*','HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*')){
        foreach($item in @(Get-ItemProperty -Path $key -ErrorAction SilentlyContinue | Where-Object {$_.DisplayName -match '^MediaPortal' -and $_.InstallLocation})){
            $candidates+=$item.InstallLocation
        }
    }
    $valid=@($candidates | Select-Object -Unique | Where-Object {
        (Test-Path -LiteralPath (Join-Path $_ 'MpeCore.dll') -PathType Leaf) -and
        (Test-Path -LiteralPath (Join-Path $_ 'MpeInstaller.exe') -PathType Leaf)
    })
    if($valid.Count -eq 1){return [IO.Path]::GetFullPath($valid[0])}
    if($SelectDirectory){
        Add-Type -AssemblyName System.Windows.Forms
        $dialog=New-Object System.Windows.Forms.FolderBrowserDialog
        try {
            $dialog.Description='Select the MediaPortal 1.39 client folder containing MpeCore.dll and MpeInstaller.exe'
            $dialog.ShowNewFolderButton=$false
            if($dialog.ShowDialog() -ne [Windows.Forms.DialogResult]::OK){throw 'MediaPortal folder selection cancelled.'}
            return Resolve-MediaPortalDirectory -Directory $dialog.SelectedPath
        } finally {$dialog.Dispose()}
    }
    throw 'MediaPortal client folder could not be determined uniquely. Pass -MediaPortalDirectory with the folder containing MpeCore.dll and MpeInstaller.exe.'
}
