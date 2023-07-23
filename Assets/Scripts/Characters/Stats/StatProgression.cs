using System;
using UnityEngine;

namespace Character.Stats
{
    [CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/StatProgression", order = 1)]
    public class StatProgression : ScriptableObject
    {
        [SerializeField] float[] statProgresionRate;
        [SerializeField] int statProgresionAmount;
        private int startLevel = 0;
        private int endLevel;
        public int GetStat(int level) {
            endLevel = statProgresionRate.Length;
            // TODO: Change progression per stat
            if (level < endLevel && level > startLevel) {
                return (level * 10);
            } else {
                var currentModifier = 0;
                for (var i = 0; i < statProgresionRate.Length; i++) {
                    currentModifier += (int) Math.Round(statProgresionRate[i] * (level * statProgresionAmount), 0);
                }
                return currentModifier;
            }
        }
    }
}