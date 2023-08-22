using System;
using UnityEngine;

namespace Character.Stats { 
    public class Health : MonoBehaviour {
        [SerializeField] int hitpoints;
        //Cassidy wrote this, startingHitPoints is acting as a 'max' hit points for enemies 12/8/23
        public int startingHitPoints;

        private void Start()
        {
             startingHitPoints=hitpoints;
        }

        public event Action OnDie;

        public bool IsDead() {
            return hitpoints <= 0;
        }

        public void Die() {
            hitpoints = 0;
            //Somewhere else should be calling out that <= 0 should die so we commented it out
          //  OnDie.Invoke();
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

        public void TakeDamage(Unit damageSource, int damage) {
            hitpoints = Mathf.Max(hitpoints - damage, 0);
            Debug.Log("Damage Applied to: " + gameObject.name);

            if (!gameObject.GetComponent<Unit>().isAPlayer)
            {
                StartCoroutine(gameObject.GetComponent<Unit>().UpdateUI());
                StartCoroutine(gameObject.GetComponent<Unit>().TurnOffUI());
            }

            if (IsDead())
            {
                if (damageSource == null)
                {
                    print("No Damage Source");
                    OnDie.Invoke();
                    return;
                }
                Debug.Log(gameObject.name + " was killed by : " + damageSource.name);
                GrantKillerExperience(damageSource.gameObject);
                OnDie.Invoke();
            }
        }
        private void GrantKillerExperience(GameObject damageSource) {
        
            Experience damageSourceExperienceComponent = damageSource.GetComponent<Experience>();
            if (damageSourceExperienceComponent == null) {
                Debug.Log("Experience component not found in 'GrantKillerExperience': " + damageSource.name);
                return;
            }
            int myExperienceReward = GetComponent<Experience>().GetExperience();
            damageSourceExperienceComponent.GrantExperience(myExperienceReward);
        }

        
    }
}