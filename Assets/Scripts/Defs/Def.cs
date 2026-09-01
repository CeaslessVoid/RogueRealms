using UnityEngine;

namespace RogueRealms
{
    public abstract class Def : ScriptableObject
    {
        public string defName;
        public string displayName;
        [TextArea] public string description;
    }
}
