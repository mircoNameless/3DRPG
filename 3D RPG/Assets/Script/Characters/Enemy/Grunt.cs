using UnityEngine;
using UnityEngine.AI;

namespace Script.Characters.Enemy
{
    public class Grunt : EnemyController
    {
        [Header("Skill")] public float kickForce = 1f;

        public void KickOff()
        {
            if (attackTarget != null)
            {
                transform.LookAt(attackTarget.transform);

                Vector3 direction = attackTarget.transform.position - transform.position;
                direction.Normalize();

                attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
                attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            }
        }
    }
}
