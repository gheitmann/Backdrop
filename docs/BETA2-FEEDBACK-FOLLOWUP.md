# BETA2 feedback follow-up

Unreleased source/local follow-up, 30 October 2026. The published BETA2 installer
and its version identity are unchanged.

## Changes

- Repair the isolated building fragments above the central window crossbar in all
  four Photos lighting images. Improve daytime central detail while retaining the
  approved night composition. The image-generation masters and edit provenance
  are in `Source/Artwork/photos-window-repair`. Future cinematic exports use the
  repaired night master.
- Remove the fixed-size frame plates behind aspect-fitted Photos filmstrip
  thumbnails. Keep native aspect ratio, reflections and focus zoom; correct the
  filmstrip right-navigation element.
- Match the Background Lighting title to the Settings font and enlarge the blue
  Artist Info heading.
- Remove the auxiliary pulsing Now Playing images in the three music action
  menus. Preserve the working native Now Playing and video buttons.
- Omit the recent previous/next station arrows from Radio Now Playing, as
  requested. Preserve its programme metadata and progress bindings.
- Add a dark readability panel behind radio list rows only.
- Rebuild both active Radio Guide entry definitions around the current Backdrop
  TV Guide's lower seven-row geometry, native control types, group button,
  clock and programme details. Use radio properties and studio lighting assets.
  Historical unused numbered-row guide definitions were not rewritten.

## Verification

- Eight Python commands passed: payload validation, guide layout, playback layout,
  menu layout, secondary layout, Day/Night contract and the Beta 1/Beta 2 feedback
  regressions. Payload validation covers 2,442 files and 560 skin XML files.
- Native x64 Day/Night probe passed 1,448 assertions across 48 expressions in
  39 XML definitions. The plugin binary is unchanged. Existing log4net probe
  warnings remain distinct from assertion failures.
- Sixteen changed skin files were backed up, copied and hash-compared locally.
  Initial receipt: project outputs `Runtime-review-e267b0b3/deployment.json`.
  Final source-byte deployment receipt: `Runtime-review-8106c5f7/deployment.json`.
- Internal review package `outputs/Beta2-feedback/Backdrop-unreleased-review.mpe1`
  passed byte comparisons for all 2,442 mapped files. SHA256:
  `c0b6315a42c6a25e82b395b937b829ac12befef64a8ed969388b68bc7a75c93c`.
  This diagnostic package is not a published release or replacement installer.
- Scoped x64 live review: the radio guide opened with four synthetic stations;
  moving between rows updated programme title, genre and description. The lower
  guide, group caption and clock were visible. No tuning or recording was tested.
- Photos live review confirmed the repaired night background and a populated
  filmstrip with landscape, portrait and square fixtures, without the oversized
  plates. A transient empty root view required a fresh client and opening the
  selected demo share. The earlier client logged a skin load from the wrong
  thread; this was not counted as a successful check or attributed to the skin
  without evidence.
- The main Apollo profile and personal menu configuration were preserved.
  Synthetic fixtures remain outside the repository and package.

## Limits

The font adjustments, radio list panel and arrow removal passed source checks;
their final live appearance still needs a short confirmation. Radio Guide
single-channel/group switching, live playback, full x86 client behaviour and
the historical recording-icon pagination flash are not certified by this pass.
No MediaPortal core, audio workaround or third-party plugin changes are included.
