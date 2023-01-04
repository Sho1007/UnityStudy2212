using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5.0f;
        [SerializeField] float suspicionTime = 5.0f;
        [SerializeField] float waypointDwellTime = 3.0f;
        [SerializeField] PatrolPath patrolPath;

        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWayPoint = Mathf.Infinity;
        int currentWayPointIndex = 0;
        float wayPointTolerance = 1.0f;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            if (player == null) print(gameObject.name + "[AIController] : player is null");
            if (fighter == null) print(gameObject.name + "[AIController] : fighter is null");
            if (health == null) print(gameObject.name + "[AIController] : health is null");
            if (mover == null) print(gameObject.name + "[AIController] : mover is null");

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsPlayerInAttackRange() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWayPoint = 0.0f;
                    CycleWayPoint();
                }

                nextPosition = GetCurrentWayPoint();
            }
            
            if (timeSinceArrivedAtWayPoint >= waypointDwellTime) mover.StartMoveAction(nextPosition, patrolSpeedFraction);
                
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPointPosition(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            return Vector3.Distance(GetCurrentWayPoint(), transform.position) < wayPointTolerance;
        }

        private void SuspicionBehaviour()
        {
            mover.StartMoveAction(transform.position, 1.0f);
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0.0f;
            fighter.Attack(player);
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