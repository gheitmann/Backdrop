# Standalone Backdrop Menu Editor

Build with `powershell.exe -NoProfile -ExecutionPolicy Bypass -File Source/build.ps1`.
The build produces `Source/BackdropMenuEditor.exe` as well as the existing plugin DLL.
The EXE embeds the freshly built editor DLL; only the EXE needs to be distributed.
An installed MediaPortal with the Backdrop skin is still required.

Close MediaPortal Configuration before editing. Close and restart MediaPortal to
apply generated menu changes. Double-click the EXE from any writable folder, or
create a Windows shortcut to it. Administrator rights are not requested by the
starter; the configured skin and settings folders must be writable.

The starter checks its own directory and standard 64-bit and 32-bit installation
paths. If no installation is found, it offers a folder picker. Custom installations
can also be selected explicitly:

```powershell
.\BackdropMenuEditor.exe --mediaportal "D:\MediaPortal"
```

The editor runs in a separate application domain with MediaPortal's application
base and Configuration.exe.config. MediaPortal's own Config API resolves the
settings and skin folders, including MediaPortalDirs overrides. Plugin directory
validation prevents accidentally loading a different installation.

Standalone saves explicitly flush MediaPortal's profile cache. Plugin-hosted saves
retain the existing Configuration OK/Cancel behavior. The plugin DLL remains
necessary for runtime skin integration.

Verification without showing or saving the window:

```powershell
.\BackdropMenuEditor.exe --verify "C:\Temp\backdrop-editor-check.txt"
```

Validated on the local 64-bit MediaPortal installation: exit code 0, real editor
construction and Load event, installed Backdrop menu loaded, correct Program Files
plugin path and ProgramData configuration/skin paths. No production menu changes
were saved by this check. Interactive editing and saving remain to be checked by
the user; the launcher does not constitute a new Alpha release.
