using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5.0f;
        private GameObject player;
        private Fighter fighter;
        private Health health;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            if (player == null) print(gameObject.name + "[AIController] : player is null");
            if (fighter == null) print(gameObject.name + "[AIController] : fighter is null");
            if (health == null) print(gameObject.name + "[AIController] : health is null");
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsPlayerInAttackRange() && fighter.CanAttack(player)) fighter.Attack(player);
            else fighter.Cancel();
        }

        private bool IsPlayerInAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return  distanceToPlayer <= chaseDistance;
        }
        
        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}