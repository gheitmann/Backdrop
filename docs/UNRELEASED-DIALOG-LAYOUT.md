# Unreleased dialog and secondary-screen layout block

Third code-only block after ALPHA21, following bc46b7d. No version bump, package,
installation, application launch, visual inspection or publication.

## Changes

- 52 native button definitions across 15 dialogs and six older Settings pages
  now use vertical text centering and zero vertical caption offset. Dialog action
  captions are horizontally centered without a residual X offset. Settings
  buttons retain a 16px inset and their original geometry.
- 19 dialog buttons previously used button.focus.png for both states. Their
  neutral state now uses the existing dark hidden-menu frame, making keyboard
  focus distinguishable. Existing focused artwork remains in place. No new
  graphics or images were generated.
- The File confirmation's Yes button now shares the 150x52 frame used by the
  other four actions, replacing its inconsistent 210x75 rectangle.
- MusicMenu's five 336px tiles fit within 1920x1080 with equal 80px outer margins
  and 20px gaps. Icons and caption anchors follow each tile's center. Previously
  the last Radio tile ended at X=1978, outside the 1920px canvas; neighboring
  tiles also touched/overlapped. Native links and left/right navigation remain.

Scope includes Album/Artist information, checklist, file confirmation,
incompatible-plugin and old-skin notices, notifications, OK, Play/Stop, progress,
shared drives, TV recording conflict and Yes/No dialogs. Settings covers legacy
Music, Screen, Skip Steps, Movies and the two EPG pages. Existing offscreen native
placeholder controls are preserved; the 52 count includes such definitions and
is not a count of simultaneously visible buttons.

## Technical verification

- 52/52 scoped dialog/settings button definitions pass alignment checks;
  19/19 scoped focus-state asset pairs differ.
- Five music tile hit regions remain within the canvas and do not intersect;
  their icons are contained and horizontally centered with their captions.
  Left/right navigation forms a closed five-item cycle.
- All five File dialog actions have matching frame dimensions.
- Nonzero native control identities/attributes, labels, visibility, commands,
  navigation, subitems and selected values compared against bc46b7d across all
  22 changed skin XML files: unchanged. Reference comments are retained.
- Existing 265 menu rows, 10 selector rows, 15 playback tracks and original
  guide/caption layout contracts pass.
- Native audit: 139 windows, zero incompatible controls; the same 69 pre-existing
  absent fields remain. Payload validation: 2405 files and 558 skin XMLs.
- Integration audit: 37 translation files, no missing English fallbacks,
  11/11 static topbar commands pass.

The new secondary-layout check is part of the validation workflow for future
runs. No CI run or new release is triggered by this local-only commit. Runtime
appearance, mouse operation and playback remain untested. The known playback
issues are not resolved by this structural layout block.
