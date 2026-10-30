param([Parameter(Mandatory=$true)][string]$Build,[Parameter(Mandatory=$true)][string]$IncompatibleCore,[string]$MediaPortalDirectory='C:\Program Files\Team MediaPortal\MediaPortal')
$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$testRoot=Join-Path ([IO.Path]::GetTempPath()) ('Backdrop fallback tests '+[Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testRoot | Out-Null
$stub=Join-Path $testRoot 'Stub.cs'
@'
using System;
using System.IO;
class NativeStub { static int Main(string[] args) { File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"native-invoked.txt"),Environment.CurrentDirectory+"\n"+string.Join("\n",args));return 0; } }
'@ | Set-Content $stub -Encoding utf8
foreach($case in @('Yes','No','Verify')){
 $dir=Join-Path $testRoot $case; $client=Join-Path $dir 'MP client';$bundle=Join-Path $dir 'Bundle'
 New-Item -ItemType Directory -Path $client,$bundle | Out-Null
 Copy-Item -LiteralPath $IncompatibleCore -Destination (Join-Path $client 'MpeCore.dll')
 Copy-Item -LiteralPath (Join-Path $MediaPortalDirectory 'Utils.dll') -Destination $client
 & 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' /nologo /target:exe ('/out:'+(Join-Path $client 'MpeInstaller.exe')) $stub
 if($LASTEXITCODE -ne 0){throw 'Stub compilation failed'}
 Copy-Item -Path (Join-Path $Build '*.exe'),(Join-Path $Build '*.dll') -Destination $bundle
 Copy-Item -Path (Join-Path $root 'Source/InstallerUi/*.ps1') -Destination $bundle
 'Synthetic package; the native stub never installs files.' | Set-Content (Join-Path $bundle 'fixture.mpe1')
 # Substitute only the dialog result in this disposable test copy, not in production.
 $script=Join-Path $bundle 'Start-BackdropInstaller.ps1'
 $text=[IO.File]::ReadAllText($script)
 $call="[Windows.Forms.MessageBox]::Show(`$message,'Backdrop Setup - Kompatibilitaet nicht bestaetigt',[Windows.Forms.MessageBoxButtons]::YesNo,[Windows.Forms.MessageBoxIcon]::Warning,[Windows.Forms.MessageBoxDefaultButton]::Button2)"
 if(-not $text.Contains($call)){throw 'Expected confirmation call not found'}
 $replacement=if($case -eq 'Yes'){'[Windows.Forms.DialogResult]::Yes'}elseif($case -eq 'No'){'[Windows.Forms.DialogResult]::No'}else{"`$(throw 'Verify must never prompt')"}
 [IO.File]::WriteAllText($script,$text.Replace($call,$replacement))
 $args='-NoProfile -STA -ExecutionPolicy Bypass -File "'+$script+'" -MediaPortalDirectory "'+$client+'"'+$(if($case -eq 'Verify'){' -VerifyOnly'}else{''})
 $p=Start-Process powershell.exe -ArgumentList $args -WindowStyle Hidden -Wait -PassThru -RedirectStandardOutput (Join-Path $dir 'out.txt') -RedirectStandardError (Join-Path $dir 'err.txt')
 $expected=if($case -eq 'Verify'){2}else{0}
 if($p.ExitCode -ne $expected){throw "${case}: unexpected exit $($p.ExitCode); inspect $dir"}
 $invoked=Test-Path -LiteralPath (Join-Path $client 'native-invoked.txt')
 if($invoked -ne ($case -eq 'Yes')){throw "${case}: incorrect native invocation"}
 if(-not(Test-Path -LiteralPath (Join-Path $bundle 'Runtime/native-fallback.txt'))){throw 'No fallback diagnostic'}
 if($invoked){
  $lines=[IO.File]::ReadAllLines((Join-Path $client 'native-invoked.txt'))
  if($lines[0] -ne $client -or $lines[1] -ne (Join-Path $bundle 'fixture.mpe1')){throw 'Wrong native working directory or package argument'}
 }
 Write-Output "PASS: $case fallback branch; native invoked=$invoked; exit=$($p.ExitCode). Dialog answer simulated; no real installation."
}
Write-Output "Test evidence retained in $testRoot"
