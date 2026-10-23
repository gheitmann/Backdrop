# Local installer bundle validation

23 October 2026, Windows 11, installed MediaPortal 1.39 x64. No new alpha, release, tag or remote publication. The current local MPE1 retains ALPHA13 metadata and includes the unreleased guide background change.

Six targeted preparation checks passed: **6/6 (100%)**.

| Check | Result |
| --- | --- |
| Native presentation and binding probes after build | Three standard readable buttons, buffered form, all 2,393 synthetic events preserved, two targeted refreshes and final flush; native installer binds to isolated MpeCore without invoking its entry point. |
| Integrated archive integrity | All 17 ZIP entries reopen byte-identically, included MPE1 SHA256 matches its sidecar, no redistributed MediaPortal core binaries. The MPE1 itself contains 2,393 verified payload files. |
| First start from extracted bundle | Windows PowerShell 5.1, path containing spaces, `Start-BackdropInstaller.ps1 -VerifyOnly`: automatic installed client discovery, initialization, presentation probe and assembly binding pass. |
| Reuse initialized bundle | A second VerifyOnly invocation succeeds without requiring manual Initialize.ps1 or recreating Runtime. |
| Explicit custom client path | Resolver accepts a fixture folder containing spaces and the two required filenames. This is path-resolution evidence, not execution of a second MediaPortal installation. |
| Invalid client path | Resolver rejects the path and identifies the complete missing MpeCore.dll filename. |

Installed MpeCore SHA256 remains `124cd3074e92c3dcc53f20f0ec6a1ab7d62eb3b53d5182ae0ae3ec9fa6ba670c`.

No real installation or package registration was invoked. Folder-picker interaction, ambiguous-installation selection, complete x86 execution and real-install progress/elevation remain unverified. The existing presentation preview and synthetic event probes do not replace a real installation test. Normal skin files and installer file associations were not changed.
