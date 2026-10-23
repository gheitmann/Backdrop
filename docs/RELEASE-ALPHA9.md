# Backdrop ALPHA9 for MediaPortal 1.39

Music Now Playing now supports clicking and dragging its progress bar to seek within local music. The required native progress control remains intact; the AnyCPU Backdrop menu editor 1.39.0.9 handles pointer messages within the application. Modal dialogs, radio, other skins and other pages are excluded.

Moving Pictures filmstrip now reserves horizontal space for the first selected cover, and its right-navigation XML spelling is corrected. Filmstrip and coverflow selection were checked with three synthetic movie covers.

The existing 37-case checklist reaches 36 passed (97.3%) and one blocked. This update revisits Moving Pictures views and progress seeking; other results are carried forward explicitly. A separate populated EPG check now covers the 12-row TV guide, descriptions and group selection using retained local fictional data. Fixtures and database backups are not included in the installer.

Remaining limits include immediate topbar focus/click timing, TVSeries online rating with offline fixtures, TuneIn, ARD HLS playback and full x86 runtime. Only the default theme is shipped. See [validation and evidence](VALIDATION.md).
