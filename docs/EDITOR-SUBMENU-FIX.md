# Menu editor submenu input fix

The Add button dereferenced unselected background, destination and latest-media controls. Validate required input before changing menu collections, report missing fields, default unspecified latest media to No, and check the actual fanart source. Clearing destination/background selections is now safe.

Built the AnyCPU DLL and standalone launcher. The native WinForms event-handler probe passed incomplete destination, type, path and fanart cases, successful window 87950 insertion, latest-media default and cleared selections. No full interactive UI run or local installation was performed. Existing ALPHA27 release assets remain unchanged.

Run: Windows PowerShell 5.1 -File tools/test-editor-submenu.ps1
