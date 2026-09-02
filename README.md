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
- Def (base): defName, displayName, description
- DefDatabase<T>: load/lookup/cached random pick from Resources/Defs
- HairDef, BodyTypeDef, HeadTypeDef, ClothingDef (Body/Head slot)
- ClassDef: baseStats (PlayerStats, defaults 0/speed 20), passives, skills, defaultClothing
- PassiveDef, SkillDef: stub defs, name/desc/icon only

**Drawers**
- AnimalBodyDrawer: 1 sprite layer
- HumanoidBodyDrawer: 5 layers (Body, BodyClothing, Head, Hair, HeadClothing)
- Head, Hair, HeadClothing shift ±0.5 on X facing East/West, 0 on North/South. Body/BodyClothing don't shift.
- **Changed:** HumanoidBodyDrawer no longer randomizes its own body/head on Awake. It's a dumb display component now — whatever calls SetBody/SetHead/SetHair/SetClothing controls what it shows. See PlayerAppearanceController below.

**Player**
- PlayerController: WASD move, faces mouse, sprite never rotates

**Menu**
- CharacterProfile: static holder for the current body/head/hair/class, in memory for the session
- CharacterSaveService: PlayerPrefs-backed save/load for CharacterProfile.
  - `EnsureProfileLoaded()` - call-once-per-session. Loads from PlayerPrefs if a save exists, otherwise randomizes body/head/hair (first launch case) and leaves it in CharacterProfile without writing to disk yet.
  - `Save()` - writes current CharacterProfile to PlayerPrefs. Called by MainMenuController when Play is pressed.
- PlayerAppearanceController: applies CharacterProfile (loading/randomizing it first if needed) to whatever HumanoidBodyDrawer is in its children, plus the selected class's default clothing. Used on **both** the menu's CharacterPreview and the Game scene's Player - this is what actually makes them show up now.
- MainMenuController: Play button → saves the profile → loads "Game" scene
- ClassSelectorUI: builds the class scrollview, loads the profile first so it reselects whatever class was picked last time (falls back to the first class if none saved), Next()/Previous() for arrow buttons, drives description panel + preview
- ClassListItemButton, ClassDescriptionPanel, CharacterPreviewDisplay, CharacterEditorController, DefListItemButton, HairSelectorUI, BodySelectorUI, HeadSelectorUI: unchanged from last patch

## Character persistence
- Body, head, and hair are saved permanently (PlayerPrefs) - same character every time you launch the game, after the first launch (which randomizes body/head/hair, no head randomization was skipped - it's included too, since a head-less character isn't playable).
- Class is also remembered as "last picked", but you can always change it in the menu before hitting Play - it's not locked in.
- Nothing else persists. No stats, no items, no run progress - true roguelike, character identity only.

## Scenes
- **MainMenu**: class select, character preview, character editor
- **Game**: currently just the test player, no enemies yet

See Setup.md for wiring both scenes, including the new PlayerAppearanceController placement.

## Performance
- SetFacing early-outs on unchanged direction
- DefDatabase caches lookups + a flat list for random picks
- List filtering (search bars) only runs on text change, not per-frame
- No code comments — documentation lives here instead
