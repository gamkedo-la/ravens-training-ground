using System;
using UnityEngine;

namespace Character.Stats { 
    public class Health : MonoBehaviour {
        [SerializeField] int hitpoints;

        public event Action OnDie;

        public bool IsDead() {
            return hitpoints <= 0;
        }

        public void Die() {
            hitpoints = 0;
            OnDie.Invoke();
        }

        public int GetMaxHP() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public int GetCurrentHP() {
            return hitpoints;
        }

        public void AddHealth(int healthToAdd) {
            hitpoints = Math.Min(hitpoints + healthToAdd, GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public void HealToFull() {
            hitpoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject damageSource, int damage) {
            hitpoints = Mathf.Max(hitpoints - damage, 0);
            if (IsDead()) {
                OnDie.Invoke();
                GrantKillerExperience(damageSource);
            }
        }
        private void GrantKillerExperience(GameObject damageSource) {
            Experience damageSourceExperienceComponent = damageSource.GetComponent<Experience>();
            if (damageSourceExperienceComponent == null) {
                return;
            }
            int myExperienceReward = GetComponent<BaseStats>().GetStat(Stat.ExperienceGainedForDefeat);
            damageSourceExperienceComponent.GrantExperience(myExperienceReward);
        }


    }
}