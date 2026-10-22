# Maintenance

Use this repository as the versioned source for future Backdrop changes. Keep repository content, commit messages, release notes, and reports in English. Chat replies to the owner remain German.

## Start every update here

1. Read this file and `VALIDATION.md`, inspect the current branch and remote changes, then check the target MediaPortal release and matching plugin architecture.
2. Preserve native required control types and IDs. Restyle compatible modern pages instead of restoring incompatible legacy XML wholesale.
3. Keep the lower BasicHome category hit areas aligned with visible captions: Y=966, height=76. Preserve the matching editor generator and mouse-move wake-up changes.
4. Keep the fixed conditional 90-degree side-caption rotation; visibility-only animations did not retain the rotation.
5. Preserve the shared Backdrop music layout in all three Now Playing variants. VU controls use IDs 999 and 998; cover 112, progress 118, and caption 1 are required bindings.
6. Never ship the personal root `SkinSettings.xml` or its backup. Retain plugin integration definitions whose longer names end in `.SkinSettings.xml`.
7. Keep regular-page topbar changes selective. Dialog, fullscreen, OSD, calibration, and screensaver behavior needs separate review.
8. Preserve fixed separators instead of unsupported `#scroll.wrap` tokens.

The skin settings page omits the DefaultWideHD BasicHome editor and the ineffective EPG row selector. Backdrop has its own external editor and the current guide uses 12 rows.

## Validation and release process

- Check local and remote changes before synchronizing; never overwrite unrelated work or force-push the default branch.
- Rebuild the AnyCPU editor only when its source changes and matching MediaPortal references are available. Review the resulting assembly flags and version.
- Refresh `packaging/baseline-sha256.json` intentionally after reviewing payload changes (`python tools/validate.py --refresh`). Refreshing hashes is not proof of compatibility.
- Run `python tools/validate.py` and `python tools/package.py`, then inspect the installer payload.
- Test affected screens in the authorized isolated MediaPortal instance. Retain reusable synthetic fixtures outside the repository; do not import personal media or databases.
- Record verified results separately from untested assumptions. Percentages must state the exact denominator. Do not silently reuse historical compatibility scores.
- Commit reviewed changes and publish a clearly versioned prerelease when appropriate. Keep significant behavior changes reviewable in a branch or pull request.

The recurring Codex check reviews official MediaPortal and relevant plugin releases plus local Backdrop changes weekly. It should stay quiet when nothing actionable has changed. New upstream versions do not by themselves establish compatibility or justify blind file replacement.

Local maintenance history and immutable fixtures remain under the owner's `Documents/Codex/Testmedien/MediaPortal` directory. Local core BASS, radio, and Latest Media Handler experiments are separate projects and must not be silently bundled into Backdrop.
