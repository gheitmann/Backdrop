# Scoped runtime review after ALPHA27

Local MediaPortal x64, 3440 x 1440, synthetic fixtures, 27 October 2026. Tested the current unpublished working tree. The main profile remains Apollo; Backdrop was launched with a separate copied configuration and /skin=Backdrop. No core or third-party plugin binaries were changed.

## Observed

- BasicHome: horizontal lower menu, category backgrounds and submenus render and can be selected. Fixed the menu helper to inspect the active skin; previously it incorrectly used the main profile skin.
- Radio: populated synthetic groups/list, original row style, studio background, action menu and navigation to Search render and respond.
- Radio Search: synthetic title results and native menu rows render. Corrected its missing localized page heading afterwards; this last heading change was XML-checked and deployed, but not separately revisited on screen.
- Music playlist: synthetic track and action menu render; Right returns to the list. Restored the remaining original list_bg_3.png and rechecked the list after restart: the modern blue panel is gone and the original focus row remains visible.
- TV recordings: populated icon grid and modest focus enlargement observed. Long titles retain native truncation and the intentional focused-caption policy. Pagination and sustained flicker behavior were not conclusively exercised; no flicker-free claim is made. Lower grid captions can meet the viewport edge and remain a cosmetic follow-up.
- Photos: the sharpened night background is visible on BasicHome, including the clearer central city view.
- Settings: enlarged heading, original button styling and top-right clock confirmed after synchronizing the previously stale installed settings.xml with the repository version.
- Test client exited normally via Alt+F4; process was gone and the log recorded Main: Exiting. Main Apollo profile remained selected and its plugin remained enabled.

## Validation and limits

Menu, guide, playback, secondary-layout and Day/Night structural checks passed. Menu-editor submenu regression and the editor build passed during this update. Final payload checksum validation is recorded separately by tools/validate.py. The error log contained only TexturePacker cleanup reporting the deliberately moved Backdrop cache directory before regeneration; this was induced by the test cache reset, not an application crash.

Not tested here: real radio tuning/playback, complete video transport, all nine animated menu pages, full x86 runtime, all locales/DPI/aspect ratios, or a fresh installer. No release was built or uploaded. Backups and the separate test configuration are outside the repository under outputs/Runtime-review-48fc5643.
