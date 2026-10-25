# ALPHA22 release verification

25 October 2026. Build tools: Windows PowerShell 5.1, .NET Framework compiler,
Python payload tooling. Editor launcher and plugin rebuilt at revision 1.39.0.22.

- All six repository checks passed (100% of these six static check commands):
  payload validation, integration audit, guide, playback, menu and secondary layout.
- 2,405 payload files and 558 skin XML files validated; MPE1 packing verified bytes.
- Private installer bundle: 21 entries verified; no MediaPortal core DLL included.
- Finished single EXE preparation passed against installed x64 and official extracted
  x86 client files, including an x86 client path containing spaces (2/2, 100%).
  Each selected its matching process architecture and native plugin destination.
- Both presentation probes passed readable-button, long-to-short filename repaint,
  buffering and all 2,393 synthetic-event checks. Neither probe installs files.
- Recent installed x64 practical coverage and remaining runtime limitations are
  described in RELEASE-ALPHA22.md. Preparation checks do not certify a fresh full
  x86 installation or every plugin, recording and tuner configuration.

The single setup EXE contains the skin package and its private initialization tools.
Only the EXE and checksum are published as release assets. The local audio shutdown
Core.dll correction is excluded; see tools/diagnostics/music-end-exit.md.
