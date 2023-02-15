using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using RPG.Saving;
using System;

namespace RPG.Combat
{

    [RequireComponent(typeof(ActionScheduler), typeof(Animator), typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [System.Serializable]
        struct FighterSaveData
        {
            public string weaponName;
        };

        [SerializeField] float timeBetweenAttacks = 1.0f;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        Weapon currentWeapon = null;
        Health target;
        private float timeSinceLastAttack = 0.0f;
        private void Start()
        {
            timeSinceLastAttack = timeBetweenAttacks;

            if (currentWeapon == null)
                EquipWeapon(defaultWeapon);
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

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == currentWeapon) return;

            currentWeapon = weapon;
            currentWeapon.Spawn(leftHandTransform, rightHandTransform, GetComponent<Animator>());
        }

        private void AttackBehavior()
        {
            if (target.IsDead())
            {
                Cancel();
                return;
            }
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
            if (currentWeapon == null)
            {
                Debug.LogError(gameObject.name +  "[Fighter] : Weapon is null");
                return;
            } else if (target == null) return;

            if (currentWeapon.HasProjectile()) currentWeapon.LaunchProjectile(leftHandTransform, rightHandTransform, target);
            else target.TakeDamage(currentWeapon.GetDamage());
        }

        private void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            if (currentWeapon == null)
            {
                Debug.LogError(gameObject.name +  " : Weapon is null");
                return false;
            }
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
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

        public object CaptureState()
        {
            FighterSaveData data;
            data.weaponName = currentWeapon.name;

            return data;
        }

        public void RestoreState(object state)
        {
            FighterSaveData data = (FighterSaveData)state;
            Weapon weapon = Resources.Load<Weapon>("Weapons/" + data.weaponName);
            EquipWeapon(weapon);
        }

        public Health GetTarget() {return target;}
    }
}