# ALPHA15 installer correction — 24 October 2026

Use **Backdrop-Setup-ALPHA15-InstallerFix.exe** for ALPHA15. This corrected single-file starter contains the original ALPHA15 MPE1 unchanged (SHA256 4b9f9178e1a40c3ef06fac71b1d2fe391ac111b5b427773c83618c7d33119aa1). Skin/editor package version remains 1.39.0.15; the corrected setup executable reports 1.39.0.16. LOCAL16 skin changes, including focused music metadata captions, are separate working changes and are not bundled in this ALPHA15 installer correction.

## Findings and correction

The original single-file starter invoked native MPE from a temporary application directory. MP Config cached that location as the native Base/Plugins root. Although 2,392 payload files installed correctly, the editor DLL and its uninstall record pointed into the temporary run directory. It was removed during cleanup, leaving the previous editor installed. A subsequent AppDomain-based experiment corrected the destination in preflight but produced severe owner-observed flicker; it was rejected. A private Config setter lookup initially failed with a null reference; the lookup and noninteractive failure handling were corrected before the final run.

The final starter retains its original WinForms application domain and corrects only the native Base/Plugins entries through the version-specific Config setter, before MPE creates its path providers. It verifies the destination and private MpeCore binding, records them for actual runs, and retains native copying/registration logic. Setup logs now retain native-run diagnostics and errors before cleaning up temporary files. No installed MediaPortal core DLL is modified.

## Final real-install evidence

The owner authorized each native install test and confirmed that the final run showed **no flicker**. The initial buffered run's filename display was also owner-confirmed correct. After the final corrected installation:

- **2,393/2,393 payload files match byte-for-byte (100%)**, including 2,388 skin files, three Windows fonts, config and menu editor DLL.
- InstalledExtensions registers Backdrop 1.39.0.15.
- The installed plugin assembly reports 1.39.0.15 and matches the MPE1 bytes.
- UninstallInfo records the actual installed client plugin path, not a temporary path.
- Native-run diagnostics confirm the intended plugin destination, private presentation assembly and a returned native entry point.

The ordinary MPE1 remains an alternative. Reinstalling the same ALPHA15 package can legitimately show equal package versions; the InstallerFix changes the starter, not the skin payload. Previous InstallerUI helper ZIPs and the original ALPHA15 EXE are superseded by this corrected starter. Full x86 runtime and a broader repeated-install/display matrix remain unverified.
