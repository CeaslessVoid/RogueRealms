using UnityEngine;

namespace RogueRealms
{
    [System.Serializable]
    public class EntityStats
    {
        public int maxHealth = 100;
        public int currentHealth;

        public int speed = 20;
        protected float speedScale = 0.1f;

        public float CurrentMoveSpeed => speed * speedScale;
        public bool IsDead => currentHealth <= 0;

        public void InitializeDefaults()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
        }

        public void Heal(int amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        }
    }
}
