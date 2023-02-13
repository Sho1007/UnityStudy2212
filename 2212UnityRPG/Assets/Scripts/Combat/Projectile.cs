using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] bool isHoming = true;
        [SerializeField] float speed = 1.0f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2.0f;
        Health target = null;
        float damage = 0.0f;

        [SerializeField] float maxLifeTime = 10.0f;
        

        private void Start() {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (isHoming && !target.IsDead()) transform.LookAt(GetAimLocation());
            
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target || target.IsDead()) return;
            target.TakeDamage(damage);

            speed = 0.0f;

            if (hitEffect != null) Instantiate(hitEffect, transform.position, transform.rotation);
            
            
            foreach (GameObject obj in destroyOnHit)
            {
                Destroy(obj);
            }

            Destroy(gameObject, lifeAfterImpact);
        }


        private Vector3 GetAimLocation()
        {
            CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
            if (collider != null) return target.transform.position + target.transform.up * collider.height * 0.5f;

            return target.transform.position;
        }

        public void SetTarget(Health target, float damage)
        {
            Destroy(gameObject, maxLifeTime);
            this.target = target;
            this.damage = damage;
            transform.LookAt(GetAimLocation());
        }
    }
}

