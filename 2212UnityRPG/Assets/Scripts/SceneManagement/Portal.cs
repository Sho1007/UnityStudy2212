using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManageMent
{
    public class Portal : MonoBehaviour
    {
        bool isActivated = false;
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] DestinationIdentifier destination;
        [SerializeField] int SceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float fadeOutTime = 1.0f;
        [SerializeField] float fadeWaitTime = 0.5f;
        [SerializeField] float fadeInTime = 2.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (isActivated) return;
                isActivated = true;
                IEnumerator transition = Transition();
                StartCoroutine(transition);
            }
        }

         private IEnumerator Transition()
        {
            if (SceneToLoad < 0)
            {
                Debug.LogError("Scene to Load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = GameObject.FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            SavingWrapper savingWrapper = GameObject.FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSecondsRealtime(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in GameObject.FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination == this.destination)
                {
                    return portal;
                }
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}