using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equipedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 5.0f;
        [SerializeField] float weaponRange = 5.0f;
        [SerializeField] bool isRightHanded = true;

        [SerializeField] Projectile projectile = null;

        const string weaponName = "WEAPON";

        public void Spawn(Transform leftHand, Transform rightHand, Animator animator)
        {
            DestroyOldWeapon(leftHand, rightHand);

            if (equipedPrefab != null)
            {
                GameObject weapon = Instantiate(equipedPrefab, GetHandTransform(leftHand, rightHand));
                weapon.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null) 
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform leftHand, Transform rightHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target)
        {
            if (!HasProjectile()) return;
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(leftHand, rightHand).position, Quaternion.identity);
            if (isRightHanded) projectileInstance.transform.position = rightHand.position;
            else projectileInstance.transform.position = leftHand.position;
            projectileInstance.SetTarget(target, weaponDamage);
        }

        private Transform GetHandTransform(Transform leftHandTransform, Transform rightHandTransform)
        {
            return isRightHanded ? rightHandTransform : leftHandTransform;
        }

        public bool HasProjectile() {return projectile != null;}

        public float GetRange() {return weaponRange;}
        public float GetDamage() {return weaponDamage;}
    }
}