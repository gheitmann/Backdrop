# ALPHA27 tester feedback follow-up

Confirmed by the external tester: TV Home background and TV ON graphics, EPG group button, seek icons, Settings clock position, Lyrics menu icon, and partially exercised time-of-day backgrounds.

Restored four exact original artwork files from the recorded native baseline: playlist focus row and the three Now Playing button states used in action menus. Added these files to the native fidelity manifest. Increased the shared Settings page heading from font12 to font32 without moving the clock or changing navigation. The existing left menu tab already matches the original bytes.

At the owner's explicit request, the Photos night background now has a clearer central city view. The generated edit was visually reviewed and exported at the existing 1920x1080 size; the prior image is backed up outside the repository. Morning/noon/day variants remain unchanged. Changes are installed in the local Backdrop test skin and remain unpublished. The scoped live review is documented in RUNTIME-REVIEW-POST-ALPHA27.md.

## Historical XML review follow-up

Restored restrained manual recording-cover focus zoom (zoomXPixels 6, zoomYPixels 6) in both retained recording facades. Kept automatic zoom disabled, the bounded viewport, existing focus-caption policy and removed grid slide animations. Structural checks conservatively allow expansion on both sides and verify cell spacing and screen bounds in small and big icon layouts. Radio restyling and sliding menu highlights were subsequently authorized; see RADIO-MENU-REFRESH.md. The scoped live review observed the small recording focus enlargement; sustained page-up/page-down flicker testing remains unverified. See RUNTIME-REVIEW-POST-ALPHA27.md.
