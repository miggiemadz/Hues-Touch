# 🎮 Unity Project: Modular Gameplay Systems (Player, NPCs, Enemies)

This Unity project includes modular systems for:
- 🧍‍♂️ A player character with movement and projectile shooting
- 🤖 NPCs that you can interact with using dialogue lines
- 👹 Enemies with patrolling, chasing, attacking, and health systems
- 🌀 A spawner that randomly places enemies on the NavMesh

---

## 🧍 Player Controller

**Main script:** `NewMonoBehaviourScript.cs`

### Features:
- WASD movement with acceleration/deceleration
- Character rotation based on movement input
- Gravity + grounded detection
- Shoots projectiles when pressing **E**
- Can take damage from enemies or projectiles

### Inputs:
- `E`: Shoot projectile (uses Unity's new Input System)
- Movement handled via `InputActionReference`

---

## 💬 NPC Dialogue System

**Script:** `NPCDialogue.cs`

### Features:
- Detects if the player is within interaction distance (no triggers required)
- Press **E** to talk to the NPC
- Scrolls through an array of dialogue lines
- Automatically ends dialogue when out of range

### Setup:
- Tag your player as `"Player"`
- Add the `NPCDialogue` script to any NPC
- Set the `interactionDistance` and fill in `dialogueLines` in the Inspector

---

## 👾 Enemy AI

**Scipt:** `RangedEnemy.cs`
### Features:
- Was the first version of Enemyai.cs
- Uses Projectile Prefab as its projectile
- Patrolling behavior when idle
- Chases player when in sight
- Attacks player when in range
- created for Spawners

**Script:** `Enemyai2.cs`

### Features:
- Supports **melee** and **ranged** enemy types
- Patrolling behavior when idle
- Chases player when in sight
- Attacks player when in range
- Ranged enemies shoot projectiles
- Health system with `TakeDamage()` and `DestroyEnemy()`

---

## 🔫 Projectile System

**Script:** `Projectile.cs`

### Features:
- Spawns from player or enemy
- Moves using Rigidbody physics
- Damages player or enemies on contact
- Self-destructs after a short lifetime

---

## 🌀 Enemy Spawner

**Script:** `EnemySpawner.cs`

### Features:
- Spawns enemies randomly around the spawner using NavMesh
- Controlled by max enemy count and spawn interval
- Each enemy can notify the spawner when it dies

---

## 🎯 Future Improvements
- Allow Spawners to change Enemy Types instead of using RangedEnemy.cs
- ^ If completed, will remove RangedEnemy.cs entirely
- Make shooting a bit smoother
- Add UI for NPC dialogue (TextMeshPro)
- Allow inputs from Player to talk to NPCs
- Health bars for player and enemies
- Quest system triggered from NPCs



