using UnityEngine;

namespace RogueRealms
{
    public enum ClothingSlot
    {
        Body,
        Head
    }

    [CreateAssetMenu(fileName = "Clothing_", menuName = "RogueRealms/Defs/Clothing")]
    public class ClothingDef : Def
    {
        [Tooltip("Which layer this clothing draws over - the body sprite or the head sprite.")]
        public ClothingSlot slot;

        public DirectionalSprites sprites;
    }
}
