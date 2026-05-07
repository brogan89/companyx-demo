# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unity 6 multiplayer demo using **Netcode for GameObjects (NGO) v2.11.2**. Two players connect in real-time and interact with networked objects (light switch, button, NPC).

- Engine: Unity 6000.3.11f1
- Language: C# (.NET)
- Rendering: Universal Render Pipeline (URP) v17.3.0
- Input: Unity Input System v1.19.0
- Navigation: Unity AI Navigation v2.0.12

## Running the Project

**Published build** (Windows x64): double-click `Builds/CompanyX.exe` twice.

**In Unity Editor**: Window → Multiplay Center → set 2 players → Ctrl+B (or File → Build and Run).

No CLI build commands — everything goes through the Unity Editor or pre-built exe.

## Architecture

All game scripts live in `Assets/_Project/Scripts/`. Single scene: `Assets/_Project/Scenes/NGO_Setup.unity`.

### Network Model

Server-authoritative. Clients send RPCs; server owns all state mutations.

### Core Scripts

| Script | Base class | Role |
|---|---|---|
| `LightSwitch.cs` | NetworkBehaviour | Owns `NetworkVariable<bool> _isOn`; server toggles via ServerRpc, all clients react via `OnValueChanged` |
| `PlayerInteractions.cs` | NetworkBehaviour | Reads local input (space bar), proximity-checks the light switch (≤1 unit), calls `LightSwitch.ToggleSwitchServerRpc()` |
| `NetworkButton.cs` | NetworkBehaviour | Generic server→all-clients button: disabled until spawned, fires `OnPressedShared` ClientRpc for audio/FX |
| `PlayerAnimations.cs` | MonoBehaviour | Local-only; derives velocity from position delta to drive PlayerMove/PlayerIdle animations |
| `Npc.cs` | MonoBehaviour | Destroys itself on clients (`IsServer` check); loops waving animation on server with random 3–6s intervals |

### Typical Data Flow (light switch)

1. Local player holds space bar near switch → `PlayerInteractions` shows UI prompt
2. Client calls `LightSwitch.ToggleSwitchServerRpc()`
3. Server flips `_isOn` NetworkVariable
4. `_isOn.OnValueChanged` fires on every client → updates Light component + Animator

### Tags Used in Code

- `"LightSwitch"` — scene lookup from `PlayerInteractions`
- `"InputPrompt"` — UI element toggled by proximity check
