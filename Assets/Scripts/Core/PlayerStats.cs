namespace RogueRealms
{
    [System.Serializable]
    public class PlayerStats : EntityStats
    {
        public int strength = 0;
        public int range = 0;
        public int wisdom = 0;
        public int hope = 0;
        public int defense = 0;
        public float critChance = 0f;
        public int dodge = 0;

        public float dashCooldown = 5f;
        public float dashDistance = 3f;
    }
}
