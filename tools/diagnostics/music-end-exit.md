# Local music end / exit deadlock (2026-10-25)

The unmodified local MP 1.39 Core.dll (SHA256 0d9160a8e6fb61a6b6bfecacc80df0207396bc4cc01d4c5dc2f5850c86bb4b39) deadlocks after the last music track. The UI thread waits in BassAudioEngine.Stop during OnFormClosing. The BASS command thread disposes MixerStream, waiting for the playback-end callback; that callback synchronously waits in BassAudioEngine.Stop. The topbar X correctly requests exit and is not the source of the wait.

The source patch makes nextSong == null enter the existing asynchronous end path. PatchBassEnd.cs (Mono.Cecil dependency) refuses any other input hash and writes a separate output. VerifyCorePatch.cs compares all 12,609 methods and confirms exactly one changed body, preserving identity, architecture, references and resources.

Local corrected SHA256: 197886c65b58f9e2711e44aaa7fc91ed531aae1153a1c3c069939cbf2259bbe4.

This is an application-core diagnostic fix, not part of the skin installer payload. Never distribute a replacement Core.dll in the Backdrop package or apply this patch to a different input build. The actual upstream source change is recorded in BASS-playlist-end-source.patch.

Local deployment retained the original at:
C:\Users\49152\Documents\Codex\Testmedien\MediaPortal\v5-demo\backups\audio-exit-20261025-112727\Core.dll

Practical verification: Default playlist Morgenlicht started at 11:29:16, ended at 11:29:47; g_Player.Process observed stopped and OnEnded on MPMain. Home then displayed all nine entries once with 24 skin-pixel inset. Topbar X requested normal shutdown at 11:31 and the MediaPortal process exited completely. No forced kill was used for this corrected run. The previously deadlocked process was terminated only after diagnostic thread capture.
