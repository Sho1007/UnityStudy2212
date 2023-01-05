using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManageMent
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int SceneToLoad = -1;
        [SerializeField] Transform spawnPoint;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                IEnumerator transition = Transition();
                print(transition.Current);
                StartCoroutine(transition);
                print(transition.Current);
            }
        }

        private IEnumerator Transition()
        {
            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in GameObject.FindObjectsOfType<Portal>())
            {
                if (portal != gameObject) return portal; 
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.transform.rotation;
            
        }
    }
}