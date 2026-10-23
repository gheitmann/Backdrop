# Backdrop ALPHA15 single-file setup

Download `Backdrop-Setup-ALPHA15.exe`, close MediaPortal and run the EXE. No companion files, manual ZIP extraction, PowerShell command or Initialize.ps1 step is required. Windows requests administrator elevation for installation. The native installer retains package information, dependency checks, replacement confirmation and registration. The alternative MPE1 can be opened with the ordinary MediaPortal Extension Installer.

Setup automatically searches both Program Files locations and registered uninstall InstallLocation values for the client folder containing MpeCore.dll and MpeInstaller.exe. If discovery is ambiguous or unsuccessful, select the client folder in the folder picker. Do not select the TV Server or download folder.

Package, setup and editor report 1.39.0.15. Revision 14 was a local build, so the public alpha advances to 15. An installed older build can still trigger native MPE's replacement/uninstall prompt; the old/new versions now identify distinct payloads.

## Presentation and isolation

The setup uses buffered panels, readable Windows button colours and progress repaints limited to one per 100 ms with a final completion flush. A fixed opaque filename viewport clears previous long filenames when shorter names appear; long text is truncated with ellipsis. Native file copying and registration methods are not patched.

The EXE contains a private bundle, extracted per run beneath `%LOCALAPPDATA%\BackdropInstaller`. It copies and patches only the installed unsigned **1.39.0.0** MpeCore into that private directory. Unsupported versions fail initialization. Installed core DLLs and MPE1 associations remain unchanged. No MediaPortal binaries are redistributed. Per-run files are removed after the child exits; diagnostic logs remain in the parent directory.

## Diagnostic options

- `--verify`: initialize and run presentation/assembly binding probes without installation or elevation.
- `--preview`: open the harmless presentation preview without installation.
- `--mediaportal "D:\MediaPortal"`: choose an explicit client folder.

Logs: `last-verification.txt`, `last-run.txt`, and `setup-error.txt` beneath `%LOCALAPPDATA%\BackdropInstaller`.

## Build and validation

1. Build the helper with Windows PowerShell: `tools/build-installer-ui.ps1 -CecilPath <Mono.Cecil.dll> -OutputDirectory <new-folder>`.
2. Validate and package the skin with `python tools/validate.py`, `python tools/check_guide_layout.py` and `python tools/package.py`.
3. Build the private ZIP with `python tools/package-installer-ui.py --build <helper-folder> --cecil-license <license-file> --package <skin.mpe1> --output <private-bundle.zip>`.
4. Embed it using `tools/build-setup.ps1 -BundlePath <private-bundle.zip> -OutputPath <setup.exe>`. Only the final EXE is required for installation; its SHA256 sidecar is optional verification material.

The private bundle includes helper sources, Mono.Cecil 0.11.6 and its MIT notice. Native presentation source inspected matches Team MediaPortal commit d1f11e283bd918521b1613b3488fe417fa2e5157. Do not apply the patch blindly to later native versions. Linux CI builds the ordinary MPE1; native setup preparation requires Windows and an installed supported MediaPortal client.

See VALIDATION.md and SETUP-EXE-VALIDATION.md for exact checks. Synthetic filename rendering, 2,393 progress events and VerifyOnly do not certify a complete real installation, elevation interaction or live-copy repainting. Full x86 execution remains unverified.
