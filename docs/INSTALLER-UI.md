# Backdrop single-file setup

The MediaPortal 1.39 Extension Installer redraws its full installation window after each file and uses fixed white button text. The optional Backdrop installer helper uses buffered panels, Windows button colours and targeted progress repaints, limited to one per 100 ms with a final completion flush.

This is separate from the skin MPE1. Installing the skin with the ordinary Extension Installer does not update that installer's own appearance.

## Single EXE (preferred delivery)

Run `Backdrop-Setup-LOCAL14.exe` after closing MediaPortal. This is the complete distributable: no ZIP extraction, manual PowerShell command, Initialize.ps1 or companion DLL is required. It contains the skin and helper bundle, extracts into a private per-run directory under `%LOCALAPPDATA%\BackdropInstaller`, then removes that directory when the child installer exits. Logs remain in that parent directory. Installed MediaPortal core DLLs and MPE1 associations are not replaced.

Normal installation requests Windows administrator elevation. `--verify` initializes and probes without installing or elevating; `--preview` opens the harmless installer presentation preview. `--mediaportal "D:\MediaPortal"` selects a custom client folder. Otherwise automatic discovery and the folder picker are used. The existing unsigned MP 1.39 assembly compatibility guard remains enforced.

The filename status label now has fixed bounds, an opaque background and ellipsis truncation. Refreshing a short name repaints the entire previous filename area. This addresses old long-name fragments without restoring per-file full-window repainting. Native file copying and registration are unchanged.

Build the private bundle ZIP as described below, then run `powershell.exe -NoProfile -ExecutionPolicy Bypass -File tools/build-setup.ps1 -BundlePath <bundle.zip> -OutputPath <setup.exe>`. Only the resulting EXE is required for installation; the SHA256 sidecar is optional verification material. LOCAL14 advances internal package, editor and setup versions to 1.39.0.14, without publishing a new alpha. Earlier LOCAL packages incorrectly retained 1.39.0.13; they are superseded. Native MPE may still ask to replace an installed package, but old/new versions now identify the actual builds.

## Integrated local bundle

Extract `Backdrop-Installer-LOCAL.zip` into a writable folder, close MediaPortal and double-click `Install Backdrop.cmd`. The bundle contains the current local skin MPE1 and the presentation helper. The starter initializes the isolated installer automatically, verifies its assembly binding and opens the native installer with the included package. Review the native installer's package information and proceed there.

The earlier ZIP bundle is superseded by the single EXE. LOCAL14 includes the full-room guide background and latest-caption/radio-menu corrections. No release or tag is published.

MediaPortal is detected in both Program Files locations and registered uninstall locations. If no unique client folder is found, a folder picker asks for the directory containing both `MpeCore.dll` and `MpeInstaller.exe`. This fixes the earlier hard-coded-path failure. Select the client installation, not the TV Server folder. An explicit invalid path reports the complete missing filename. Multiple detected installations require selection.

For a custom path or checks without installing:

```powershell
powershell.exe -NoProfile -STA -ExecutionPolicy Bypass -File .\Start-BackdropInstaller.ps1 -MediaPortalDirectory 'D:\MediaPortal'
powershell.exe -NoProfile -STA -ExecutionPolicy Bypass -File .\Start-BackdropInstaller.ps1 -VerifyOnly
powershell.exe -NoProfile -STA -ExecutionPolicy Bypass -File .\Start-BackdropInstaller.ps1 -Preview
```

`-VerifyOnly` initializes and checks binding without opening the installer or selecting a folder interactively. `-Preview` opens the harmless presentation preview. A verified Runtime can be reused. To select a different MediaPortal installation, extract a fresh bundle. Ordinary MPE1 file associations are unchanged.

## Use the optional ZIP

1. Extract `Backdrop-InstallerUI-ALPHA11.zip` to a new writable folder.
2. Run `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\Initialize.ps1`. Current sources detect the installed client folder; for a different installation directory, pass `-MediaPortalDirectory 'D:\MediaPortal'`. The old ALPHA11 ZIP still requires that explicit parameter when its fixed default path is wrong.
3. Initialization copies and patches the installed **unsigned 1.39.0.0** MpeCore into the extracted folder's `Runtime` subdirectory and checks presentation and assembly binding. It never overwrites Program Files. Unsupported versions or unexpected code fail initialization.
4. Double-click `Runtime\BackdropInstaller.exe` for a preview that installs nothing. To install, close MediaPortal and drag the desired `.mpe1` onto that executable. The original MediaPortal installer handles package checks, installation and registration using the separate presentation assembly.

The ZIP contains no MediaPortal binaries. It includes the helper, local patcher, Mono.Cecil 0.11.6 and its MIT notice, probes and complete helper sources. Remove the extracted folder to remove this optional helper; ordinary MPE1 file associations remain unchanged.

## Validation and limits

On Windows 11 with MP 1.39 x64: native preview buttons are readable; three button styles and buffered form pass the probe. All 2,393 synthetic file events reach the native progress handler, with two targeted repaint calls during the short burst and a final completion flush. A separate probe confirms that the original installer assembly binds to the copied MpeCore without invoking its entry point. These probes do not install or register a package. A full real installation through this helper remains unverified; use the ordinary installer if the helper fails.

## Build from source

Use `tools/build-installer-ui.ps1 -CecilPath <Mono.Cecil.dll> -OutputDirectory <new-folder>` with Windows PowerShell and the .NET Framework compiler. The native source inspected matches Team MediaPortal commit `d1f11e283bd918521b1613b3488fe417fa2e5157`: `Tools/MPE/MpeCore/Classes/SectionPanel/InstallSection.cs` and the base horizontal/vertical layouts. Native file copying, dependencies and registration methods are not patched.

After building the helper and skin MPE1, create an integrated ZIP with `python tools/package-installer-ui.py --build <build-folder> --cecil-license <license-file> --package <skin.mpe1> --output <bundle.zip>`. The skin checksum and all archived bytes are verified. Omit `--package` to produce a helper-only ZIP. Build probes require an installed supported MediaPortal; Linux CI continues to produce the ordinary MPE1 only.

The source and exact validation limits are retained here for future maintenance. Do not apply this patch blindly to later MediaPortal versions.
