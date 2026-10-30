# Backdrop ALPHA24: cinematic backgrounds

Revision 1.39.0.24 adds a coordinated photographic night theme. The single-file
installer is Backdrop-Setup-ALPHA24.exe; close MediaPortal before installation.

## Changes

- Cinema entrance at night for startup and Movies; matching large-TV room,
  music studio, camera, gaming and AV-equipment backgrounds for the other
  BasicHome categories.
- Category-specific fallbacks for selected media and Latest Media.
- New vector-source Backdrop branding composited into the startup picture;
  actual native loading status is placed unobtrusively at bottom right.
- Original full-width horizontal BasicHome menu retained. If a saved session
  opens the classic Home list instead, select BasicHome as the startup screen
  and disable restoring the last active module. Personal settings are not
  overwritten by this skin package.
- ALPHA23 capability-based installer compatibility and native-installer fallback
  are retained; a version number alone does not reject another MP1 release.

## Verification and limits

All six category changes were checked in the local MP1 1.39 x64 client at
3440x1440 (6/6, 100% of that narrow check). This is not an overall compatibility
percentage. Repository checks and package-preparation results are recorded in
the build report. Full x86 runtime coverage remains pending; the editor is
compiled against 1.39 APIs. This package does not include a modified MediaPortal
core or the separate local audio correction.

The package contains 1920x1080 exports from 1672x941 photographic masters, not
native 4K photographs. Local ultrawide exports were tested separately. The native
startup PictureBox stretches its image to the display; for another aspect ratio,
re-export artwork with tools/build-cinematic-artwork.cjs. Startup artwork and
native status-label placement are checked as artifacts/source; a live splash
capture is not claimed.

## Security status — release held for review

ALPHA23 was reported as Trojan:Win32/Wacatac.B!ml during download. The recorded
VirusTotal result is 7/63 detections; Microsoft's engine there was Undetected.
Local Defender scans did not reproduce the detection. This does not establish a
false positive. ALPHA24 changes artwork/version metadata and retains the same
installer architecture; rebuilding does not resolve or certify that concern.
Do not disable protection or create exclusions to install it. This release is
prepared as a draft pending clarification, not presented as a security-cleared
replacement. See DEFENDER-ALPHA23-COMPARISON.md for the bounded comparison.
