using Character.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.Stats
{
    public class BaseStats : MonoBehaviour
    {
        private int startingLevel = 1;
        [SerializeField] StatProgression[] statProgressions = null;

        Experience experience;
        [SerializeField] int currentLevel;
        [SerializeField] int[] experienceRequirements = new int[] { 100, 200, 300, 400, 500, 1000 };

        private float magicEnhancementAmount;
        private float physicalEnhancementAmount;
        private float finesseEnhancementAmount;
        private float agilityEnhancementAmount;
        private float healthEnhancementAmount;

        Enhancement[] enhancements = new Enhancement[0];
        public void Initialize()
        {
            
        }
        public void RegisterEnhancementEvent(Enhancement enhancement) {
            enhancements.Append(enhancement);
            enhancement.OnEnhancementEvent += HandleNewStatEnhancement;
        }

        public bool HasStat(Stat targetStat) {
            List<StatProgression> statList = statProgressions.ToList();
            int statIndex = statList.FindIndex(stat => stat.statName.Equals(targetStat.ToString()));
            return statIndex > -1;
        }

        public int GetStat(Stat targetStat) {
            switch (targetStat) {
                case Stat.Finesse: {
                        return GetBaseStat(targetStat) + (int) finesseEnhancementAmount;
                    }
                case Stat.Physical: {
                        return GetBaseStat(targetStat) + (int) physicalEnhancementAmount;
                    }
                case Stat.Health: {
                        return GetBaseStat(targetStat) + (int) healthEnhancementAmount;
                    }
                case Stat.Agility: {
                        return GetBaseStat(targetStat) + (int) agilityEnhancementAmount;
                    }
                case Stat.Magic: {
                        return GetBaseStat(targetStat) + (int) magicEnhancementAmount;
                    }
                default:
                    return GetBaseStat(targetStat);
            }
        }

        private int GetBaseStat(Stat targetStat) {
            if (HasStat(targetStat)) {
                if (statProgressions != null) {
                   List<StatProgression> availableStats = statProgressions.ToList();
                   StatProgression desiredStat = availableStats.Where(stat => stat.statName == targetStat.ToString()).First();
                   int statBase = desiredStat.GetStat(currentLevel);
                   return statBase;
                } else {
                    throw new Exception($" {this.ToString()} has null statProgressionss");
                }
            } else {
                throw new Exception($" {this.ToString()} does not have requested stat {targetStat}");
            }
        }

        private void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = CalculateLevel();
        }

        private int CalculateLevel() {
            for (int i = 0; i < experienceRequirements.Length; i++) {
                if (experienceRequirements[i] > experience.GetExperience()) {
                    return i + 1;
                }
            }
            return startingLevel;
        }

        private void HandleNewStatEnhancement(object sender, EnhancementStatAmountArgs e) {
            Stat affectedStat = e.EffectedStat;
            float enhancementAmount = e.EnhancementAmount;
            print(enhancementAmount);
            switch (affectedStat) {
                case Stat.Magic: {
                        magicEnhancementAmount = enhancementAmount;
                        break;
                    }
                case Stat.Finesse: {
                        finesseEnhancementAmount = enhancementAmount;
                        break;
                    }
                case Stat.Physical: {
                        physicalEnhancementAmount = enhancementAmount;
                        break;
                    }
                case Stat.Health: {
                        healthEnhancementAmount = enhancementAmount;
                        break;
                    }
                case Stat.Agility: {
                        agilityEnhancementAmount = enhancementAmount;
                        break;
                    }
            }
        }
    }
}