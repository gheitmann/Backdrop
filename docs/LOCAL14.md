# LOCAL14: version identity and menu overlays

Local package, editor and setup version: **1.39.0.14**. Last published alpha remains ALPHA13. No GitHub release or tag is created.

The previous LOCAL executable bundled changed files under package version 1.39.0.13. Native MPE consequently displayed the same old/new version. This build assigns revision 14 and describes itself as LOCAL, separating build identity from alpha publication. Native replacement/uninstall confirmation is retained; it now compares the actual version numbers. Reinstalling exactly LOCAL14 can still legitimately show equal versions.

Music library radio control 18 remains a native checkbutton with hyperlink 30 and its checkmark state intact. An explicit textureNoFocus `-` overrides the reference's inherited hiddenmenu_togglebutton_nof_on.png, whose pixels contain the unwanted yellow orb. Focus navigation and native bindings are unchanged in both songs and genre views.

Nine Latest Media title definitions use explicit absolute 90-degree pivots and bounded 400 x 40 left-aligned viewports ending at Y=970, above the home bar at Y=980. Corresponding generator definitions are updated and the AnyCPU editor rebuilt, so saving the home menu retains these corrections. Visibility conditions and media actions are unchanged.

Static containment: **18/18 cases (100%)**, nine definitions at 1920 x 1080 and 3440 x 1440. Static geometry is not fresh runtime visual certification. Both radio overrides and all generator geometry/pivots pass structural checks. Native binding audit: 139 windows, zero incompatible types, 69 absent fields unchanged. Payload validation covers 2,393 files and 557 XML definitions. Native package serializer accepts revision 14.

The isolated application was opened for visual testing, but concurrent user input prevented controlled navigation. No fresh runtime pass is claimed for the two reported overlays. The test application was left available. Full installation/replacement and live filename repaint remain owner confirmation items; prior setup preparation and synthetic repaint probes remain separately documented.
