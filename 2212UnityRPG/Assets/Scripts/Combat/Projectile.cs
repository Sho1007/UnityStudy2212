using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    Health target = null;
    float damage = 0.0f;

    [SerializeField] float maxLiveTime = 10.0f; // 생존 시간 임시 코드
    float nowLiveTime = 0.0f;                   // 생존 시간 임시 코드
    
    void Update()
    {
        nowLiveTime += Time.deltaTime;
        if (nowLiveTime > maxLiveTime) Destroy(this.gameObject);
        if (target != null)
        {
            transform.LookAt(GetAimLocation());
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Health>() != target) return;
        target.TakeDamage(damage);
        Destroy(this.gameObject);
    }


    private Vector3 GetAimLocation()
    {
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
        if (collider != null) return target.transform.position + target.transform.up * collider.height * 0.5f;

        return target.transform.position;
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }
}
