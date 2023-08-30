using System;
using UnityEngine;

namespace Character.Stats { 
    public class Magic : MonoBehaviour {
        [SerializeField] int magicpoints;

        public int MagicPoints
        {
            set
            {
                magicpoints = value;
                if (MagicPointsChangedEvent != null)
                    MagicPointsChangedEvent(magicpoints);
            }
            get
            {
                return magicpoints;
            }
        }

        public delegate void MagicPointsChanged(int magic);
        public event MagicPointsChanged MagicPointsChangedEvent;

        public int GetMaxMP() {
            return GetComponent<BaseStats>().GetStat(Stat.Magic);
        }

        public int GetCurrentMP() {
            return MagicPoints;
        }

        public void AddMagic(int MagicToAdd) {
            MagicPoints = Math.Min(MagicPoints + MagicToAdd, GetComponent<BaseStats>().GetStat(Stat.Magic));
        }

        public void RegainMaxMP() {
            MagicPoints = GetComponent<BaseStats>().GetStat(Stat.Magic);
        }

        public void UseMagic(int amountUsed) {
            MagicPoints = Mathf.Max(MagicPoints - amountUsed, 0);
        }
    }
}