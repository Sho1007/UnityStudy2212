using UnityEngine;
using System.Collections.Generic;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass targetClass, int targetLevel)
        {
            foreach(ProgressionCharacterClass iter in characterClasses)
            {
                if (iter.GetClass() == (CharacterClass)targetClass)
                {
                    return iter.GetHealth(targetLevel-1);
                }
            }

            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass GetClass() {return characterClass;}
            public float GetHealth(int level)
            {
                if (level >= health.Length)
                {
                    Debug.LogError("[Progression] : " + characterClass + " has no health data in level " + (level + 1));
                }
                return health[level];
            }
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] health;
        }
    }
}