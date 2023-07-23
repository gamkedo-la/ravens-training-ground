using System;
using UnityEngine;

namespace Character.Stats
{
    public class Experience : MonoBehaviour {
        [SerializeField] int experience = 0;

        public event Action onGainedExperience;
        public void GrantExperience(int experienceToAdd) {
            experience += experienceToAdd;
            onGainedExperience();
        }

        public int GetExperience() {
            return experience;
        }
    } 
}