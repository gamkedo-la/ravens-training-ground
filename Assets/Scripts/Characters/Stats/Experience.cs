using System;
using UnityEngine;

namespace Character.Stats
{
    public class Experience : MonoBehaviour {
        [SerializeField] int experience = 0;

        public event Action OnGainedExperience;
        public void GrantExperience(int experienceToAdd) {
            Debug.Log("Experience Granted:" + experienceToAdd);
            experience += experienceToAdd;
            if(OnGainedExperience != null)
            OnGainedExperience();
        }

        public int GetExperience() {
            return experience;
        }
    } 
}