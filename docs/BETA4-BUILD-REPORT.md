# BETA4 build verification — 3 November 2026

Version **1.39.0.31**, tag `v1.39.0-beta.4`. Editor, Day/Night plugin and
installer helper binaries are unchanged. The outer setup is rebuilt with the
new version and reviewed payload.

- The preceding feedback verification passed seven Python checks: payload,
  BETA1/BETA2/BETA3 feedback, menus, secondary layout and Day/Night. The final
  versioned payload validation passed for 2,443 mapped files and 560 XML files.
- All 44 supplied logos decoded and matched the archive. Both MovingPictures
  XML files match the supplied corrections; the Photos facade matches BETA2
  byte-for-byte. See [feedback evidence](BETA3-FEEDBACK-FOLLOWUP.md).
- The MPE1 was reopened and all 2,443/2,443 mapped files byte-compared. The
  private setup bundle passed 21/21 entry comparisons and excludes MediaPortal
  core binaries.
- Final EXE VerifyOnly passed on the installed x64 client and isolated x86
  client (2/2 architectures). Fresh reports confirm the expected architecture,
  presentation and native binding probes. No installation was performed.
  Reports are retained outside the repository in project outputs
  `Backdrop-BETA4/setup-x64.txt` and `setup-x86.txt`.

EXE: **169,003,520 bytes**. SHA256:
`4bd546733167e1a4dba98cd250229c3e207df0444546d5b4ba47360234543836`.

No new client visual review or full installer wizard installation was performed.
Actual-media MovingPictures logo rendering and long slideshow title clipping
remain to be confirmed in the client. Earlier full x86 runtime, radio playback,
recording-icon pagination and Live TV progress limitations remain unchanged.
No new compatibility claim or MediaPortal core/audio fix is included.
