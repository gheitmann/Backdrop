# Backdrop ALPHA25

Version **1.39.0.25** combines the cinematic artwork from the ALPHA24 draft with
the latest corrections following ALPHA22 feedback. Close MediaPortal and run
**Backdrop-Setup-ALPHA25.exe**. Only this single executable is required.
Back up your existing skin before installation.

## Changelog – highlights since ALPHA23

- Added coordinated cinematic startup and category backgrounds while retaining
  the original horizontal BasicHome menu across the bottom of the screen.
- Restored 37 original Backdrop graphics: topbar icons and backing, Home and
  Settings list textures, dialog frame and Music Now Playing separators.
- Separated Latest Recordings titles and genres from the heading to prevent
  overlap. The menu editor generates the same corrected layout.
- Removed manual focus zoom from recording icon grids to avoid drawing beyond
  the viewport. The brief pagination flash is not confirmed resolved.
- Anchored the MENU caption vertically in TV and radio recording menus.
- Updated skin, editor and setup versions to 1.39.0.25. Flexible MediaPortal
  compatibility detection and the native installer fallback are retained.

## Tested scope and remaining limits

The local x64 client loaded 72 synthetic recordings. Normal recording icons
paginate in both directions; large icons load, but their pagination is not yet
verified. All three populated Latest Recordings caption slots were checked
(3/3, 100% of those slots). The last vertical MENU correction passed geometry
checks at 1920x1080 and 3440x1440; its live appearance still needs confirmation.
Full x86 runtime coverage remains pending. These are scoped results, not an
overall compatibility percentage.

The photographic masters are 1672x941, exported at 1920x1080 for distribution.
Personal demo media, databases, machine settings and the separate local audio
core correction are not included. No modified MediaPortal core is distributed.

## Unresolved security report

The earlier ALPHA23 Defender/VirusTotal report remains unresolved. Publishing
this requested test release does not establish a false positive or certify the
new executable as detection-free. See DEFENDER-ALPHA23-COMPARISON.md.

Build checks: [ALPHA25 build report](ALPHA25-BUILD-REPORT.md).
