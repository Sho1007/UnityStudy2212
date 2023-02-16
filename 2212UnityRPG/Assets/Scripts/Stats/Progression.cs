using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass targetClass, int targetLevel)
        {
            BuildLookup();
            // foreach(ProgressionCharacterClass iter in characterClasses)
            // {
            //     if (iter.characterClass != targetClass) continue;

            //     foreach (ProgressionStat progressionStat in iter.stats)
            //     {
            //         if (progressionStat.stat != stat) continue;

            //         if (progressionStat.levels.Length < targetLevel)
            //         {
            //             Debug.LogError(targetClass + " has no " + stat + " data in level " + targetLevel);
            //         }

            //         return progressionStat.levels[targetLevel-1];
            //     }
            // }

            // return 0;
            if (lookupTable[targetClass][stat].Length < targetLevel)
            {
                Debug.LogError(targetClass + " has no " + stat + " data in level " + targetLevel);
                return 0;
            }
            return lookupTable[targetClass][stat][targetLevel-1];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                lookupTable.Add(progressionCharacterClass.characterClass, new Dictionary<Stat, float[]>());
                foreach (ProgressionStat progressionStat in progressionCharacterClass.stats)
                {
                    lookupTable[progressionCharacterClass.characterClass].Add(progressionStat.stat, progressionStat.levels);
                }
            }
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