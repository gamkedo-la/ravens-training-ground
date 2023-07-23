using Character.Stats;
using System;
using UnityEngine;

namespace Character.BaseStats
{
    public class StatProgression : MonoBehaviour
    {
        [SerializeField] int startingLevel = 1;
        [SerializeField] StatProgression[] statProgressions = null;

        Experience experience;
        [SerializeField] int currentLevel;
        private int[] experienceRequirements = new int[] { 100, 200, 300, 400, 500, 1000 };

        private void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = CalculateLevel();
        }

        private int CalculateLevel() {
            for (int i = 0;  i < experienceRequirements.Length; i++) {
                if (experienceRequirements[i] > experience.GetExperience()) {
                    return i;
                }
            }
            return startingLevel;
        }
    }
}