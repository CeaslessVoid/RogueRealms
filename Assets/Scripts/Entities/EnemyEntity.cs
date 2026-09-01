namespace RogueRealms
{
    public class EnemyEntity : Entity
    {
        public EnemyStats stats = new EnemyStats();
        public override EntityStats BaseStats => stats;
    }
}
