using Script.Character_Stats.ScriptableObject;

namespace Script.Character_Stats.MonoBehaviour
{
    public class CharacterStats : UnityEngine.MonoBehaviour
    {
        public CharacterData_SO characterData;

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
        
        
    }
}