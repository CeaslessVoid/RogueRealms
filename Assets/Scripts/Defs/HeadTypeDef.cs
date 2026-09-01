using UnityEngine;

namespace RogueRealms
{
    public enum HeadGender
    {
        Male,
        Female
    }

    [CreateAssetMenu(fileName = "Head_", menuName = "RogueRealms/Defs/Head Type")]
    public class HeadTypeDef : Def
    {
        public HeadGender gender;
        public DirectionalSprites sprites;
    }
}
