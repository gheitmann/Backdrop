# Optional installer presentation fix

The MediaPortal 1.39 Extension Installer redraws its full installation window after each file and uses fixed white button text. The optional Backdrop installer helper uses buffered panels, Windows button colours and targeted progress repaints, limited to one per 100 ms with a final completion flush.

This is separate from the skin MPE1. Installing the skin with the ordinary Extension Installer does not update that installer's own appearance.

## Use the optional ZIP

1. Extract `Backdrop-InstallerUI-ALPHA11.zip` to a new writable folder.
2. Run `powershell.exe -NoProfile -File .\Initialize.ps1`. For a different installation directory, pass `-MediaPortalDirectory 'D:\MediaPortal'`.
3. Initialization copies and patches the installed **unsigned 1.39.0.0** MpeCore into the extracted folder's `Runtime` subdirectory and checks presentation and assembly binding. It never overwrites Program Files. Unsupported versions or unexpected code fail initialization.
4. Double-click `Runtime\BackdropInstaller.exe` for a preview that installs nothing. To install, close MediaPortal and drag the desired `.mpe1` onto that executable. The original MediaPortal installer handles package checks, installation and registration using the separate presentation assembly.

The ZIP contains no MediaPortal binaries. It includes the helper, local patcher, Mono.Cecil 0.11.6 and its MIT notice, probes and complete helper sources. Remove the extracted folder to remove this optional helper; ordinary MPE1 file associations remain unchanged.

## Validation and limits

On Windows 11 with MP 1.39 x64: native preview buttons are readable; three button styles and buffered form pass the probe. All 2,393 synthetic file events reach the native progress handler, with two targeted repaint calls during the short burst and a final completion flush. A separate probe confirms that the original installer assembly binds to the copied MpeCore without invoking its entry point. These probes do not install or register a package. A full real installation through this helper remains unverified; use the ordinary installer if the helper fails.

## Build from source

Use `tools/build-installer-ui.ps1 -CecilPath <Mono.Cecil.dll> -OutputDirectory <new-folder>` with Windows PowerShell and the .NET Framework compiler. The native source inspected matches Team MediaPortal commit `d1f11e283bd918521b1613b3488fe417fa2e5157`: `Tools/MPE/MpeCore/Classes/SectionPanel/InstallSection.cs` and the base horizontal/vertical layouts. Native file copying, dependencies and registration methods are not patched.

The source and exact validation limits are retained here for future maintenance. Do not apply this patch blindly to later MediaPortal versions.
