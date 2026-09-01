# RogueRealms

## Implemented

**Core**
- Entity (abstract): move via Rigidbody2D, tracks facing, holds stats via `BaseStats`
- EntityStats: maxHealth, currentHealth, speed (int), speedScale
- EnemyStats : EntityStats + damageMultiplier
- PlayerStats : EntityStats + strength, range, wisdom, hope, defense, critChance (float, uncapped), dodge
- Direction / DirectionalSprites: N/E/S + auto-flip West
- IBodyDrawer: shared interface for Animal/Humanoid drawers

**Entities**
- PlayerEntity : Entity, uses PlayerStats
- EnemyEntity : Entity, uses EnemyStats
- No more HumanoidEntity/AnimalEntity classes. Body type (animal vs humanoid) is just whichever drawer component sits on the entity's child - not a C# class. PlayerEntity and EnemyEntity both work with either drawer.

**Defs**
- Def (base ScriptableObject, defName) + DefDatabase<T> (loads from Resources/Defs, lookup by name, cached random pick)
- HairDef, BodyTypeDef, HeadTypeDef, ClothingDef (Body/Head slot)

**Drawers**
- AnimalBodyDrawer: 1 sprite layer
- HumanoidBodyDrawer: 5 layers (Body, BodyClothing, Head, Hair, HeadClothing). Auto-picks a random Body + Head def on Awake if none set yet.

**Player**
- PlayerController: WASD move, faces mouse (bucketed N/E/S/W, sprite never rotates)

## Stats: why extension, not two separate classes

EnemyStats and PlayerStats both extend EntityStats. Both need health + speed + move/damage math - that logic lives once in EntityStats and Entity. Player-only stuff (strength, crit, etc) and enemy-only stuff (damageMultiplier) just get added on top. Entity itself never needs to know which one it's holding - it only calls `BaseStats.CurrentMoveSpeed` etc.

## Bugs fixed

**Player invisible**
1. `Entity` called `SetFacing()` in `Awake()`. Body drawer sets up sprites in its own `Awake()`. Unity doesn't guarantee child Awake runs before parent Awake, so the first `SetFacing` call could fire before any sprite existed - and since `SetFacing` skips redraw when direction hasn't changed, it stayed blank forever. Moved the initial `SetFacing` call to `Start()` (all Awakes finish before any Start runs).
2. Nothing ever assigned a body/head. `HumanoidBodyDrawer` now grabs a random `BodyTypeDef` / `HeadTypeDef` from `DefDatabase` on Awake if none is set.

**Still will be invisible if:** there are zero BodyTypeDef / HeadTypeDef assets under `Resources/Defs/` yet. Random pick returns null if the database's empty - see Setup.md.

## Removed / renamed files (delete manually, see Setup.md)
- Scripts/Core/Stats.cs → replaced by EntityStats.cs
- Scripts/Entities/HumanoidEntity.cs → replaced by PlayerEntity.cs
- Scripts/Entities/AnimalEntity.cs → replaced by EnemyEntity.cs

## Performance
- SetFacing early-outs on unchanged direction, no redundant sprite writes
- DefDatabase caches lookups + a flat list for O(1) random picks, no re-scan per call
- No code comments (per project rule) - all documentation lives here instead
