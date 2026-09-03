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

    [CreateAssetMenu(fileName = "Weapon_", menuName = "RogueRealms/Defs/Weapon")]
    public class WeaponDef : Def
    {
        public WeaponType type;
        public Sprite sprite;

        [Tooltip("Degrees the art is drawn at. 0 = drawn pointing straight right. 45 = drawn on a diagonal. Used to correct rotation so the weapon still points exactly at the mouse.")]
        public float spriteAngleOffset = 0f;
    }
}
