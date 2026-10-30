# Backdrop ALPHA23: version-tolerant setup

Includes the ALPHA22 skin and menu editor changes. Setup, skin and editor revision
is 1.39.0.23. Download the single Backdrop-Setup-ALPHA23.exe and close MediaPortal
before installation.

## Changes

- Removed the exact MpeCore 1.39.0.0 version lock that rejected the reported 1.39.1
  installation. Presentation adaptation now checks actual types, signatures,
  controls and the full presentation-only refresh-method contract.
- Unknown or incompatible installer presentation now gives a readable explanation
  and offers the installed original MediaPortal Extension Installer, explicitly at
  the user's own risk. No is the default; declining installs nothing. The fallback
  retains native dependency checks and never uses an unchecked presentation patch.
- MP1 versions outside the tested 1.39 family receive a compatibility warning even
  when adapter probes pass. The package range no longer caps installation at 1.39.
  Architecture, actual APIs, skin-engine and native dependency constraints remain.
- Failed preparation is logged without exposing a raw stack trace as the normal
  compatibility decision. Verify-only runs never prompt or start installation.

## Verification and remaining limits

All repository payload/layout checks pass. Final single-EXE preparation passes
against installed MP1 1.39 x64 and official extracted 1.39 x86 client files.
Four metadata-only version fixtures (1.38, 1.39.1, 1.40 and 2.0) verify that version
numbers alone no longer reject the presentation contract; the 2.0 fixture does
not represent MediaPortal 2 support. Five incompatible-contract fixtures are
rejected without writing a patched assembly. Accepted fixtures retain assembly
identity and change only four presentation methods. Installed input is unchanged.
Three native fallback branches (Yes, No, Verify) pass using simulated dialog
answers and a harmless original-installer stub: Yes invokes it with the correct
package and client working directory, No does not, Verify fails without a prompt.

These are regression and preparation tests, not an actual 1.39.1 installation.
Peter's exact 1.39.1 binaries were not available locally. Older/newer MP1 releases
remain unverified for full runtime use; the editor is compiled against 1.39 APIs.
The ALPHA22 playback and visual limitations remain. The separate local audio
Core.dll correction is not distributed in this setup.
