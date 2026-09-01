namespace RogueRealms
{
    public class PlayerEntity : Entity
    {
        public PlayerStats stats = new PlayerStats();
        public override EntityStats BaseStats => stats;
    }
}
