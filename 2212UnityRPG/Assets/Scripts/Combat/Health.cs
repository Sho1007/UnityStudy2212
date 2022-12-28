using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100.0f;
        private bool isDead = false;

        public void TakeDamage(float damage)
        {
            if (isDead) return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Animator>().SetTrigger("Die");
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}