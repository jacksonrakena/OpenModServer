# OpenModServer
OpenModServer (OMS) is a new project aimed at creating an omnibus mod/plug-in distribution server for many different games.

### Roadmap
**Identity & Profiles**
- [x] Support creating and logging in to user accounts
- [ ] Support logging in with Google, Twitter, Discord, etc
- [ ] Discord, YouTube links on profile
- [ ] Profile page and profile picture support

**Mods**
- [x] Creating mods
  - [x] Basic identifiers
  - [ ] Screenshots/advertising material
  - [ ] YouTube links
- [ ] Deleting mods
- [ ] Creating releases
  - [ ] Upload management for releases
  - [ ] Admin page to manage uploaded files
  - [ ] VirusTotal scanning integration
- [ ] Deleting/unapproving releases
- [ ] Allow site moderators to approve releases

**Distribution**
- [x] Mod downloads over HTTP
- [x] Dynamic publishers (Dalamud plugin source)
- [ ] Appearance overhaul

**Community**
- [ ] Comments on mods
- [ ] User forum
### Goals
**Short-term:**  
- Support uploading, managing, downloading, and acting as a plugin/mod repository for the following games/platforms:
  - Final Fantasy XIV Online
    - Providing an API that complies with the Dalamud plugin source specification
  - Grand Theft Auto V
    - Providing an upload/download service for Script Hook V/SHVDN DLL files
- Allow site administrators to publish, draft, retract, and release new versions of mods for specific games
- Allow FFXIV Dalamud users to add an OMS server as a plugin source
- Support all Dalamud specification data structures
  - Download count
  - String tags
  - Last update time
  - Changelogs
  - Different release strata
    - Production (Install)
    - Staging
    - Testing
  
**Long-term:**
- Support assembly validation/inspection of FFXIV and GTA 5 mods
- Support more games and applications

### Copyright
OpenModServer (OMS) copyright &copy; 2022 Jackson Rakena (abyssal) under the MIT License