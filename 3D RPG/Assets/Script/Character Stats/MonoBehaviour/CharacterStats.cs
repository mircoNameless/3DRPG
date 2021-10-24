using Script.Character_Stats.ScriptableObject;
using Script.Combat;
using UnityEngine;

namespace Script.Character_Stats.MonoBehaviour
{
    public class CharacterStats : UnityEngine.MonoBehaviour
    {
        public CharacterData_SO characterData;

        public AttackData_SO attackData;

        [HideInInspector] public bool isCritical;

        #region Read from Data_SO

        public int maxHealth
        {
            get
            {
                if (characterData != null)
                {
                    return characterData.maxHealth;
                }

                return 0;
            }
            set => characterData.maxHealth = value;
        }

        public int currentHealth
        {
            get
            {
                if (characterData != null)
                {
                    return characterData.currentHealth;
                }

                return 0;
            }
            set => characterData.currentHealth = value;
        }

        public int baseDenfense
        {
            get
            {
                if (characterData != null)
                {
                    return characterData.baseDefense;
                }

                return 0;
            }
            set => characterData.baseDefense = value;
        }

        public int currentDefense
        {
            get
            {
                if (characterData != null)
                {
                    return characterData.currentDefense;
                }

                return 0;
            }
            set => characterData.currentDefense = value;
        }

        #endregion

        #region Character Combat

        public void TakeDamage(CharacterStats attacker, CharacterStats defender)
        {
            int damage = Mathf.Max(0, attacker.CurrentDamage() - defender.currentDefense);
            currentHealth = Mathf.Max(currentHealth - damage, 0);

            if (isCritical)
            {
                defender.GetComponent<Animator>().SetTrigger("Hit");
            }
            // todo: Update UI
            // todo: 经验Update
        }

        private int CurrentDamage()
        {
            float coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);
            if (isCritical)
            {
                coreDamage *= attackData.criticalMultiplier;
            }

            return (int) coreDamage;
        }

        #endregion
    }
}