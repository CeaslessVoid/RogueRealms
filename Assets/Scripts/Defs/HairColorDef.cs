using UnityEngine;

namespace RogueRealms
{
    [CreateAssetMenu(fileName = "HairColor_", menuName = "RogueRealms/Defs/Hair Color")]
    public class HairColorDef : Def
    {
        public Color color = Color.white;
    }
}
