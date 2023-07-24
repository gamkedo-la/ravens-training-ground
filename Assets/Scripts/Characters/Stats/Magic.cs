using System;
using UnityEngine;

namespace Character.Stats { 
    public class Magic : MonoBehaviour {
        [SerializeField] int magicpoints;

        public int GetMaxMP() {
            return GetComponent<BaseStats>().GetStat(Stat.Magic);
        }

        public int GetCurrentMP() {
            return magicpoints;
        }

        public void AddMagic(int MagicToAdd) {
            magicpoints = Math.Min(magicpoints + MagicToAdd, GetComponent<BaseStats>().GetStat(Stat.Magic));
        }

        public void RegainMaxMP() {
            magicpoints = GetComponent<BaseStats>().GetStat(Stat.Magic);
        }

        public void UseMagic(int amountUsed) {
            magicpoints = Mathf.Max(magicpoints - amountUsed, 0);
        }
    }
}