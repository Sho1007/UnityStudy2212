using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        };

        NavMeshAgent    navMeshAgent;
        Health          health;

        [SerializeField] float maxSpeed = 6.0f;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            if (navMeshAgent == null) print("[Mover] : navMehsAgent is null in " + gameObject.name);
            if (health == null) print("[Mover] : health is null in " + gameObject.name);
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void SetSpeed(float speed)
        {
            navMeshAgent.speed = speed;
        }
        

        private void UpdateAnimator()
        {
            GetComponent<Animator>().SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);

            // Dictionary<string, object> data = new Dictionary<string, object>();
            // data["position"] = new SerializableVector3(transform.position);
            // data["rotation"] = new SerializableVector3(transform.eulerAngles);

            return data;
        }
        public void RestoreState(object state)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            
            // Dictionary<string, object> data = (Dictionary<string, object>)state;
            // transform.position = ((SerializableVector3)data["position"]).ToVector();
            // transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();

            MoverSaveData data = (MoverSaveData)state;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            

            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}