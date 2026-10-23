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

The skin settings page omits the DefaultWideHD BasicHome editor and the ineffective EPG row selector. Backdrop has its own external editor. The TV guide uses the original lower seven-row layout from ALPHA12; the radio guide retains its prior layout.

## ALPHA6 lessons

- Music artist/album dialogs now receive `#ArtistInfo.*` and `#AlbumInfo.*` properties. Legacy label IDs 20-27 and empty textbox/image controls no longer receive values automatically. Bind every value, image, biography/review, and discography/track-list field explicitly. Button 6 remains the native refresh binding; button 5 toggles the skin's alternate content.
- Keep the existing Backdrop dialog graphics. Use `textboxscrollup` for the property-driven text and leave enough panel height for the body. Preserve aspect ratio for available artwork.
- Now Playing settings should expose the native lookup switch and None/Analog/Led selector. The imported DWHD LED designs and spectrum options have no consumer in Backdrop's three active Now Playing pages. The copied, unused `mp139.MyMusicPlayingNowLedVU.xml` is not evidence that these options work in the active layout.
- Classic Home layout and DWHD-specific thumbnail/fanart controls must not be exposed merely because they appear in an imported settings page. Check the actual rendering path before retaining each option. Native MediaPortal configuration and working settings-page help are separate from these skin-only choices.
- Previous-track action 15 (topbar control 22) uses `top_skipback_*`, while navigation Back uses `top_back_*`.
- Guide time headers use `tvguide_time_bg.png` with Backdrop typography. Keep the required `button3part` control 100; use `tv_group_bg_focus.png` and `tv_group_bg.png` in its middle textures with no DWHD overlay. Do not replace the bound control type with the old plain button. The group state still needs a populated EPG check.
- TuneIn/RadioTime is not installed in the isolated runtime. The later V6 user report and screenshots confirm playback, search and virtual keyboard operation; the earlier freeze is not currently reproduced by the owner. No local runtime certification or binary fix is claimed. If a freeze returns, obtain the actual plugin version and log/thread evidence before changing binaries. Keep plugin repairs separate from the skin package.
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

## ALPHA10 lessons

- Apply the V6 feedback cleanup before future fallback-page updates: remove guide row-count captions in all 20 TV/radio variants and the static Skin settings caption in both definitions.
- Do not reintroduce DWHD Keyboard Shortcuts or DriveFreeSpace panels into General settings. Keep native MP buttons 2–7, remove obsolete visibility conditions, and wrap focus 2 Up to 7 / 7 Down to 2. Saved flags may remain in personal settings and must not hide native options.
- Keep current MP 1.39 category navigation vertical; restoring the entire old settings_general.xml would discard current native settings navigation.
- Distinguish a second full-screen background from Backdrop's original sofa image fading to black. The populated TV guide check did not reproduce a second image; retain this as open rather than claiming a speculative fix.

## ALPHA11 lessons

- Preserve PointerTopbar installation/removal alongside PointerSeek. Handle only the native visible enabled topbar, capture down/up to avoid duplicate activation, keep modal/fullscreen/skin/form guards and preserve native dynamic ActionID. Do not certify speed controls from an activation log alone. Native cursor/bar auto-hide can consume wake-up clicks.
- Use mutually exclusive translated label controls for music toggle targets. A skin.hassetting expression in a button label remained cached after clicking; visibility conditions update correctly. Preserve bound button IDs 5 and 6.
- Installer repainting and white-on-light buttons originate in MpeCore, not skin XML. Keep the optional helper separate, version-guarded and local-copy-only. Do not ship modified MediaPortal DLLs in the helper ZIP or claim synthetic progress events prove a real install. Retest assembly binding for each supported native version.

## Original-layout reference after ALPHA11 feedback

- Before the next guide update, read ORIGINAL-TVGUIDE-LAYOUT.md and its JSON anchors. The original guide is in the lower screen area (panel Y=612); description Y=225 and preview X=1537/Y=162 are above it. The original clock uses a right anchor X=1912/Y=6. Do not preserve the active DWHD 12-row template geometry as Backdrop's final layout.
- Original mytvguide.xml byte-matches the supplied MPE1. Its coordinate canvas is already 1920 x 1080; no second 1.5 scaling. Resolve imports and row-relative coordinates, and preserve modern required bindings such as group button3part 100.

## ALPHA12 lessons

- Enforce ORIGINAL-TVGUIDE-LAYOUT.json anchors with tools/check_guide_layout.py. Keep the two active TV guide definitions identical; do not reintroduce mp139.mytvguide.12lines.xml as their template.
- Preserve the original 1920 x 1080 canvas without scaling coordinates again. Current control bindings still take precedence over legacy control types: group 100 is button3part, preview is videowindow 99 and keyed-channel label 34 remains present.
- Inline the preview overlay exactly once. Retain common.time.xml for the original right-aligned clock. Channel label width, header logo position and clipped timeline are documented compatibility adjustments.
- Seven visible rows require pagination testing. Retained fixtures exercised selection, long descriptions, keyboard pagination, group switching and single-channel mode. Actual Live TV remains unverified without a tuner mapping.

## ALPHA13 lessons

- Rotation `center` is an absolute skin-coordinate pivot. A zero component invokes the native automatic centre; it is not a local zero offset. Use an explicit nonzero lower-left anchor for bottom-to-top captions, and keep left text alignment.
- Account for native scaling before rotation: horizontal text width becomes vertical extent, so ultrawide output needs a shorter bounded viewport. check_guide_layout.py verifies containment at 1920 x 1080 and 3440 x 1440.
- Keep guide group captions inside X=7..59/Y=612..1060. Active definitions use X=12/Y=1050 and a 320 x 34 viewport; long names scroll within it. Do not use the old centred 417-pixel label or mixed pivot 35,0.
- Shared dialog corrections retain IDs and actions. DialogMenu title 4 is also used by music Actions; verify the actual runtime route when testing an equivalent context menu.
