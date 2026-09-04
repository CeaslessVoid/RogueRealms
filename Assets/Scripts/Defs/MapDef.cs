using UnityEngine;

namespace RogueRealms
{
    [CreateAssetMenu(fileName = "Map_", menuName = "RogueRealms/Defs/Map")]
    public class MapDef : Def
    {
        public int width = 50;
        public int height = 50;
        public float tileSize = 1f;
    }
}
