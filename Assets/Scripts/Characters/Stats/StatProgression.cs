using System;
using UnityEngine;

namespace Character.Stats
{
    [CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/StatProgression", order = 1)]
    public class StatProgression : ScriptableObject
    {

        [SerializeField] public string statName;

        [SerializeField] private float[] statProgresionRate;
        [SerializeField] private int statProgresionAmount;
        [SerializeField] private int metallicModifier = 0;
        [SerializeField] private int baseAmount = 20;

        private int startLevel = 0;
        private int endLevel;
        public int GetStat(int level) {
            endLevel = statProgresionRate.Length;
            if (level > endLevel || level < startLevel) {
                return 0;
            } else {
                var currentModifier = 0;
                for (var i = 0; i < level; i++) {
                    currentModifier += (baseAmount + (int) Math.Round(statProgresionRate[i] * (level * (statProgresionAmount + metallicModifier)), 0));
                }
                return currentModifier;
            }
        }
    }
}