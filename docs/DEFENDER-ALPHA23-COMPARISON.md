# ALPHA22 / ALPHA23 Defender comparison — 25 October 2026

The user reported Trojan:Win32/Wacatac.B!ml while downloading ALPHA23 and asked
whether the new compatibility detection caused it. No protection was disabled,
no exclusions were added, and no installer was executed during these checks.

## Same-signature comparison

Microsoft Defender definitions: 1.461.199.0, unchanged before/after the scans.
MpCmdRun platform: 4.18.26090.9-0. Both original setup files reported
"found no threats", exit 0:

| Original artifact | SHA256 | Result |
| --- | --- | --- |
| ALPHA22 | f4f0329595f6414ac1bfe4bf3ed8677461f09c94f05e1249219c6fc043ebdd26 | No threat found |
| ALPHA23 | 13fb8732ab607c483cec8d06091981f13961ac8e90a18c432613b0ca5343226d | No threat found |

The embedded Backdrop.Bundle ZIP was extracted directly from each original PE
resource and checked byte-for-byte against its private build archive. Both contain
21 entries; 13 normalized entries differ. All extracted files retain original bytes.

Eight installer components per release were then individually scanned (16 scans):
presentation helper, x86/x64 launchers, x86/x64 presentation probes, PatchInstaller,
Initialize.ps1 and Start-BackdropInstaller.ps1. All 16 logs explicitly contain
"found no threats" and return exit 0. No new/repacked installer was distributed.

## Code / build distinction

Cecil metadata/IL inspection reads assemblies without loading their code:

- Presentation helper, x86/x64 launchers and x86/x64 probes have zero changed IL
  method bodies, but different module build IDs (MVIDs) and binary hashes.
- PatchInstaller contains the actual contract-check changes, including compiler
  generated methods. This comparison reports 19 added/removed/changed method bodies;
  that number is not a count of new source-level features.
- Setup.cs changes are version metadata and diagnostic reporting. Embedded archive
  extraction, administrator elevation and starting PowerShell with ExecutionPolicy
  Bypass already existed in ALPHA22.
- The initialization script changes child-process invocation/log capture and handles
  failed preparation. The start script adds consent, native fallback and package
  checksum checking. Package/editor revision metadata changes are also bundled.

## Conclusion and limits

The reported detection is not reproducible on this machine with the stated current
signatures. The comparison neither proves a false positive nor identifies a specific
code change as its trigger. Local file scans do not reproduce the reporting machine's
browser-download/cloud assessment. A newer unsigned build has different bytes even
where IL is unchanged, so the absence of prior detections alone does not isolate the
compatibility check. Building hybrid installers has no useful discriminatory result
while both original inputs are undetected, and was therefore not done.

Evidence directory: outputs/Defender-Comparison-20261025 (project workspace root),
including bundle-comparison.json, component-scans.txt and assembly-comparison.txt.
The public ALPHA23 warning remains appropriate. Needed next: update signatures on
the reporting machine and check the original download again without overriding any
block; if detection persists, submit its exact verdict/hash to Microsoft through
an approved submission route. No Microsoft submission or final verdict is claimed.

## VirusTotal update: analysis timestamp 25 October 2026, 16:24:42 Europe/Berlin

Visible result: 7/63 engines flagged the exact ALPHA23 SHA256. New relative to
our earlier observation: Palo Alto Networks, Generic.ml. Microsoft still shows
Undetected. The denominator also changed; multiple engines show failure/timeout.

The grouped Behavior report now contains actual sandbox observations:
- Expected Backdrop files extracted beneath AppData/Local/BackdropInstaller/run-...
  and the known Start-BackdropInstaller.ps1 PowerShell command line.
- MITRE summary: five LOW and sixteen INFO; behavior tags include obfuscated,
  detect-debug-environment and idle; highlighted calls include IsDebuggerPresent
  and GetTickCount. These annotations are observations, not proof of intent.
- One listed UDP endpoint: 162.159.36.2:53. Its attribution to application versus
  environment is not established from the grouped summary alone.
- Registry changes list the Explorer .exe OpenWithProgids exefile association.
- Observer terminated-process entries also list schtasks, Windows telemetry and
  other system processes. The visible summary does not show a parent/child causal
  chain proving that the installer started or terminated all those processes.
- Summary Detections, IDS Rules and Sigma Rules are marked NOT FOUND. This is a
  separate behavior-analysis section, not a reversal of the seven AV verdicts.

Parts match the known bootstrap behavior; unexplained tags/network/system entries
remain to be attributed in detailed reports. Neither infection nor a false positive
is established. Local undetected scans do not override the reported cloud verdicts.

Sources:
https://www.virustotal.com/gui/file/13fb8732ab607c483cec8d06091981f13961ac8e90a18c432613b0ca5343226d/behavior
https://www.virustotal.com/gui/file/13fb8732ab607c483cec8d06091981f13961ac8e90a18c432613b0ca5343226d/detection
