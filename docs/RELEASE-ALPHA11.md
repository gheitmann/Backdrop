# Backdrop ALPHA11 for MediaPortal 1.39

Adds direct hit-tested topbar mouse gestures to the AnyCPU plugin and single-target translated captions for artist/album information toggles. Preserves the native controls and actions, including Play/Resume. Editor version: 1.39.0.11.

Home, Pause/Resume, Next/Previous, player navigation and Exit passed isolated x64 checks; a cross-button drag correctly cancels; artist biography/discography and matching captions were tested with retained synthetic metadata. Forward/Rewind and remaining pointer edge cases are explicitly limited in validation. This is not certification of all mouse commands.

The separate optional InstallerUI ZIP addresses full-window repainting per file and fixed white button text in the MP 1.39 Extension Installer. Extract it and run Initialize.ps1, then use Runtime/BackdropInstaller.exe. It patches a local copy and leaves installed MediaPortal binaries unchanged. Preview, 2,393 synthetic events and assembly binding passed; full real installation via this helper remains unverified. Installing the ordinary skin MPE1 alone does not change the installer's appearance. See docs/INSTALLER-UI.md.

Historical checklist remains 36/37 (97.3%), carried forward, not rerun in full. Native binding audit: zero incompatible control types in 139 windows. Close MediaPortal before installing. Full x86 runtime remains unverified.
