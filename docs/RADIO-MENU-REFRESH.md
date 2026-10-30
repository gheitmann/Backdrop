# Radio presentation and menu animation refresh

Unreleased follow-up to ALPHA27, explicitly requested by the owner.

## Changes

- Radio and Radio Search use existing four-period studio backgrounds, Backdrop typography, original list textures, a centered dark action menu and localized vertical MENU caption. Native IDs, commands and search selector are retained. The radio facade is local to myradio.xml; its inherited focus-dependent visibility was removed so the focused list remains visible. Radio Search has an explicit localized heading.
- Added 76 independent 160 ms focus-background animations across nine pages: music files/library/playlist, photos, video files/library, TV recordings, radio and radio search. Background images are siblings drawn before the native StackLayout. Buttons, text, checkmarks and pointer hit areas remain stationary. Optional rows retain their native visibility. No exit fade or grid slide animation was added.
- Runtime review rejected the first nested stack-cell implementation: MediaPortal did not render its buttons correctly. Replaced it with sibling images and checked radio/search/playlist menu navigation in the live client.
- Restored original list_bg_3.png after the live playlist check exposed a remaining modern blue panel; added its original hash and dimensions to the native fidelity manifest.
- The menu helper now checks the active GUI skin instead of the main configuration file, allowing Backdrop submenus when launched with an alternate test profile.

## Verification

- Static menu check: 265 rows across 48 pages, 10 native selection rows and 76 independent animated focus backgrounds passed. The earlier binding comparison retained 100/100 pre-change binding records.
- Day/Night check: 30 artwork records, 44 bindings in 35 XML files, 37 languages x 18 entries passed.
- Guide, playback and secondary layout checks passed. Payload validation covers 2442 files and 560 skin XML files.
- Live scoped review: see RUNTIME-REVIEW-POST-ALPHA27.md. This is not a released Alpha; no new package or compatibility claim was published.
