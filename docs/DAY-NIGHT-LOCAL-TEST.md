# Local background-lighting verification

## Four-period update â€” 26 October 2026

Plugin 0.2.0.0 was installed in the existing x64 MediaPortal test client.
The previous two-period prototype was backed up before updating 69 files;
all 69/69 written hashes were verified after installation and after the UI test.
Backup receipt: outputs/DayNight-Local-20261026-d059f62c/deployment.json
outside the repository. Existing personal settings and custom menus were kept.

Full-client checks performed:

- Settings > GUI > Skin displays Background lighting in the native button style.
- The native dialog opens and returns to the Skin settings page normally.
- Mode selection shows Automatic, Morning, Noon, Day / afternoon, Night and Off.
- Four start times show 06:00, 12:00, 15:00 and 19:00 after version-1 migration.
- Saving Morning and then Noon changes the visible background immediately,
  without restarting MediaPortal. The log confirms morning:morning and noon:noon.
- Hour and minute dialogs open; a staged change from 06:00 to 06:01 is visible.
  Cancel discards it: the persisted configuration remains 06:00.
- Automatic was restored through the native dialog; the log returned to
  automatic:night and the persisted schedule remains 06/12/15/19.
- The existing English client language was retained. All 37 language resources
  were checked programmatically; every language was not visually inspected.

Native assembly probes separately pass 1,364/1,364 assertions on each of x86
and x64. These include localized resources, form rendering/save, schedule and
migration boundaries, old two-period skin fallback, 42 expressions and lifecycle.
These probes do not constitute full x86 client interaction coverage.

No new Alpha, GitHub release or single-file setup was built/published.
The private packaging probe is not a release artifact; its metadata is ALPHA26.
Suspend/resume and prolonged playback were not tested in this update.

## Earlier two-period prototype

The preceding local deployment verified 48/48 files, enabled the independent
process plugin, and demonstrated day/night transitions without restart.
Its backup remains outside the repository in
outputs/DayNight-Local-20261026-6051aa57. The four-period update supersedes it.
