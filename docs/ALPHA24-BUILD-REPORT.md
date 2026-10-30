# ALPHA24 build verification â€” 25 October 2026

- Rebuilt AnyCPU editor, embedded x86 editor worker, isolated installer helper,
  x86/x64 launchers and presentation probes with Windows PowerShell 5.1 and the
  installed .NET Framework compiler.
- Six static check commands passed: validate.py, audit_integrations.py,
  check_guide_layout.py, check_playback_layout.py, check_menu_layout.py and
  check_secondary_layout.py (6/6, 100% of this command set).
- MPE1: 2405 payload files re-read and compared byte-for-byte; 558 skin XML files.
- Bundle: 21 entries compared against source; no MediaPortal core redistributed.
- Final EXE --verify: x64 and extracted official x86 client both exit 0 (2/2).
  Both select the correct architecture and plugin destination. Presentation
  probes confirm three readable buttons, opaque filename repaint, 2393/2393
  synthetic events preserved and only two targeted refreshes. No installation
  is performed by these preparation checks.
- Final EXE SHA256:
  55c7c3e5eb0e679ee99c08cb859f8a0bf1af30539b3d7b478f6f8a2634660147
- Local Defender signature 1.461.199.0: explicit "found no threats", exit 0.
  This single local result does not establish safety or explain ALPHA23's
  reported download detections. The release remains a draft.
- Live local x64 client: six category backgrounds verified; full-width horizontal
  BasicHome menu remains readable. Normal Alt+F4 exit completed, and a subsequent
  launch returned to BasicHome rather than the classic Home list.
- The user subsequently confirmed that actual startup looks correct. Direct
  splash capture through the window inventory was unavailable; the visual
  acceptance is user-confirmed. A full x86 runtime pass remains pending.
  Local display uses separate 3440x1440 artwork exports; distributable is 16:9.

These counts describe their stated checks, not overall completion or compatibility.
The private local startup settings and audio core correction are not in the
payload. The original horizontal BasicHome geometry is unchanged from ALPHA22.
