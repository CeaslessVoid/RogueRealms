# RogueRealms

Full record of what's implemented and how it's wired. Updated in place every patch - always the whole picture, not a diff.

## Core
- **Entity** (abstract): moves via Rigidbody2D (`Move(dir)`), tracks `FacingDirection`, exposes stats through abstract `BaseStats`. `Awake()` finds its body drawer and Rigidbody2D; `Start()` pushes the initial facing (deliberately in Start, not Awake - see Drawers below for why).
- **EntityStats**: maxHealth, currentHealth, speed (int), speedScale. `CurrentMoveSpeed = speed * speedScale`. `TakeDamage`/`Heal`/`InitializeDefaults`.
- **EnemyStats : EntityStats** + damageMultiplier.
- **PlayerStats : EntityStats** + strength, range, wisdom, hope, defense, critChance (float, uncapped), dodge, dashCooldown (5), dashDistance (3), dashDuration (0.15). All attribute stats default 0; speed defaults 20 (inherited). Every dash number lives here, not on any component.
- **Direction / DirectionalSprites**: N/E/S sprites + optional explicit West, otherwise West = East flipped.
- **IBodyDrawer**: interface both AnimalBodyDrawer and HumanoidBodyDrawer implement.

## Entities
- **PlayerEntity : Entity** - `public PlayerStats stats`, `BaseStats => stats`.
- **EnemyEntity : Entity** - `public EnemyStats stats`, `BaseStats => stats`.
- Body type (animal vs humanoid) isn't a class - it's just whichever drawer component sits on the entity's child. Both entity types work with either drawer.

## Defs
RimWorld-style: `Def` (base ScriptableObject) has defName, displayName, description. `DefDatabase<T>` lazy-loads every asset of type T from any `Resources/Defs` folder, indexed by defName, with cached lookup/random-pick.

- **HairDef**: DirectionalSprites.
- **BodyTypeDef**: DirectionalSprites (Male/Female/Fat/Hulk/Thin etc).
- **HeadTypeDef**: HeadGender + DirectionalSprites.
- **ClothingDef**: ClothingSlot (Body/Head) + DirectionalSprites.
- **SkinToneDef / HairColorDef**: just a Color, no sprites.
- **ClassDef**: baseStats (PlayerStats), passives (List<PassiveDef>), skills (List<SkillDef>), defaultClothing (List<ClothingDef>), startingWeapons (List<WeaponDef>).
- **PassiveDef / SkillDef**: stub - displayName/description/icon only, no behavior yet.
- **WeaponDef**: type (Melee/Ranged/Magic/Consumable), sprite (single, drawn pointing right or on a diagonal), spriteAngleOffset (degrees the art is drawn at, so aiming math can correct for it). No usage/effects implemented - display and inventory only.
- **MapDef**: width, height (in tiles), tileSize. Just a size - see Map below.

