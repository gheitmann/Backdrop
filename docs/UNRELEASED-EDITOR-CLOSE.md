# Unreleased menu editor close fix

The save question was wired to FormClosed and called Close again after the form
had already closed. Saving also called Close recursively. The question now runs
in FormClosing: No returns without saving, Yes allows closure only after a
successful save, and validation or save failure cancels closure. The event and
save method never invoke Close. Prior cancellation is respected. Exceptions are
reported with the editor kept open.

EditorCloseLifecycleProbe exercises the actual designer-wired event on an
undisplayed form with controlled prompt/save overrides. All six paths pass in
both x86 and x64: No, successful Yes, validation failure, save exception,
cancellation, and a previously cancelled event. It checks prompt/save/error call
counts, the cancellation result and absence of recursive disposal. No production
settings are loaded or written by this probe.

The rebuilt single EXE also passes editor Load verification for x86 and x64,
including the embedded x86 worker and a path with spaces. This does not claim an
interactive save test. Payload validation passes for 2405 files and 558 XMLs.
No new Alpha, installation or visual acceptance pass was performed.
