# Backdrop 1.39.0.39

## Changes compared with 1.39.0.38

- Corrected the installer file index. The 1.39.0.38 package contained all 2,574
  files, but the serialized `UniqueFileList` still listed 2,444 entries, so the
  native installer skipped the 130 new weather files. 1.39.0.39 regenerates that
  index with MpeCore and installs the complete payload.
- Includes the 1.39.0.38 content: the rebuilt InfoService feeds page, the Wikipedia
  article layout with its DayNight background, the WorldWeatherLite window set and
  media, and the hidden Extensions placeholder labels.

Download the single Backdrop-Setup-1.39.0.39.exe and close MediaPortal before installation.

Verified: the package reports 2,574 installable files and 2,574 indexed files; the
setup VerifyOnly probes pass. On the real client, 2,573 of 2,574 payload entries
match the repository byte for byte, the only difference being the personal
`%Config%\BackdropMenuEditor.xml`; editor and skin report 1.39.0.39. No full x86
runtime review is claimed. No MediaPortal core or third-party plugin binaries are
included.
