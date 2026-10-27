# Backdrop BETA1

Version **1.39.0.28**. Download **Backdrop-Setup-BETA1.exe**, close MediaPortal and run the single executable. Back up your existing Backdrop installation and preferences before updating. The setup bundles the skin, standalone menu editor and optional Day/Night plugin; no companion downloads or manual PowerShell initialization are needed.

## Changes since ALPHA27

- Restored original playlist focus rows, playlist panel and Now Playing action-menu button artwork.
- Restyled Radio and Radio Search with existing Backdrop backgrounds, classic list graphics, typography and a localized search heading, while retaining native controls and commands.
- Added restrained animated focus backgrounds to nine action menus. Buttons, captions, checkmarks and mouse hit areas stay stationary.
- Restored a small recording-cover focus enlargement. Increased small-icon row spacing so the bottom captions are not clipped; retained the intentional hidden caption on the selected cover.
- Fixed recording page-up/page-down navigation leaving the idle focus placeholder.
- Fixed incomplete menu-editor input causing a null-reference exception when adding entries such as DayNight.xml. Missing fields now produce a useful validation message.
- Fixed BasicHome submenu handling when Backdrop is the active skin but a different skin is selected in the main configuration file.
- Enlarged the Settings heading and sharpened the central view in the Photos night background. Morning, Noon and Day images are unchanged.
- Kept the horizontal BasicHome menu, original control artwork, cinematic backgrounds and configurable four-period lighting from ALPHA27.

## Verification

The x64 client was exercised locally at 3440 x 1440 with synthetic fixtures: BasicHome, Radio/Search, playlist and action-menu navigation, Photos, Settings and recording icon layouts. Page down/up changed pages in both small and big icon layouts, and the corrected lower captions remained visible. This short check does not certify the absence of single-frame flashes.

The final single EXE passed VerifyOnly preparation for x64 and x86. A native x64 update completed with exit code 0: 2,441 installed files match the package byte-for-byte and the remaining personal BackdropMenuEditor.xml was intentionally preserved. An isolated native copy-provider test also compared all 2,442 mapped payload files. Seven selected static checks passed. The menu editor loaded successfully, its submenu regression passed, and the x64 Day/Night integration probe passed 1,392 assertions across 44 background bindings.

## Known limitations

- Full x86 client interaction, all screen sizes/DPI/locales and prolonged playback are not certified.
- A brief recording-grid flash may still occur during paging; the short review cannot rule it out.
- Actual radio tuning/playback was not tested locally because the synthetic station fixtures have no tuner mapping. Radio display/search/navigation were checked.
- The previously reported Live TV progress irregularity remains deferred.
- Optional third-party plugins require compatible versions. No MediaPortal core/audio or third-party experiment is bundled.

See [build report](BETA1-BUILD-REPORT.md) and [prior scoped review](RUNTIME-REVIEW-POST-ALPHA27.md). The optional [Day/Night plugin](https://github.com/gheitmann/MediaPortal-DayNight) remains independently maintained; Backdrop includes the same published DLL.
