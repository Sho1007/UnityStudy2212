using System;
using System.Collections;
using System.Collections.Generic;
using RPG.SceneManageMent;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;
        
        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObject();
            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject PersistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(PersistentObject);
        }
    }
}