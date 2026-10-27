# BETA1 build verification — 28 October 2026

Package/editor/setup version: 1.39.0.28; release tag v1.39.0-beta.1. The MPE development status is Beta. Numeric revision 28 upgrades ALPHA27 revision 27. The optional Day/Night DLL is unchanged from the independent v0.2.0-alpha.1 release.

- Seven static commands passed: payload, integration audit, guide, playback, menu, secondary layout and Day/Night checks. Payload: 2,442 files, 560 skin XML definitions. Menu: 265 rows across 48 pages, 10 native selection rows, 76 independent animated focus backgrounds. Day/Night: 30 artwork records, 44 expressions in 35 XML files, 37 languages x 18 strings.
- Package reopened and compared 2,442/2,442 payload files; private setup bundle compared 21/21 entries. No MediaPortal core binaries are redistributed.
- Final EXE VerifyOnly passed on installed x64 and extracted x86 clients (2/2 architectures). Each presentation probe checked 3/3 readable buttons and retained 2,393/2,393 synthetic events. VerifyOnly does not install.
- Isolated native x64 copy-provider runs installed and compared 2,442/2,442 mapped files, including a repeat against the final rebuilt package. Font destinations/providers were redirected to ordinary test-folder copies; this does not test Windows font registration. Personal SkinSettings.xml sentinel remained unchanged.
- Actual local update used the installed native MpeInstaller /S with the final MPE1, exit code 0. Installed comparison: 2,441/2,442 exact byte matches; the remaining BackdropMenuEditor.xml is a NeverOverwrite personal configuration and was preserved. Existing Backdrop and editor binaries were backed up before updating. This tests native installation separately from the EXE preparation probes; it is not a fresh end-to-end click-through test of the EXE wizard.
- The final editor launcher created and loaded its real form under x64; UI accessibility confirmed the editor and Save Changes dialog. Closed without saving. Submenu input regression passed.
- Native Day/Night x64 integration: 1,392/1,392 assertions and 44 background expressions passed, including configuration save/render and GUI-thread lifecycle. The probe expectation was updated from ALPHA27's 42 bindings to the current 44 radio-inclusive bindings; no product change was needed for this test-count correction. Earlier full-client mode/time-picker interaction is carried forward, not rerun here.
- Live recording small and big icon layouts opened and page down/up changed page counters and selected titles. Modest cover focus zoom and complete bottom caption lines observed. The first review exposed caption clipping and page keys not leaving the idle placeholder; corrected before the final package. Temporarily increased the local idle timeout for the bounded paging review, then restored the original settings. Rapid single-frame flicker remains unverified.
- Other local screen observations are recorded in RUNTIME-REVIEW-POST-ALPHA27.md. Client exited normally. The main configuration remains Apollo; separate Backdrop review profile was used.

EXE: 167,814,144 bytes. SHA256: `ed133ae6f7b6fdc2f19caede19a0cfcaa57fe344530140e9dbee49c88928bcfe`.

Known limits: real radio tuning/playback is unavailable with unmapped synthetic stations; full x86 client interaction, every locale/DPI, suspend/resume and prolonged playback remain open. No blanket compatibility or flicker-free claim. Local core/audio and third-party experiments remain excluded.

Official upstream review still lists MediaPortal 1.39 in x86/x64 form: https://www.team-mediaportal.com/news/mediaportal-139-release . This is context, not proof of compatibility with every MediaPortal build.
