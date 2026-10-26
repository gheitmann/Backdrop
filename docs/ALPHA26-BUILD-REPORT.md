# ALPHA26 build verification - 26 October 2026

- Three new generated background assets are packaged at 1672x941. Home,
  Settings and Plugins textures are referenced by eight intended XML definitions.
- Rebuilt AnyCPU menu editor DLL and standalone EXE, including its x86 worker.
- Six static commands passed (6/6): payload validation, integration audit,
  guide layout, playback layout, menu layout and secondary layout. The artwork
  decoder passed separately: 518 textures decoded, 419 original asset hashes
  and dimensions preserved.
- Package re-read and byte-compared all 2,408/2,408 payload files, including
  558 skin XML definitions and the three new images.
- Private installer bundle re-read and byte-compared 21/21 entries; no
  MediaPortal core binaries are redistributed.
- Final EXE preparation passed on the installed x64 client and extracted x86
  test client (2/2). Both checks were VerifyOnly and performed no installation.
  The presentation probes confirmed 3/3 readable buttons, opaque filename
  repaint and all 2,393/2,393 synthetic progress events on each architecture.
- Single EXE: 95,085,568 bytes; SHA256
  `e9b9c9d5c4a0a1a7006c29fc7f7e99a59a2bb5b404c9bd6a717b39398b5b6bc7`.

Live visual confirmation of the three new backgrounds remains pending. The
recording pagination flash and full x86 MediaPortal runtime coverage also
remain open. The earlier Defender report has not been resolved. Personal demo
data and the separate local audio correction are excluded.
