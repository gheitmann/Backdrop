# Backdrop 1.39.0.36

## Changes compared with 1.39.0.35

- Added a static skin reference check that resolves textures, `mask` attributes,
  fonts, imports and MediaPortal 1.39 window IDs against the shipped files. It runs
  in GitHub Actions and fails on new unresolved references instead of relying on a
  visual review.
- Corrected the topbar home window ID: it used the volume-overlay ID 3004 instead of
  the topbar ID 3005.
- Corrected the paging-arrow and scrollbar texture paths in `references.xml`.
- Repaired the remaining static texture references in the no-signal progress
  background, the file-dialog and cover-art-grabber progress backgrounds, the
  SubCentral default poster path, and the unused video facade templates.
- Aligned the README, maintenance notes and the in-skin version label with the
  package version.

Download the single Backdrop-Setup-1.39.0.36.exe and close MediaPortal before installation.

The texture corrections are statically verified, and a scoped start of an isolated
client copy reported no texture-loading errors. The changed video facade templates
are not loaded by MediaPortal (window fragments without an ID), so their correction
has no visible effect. No full x86 runtime review, installer wizard run or live TV
no-signal test is claimed. The recording and InfoService changes from 1.39.0.35
remain included. No MediaPortal core or third-party plugin binaries are included.
