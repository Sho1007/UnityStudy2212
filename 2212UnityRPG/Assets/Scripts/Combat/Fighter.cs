using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    [RequireComponent(typeof(ActionScheduler), typeof(Animator), typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 5.0f;
        [SerializeField] float timeBetweenAttacks = 1.0f;
        [SerializeField] float weaponDamage = 5.0f;
        Health target;
        private float timeSinceLastAttack = 0.0f;

        private void Start()
        {
            timeSinceLastAttack = timeBetweenAttacks;
        }

        private void Update()
        {
            if (timeSinceLastAttack < timeBetweenAttacks)
                timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange()) GetComponent<Mover>().MoveTo(target.transform.position, 1.0f);
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                timeSinceLastAttack = 0;
                TriggerAttack();
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            // This will trigger the Hit() Event.
            GetComponent<Animator>().SetTrigger("Attack");
        }

        // Animation Event
        private void Hit()
        {
            if (target)
                target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health health = combatTarget.GetComponent<Health>();
            return health != null && !health.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        
        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }

        public float GetWeaponRange() {return weaponRange;}
    }

}