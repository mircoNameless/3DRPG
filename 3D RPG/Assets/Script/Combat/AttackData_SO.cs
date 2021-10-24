using UnityEngine;

namespace Script.Combat
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
    public class AttackData_SO : ScriptableObject
    {
        public float attackRange;
        public float skillRange;

        public float coolDown;

        public int minDamage;
        public int maxDamage;

        public float criticalMultiplier;

        public float criticalChange;
    }
}