## Drawers
- **AnimalBodyDrawer**: one SpriteRenderer, N/E/S + auto-flip West.
- **HumanoidBodyDrawer**: 5 layered SpriteRenderers - Body, BodyClothing, Head, Hair, HeadClothing. Independently swappable (SetBody/SetHead/SetHair/SetClothing/ClearClothing).
  - Head, Hair, HeadClothing shift on X when facing East/West (currently **±0.05**, tuned locally - don't reset this to 0.5). Body/BodyClothing never shift.
  - `SetSkinTone(SkinToneDef)` tints Body + Head renderers. `SetHairColor(HairColorDef)` tints Hair only. Both are plain multiply tints (`SpriteRenderer.color`) - black outlines stay black for free, but the source art needs to be white/grayscale fill + black outline for this to work; art with actual colored pixels will multiply against that color instead.
  - It's a dumb display component - doesn't randomize or load anything itself. `PlayerAppearanceController` is what actually populates it.

## Player & Camera (Game scene)
- **PlayerController**: WASD move, faces the mouse (bucketed N/E/S/W, sprite never rotates), and owns dash (Space bar). One script, no separate dash component.
  - Dash direction: current WASD input if moving, otherwise toward the mouse.
  - Moves over `dashDuration` seconds covering `dashDistance` units, then starts `dashCooldown`. Normal WASD movement is skipped for the duration of a dash so they don't fight over the Rigidbody.
  - Drives an optional `DashCooldownUI` if assigned.
- **CameraController**: follows the player, leans toward the mouse (clamped, not 1:1 distance), zooms with scroll wheel (orthographicSize).

## Weapons (display + inventory only, no usage/effects)
- **WeaponInventory**: 5 slots, filled from the selected class's startingWeapons on Start, `SelectSlot`/`NextSlot`/`PreviousSlot`, fires `OnChanged`.
- **WeaponInputController**: number keys 1-5 select a slot directly. (Scroll wheel isn't used for this - it's reserved for camera zoom.)
- **WeaponHolder**: positions the current weapon out from the player toward the mouse (orbits at a fixed distance, not a static offset), rotates to face the mouse, subtracts `spriteAngleOffset` so diagonally-drawn art still points true, flips vertically (`flipY`) whenever the mouse is left of the player.
- **WeaponSlotUI**: one HUD slot - icon + name, scales up when it's the active slot.
- **WeaponHudController**: drives all 5 WeaponSlotUI from the inventory.

**Input conflict note:** slot switching was asked to work via number keys *or* scrolling, and the camera was asked to zoom - scroll can't do both. Scroll drives camera zoom; slots are number-keys-only.

## Map
- **MapDef**: width, height, tileSize - the play area's size limit. Just data.
- **MapManager**: holds a MapDef + an origin point. `GetBounds()`/`Contains(worldPos)` for later use (nothing clamps to it yet - just defined and visible for now). Draws the boundary two ways:
  - A `LineRenderer` rectangle drawn once in `Awake()` (bounds are static, so no per-frame redraw) - this is what's actually visible during play, in the Game view and in builds.
  - `OnDrawGizmos()` still there too, for seeing the boundary in the Scene view while editing without pressing Play.
- **MapTilemaps**: just holds references to the Floor and Blood Tilemaps for later systems to use. No logic - blood/cleanup isn't implemented yet, this just reserves the layer.
- Floor tiles are hand-painted with Unity's built-in Tile Palette - no code needed for that part, see Setup.md.

## Menu & character creation (MainMenu scene)
- **CharacterProfile**: static holder - body, head, hair, skinTone, hairColor, selectedClass. Lives in memory for the session, survives the scene load into Game since it's just a static class.
- **CharacterSaveService**: PlayerPrefs-backed.
  - `EnsureProfileLoaded()` - call-once-per-session guard. Loads from PlayerPrefs if a save exists; if any saved def can't be resolved (renamed/deleted asset), self-heals with a fresh random pick instead of leaving it null. First launch (no save at all) randomizes body/head/hair/skinTone/hairColor.
  - `Save()` - writes CharacterProfile to PlayerPrefs. Called by MainMenuController when Play is pressed - this is the only thing that transfers character data into the Game scene.
- **PlayerAppearanceController**: applies CharacterProfile (loading/randomizing first if needed) to whatever HumanoidBodyDrawer is in its children - body, head, hair, skin tone, hair color, and the selected class's default clothing. Used on **both** the menu's CharacterPreview and the Game scene's Player.
- **MainMenuController**: Play button → `CharacterSaveService.Save()` → loads "Game" scene.
- **ClassSelectorUI**: builds the class scrollview from DefDatabase<ClassDef>, loads the profile first so it reselects whatever class was picked last time (falls back to index 0), `Next()`/`Previous()` for arrow buttons, drives the description panel + character preview.
- **ClassListItemButton**: one row in the class scrollview.
- **ClassDescriptionPanel**: right-side name + description display.
- **CharacterPreviewDisplay**: the character shown in the middle of the menu; wears the selected class's clothing; clicking it opens the character editor (`OnMouseDown`, needs a Collider2D).
- **CharacterEditorController**: shows/hides editor vs class-selection panels; `Spin()` cycles S→E→N→W on the preview; `Exit()` returns to class selection.
- **DefListItemButton**: reusable list button (N/E/S preview images + name), used by Hair/Body/Face tabs.
- **HairSelectorUI / BodySelectorUI / HeadSelectorUI**: scrollview + search bar per Def type, applies selection live to the preview drawer + CharacterProfile.
- **ColorSwatchButton**: a button that's just a colored square, used by the tone/color tabs.
- **SkinToneSelectorUI / HairColorSelectorUI**: scrollview (no search - there usually aren't many colors) of ColorSwatchButton, same live-apply pattern.

## UI widgets
- **DashCooldownUI**: two stacked Images - a static gray one behind, a full-color one in front set to Filled/Horizontal/Left in code. Fill drops to 0 on dash, refills left-to-right as cooldown counts down - the icon itself "fills up", not a separate bar. Timer text shows remaining seconds to 1 decimal, blank when ready.

## Prefabs / scene objects
- **Player** (Game scene): Rigidbody2D (Kinematic, gravity 0) + PlayerEntity + PlayerController (cooldownUI assigned) + PlayerAppearanceController + WeaponInventory + WeaponInputController. Child with HumanoidBodyDrawer and its 5 SpriteRenderers (Body, BodyClothing, Head, Hair, HeadClothing). Separate child `WeaponAnchor` with SpriteRenderer + WeaponHolder (player = the root transform).
- **Main Camera** (Game scene): CameraController, target = Player.
- **Map** (Game scene): MapManager + LineRenderer (auto-added), mapDef assigned.
- **Grid** (Game scene): Grid component + child Tilemaps "Floor" and "Blood". Optionally a MapTilemaps component with both assigned.
- **CharacterPreview** (MainMenu scene): HumanoidBodyDrawer (same 5-renderer setup) + Collider2D + CharacterPreviewDisplay + PlayerAppearanceController.
- **ClassItemPrefab**: Button + TMP_Text + ClassListItemButton, instantiated into the class scrollview.
- **DefListItemButton prefab**: Button + 3 Images (N/E/S) + TMP_Text, reused for Hair/Body/Face tabs.
- **ColorSwatchButton prefab**: Button + Image, reused for Skin Tone/Hair Color tabs.
- **WeaponSlotUI prefab**: Image (icon) + TMP_Text, instantiated ×5 for the weapon HUD.
- **Dash icon**: two overlapping Image GameObjects (gray + color) + a TMP_Text timer, driven by one DashCooldownUI.

## Character persistence
Body, head, hair, skin tone, and hair color are saved permanently via PlayerPrefs - same character every launch after the first (which randomizes all of them). Class is remembered as "last picked" but always changeable before Play. Nothing else persists - no stats, items, or run progress. True roguelike: character identity only.

## Scenes
- **MainMenu**: class select, character preview, character editor.
- **Game**: player, camera, weapon holding/HUD, dash, map boundary + floor tiles. Still no enemies.

See Setup.md for wiring steps (only for what's new/changed in the latest patch - older wiring isn't repeated once it's done).

## Performance
- SetFacing early-outs on unchanged direction.
- DefDatabase caches lookups + a flat list for random picks.
- Skin tone / hair color tinting is a color set, not a shader - free.
- List filtering (search bars) only runs on text change, not per-frame.
- Weapon holding / camera follow / dash are plain per-frame math, no allocations, no GetComponent in Update.
- Map border is drawn once (bounds are static, no reason to redraw every frame).
- No code comments — documentation lives here instead.
