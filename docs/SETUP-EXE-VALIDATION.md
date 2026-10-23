# Single-EXE setup and filename repaint validation

23 October 2026, Windows 11, MediaPortal 1.39 x64. Local build only; no new alpha published. Earlier bundle checks are recorded separately in INSTALLER-BUNDLE-VALIDATION.md.

Four targeted checks pass: **4/4 (100%)**.

1. Native presentation probe: fixed opaque filename viewport remains the same size after a 180-character filename changes to `a.png`; rendered right-hand pixels equal the background colour. Three readable button styles, buffering, all 2,393 synthetic events and final progress flush still pass.
2. Embedded archive: all private bundle entries match their source bytes; the skin package checksum is verified. Setup's embedded resource matches the complete private bundle ZIP. No installed MediaPortal core binaries are distributed.
3. End-to-end EXE verification: `Backdrop-Setup-LOCAL.exe --verify` extracts, discovers the installed client, patches only its private copy, runs presentation checks and verifies native assembly binding. No installation entry point is invoked.
4. Standalone delivery: only the EXE is copied into a separate folder containing spaces; --verify passes with no sibling script, DLL or MPE1. Its private run directory is removed afterwards.

Installed MpeCore retains SHA256 `124cd3074e92c3dcc53f20f0ec6a1ab7d62eb3b53d5182ae0ae3ec9fa6ba670c`. Setup source targets AnyCPU; complete x86 execution remains unverified. Actual installation, elevation interaction and live file-copy repainting are not newly certified by these synthetic/preparation checks. The owner's reported artifact is addressed in the label presentation; a real-install confirmation is still needed.
