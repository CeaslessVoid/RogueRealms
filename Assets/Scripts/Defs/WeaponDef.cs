using UnityEngine;

namespace RogueRealms
{
    public enum WeaponType
    {
        Melee,
        Ranged,
        Magic,
        Consumable
    }

    public enum MeleeSwingType
    {
        Impact,
        Slashing,
        Puncture
    }

    [CreateAssetMenu(fileName = "Weapon_", menuName = "RogueRealms/Defs/Weapon")]
    public class WeaponDef : Def
    {
        public WeaponType type;
        public Sprite sprite;

        [Tooltip("Degrees the art is drawn at. 0 = drawn pointing straight right. 45 = drawn on a diagonal.")]
        public float spriteAngleOffset = 0f;

        [Tooltip("Base visual/hitbox size. Multiplied by the player's weaponSizeMultiplier stat.")]
        public float defaultSize = 1f;

        [Tooltip("Only used when type is Melee.")]
        public MeleeSwingType swingType;
    }
}
