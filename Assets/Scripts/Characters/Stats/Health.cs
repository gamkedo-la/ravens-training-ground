using System;
using UnityEngine;

namespace Character.Stats { 
    public class Health : MonoBehaviour {
        [SerializeField] int hitpoints;

        public int HitPoints
        {
            set
            {
                hitpoints = value;
                if (HealthChangedEvent != null)
                    HealthChangedEvent(hitpoints);
            }
            get
            {
                return hitpoints;
            }
        }
        //Cassidy wrote this, startingHitPoints is acting as a 'max' hit points for enemies 12/8/23
        public int startingHitPoints;

        public delegate void HealthChanged(int health);
        public event HealthChanged HealthChangedEvent;
        private void Start()
        {
             startingHitPoints= HitPoints;
        }

        public event Action OnDie;

        public bool IsDead() {
            return HitPoints <= 0;
        }

        public void Die() {
            HitPoints = 0;
            //Somewhere else should be calling out that <= 0 should die so we commented it out
          //  OnDie.Invoke();
        }

        public int GetMaxHP() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public int GetCurrentHP() {
            return HitPoints;
        }

        public void AddHealth(int healthToAdd) {
            HitPoints = Math.Min(HitPoints + healthToAdd, GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public void HealToFull() {
            hitpoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(Unit damageSource, int damage) {
            HitPoints = Mathf.Max(HitPoints - damage, 0);
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