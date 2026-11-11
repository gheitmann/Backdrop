# Video dialog and facade follow-up

This source follow-up addresses the tester's screenshots 40–43 after release 1.39.0.35. It is not a new release.

- Restored the original Backdrop `DialogVideoInfo.xml` and `DialogVideoArtistInfo.xml` layouts from the archived v1.0.3 reference. The actor dialog uses two existing Backdrop textures in place of two references absent from the current media payload (`overlay.default.solid.png` and `movieinfo_logo.png`). Window IDs and native video controls remain present.
- Shifted the TVSeries icon-view separator 24 skin pixels left, giving the series banner a visible gap.
- Restricted the Moving Pictures `new_icon.png` overlay to non-icon views, where its accompanying cover is shown. The prior visibility condition allowed it to appear alone below the icon grid.
- The tester's Photos video entries were retained in the editor settings and reappeared after saving. The package currently installs `BasicHome.xml` with `AlwaysOverwrite`; this explains why an editor-generated menu can be replaced during an update. Preserving or regenerating a customized BasicHome during installation remains separate work.

Validation: all 2,444 mapped payload files match their SHA-256 manifest entries and all 561 skin XML files parse. The restored dialogs passed literal texture/import checks; the changed files passed `git diff --check`. The dialog behavior, TVSeries banner gap, Moving Pictures view switching, and upgrade preservation have not yet been tested in a live MediaPortal client.
