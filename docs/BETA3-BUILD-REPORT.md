# BETA3 build verification — 30 October 2026

Package/setup version **1.39.0.30**, tag `v1.39.0-beta.3`. Editor and Day/Night
plugin binaries are unchanged. The existing installer presentation/launcher
helpers are reused; the outer setup is rebuilt with the new version and payload.

- Nine static Python checks passed across the feedback and release preparation:
  payload, integrations, guide, playback, menus, secondary layout, Day/Night,
  BETA1 feedback and BETA2 feedback. The final versioned payload was validated
  separately: 2,442 mapped files and 560 skin XML definitions.
- Artwork verification decoded 518 control textures and checked the protected
  original resources. The four repaired Photos masters were inspected; image
  provenance and export hashes are retained in the repository.
- The BETA3 MPE1 was reopened and byte-compared against all 2,442/2,442 payload
  files. The private setup bundle passed 21/21 entry comparisons and contains no
  redistributed MediaPortal core binaries.
- Final single EXE VerifyOnly passed for the installed x64 client and isolated
  x86 client (2/2 architectures). Fresh reports explicitly confirm the selected
  architecture, native binding and presentation probes. No installation was
  performed by these final checks. Reports: project outputs
  `Backdrop-BETA3/setup-x64.txt` and `Backdrop-BETA3/setup-x86.txt`.
- Scoped x64 live review confirmed the populated classic lower radio guide,
  programme-description updates and Photos filmstrip with landscape, portrait
  and square fixtures. All 16 changed skin files were installed locally with
  backups and byte comparisons. See [feedback verification](BETA2-FEEDBACK-FOLLOWUP.md).
- Native x64 Day/Night probe passed 1,448 assertions across 48 expressions in
  39 XML definitions. Existing log4net probe warnings did not fail assertions.

EXE: **168,974,336 bytes**. SHA256:
`45c102565189e9c7bdf67c641707a636be438c15c117e99f06f5833e92bee64f`.

No fresh full EXE wizard installation, full x86 interaction, real radio playback
or guide group/single-channel switching was performed. Final live confirmation
of the changed heading sizes, radio list panel and omitted arrows remains open.
Recording-icon pagination flash and Live TV progress irregularity are not
claimed fixed. No new blanket MediaPortal-version compatibility claim is made.
