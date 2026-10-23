# Unreleased TV guide background correction

Status: local working branch only. The owner requested that the next alpha be held. No version bump, new installer, release, tag or pull request is created for this change.

The quarter-screen room was embedded in the original `Media/background.tvguide.png`, rather than caused by reduced XML width or height. Both active TV guide definitions now reuse the existing complete 1920 x 1080 room photograph `Media/default.png`, drawn at 0,0 with width 1920 and height 1080. Diffuse colour `ff505050` darkens the photograph without adding another full-screen layer. The artwork itself is unchanged; its SHA256 is `928782712ca22fcb1072ffadd05e6e2339994bdee30d65d344853fbfcbad07ae`.

All nine original guide anchors, lower grid, native preview 99 and upper-right clock remain unchanged. Radio guide definitions and the AnyCPU editor are unchanged. The original background resource is retained for other pages that reference it.

## Targeted runtime checks

Isolated MediaPortal 1.39 x64, 23 October 2026, fullscreen 3440 x 1440, retained fictional EPG data: **3/3 passed (100%)**.

1. The complete room fills the guide canvas, including the right wall, couch and lower floor; no upper-left quarter cutoff remains.
2. Programme title, time, genre, wrapped description, clock and programme grid remain readable. Down changes the selected channel and updates the programme time/details without tuning.
3. F9 opens the populated native guide context menu. Its text and vertical title remain readable over the room; the menu is dismissed without invoking tuning, recording, deletion or metadata commands.

These are three bounded visual checks, not a full compatibility score. Actual Live TV playback was not exercised locally because the fictional channels have no tuner mappings. The owner's earlier confirmation of working upper-right Live TV and clickable channel changes remains separate evidence. Paused/uneven Live TV progress is explicitly deferred. Other historical regression results are not newly rerun.

The isolated test application was closed after inspection and its startup page restored. Normal installed skin files were not overwritten.

Before later publication, assign the next revision, update release documentation, rerun packaging/native metadata verification and obtain fresh active-playback background evidence if a tuner is available.
