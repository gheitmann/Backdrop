# Backdrop ALPHA25

Version **1.39.0.25** vereint die neuen Hintergründe aus dem ALPHA24-Entwurf mit
den aktuellen Korrekturen zu den Rückmeldungen von ALPHA22.
Zur Installation MediaPortal schließen und **Backdrop-Setup-ALPHA25.exe** starten.
Es wird nur diese eine EXE benötigt. Vorhandenen Skin vorher sichern.

## Changelog – wichtigste Änderungen gegenüber ALPHA23

- Neue, aufeinander abgestimmte Start- und Kategoriehintergründe im Kino-Stil.
  Das waagerechte BasicHome-Menü über die gesamte untere Bildschirmbreite bleibt erhalten.
- 37 Originalgrafiken wiederhergestellt: Topbar-Icons und Hintergrund, Listen in
  Home und Einstellungen, Dialograhmen sowie Trennlinien bei Music Now Playing.
- Titel und Genre unter Latest Recordings ohne Überlagerung angeordnet;
  dieselbe Korrektur ist im Menüeditor enthalten.
- Manuellen Fokus-Zoom in Aufnahme-Iconansichten entfernt, um Zeichnen über den
  sichtbaren Bereich hinaus zu vermeiden. Das kurzzeitige Aufblitzen beim
  Blättern gilt damit noch nicht als nachweislich behoben.
- „MENU“ in den Menüs für TV- und Radioaufnahmen senkrecht verankert.
- Versionsnummern von Skin, Editor und Setup auf 1.39.0.25 angehoben.
  Die flexible MP-Kompatibilitätserkennung und der native Installer-Fallback bleiben erhalten.

## Tested scope and remaining limits

The local x64 client loaded 72 synthetic recordings. Normal recording icons
paginate in both directions; large icons load, but their pagination is not yet
verified. All three populated Latest Recordings caption slots were checked
(3/3, 100% of those slots). The last vertical MENU correction passed geometry
checks at 1920x1080 and 3440x1440; its live appearance still needs confirmation.
Full x86 runtime coverage remains pending. These are scoped results, not an
overall compatibility percentage.

The photographic masters are 1672x941, exported at 1920x1080 for distribution.
Personal demo media, databases, machine settings and the separate local audio
core correction are not included. No modified MediaPortal core is distributed.

## Unresolved security report

The earlier ALPHA23 Defender/VirusTotal report remains unresolved. Publishing
this requested test release does not establish a false positive or certify the
new executable as detection-free. See DEFENDER-ALPHA23-COMPARISON.md.

Build checks: [ALPHA25 build report](ALPHA25-BUILD-REPORT.md).
