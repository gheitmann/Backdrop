# Synthetic EPG validation

23 October 2026, MediaPortal 1.39 x64, isolated Backdrop client.

The local test TV Server now contains 16 fictional TV channels and four fictional radio channels, with 4,200 programmes spanning seven days. The first generated period runs from 22 October 2026 00:00 through 29 October 2026 00:00 exclusive. All 20 channels have a current programme. Durations vary from 15 to 90 minutes and include long titles and multi-paragraph descriptions.

Persistent fixture location: `C:\Users\49152\Documents\Codex\Testmedien\MediaPortal\v4-epg`. `Refresh-TestEpg.ps1` regenerates a current seven-day period, preserving channel identities and backing up database tables before mutation. Existing v1 media are untouched. Test database snapshots and local configuration remain outside the public skin package.

The TV guide was opened with 12 rows. Channel names, programme cells, time headers, current-time marker and selected programme details populate. The group selector opens and selecting `Backdrop Test TV` updates both header and vertical group caption. The original reported white group area is no longer visible in this inspected populated state. These checks supplement ALPHA8 validation; they do not change its historical 37-case results.

Radio uses separate `radiochannelgroup` and `radiogroupmap` tables. MP requires the native radio `All Channels` group even when ChannelNavigator loads TV channels; its absence initially caused a NullReferenceException. That configuration prerequisite is now present. No TVPlugin or core binary was changed for EPG fixtures.

Radio guide rendering, live tuning, recording, reminders and all other row-count variants remain unverified. Synthetic channels have no tuner mappings and cannot supply live television.
