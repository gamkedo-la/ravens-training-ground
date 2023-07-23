using Character.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Stats
{
    public class BaseStats : MonoBehaviour
    {
        private int startingLevel = 1;
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

        public bool HasStat(Stat stat) {
            List<String> availableStats = (from currentStats in statProgressions.ToList() select name).ToList();
            return availableStats.Contains(stat.ToString());
        }

        public int GetStat(Stat statToGetValueOf) {
            if (HasStat(statToGetValueOf)) {
                if (statProgressions != null) {
                   List<StatProgression> availableStats = statProgressions.ToList();
                   StatProgression desiredStat = availableStats.Where(stat => stat.statName == statToGetValueOf.ToString()).First();
                   return desiredStat.GetStat(currentLevel);
                } else {
                    throw new Exception($" {this.ToString()} has null statProgressionss");
                }
            } else {
                throw new Exception($" {this.ToString()} does not have requested stat {statToGetValueOf}");
            }
        }
    }
}