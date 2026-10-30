# Cinematic night artwork — local work after ALPHA23

The approved cinema-at-night concept is now the startup and Movies background.
TV, Music/Radio, Photos, Games/Sports and Plugins use matching warm, photographic
night scenes. Category focus routing is retained. Generic fallback uses cinema;
selected music/movie/sport and Latest Media fallbacks now use their own theme.
User-supplied fanart remains available above these fallback layers.

The startup uses a clean photo and a code-native SVG Backdrop mark/wordmark,
composited for MediaPortal's single splash-image contract. The photographic
masters, logo and prompts remain separate sources. The actual native startup
status, version and edition labels are explicitly placed at bottom right.
No fake percentage or fixed fill is embedded: native FullScreenSplashScreen
exposes status text, not a skin-controlled numeric progress value.

Default exports are 1920x1080. Local 3440x1440 exports cover-crop photographs and
rasterize branding at that exact target size, avoiding the native WinForms splash
PictureBox stretching a 16:9 logo across an ultrawide monitor. A different target
aspect ratio requires re-export. Original image_gen photographs are 1672x941;
exports above that size are resampled, not native 4K photography.

Build: set NODE_PATH to a Node modules folder containing sharp, then run
`node tools/build-cinematic-artwork.cjs`. Optional `--width`, `--height` and
`--output` parameters create a matching local Media directory without changing
repo masters. Default source and export hashes are tracked. Old mixed photo/icon
fetch commands now stop before replacing the selected collection.

This is local artwork work, not a new Alpha or a Defender workaround. No installer
or MediaPortal core binary is changed. The ALPHA23 Defender investigation remains
separate. A full live MediaPortal acceptance run has not yet been repeated here.

Local deployment: 14 files copied with pre-change backups and post-copy SHA256
verification to the installed Backdrop skin. Backup directory:
C:\Users\49152\Documents\Codex\Testmedien\MediaPortal\v5-demo\backups\cinematic-night-20261025-132020
Installed image exports match 3440x1440. No MediaPortal process was running during
deployment. Local Core.dll and the published ALPHA23 EXE retained their prior hashes.

Verification: 6/6 existing repository check commands passed (100% of that static
scope). Six source/export hashes, six category bindings, a single splash image
and three native status-label bounds/alignment checks also passed. Generated and
composited images were inspected as artifacts; this does not certify the live
startup or a complete runtime UI pass. No new release was built or published.


## Live acceptance, 25 October 2026

All six category backgrounds were observed in the running x64 client at
3440x1440: Photos, Music/Radio, Movies, TV, Games/Sports and Plugins (6/6).
Left/right navigation changes the associated photograph and keeps the six
category labels readable across the bottom. These percentages refer only to
this six-category check, not overall skin or MediaPortal compatibility.
The ordinary Home window had been selected by saved local startup state;
the local configuration now starts BasicHome (window 35) and uses one Home.
BasicHome.xml remains byte-identical to ALPHA22, with the original horizontal
coordinates. Personal MediaPortal.xml is not part of the release payload.
The cinematic splash artifact/native label contract passed source inspection;
a live startup splash screenshot is still pending. No full runtime regression
pass or complete x86 runtime pass is claimed.
