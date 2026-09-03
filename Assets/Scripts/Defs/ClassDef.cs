using System.Collections.Generic;
using UnityEngine;

namespace RogueRealms
{
    [CreateAssetMenu(fileName = "Class_", menuName = "RogueRealms/Defs/Class")]
    public class ClassDef : Def
    {
        public PlayerStats baseStats = new PlayerStats();
        public List<PassiveDef> passives = new List<PassiveDef>();
        public List<SkillDef> skills = new List<SkillDef>();
        public List<ClothingDef> defaultClothing = new List<ClothingDef>();
        public List<WeaponDef> startingWeapons = new List<WeaponDef>();
    }
}
