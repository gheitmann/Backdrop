# Backdrop ALPHA23 single-file setup

Download `Backdrop-Setup-ALPHA23.exe`, close MediaPortal and run the EXE. No companion files, manual ZIP extraction, PowerShell command or Initialize.ps1 step is required. Windows requests administrator elevation for installation. The native installer retains package information, dependency checks, replacement confirmation and registration. The alternative MPE1 can be opened with the ordinary MediaPortal Extension Installer.

Setup automatically searches both Program Files locations and registered uninstall InstallLocation values for the client folder containing MpeCore.dll and MpeInstaller.exe. If discovery is ambiguous or unsuccessful, select the client folder in the folder picker. Do not select the TV Server or download folder.

Package, setup and editor report 1.39.0.25. The ALPHA23 helper process architecture correction is retained. An installed older build can still trigger native MPE's replacement/uninstall prompt; the old/new versions identify distinct payloads.

## Presentation and isolation

The setup uses buffered panels, readable Windows button colours and progress repaints limited to one per 100 ms with a final completion flush. A fixed opaque filename viewport clears previous long filenames when shorter names appear; long text is truncated with ellipsis. Native file copying and registration methods are not patched.

The EXE contains a private bundle, extracted per run beneath `%LOCALAPPDATA%\BackdropInstaller`. It reads MpeCore/MpeInstaller/Utils architecture metadata without loading those assemblies into PowerShell, and selects matching x86 or x64 launcher/probe executables. Both variants are embedded in the single Setup EXE. It copies and patches only a structurally compatible unsigned MpeCore into that private directory. Compatibility checks use the form hierarchy, constructors, expected controls and the complete presentation-only RefreshControls instruction contract, followed by execution/binding probes. A different assembly version alone is not rejected. If preparation fails, setup explains the limitation and offers the original installed MpeInstaller.exe with explicit Yes/No consent (No is the default). This fallback never uses the private patched copy; presentation improvements are unavailable. Unknown MP1 release families require a separate compatibility warning even when the adapter probes pass. Declining performs no installation. Installed core DLLs and MPE1 associations remain unchanged. No MediaPortal binaries are redistributed. Per-run files are removed after the child exits; diagnostic logs remain in the parent directory.

## Diagnostic options

- `--verify`: initialize and run presentation/assembly binding probes without installation, prompts or elevation. If only the native fallback is available, verification fails instead of claiming compatibility.
- `--preview`: open the harmless presentation preview without installation.
- `--mediaportal "D:\MediaPortal"`: choose an explicit client folder.

Logs: `last-verification.txt`, `last-run.txt`, and `setup-error.txt` beneath `%LOCALAPPDATA%\BackdropInstaller`.

## Build and validation

1. Build the helper with Windows PowerShell: `tools/build-installer-ui.ps1 -CecilPath <Mono.Cecil.dll> -OutputDirectory <new-folder>`.
2. Validate and package the skin with `python tools/validate.py`, `python tools/check_guide_layout.py` and `python tools/package.py`.
3. Build the private ZIP with `python tools/package-installer-ui.py --build <helper-folder> --cecil-license <license-file> --package <skin.mpe1> --output <private-bundle.zip>`.
4. Embed it using `tools/build-setup.ps1 -BundlePath <private-bundle.zip> -OutputPath <setup.exe>`. Only the final EXE is required for installation; its SHA256 sidecar is optional verification material.

The private bundle includes helper sources, Mono.Cecil 0.11.6 and its MIT notice. Native presentation source inspected matches Team MediaPortal commit d1f11e283bd918521b1613b3488fe417fa2e5157. Later or earlier releases must satisfy the same structural contract; otherwise use the explicit native fallback. Linux CI builds the ordinary MPE1; native setup preparation requires Windows and an installed supported MediaPortal client.

See VALIDATION.md and SETUP-EXE-VALIDATION.md for exact checks. Synthetic filename rendering, 2,393 progress events and VerifyOnly do not certify a complete real installation, elevation interaction or live-copy repainting. The presentation and native binding probes pass against both the real x64 installation and official extracted x86 client binaries. A complete interactive x86 installation has not been performed.

Failed setup runs retain their private working directory and write setup-error.txt plus last-run.txt (or last-verification.txt). Successful runs clean up their private working directory.


## Version policy

The published setup targets MP1. The package permits MP1 versions from 1.4 through
1.999 and keeps the native skin-engine 1.4 requirement. This broad installer range
is an opt-in compatibility policy, not a claim that every old release can load
the current editor or supports every native binding. The editor is built against
1.39 APIs; real API, runtime, architecture and skin-engine constraints still apply.
MediaPortal 2 is a separate application and is not supported.

Reproduce structural regression checks by compiling tools/InstallerCompatibilityProbe.cs
against Mono.Cecil and passing the native MpeCore, PatchInstaller.exe, helper DLL
and a new fixture directory. tools/test-installer-fallback.ps1 exercises accepted,
declined and non-interactive fallback with a harmless native stub and simulated
dialog answers. These tests do not install MediaPortal or certify another release.
