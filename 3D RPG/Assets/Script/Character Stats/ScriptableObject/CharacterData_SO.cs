using UnityEngine;

namespace Script.Character_Stats.ScriptableObject
{
    [CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
    public class CharacterData_SO : UnityEngine.ScriptableObject
    {
        [Header("Stats Info")] public int maxHealth;
        public int currentHealth;
        public int baseDefense;
        public int currentDefense;

        [Header("Kill")] public int killPoint;

        [Header("Level")] public int currentLevel;
        public int maxLevel;
        public int baseExp;
        public int currentExp;
        public float levelBuff;

        public float LevelMultiplier => 1 + (currentLevel - 1) * levelBuff;

        public void UpdateExp(int point)
        {
            currentExp += point;
            if (currentExp >= baseExp)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
            baseExp += (int) (baseExp * LevelMultiplier);

            maxHealth = (int) (maxHealth * LevelMultiplier);
            currentHealth = maxHealth;
            
            Debug.Log("Level up!" + currentLevel + "Max Health" + maxHealth);
        }
    }
}