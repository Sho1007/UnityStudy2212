using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Movement;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
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
    }
}


