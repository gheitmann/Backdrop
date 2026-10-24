# Unreleased remaining plugin menu block

Second code-only block after ALPHA21, following c2838df. Internal version is
unchanged (1.39.0.21). No deployment, visual session, packaging or new Alpha.

## Changes

- 81 additional normal menu rows receive the existing dark native frames, text
  inset and vertical centering. Coverage includes 4TR programme/search/recording
  pages, InfoService feeds/Twitter, MusicVids, OnlineVideos Updater, RSS,
  Trailers, Rockstar, Sudoku, Tetris, Burner, SleepControl, Wikipedia and the
  common Settings navigation. Horizontal menus retain their widths, positions,
  visibility and native routing. Settings retains its 52px row height.
- Removed 110 independent focus images. Native control surfaces now draw beneath
  their labels and state marks.
- Ten native selection fields in GlobalSearch Music, OnlineVideos, OnlineVideos
  Updater and TV Search have framed inactive/selection states. Caption offsets
  agree between states, and focused/unfocused arrows use the same 20px size.
  Subitems, selected values, control types and handlers are retained.
- Sudoku's eight fixed-position rows have a 40px vertical pitch. Their frames
  are now 38px high, giving a 2px gap and preventing intersecting hit regions.

Selection controls render a separate background and separate caption offsets
while focused; they do not implement the button's textvalign field. Their layout
therefore uses the native selector fields rather than a button conversion.
Reference: [MediaPortal 1.39 GUISelectButtonControl](https://github.com/MediaPortal/MediaPortal-1/blob/Release_1.39.0/mediaportal/Core/guilib/GUISelectButtonControl.cs).

## Technical verification

- 265/265 normal menu rows across 48 pages pass the expanded checks (including
  previous work), plus 10/10 scoped native selector rows. The Sudoku hit regions
  pass a non-intersection check. Frame assets and sort indicator bounds pass.
- Compared native nonzero control identities/attributes, labels, commands,
  visibility, navigation, subitems, selected values and checkmark textures against
  the preceding commit across all 23 changed XML files: unchanged. XML reference
  comments are retained.
- All 15 playback-track checks and the original guide/caption checks still pass.
- Native binding audit: 139 windows; zero incompatible controls. The same 69
  pre-existing absent fields remain.
- Payload integrity: 2405 files and 558 skin XML files. Integration audit: 37
  translation files, no missing English fallback properties, 11/11 static topbar
  commands pass.

These are source and structural checks. Runtime rendering, selector operation,
mouse behavior and optional plugin execution remain untested in this block.
The previously reported playback issues remain open. Existing dist files still
belong to the published ALPHA21 release.
