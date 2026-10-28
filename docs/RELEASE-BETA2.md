# Backdrop BETA2

Version **1.39.0.29**. Download **Backdrop-Setup-BETA2.exe**, close MediaPortal and run the single executable. The skin, standalone menu editor and optional Day/Night plugin are included. Personal menu preferences are preserved. Back up your current installation before updating.

## Changes since BETA1

- Restored 76 original control textures from the supplied Backdrop v1.0.1 media archive, including list states, dialog surfaces, checkbuttons, thumbnail focus frames and paging graphics. Cinematic backgrounds and the startup screen are retained.
- Fixed action-menu highlights remaining on previously focused music/playlist buttons. Nine pages now use native button focus rendering instead of independent animated highlight layers; menu opening and closing animations remain.
- Removed duplicate Settings headings behind the virtual keyboard, with localized standard/SMS keyboard caption handling.
- Corrected the Photos page to use its own morning/noon/day/night backgrounds. Improved the night photograph's central detail and depth of field, particularly the chair, books and city view. Other lighting variants are unchanged.
- Fixed missing Radio Icons/Big Icons caused by the thumbnail mask. Native station-icon fallback now renders once, with captions and original focus artwork.
- Replaced inherited DWHD radio scrollbars with Backdrop paging arrows, corrected paging bounds and stabilized the localized radio heading.
- Restyled Radio Now Playing with existing Backdrop backgrounds, typography, separators, progress textures and channel arrows while retaining native commands.

## Verification and limitations

Scoped local x64 interaction covered BasicHome, music library/playlist focus navigation, Photos, Settings/keyboard, radio view switching and the Radio Now Playing empty-data fallback. Final keyboard captions and nonduplicated focused radio icons were visually confirmed. The client exited normally. Synthetic fixtures remain outside the release payload.

Real radio tuning, station-specific logos, full x86 client interaction, every DPI/locale and prolonged playback are not certified. Recording-grid paging flashes and the previously reported Live TV progress irregularity remain open. No MediaPortal core/audio or third-party experiments are bundled. The optional Day/Night plugin and menu editor binaries are unchanged from BETA1.

See the [build report](BETA2-BUILD-REPORT.md) for package and installer verification, and the [feedback follow-up](BETA1-FEEDBACK-FOLLOWUP.md) for detailed scope.
