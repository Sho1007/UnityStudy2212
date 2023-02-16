using UnityEngine;
using System.Collections.Generic;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(Stat stat, CharacterClass targetClass, int targetLevel)
        {
            foreach(ProgressionCharacterClass iter in characterClasses)
            {
                if (iter.characterClass != targetClass) continue;

                foreach (ProgressionStat progressionStat in iter.stats)
                {
                    if (progressionStat.stat != stat) continue;

                    if (progressionStat.levels.Length < targetLevel)
                    {
                        Debug.LogError(targetClass + " has no " + stat + " data in level " + targetLevel);
                    }

                    return progressionStat.levels[targetLevel-1];
                }
            }

            return 0;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }
    }
}