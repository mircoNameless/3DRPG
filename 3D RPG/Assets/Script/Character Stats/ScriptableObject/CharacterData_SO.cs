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
    }
}
