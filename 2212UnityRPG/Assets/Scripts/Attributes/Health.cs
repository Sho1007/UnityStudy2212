using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100.0f;
        float maxHealthPoints;
        private bool isDead = false;

        private void Start()
        {
            healthPoints = maxHealthPoints  = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(float damage)
        {
            if (isDead) return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(gameObject.name + " current health : " + healthPoints);
            if (healthPoints == 0)
            {
                Die();
            }
        }
        
        public float GetPercentage()
        {
            return 100 * healthPoints / maxHealthPoints;
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}