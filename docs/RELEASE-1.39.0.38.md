# Backdrop 1.39.0.38

## Changes compared with 1.39.0.37

- Rebuilt the InfoService feeds page with a content panel, larger typography and a
  full-height feed list.
- Re-laid out the Wikipedia article page: image and a separate text column, plus a
  cinematic DayNight background instead of the previous black page.
- Added the WorldWeatherLite window set and media (11 window files, the
  `Media\WorldWeather` assets and support images), mapped to Backdrop fonts, the
  DayNight background and fixed separators instead of the unsupported `#scroll.wrap`
  tokens. The package payload grows to 2,574 files.
- Hid the static Extensions placeholder labels.

Download the single Backdrop-Setup-1.39.0.38.exe and close MediaPortal before installation.

Verified in the isolated client: a real Wikipedia search for "Berlin" renders the
article in the new text column with the title and page indicator; the weather window
opens as window 7977 and renders the current/forecast layout; the Extensions detail
page shows no placeholder labels. Twelve Python checks pass and the setup VerifyOnly
probes pass. The Extensions list itself is drawn by the plugin and stays empty until
it indexes extensions; weather values require a configured location. No full x86
runtime review is claimed. No MediaPortal core or third-party plugin binaries are
included.
