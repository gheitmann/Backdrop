# Backdrop ALPHA27

Version **1.39.0.27**. Close MediaPortal and run **Backdrop-Setup-ALPHA27.exe**.
The single EXE contains the complete skin/editor package and optional background
lighting plugin. Back up existing Backdrop preferences before updating.

## Changelog since ALPHA26

- Added distinct Morning and Noon backgrounds for all ten themes, alongside Day
  and the existing cinematic Night images: 20 new images, 30 daylight images.
- Added Settings > GUI > Skin > Background lighting, using native menu dialogs.
- Added Automatic, Morning, Noon, Day / afternoon, Night and Off modes; four
  configurable start times default to 06:00, 12:00, 15:00 and 19:00.
- Added 37 translated plugin UI catalogues with English fallback and Save/Cancel.
- Preserved existing two-period preferences through migration and stock-image
  mappings when saving menus in the editor. Custom images and fanart stay intact.
- Published the independent [MediaPortal Day/Night plugin](https://github.com/gheitmann/MediaPortal-DayNight)
  separately; Backdrop already bundles the same DLL.
- Corrected TV HomeServer background and native TV guide group/toggle artwork.
- Restored the original Settings clock include and raised video seek overlays
  to correct their vertical placement after ALPHA26 feedback.
- Retained original control artwork and the horizontal BasicHome main menu.
- Advanced the editor and single-file setup identity to 1.39.0.27.

## Use and fallback

Enable Day/Night Backgrounds under MediaPortal Configuration > Plugins > Process
plugins if needed. The in-skin entry is available when the plugin is running.
Save applies changes immediately; Cancel discards them. Off retains original
backgrounds. Unsupported skins are not automatically converted. The startup
splash remains static. An absent/disabled plugin uses original Night images.

## Validation and limits

Native x86 and x64 probes each passed 1,364/1,364 assertions before release
packaging. The full local x64 client opened the native dialog, applied Morning
and Noon without restart, discarded a staged time change, and returned to
Automatic. All 425 protected original artwork hashes/dimensions are unchanged.
See [the build report](ALPHA27-BUILD-REPORT.md) for final package/setup checks.

This is an alpha prerelease. Full x86 client interaction, suspend/resume,
prolonged playback, every translated screen and broader plugin coverage remain
unverified. The reported recordings pagination flash and deferred TV progress
irregularities are not claimed fixed. No personal demo databases, test fixtures,
local audio fixes, MediaPortal core DLLs or third-party plugin experiments are
bundled. Compatibility beyond tested clients is not inferred from version alone.
