# RogueRealms

## Implemented so far

**Core**
- Entity (abstract): move via Rigidbody2D, tracks facing, holds stats via `BaseStats`
- EntityStats: maxHealth, currentHealth, speed (int), speedScale
- EnemyStats : EntityStats + damageMultiplier
- PlayerStats : EntityStats + strength, range, wisdom, hope, defense, critChance (uncapped), dodge — all attributes default 0, speed defaults 20 (inherited)
- Direction / DirectionalSprites: N/E/S + auto-flip West
- IBodyDrawer: shared interface for Animal/Humanoid drawers

**Entities**
- PlayerEntity : Entity, uses PlayerStats
- EnemyEntity : Entity, uses EnemyStats
- Body type (animal vs humanoid) is just whichever drawer sits on the entity's child, not a class

**Defs**
- Def (base): defName, displayName, description — every def now has a name and desc
- DefDatabase<T>: load/lookup/cached random pick from Resources/Defs
- HairDef, BodyTypeDef, HeadTypeDef, ClothingDef (Body/Head slot)
- ClassDef: baseStats (PlayerStats, defaults 0/speed 20), passives (List<PassiveDef>), skills (List<SkillDef>), defaultClothing (List<ClothingDef>)
- PassiveDef, SkillDef: stub defs, name/desc/icon only for now

**Drawers**
- AnimalBodyDrawer: 1 sprite layer
- HumanoidBodyDrawer: 5 layers (Body, BodyClothing, Head, Hair, HeadClothing), auto-random body/head on Awake if unset
- Head, Hair, and HeadClothing now shift ±0.5 on X when facing East/West (+0.5 East, -0.5 West, 0 North/South) — Body and BodyClothing don't shift

**Player**
- PlayerController: WASD move, faces mouse, sprite never rotates

**Menu (new)**
- CharacterProfile: static holder for chosen body/head/hair/class, survives the scene load into Game
- MainMenuController: Play button → loads "Game" scene
- ClassSelectorUI: builds the class scrollview from DefDatabase<ClassDef>, exposes Next()/Previous() for arrow buttons, drives the description panel and the character preview
- ClassListItemButton: single row in the class scrollview
- ClassDescriptionPanel: right-side name + description display
- CharacterPreviewDisplay: the character shown in the middle, wears the selected class's default clothing, click opens the character editor
- CharacterEditorController: shows/hides the editor overlay vs class selection, Spin() cycles S→E→N→W on the preview, Exit() returns to class selection
- DefListItemButton: reusable list button — N/E/S preview images + name, used by all three appearance tabs
- HairSelectorUI / BodySelectorUI / HeadSelectorUI: scrollview + search bar for their respective Def type, applies selection live to the preview drawer and CharacterProfile

## Scenes
- **MainMenu**: class select, character preview, character editor
- **Game**: currently just the test player, no enemies yet

See Setup.md for wiring both scenes.

## Performance
- SetFacing early-outs on unchanged direction
- DefDatabase caches lookups + a flat list for random picks
- List filtering (search bars) only runs on text change, not per-frame
- No code comments — documentation lives here instead
