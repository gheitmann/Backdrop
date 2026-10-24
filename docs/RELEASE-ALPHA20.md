# Backdrop ALPHA20: installer architecture fix

ALPHA19's AnyCPU presentation probe ran as a 64-bit process on 64-bit Windows.
When the installed MediaPortal client was 32-bit, loading its MpeCore.dll failed
with BadImageFormatException before installation started.

The single Setup EXE now contains dedicated x86 and x64 installer launchers and
presentation probes. Initialization reads the architecture metadata of installed
MpeCore.dll, MpeInstaller.exe and Utils.dll without loading them into PowerShell,
then selects matching helpers before testing or starting the native installer.
Mixed incompatible dependencies are rejected with an explicit error.

Failed runs retain their private directory and diagnostic files. The error dialog
shows the diagnostic paths instead of squeezing the full stack trace into a long
message box. Successful runs still clean up their private temporary directory.
No MediaPortal core binaries are distributed or replaced.

## Verification

- Reproduced the original failure with the ALPHA19 AnyCPU probe against the
  official MediaPortal 1.39 x86 package on 64-bit Windows: exit 1,
  BadImageFormatException loading MpeCore.dll.
- The matching x86 probe passes against those same official client binaries:
  three readable buttons, long-to-short filename repaint, buffered form and
  2393/2393 synthetic progress events. Native installer binding also passes.
- Matching x64 presentation and binding probes pass against the installed x64
  client. Both test sets use private patched copies of MpeCore.
- The completed single-file Setup is verified separately against both x86 and
  x64 clients without installing or invoking the native installation entry point.
- Payload validation remains 2405 files and 558 skin XMLs. Playback geometry
  contracts pass. The skin changes and remaining playback test limitations from
  ALPHA19 remain unchanged.

A complete interactive installation on the user's x86 HTPC has not been performed.
Download Backdrop-Setup-ALPHA20.exe; no manual helper selection or extra files are
required. Internal package/editor/setup revision is 1.39.0.20.
