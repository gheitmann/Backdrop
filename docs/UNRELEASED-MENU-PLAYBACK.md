# Unreleased menu and playback layout block

Code-only changes after ALPHA21. Internal version remains 1.39.0.21. No package,
installer, deployment, visual session or release is part of this block.

## Menu surfaces

Applied the existing native music-menu treatment to 151 additional rows across
25 pages: Pictures; Videos, Video Titles and Video Playlist; TV and Radio
Recordings; TV Scheduler, Search and Programme; Moving Pictures; TV Series main,
Playlist, Actors and FanArt; OnlineVideos; RadioTime; Last.fm; Lyrics; all three
NowPlaying variants; GlobalSearch Music and Details; ShowTimes and SubCentral.

Each affected button, menu button, sort button or checkbutton has a 42px height,
16px text inset, native vertical centering and the existing dark neutral/focused
row surfaces. Removed 280 independent focus images and per-row focus zooms, which
could cover text/state marks or move the surface independently of its hit area.
Sort indicators fit inside the row with a 12px right inset. Checkmark artwork and
native commands remain intact. Specialized selectbuttons retain their existing
rendering and are outside this block.

## Playback tracks

Thirteen further progress controls now paint their track and fill together using
native only-mid rendering, with matching sizes and zero fill offsets. This covers
common overlays, guide overlays, both guide XML copies, Analog/LED VU NowPlaying
and RadioTime NowPlaying. The former track rectangle determines the geometry.
Nonzero native image IDs are retained as empty texture placeholders. Percentage
properties and visibility rules are preserved; no smoothing or synthetic TV
playback percentage is introduced. The separate live-TV progress issue remains
open.

## Technical verification

- 184/184 scoped menu rows across 28 pages pass checks, including the 33 ALPHA21
  rows. Checks cover frame assets, centering, duplicate property declarations,
  indicator containment and absence of independent focus overlays for those IDs.
- 15/15 scoped playback tracks pass co-location and fill-bound checks, including
  the two previously corrected main NowPlaying controls.
- Compared nonzero control IDs/types, labels, visibility, commands, navigation,
  selected state, subitems and checkmark textures against ALPHA21 across all
  30 changed skin XML files: unchanged.
- Native binding audit: 139 windows, zero incompatible controls; 69 pre-existing
  absent fields remain. Original guide/caption and playback contracts pass.
- Payload integrity validation covers 2405 files and 558 skin XML documents.

Runtime appearance, mouse behavior and playback are deliberately untested in
this code-only block. No claim of visual acceptance or full plugin coverage is
made. Installation files in dist still belong to the published ALPHA21 build.
