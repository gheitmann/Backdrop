# Backdrop ALPHA7 for MediaPortal 1.39

ALPHA7 fixes a music playlist that slid out of view on focus, clears music overlays from Sudoku/Tetris, widens Tetris buttons, moves the Moving Pictures thumbnail grid below its heading, and adds a short localized EPG-row caption. The topbar now uses general Play/Forward/Rewind commands and removes duplicate action entries. The AnyCPU menu editor remains 1.39.0.5.

The expanded x64 regression has **32/37 passed (86.5%), four partial and one blocked**. Moving Pictures details/rating, TVSeries season/episode information, OnlineVideos categories, games, five representative locales and the installed default theme were exercised. This is a specific checklist, not an overall compatibility percentage.

Known limits: immediate mouse focus/click timing, playlist Play restarting rather than resuming, unconfirmed right-click and progress dragging, and first-cover zoom clipping in the movie grid. TVSeries online rating cannot be tested with the offline fixture. TuneIn freeze, populated EPG, ARD HLS playback and full x86 runtime remain unverified/unresolved. Additional themes are not shipped; 37 language files do not imply 37 complete translations.

See [validation and full evidence](VALIDATION.md). Close MediaPortal and back up an existing skin before installing. Personal settings, databases, fixtures, caches and modified core/plugin binaries are excluded.
