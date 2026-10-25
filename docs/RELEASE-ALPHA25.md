# Backdrop ALPHA25

Version 1.39.0.25 combines the cinematic artwork from the ALPHA24 draft with the
latest corrections following ALPHA22 feedback. Close MediaPortal and run the
single Backdrop-Setup-ALPHA25.exe. Back up your current skin first.

## Changes

- Coordinated cinematic startup and category backgrounds, retaining the original
  horizontal menu across the bottom of BasicHome.
- Restored 37 original Backdrop graphics: topbar icons and backing, Home and
  Settings list textures, dialog frame and Now Playing separators.
- Latest Recordings title and genre now appear after the heading without overlap.
  The menu editor generates the same corrected layout.
- Recording grids no longer request manual focus zoom beyond the viewport.
  This mitigates clipping; the reported brief pagination flash is not confirmed
  resolved.
- TV and radio recording menus use an explicit vertical MENU caption anchor.
- Version-tolerant installer detection and explicit native fallback are retained.
  A MediaPortal version number alone does not reject installation.

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
