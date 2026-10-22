# Backdrop ALPHA6 for MediaPortal 1.39

ALPHA6 addresses the next set of settings, topbar, guide, and music-information regressions. The AnyCPU editor remains version 1.39.0.5; no editor or MediaPortal core binary changes are included.

- Removed unused DWHD LED-design/spectrum options from the two Now Playing settings XMLs. Native lookup and None/Analog/Led controls remain.
- Hid unsupported classic-home/thumbnail/fanart choices from the skin settings pages and kept keyboard navigation among the remaining native choices.
- Changed topbar previous-track control 22 to Backdrop's existing skip-back artwork.
- Restored original Backdrop time-header and channel-group textures in guide variants while retaining modern control types and geometry. Program descriptions remain unchanged.
- Bound artist and album dialogs to current `#ArtistInfo.*` and `#AlbumInfo.*` properties. Added working biography/albums and review/tracks toggles; retained Backdrop dialog styling and native refresh control.

Runtime verification: settings, topbar artwork, and the music-information area verified on MP 1.39 x64; guide time headers inspected without channels. The populated channel-group state, full x86 runtime, and TuneIn hang remain unverified. See [VALIDATION.md](VALIDATION.md) for the exact scope.

Close MediaPortal before installing the `.mpe1` file through its Extension Installer. External plugins must match the MediaPortal process architecture. Personal skin settings, test media, core/radio experiments, and third-party plugin packages are not included.
