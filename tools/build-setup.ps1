param(
    [Parameter(Mandatory=$true)][string]$BundlePath,
    [Parameter(Mandatory=$true)][string]$OutputPath
)
$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$bundle=(Get-Item -LiteralPath $BundlePath).FullName
$output=[IO.Path]::GetFullPath($OutputPath)
$compiler='C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
& $compiler /nologo /target:winexe /platform:anycpu /r:System.Windows.Forms.dll /r:System.IO.Compression.dll /r:System.IO.Compression.FileSystem.dll ("/resource:"+$bundle+',Backdrop.Bundle') ("/out:"+$output) (Join-Path $root 'Source/InstallerUi/Setup.cs')
if($LASTEXITCODE -ne 0){throw 'Single-file setup build failed.'}
$hash=(Get-FileHash -LiteralPath $output -Algorithm SHA256).Hash.ToLower()
[IO.File]::WriteAllText(($output+'.sha256'),($hash+'  '+[IO.Path]::GetFileName($output)+[Environment]::NewLine))
Write-Output "Built single-file setup: $output"
