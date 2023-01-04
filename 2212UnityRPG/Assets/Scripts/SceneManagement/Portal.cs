using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManageMent
{
    public class Portal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") print("Portal Triggered");
        }
    }
}