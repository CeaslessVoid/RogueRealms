namespace RogueRealms
{
    [System.Serializable]
    public class PlayerStats : EntityStats
    {
        public int strength = 10;
        public int range = 10;
        public int wisdom = 10;
        public int hope = 10;
        public int defense = 10;
        public float critChance = 5f;
        public int dodge = 5;
    }
}
