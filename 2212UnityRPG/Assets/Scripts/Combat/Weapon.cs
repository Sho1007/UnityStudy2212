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

        public void Spawn(Transform leftHand, Transform rightHand, Animator animator)
        {
            if (equipedPrefab != null) 
            {
                Instantiate(equipedPrefab, GetHandTransform(leftHand, rightHand));
            }
            if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
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