using System;
using UnityEngine;

namespace Units
{
    public class HealthSystem : MonoBehaviour
    {
        public event EventHandler OnDead;
        public event EventHandler OnDamaged;
    
        [SerializeField] private int health = 100;
        [SerializeField] private int maxHealth = 100;

        public void TakeDamage(int damageAmount)
        {
            health -= damageAmount;
        
            OnDamaged?.Invoke(this, EventArgs.Empty);

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        private void Die()
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }

        public float GetHealthNormalized()
        {
            return (float)health / maxHealth;
        }
    }
}
