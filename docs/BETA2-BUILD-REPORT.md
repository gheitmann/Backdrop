# BETA2 build verification — 29 October 2026

Package/setup version **1.39.0.29**, tag `v1.39.0-beta.2`. The menu editor and optional Day/Night binaries are unchanged from BETA1 (editor assembly 1.39.0.28). No unrelated MediaPortal or third-party binaries are included.

- Eight static checks passed: payload, integrations, guide, playback, menus, secondary layout, Day/Night and BETA1 feedback regressions. Payload validation covers 2,442 mapped files and 560 skin XML definitions.
- Artwork verification passed: 518 decoded control textures and 506 protected original resources. The 76 restored textures have exact archive hashes.
- MPE1 reopened and byte-compared all 2,442/2,442 payload files. Private setup bundle compared 21/21 entries and excludes MediaPortal core binaries.
- An isolated native x64 copy-provider run installed and byte-compared 2,442/2,442 mapped files. A pre-existing personal SkinSettings.xml sentinel was preserved. Font registration was redirected to ordinary test-folder copies and was not tested.
- Final single EXE VerifyOnly passed on the installed x64 client and isolated x86 client (2/2 architectures), without installing. The rebuilt presentation helper passed its readability, buffered filename repaint and native binding probes.
- Scoped live x64 interaction was completed for the unchanged XML fixes before packaging: music/playlist focus navigation, Photos, Settings keyboard captions, radio layout switching and Radio Now Playing empty-data fallback. The final keyboard correction and nonduplicated focused radio icon were confirmed. The client exited normally. See BETA1-FEEDBACK-FOLLOWUP.md for exact scope.
- Existing native x64 Day/Night evidence: 1,420 assertions for 46 bindings in 37 XML files. No plugin or expression changes were made after that probe.
- The Photos night image was edited with the built-in image tool and inspected at its final 1920x1080 JPEG export. The 1672x941 generated master is retained as `Source/Artwork/cinematic-night/photos-beta2.png`; this is not native 4K. The prompt is recorded in `Source/Artwork/cinematic-night/prompts.json`. Other lighting variants are unchanged. No additional full-client run was performed solely for this image replacement.

EXE: **168,790,016 bytes**. SHA256: `39eaefaf0cc6418058f32cae62e0b9fcba1bc7dd266898bab0b049f14fd4d855`.

This candidate has not undergone a fresh end-to-end EXE wizard installation in the main profile. Real radio playback/logos, full x86 interaction, prolonged playback, recordings paging flashes and Live TV progress irregularity remain unverified or open. No blanket compatibility or flicker-free claim is made.

Official upstream context checked: [MediaPortal 1.39 release](https://www.team-mediaportal.com/news/mediaportal-139-release), available for x86 and x64. This does not prove compatibility with every build.
