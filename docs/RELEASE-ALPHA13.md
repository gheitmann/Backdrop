# Backdrop ALPHA13 for MediaPortal 1.39

Corrects displaced vertical titles in DialogMenu, DialogSelect, DialogSelect2 and DialogMultiSelect. Titles use explicit nonzero absolute rotation pivots inside the left frame strip, rather than the automatic horizontal text centre.

The TV guide group caption now starts at the lower left. Its bounded viewport keeps long scrolling names below the time headings. Original lower-grid geometry and the seven visible rows remain unchanged. Editor version stays 1.39.0.11; radio guides and the separate ALPHA11 InstallerUI helper are unchanged.

Targeted caption checks: 3/3 passed (100%) in isolated MP 1.39 x64, including the group selector and shared GUIDialogMenu through guide F9. The exact music Actions command list was not reproduced; the shared native dialog/heading definition was verified. Static rotated-caption containment passes 12/12 cases over six definitions and two output sizes. Native binding audit reports zero incompatible types in 139 windows. See docs/VALIDATION.md for precise scope and inherited limitations.

Historical 36/37 and ALPHA12 guide 8/9 results are carried forward without full reruns. Actual Live TV preview and full x86 runtime remain unverified. Close MediaPortal before installing.
