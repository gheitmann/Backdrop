# ALPHA25 build verification - 25 October 2026

- Editor DLL and standalone EXE rebuilt, including the embedded x86 worker.
- Installer helper, architecture launchers and presentation probes rebuilt.
- Six static commands passed: validate.py, audit_integrations.py,
  check_guide_layout.py, check_playback_layout.py, check_menu_layout.py and
  check_secondary_layout.py (6/6, 100% of this command set).
- Package re-read and byte-compared: 2405 payload files, 558 skin XML files.
- Private bundle re-read and byte-compared: 21 entries; no MediaPortal core
  binaries redistributed.
- Final single EXE preparation: x64 and official extracted x86 client both exit
  0 (2/2). Architecture selection and native plugin destinations are correct.
  Both presentation probes pass 3/3 readable buttons, opaque filename repaint,
  buffering and 2393/2393 synthetic progress events. Neither run installs files.
- Final EXE: 87,325,696 bytes.
- SHA256: 468eca127e0b7085bf11b81731148de8337a87a036fd6933b1ae81fa934f2317
- Previous local runtime results and limits: ALPHA22-FEEDBACK-FOLLOWUP.md.
  The last vertical recording-menu caption correction is geometry-checked;
  a live recheck is pending. Full x86 runtime coverage remains pending.
- No fresh security verdict is claimed. The earlier report remains unresolved;
  this test release is published at the user's request without waiting for it.

Personal demo data and the separate local audio core correction are excluded.
