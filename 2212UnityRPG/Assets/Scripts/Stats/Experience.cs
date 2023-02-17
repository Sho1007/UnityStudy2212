using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0.0f;
        // Action : return type 이 없는 사전에 정의된 delegate
        public event Action OnExperienceGained;

        public float GetExperiencePoints() {return experiencePoints;}

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
