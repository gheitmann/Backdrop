# Backdrop ALPHA15 for MediaPortal 1.39

**Recommended download: Backdrop-Setup-ALPHA15.exe.** This is one executable containing the skin and installer helper; no companion files, ZIP extraction or manual PowerShell steps are required. Close MediaPortal before running it. The MPE1 is an alternative for the ordinary Extension Installer. SHA256 sidecars are optional integrity checks.

Changes since ALPHA13:

- Single-file setup with automatic client-folder discovery and a folder picker for custom installations. The isolated installer copy uses readable Windows button colours, buffered panels and throttled progress repainting. A fixed opaque filename viewport clears previous long filenames when shorter names appear.
- Complete original room background in the TV guide, darkened for readability, preserving the lower EPG grid and upper-right preview/clock.
- Latest Media titles anchored above the home menu, including matching generator changes in the rebuilt AnyCPU editor.
- Removed the inherited yellow nonfocused background from the music Radio menu item, preserving its native checkbutton binding and state.
- Package, setup and editor version 1.39.0.15. Revision 14 was a local build; the alpha number advances to distinguish it from that delivered payload.

Validation: 3/3 targeted guide-background runtime checks; 4/4 single-EXE/repaint preparation checks; 18/18 static Latest title containment cases. These are separate bounded checklists, not an overall compatibility score. Payload validation covers 2,393 files and 557 skin XML definitions; native binding audit finds zero incompatible types in 139 windows.

The latest title/radio changes still need fresh runtime visual confirmation. Real installation/elevation and live filename repainting are not certified by the preparation probes. Historical regression results are carried forward without a complete rerun. Full x86 runtime remains unverified. The reported Live TV progress irregularity is deferred. See docs/VALIDATION.md for detailed scope.

The setup does not distribute or overwrite MediaPortal core binaries. It requires an installed supported unsigned MediaPortal 1.39 client and retains native dependency, package replacement and registration handling. Native MPE can still ask to replace an installed version; this build now reports a distinct version.
