# ALPHA23 verification

25 October 2026. Targeted checks: six repository commands, nine contract fixtures,
three fallback-flow cases and two final-EXE preparation runs: 20/20 passed (100%
of this defined scope). This is not an overall compatibility or runtime percentage.

The repository commands validate 2,405 payload files, 558 skin XML files,
integration inventory and guide/playback/menu/secondary layouts. Packaging verifies
all payload bytes and 21 private bundle entries, excluding MediaPortal core DLLs.

Contract fixtures clone local MpeCore metadata with versions 1.38.0.0, 1.39.1.0,
1.40.0.0 and 2.0.0.0. All four retain identity and alter only four presentation
methods. Five negative fixtures (extra refresh logic, missing control, missing
constructor, wrong assembly name, signed identity) produce no patched output.
These fixtures are not binaries from those actual releases, and do not establish
MediaPortal 2 support. The original installed input bytes remain unchanged.

Fallback tests run the production scripts in disposable copies with only the
confirmation result substituted. Yes invokes a harmless native stub with the
correct package and client working directory; No invokes nothing; Verify exits
unsuccessfully without prompting or invoking installation. These tests check flow,
not the visual appearance of the dialog or a real installation.

The finished single EXE passes preparation using installed 1.39 x64 and official
extracted 1.39 x86 client files, including a path containing spaces. Both select
the correct architecture and native plugin destination. Real 1.39.1 client files
were unavailable locally; full older/newer runtime checks remain open. Existing
ALPHA22 practical coverage and limitations remain applicable.
