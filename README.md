# Backdrop

Experimental port of cul8er's **Backdrop 1.0.1** skin to **MediaPortal 1.39**.

Current version on this branch: **ALPHA13**, menu editor **1.39.0.11**. The editor and installer target AnyCPU. Runtime checks were performed on MediaPortal 1.39 x64; a complete x86 runtime test is still pending.

## Install

Download the `.mpe1` installer from [Releases](https://github.com/gheitmann/Backdrop/releases), close MediaPortal, and install it using the Extension Installer supplied with your MediaPortal installation. Back up any existing Backdrop installation first.

The unreleased integrated installer bundle combines the skin MPE1 with the buffered installer UI: extract the ZIP and double-click `Install Backdrop.cmd`. Initialization and client-folder discovery run automatically. See [installer instructions](docs/INSTALLER-UI.md). This local bundle does not advance the alpha version or replace the installed Extension Installer.

Use architecture-compatible versions of Fanart Handler, Latest Media Handler, and any other external plugins. This repository does not bundle third-party plugin installers or modified MediaPortal core binaries.

## Included changes

- Rebuilt AnyCPU menu editor with optional plugin adapters.
- Updated settings pages with Backdrop textures, fonts, and vertical navigation.
- Original lower TV guide layout, upper-left description and upper-right preview/clock, with current native bindings.
- Modern radio guide controls styled with Backdrop assets.
- Consistent standard, analog VU, and LED VU music Now Playing pages.
- Corrected lower-menu mouse hit areas and submenu wake-up behavior.
- Enabled the topbar on regular pages and fixed vertical side-menu captions.
- Anchored dialog titles inside their frames and TV guide group captions at the lower left.
- Replaced visible `#scroll.wrap` placeholders with a static separator.
- Removed unused DWHD-specific Now Playing design and classic-home settings.
- Corrected the previous-track topbar icon, guide time-header artwork, and group-button textures.
- Restored artist/album information using current MediaPortal properties, including biography/discography and review/track-list toggles.
- Fixed off-screen playlist focus, game overlay collisions, and movie-grid title overlap.
- Added locale/theme/topbar auditing to validation and CI.
- Fixed paused-track Play/Resume and thumbnail/filmstrip focus-zoom edge clipping.
- Removed inherited guide row-count captions and General settings shortcut/drive-space panels.
- Added direct hit-tested topbar mouse gestures in the AnyCPU plugin.
- Shortened artist/album toggle captions to the current target view.
- Added an optional, separate installer presentation helper for readable buttons and reduced repainting.
- Added click and drag seeking to the native music Now Playing progress bar.

See [validation and limitations](docs/VALIDATION.md) and [maintenance guidance](docs/MAINTENANCE.md). The existing checklist remains 36/37 passed (97.3%) and one blocked, carried forward from ALPHA9. The separate [V6 feedback review](docs/V6-FEEDBACK-REVIEW.md) records new settings/guide checks and user-confirmed TuneIn operation. New targeted topbar evidence and remaining input limits are documented. See the optional [installer UI helper](docs/INSTALLER-UI.md); the ordinary MPE1 does not change the installed Extension Installer. The original-TVGuide checklist remains 8/9 passed (88.9%), carried forward, with active Live TV preview blocked by missing test tuners. New caption checks pass 3/3; exact scope is recorded in validation. This is **not** an overall compatibility percentage.

## Repository layout

| Path | Contents |
| --- | --- |
| `skin/Backdrop/` | Complete installable skin, including images, sounds, and translations |
| `Source/` | Menu editor source, build script, and 1.39.0.11 reference DLL |
| `fonts/` | Fonts carried over from the original installer |
| `config/` | Default menu editor configuration |
| `packaging/` | Portable installer template and payload checksums |
| `tools/` | Validation and installer generation |

Personal `SkinSettings.xml`, caches, databases, local media fixtures, credentials, and machine-specific project settings are excluded. Plugin integration files named `LatestMediaHandler.SkinSettings.xml` and `TVSeries.SkinSettings.xml` are intentional skin definitions.

## Build

Build the editor with **Windows PowerShell 5.1**, .NET Framework 4, and an installed MediaPortal 1.39:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File Source/build.ps1 -MediaPortalDirectory 'C:\Program Files\Team MediaPortal\MediaPortal'
python tools/validate.py --refresh
python tools/validate.py
python tools/package.py
```

Review the rebuilt DLL and assembly version before refreshing the payload hashes. The refresh records an intentional payload change; it is not a compatibility test. To package the unchanged checked-in reference DLL, skip the editor build and hash refresh.

The installer is written to `dist/Backdrop-MP139-AnyCPU-ALPHA13.mpe1`. Packaging can also use the checked-in reference DLL. GitHub Actions validates the payload and builds an installer artifact; it does not run the MediaPortal GUI or compile the editor against MediaPortal binaries.

## Credits

Original Backdrop skin and editor: **cul8er**. Original project: [Google Code archive](https://code.google.com/archive/p/backdrop/). Modern fallback pages and selected resources originate from MediaPortal's DefaultWideHD skin. Existing attribution and third-party permission files are retained; see [NOTICE.md](NOTICE.md).
