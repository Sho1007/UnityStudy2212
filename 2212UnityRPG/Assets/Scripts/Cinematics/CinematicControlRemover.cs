using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
            if (player == null) print(gameObject.name + " : [CinematicControlRemover] player is null");
        }

        public void DisableControl(PlayableDirector director)
        {
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            print("Disable Control");   
        }

        public void EnableControl(PlayableDirector director)
        {
            player.GetComponent<PlayerController>().enabled = true;
            print("Enable Control");
        }

    }
}