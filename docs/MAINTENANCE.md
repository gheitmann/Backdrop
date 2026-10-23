# Maintenance

Use this repository as the versioned source for future Backdrop changes. Keep repository content, commit messages, release notes, and reports in English. Chat replies to the owner remain German.

## Start every update here

1. Read this file and `VALIDATION.md`, inspect the current branch and remote changes, then check the target MediaPortal release and matching plugin architecture.
2. Preserve native required control types and IDs. Restyle compatible modern pages instead of restoring incompatible legacy XML wholesale.
3. Keep the lower BasicHome category hit areas aligned with visible captions: Y=966, height=76. Preserve the matching editor generator and mouse-move wake-up changes.
4. Keep the fixed conditional 90-degree side-caption rotation; visibility-only animations did not retain the rotation.
5. Preserve the shared Backdrop music layout in all three Now Playing variants. VU controls use IDs 999 and 998; cover 112, progress 118, and caption 1 are required bindings.
6. Never ship the personal root `SkinSettings.xml` or its backup. Retain plugin integration definitions whose longer names end in `.SkinSettings.xml`.
7. Keep regular-page topbar changes selective. Dialog, fullscreen, OSD, calibration, and screensaver behavior needs separate review.
8. Preserve fixed separators instead of unsupported `#scroll.wrap` tokens.

The skin settings page omits the DefaultWideHD BasicHome editor and the ineffective EPG row selector. Backdrop has its own external editor and the current guide uses 12 rows.

## ALPHA6 lessons

- Music artist/album dialogs now receive `#ArtistInfo.*` and `#AlbumInfo.*` properties. Legacy label IDs 20-27 and empty textbox/image controls no longer receive values automatically. Bind every value, image, biography/review, and discography/track-list field explicitly. Button 6 remains the native refresh binding; button 5 toggles the skin's alternate content.
- Keep the existing Backdrop dialog graphics. Use `textboxscrollup` for the property-driven text and leave enough panel height for the body. Preserve aspect ratio for available artwork.
- Now Playing settings should expose the native lookup switch and None/Analog/Led selector. The imported DWHD LED designs and spectrum options have no consumer in Backdrop's three active Now Playing pages. The copied, unused `mp139.MyMusicPlayingNowLedVU.xml` is not evidence that these options work in the active layout.
- Classic Home layout and DWHD-specific thumbnail/fanart controls must not be exposed merely because they appear in an imported settings page. Check the actual rendering path before retaining each option. Native MediaPortal configuration and working settings-page help are separate from these skin-only choices.
- Previous-track action 15 (topbar control 22) uses `top_skipback_*`, while navigation Back uses `top_back_*`.
- Guide time headers use `tvguide_time_bg.png` with Backdrop typography. Keep the required `button3part` control 100; use `tv_group_bg_focus.png` and `tv_group_bg.png` in its middle textures with no DWHD overlay. Do not replace the bound control type with the old plain button. The group state still needs a populated EPG check.
- TuneIn/RadioTime is not installed in the isolated runtime. Its legacy source has synchronous network/player work, but that alone does not identify the owner's freeze. Obtain the actual plugin version, reproduction details, and log/thread evidence before changing its binaries or claiming a fix. Keep any plugin repair separate from the skin package.
- Persistent artist/album info fixtures are separate from immutable v1 media. The artist-only MP3 intentionally omits its album tag to exercise the F9 artist-information branch; ordinary album-tagged songs open album information first. Album track metadata uses `number@title@seconds|` format.

## Validation and release process

- Check local and remote changes before synchronizing; never overwrite unrelated work or force-push the default branch.
- Rebuild the AnyCPU editor only when its source changes and matching MediaPortal references are available. Review the resulting assembly flags and version.
- Refresh `packaging/baseline-sha256.json` intentionally after reviewing payload changes (`python tools/validate.py --refresh`). Refreshing hashes is not proof of compatibility.
- Run `python tools/validate.py` and `python tools/package.py`, then inspect the installer payload.
- Test affected screens in the authorized isolated MediaPortal instance. Retain reusable synthetic fixtures outside the repository; do not import personal media or databases.
- Record verified results separately from untested assumptions. Percentages must state the exact denominator. Do not silently reuse historical compatibility scores.
- Commit reviewed changes and publish a clearly versioned prerelease when appropriate. Keep significant behavior changes reviewable in a branch or pull request.

