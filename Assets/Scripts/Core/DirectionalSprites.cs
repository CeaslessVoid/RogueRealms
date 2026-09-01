using UnityEngine;

namespace RogueRealms
{
    [System.Serializable]
    public class DirectionalSprites
    {
        public Sprite north;
        public Sprite east;
        public Sprite south;

        [Tooltip("Optional.")]
        public Sprite west;

        public Sprite GetSprite(Direction dir, out bool flipX)
        {
            flipX = false;
            switch (dir)
            {
                case Direction.North: return north;
                case Direction.South: return south;
                case Direction.East: return east;
                case Direction.West:
                    if (west != null) return west;
                    flipX = true;
                    return east;
            }
            return south;
        }
    }
}
