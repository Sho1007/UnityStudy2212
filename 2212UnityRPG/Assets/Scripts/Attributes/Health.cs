using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        float healthPoints = -1.0f;
        private bool isDead = false;

        private void Start()
        {
            if (healthPoints < 0)
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (isDead) return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(gameObject.name + " current health : " + healthPoints);
            if (healthPoints == 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }
        
        public float GetPercentage()
        {
            return 100 * healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
                    experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
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