The recurring Codex check reviews official MediaPortal and relevant plugin releases plus local Backdrop changes weekly. It should stay quiet when nothing actionable has changed. New upstream versions do not by themselves establish compatibility or justify blind file replacement.

Local maintenance history and immutable fixtures remain under the owner's `Documents/Codex/Testmedien/MediaPortal` directory. Local core BASS, radio, and Latest Media Handler experiments are separate projects and must not be silently bundled into Backdrop.


## ALPHA7 lessons

- Remove the playlist facade's opposing 950-pixel focus/unfocus slides; they pushed its loaded rows off the right edge while focused.
- Shared music overlays must not cover Sudoku/Tetris gameplay. Preserve the games' native controls; widen translated buttons where necessary.
- Reserve the title area above Moving Pictures thumbnails. Its first selected cover still zooms beyond the left edge; treat that as a remaining layout limitation.
- Use general topbar actions 68/16/17 for Play/Forward/Rewind. The music-specific 77/78 depend on BASS skip-step configuration. Preserve required IDs; remove duplicate action tags. Playlist Play still restarts the track, so do not claim resume is fixed.
- Distinguish a button's focused command from immediate mouse hit testing. Native first-click cursor gating and previous-focus activation affected automation; report this rather than counting all mouse commands as passed. Keep progress control 118's bound type.
- Parse all 37 translations, verify English property fallback, and report untranslated values. Representative Latin/Cyrillic/CJK rendering is not complete translation certification. Only the default theme is present.
- For language changes, investigate stale generated font caches before changing font ranges or rebuilding broad Unicode atlases.
- Moving Pictures synthetic selected cover paths must also be listed in pipe-delimited AlternateCovers or the plugin clears them. Fixture repairs belong in the isolated database, never the skin package.
- Direct plugin startup can bypass the menu editor's window activation timer. Test TVSeries delayed info after real Home-to-plugin navigation with the configured idle delay; offline ID -1 does not expose online rating.
- Retain versioned input fixtures outside immutable v1 and keep exact case-level evidence. Run `python tools/audit_integrations.py` with payload validation before packaging.


## ALPHA8 lessons

- GUIButtonControl action IDs accept property expressions. Use the native `#playlogo` paused state to select action 12 for Resume and 68 otherwise; this avoids GUIMusicPlaylist.OnNewAction restarting the selected song while resuming. The x64 runtime confirmed resolved action 12 and continued position at 1:28, 1:30 and 1:48.
- Thumbnail focus zoom needs horizontal margins as well as title clearance: X=60, width=1800 keeps the first cover visible in normal and large grids. The separate filmstrip still has edge clipping; do not claim every layout is fixed.
- Right-click context menus work with a visible cursor. Native MouseClickEvent suppresses the first click after auto-hide; temporarily disabling autohidemouse isolates that behavior. Restore the test setting and keep it out of the installer. Immediate topbar focus/click timing remains separately unverified.
- Required progress control 118 is GUIProgressControl, whose CanFocus returns false. Skin geometry cannot add drag seeking; preserve the binding and treat interactive seek as separate functionality.
- Follow-up regression counts must distinguish newly assessed cases from inherited passes. ALPHA8 revisits four cases and carries forward unchanged ALPHA7 evidence.

## ALPHA9 lessons
- Keep GUIProgressControl 118 bound to Now Playing. Interactive seeking lives in PointerSeek.cs in the existing AnyCPU plugin, with a WinForms IMessageFilter installed/removed on the app thread. Exclude routed dialogs, other skins/pages, radio and non-seekable playback. Preserve the exception boundary and end-of-track margin.
- Use tools/test-pointer-seek.ps1 for the twelve pointer geometry checks. Runtime click and forward/backward drag still need a real local music fixture; geometry alone is not input validation.
- Filmstrip X=60/width=1800 reserves first-cover zoom space. Correct onright spelling; coverflow must be checked separately. Three-cover fixtures confirmed both layouts and selection changes.
- Refresh retained v4-epg fixture dates before future guide testing. TV and radio use separate group tables; ChannelNavigator requires the standard radio All Channels group even for TV. See EPG-FIXTURES.md.
- The 37-case follow-up reaches 36 passed, one offline-rating blocker; topbar immediate pointer timing remains explicitly limited despite earlier functional command passes.
