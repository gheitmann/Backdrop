# Backdrop 1.39.0.37

## Changes compared with 1.39.0.36

- Fixed the menu editor's idle refresh. It now hands window and control updates to
  MediaPortal's GUI thread instead of loading skin resources from a timer thread.
  This removes the `LoadSkin: Running on wrong thread` errors and the follow-up
  `GUIMultiImage` exception that could appear while another window was loading.
- Refreshed the installer welcome text; it no longer calls the package an
  experimental test build or points to a German readme.
- Rebuilt the AnyCPU menu editor and the optional Day/Night plugin at package
  version 1.39.0.37.

Download the single Backdrop-Setup-1.39.0.37.exe and close MediaPortal before installation.

Verified on the isolated client: BasicHome navigation with the menu refresh timer
active produced no `BackdropMenuEditor` thread errors. The remaining `wrong thread`
entries in that session came from the external LatestMediaHandler plugin. No full
x86 runtime review is claimed. The static reference check, the corrected texture
paths and the 1.39.0.36 changes remain included. No MediaPortal core or
third-party plugin binaries are included.
