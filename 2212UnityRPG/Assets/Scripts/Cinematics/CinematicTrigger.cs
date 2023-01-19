using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Movement;
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool isPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !isPlayed)
            {
                isPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        public object CaptureState()
        {
            return isPlayed;
        }

        public void RestoreState(object state)
        {
            isPlayed = (bool)state;
        }
    }
}


