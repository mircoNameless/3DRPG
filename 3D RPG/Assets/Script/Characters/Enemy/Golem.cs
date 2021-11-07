using Script.Character_Stats.MonoBehaviour;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Script.Characters.Enemy
{
    public class Golem : EnemyController
    {
        [Header("Skill")] public float kickForce = 2.5f;

        public GameObject rockPrefab;
        public Transform handPos;


        // Animation Event
        public void KickOff()
        {
            if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
            {
                Debug.Log("触发");
                var targetStats = attackTarget.GetComponent<CharacterStats>();

                Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
                // direction.Normalize();

                targetStats.GetComponent<NavMeshAgent>().isStopped = true;
                targetStats.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

                // 根据个人爱好添加
                targetStats.GetComponent<Animator>().SetTrigger("Dizzy");
                targetStats.TakeDamage(characterStats, targetStats);
            }
        }

        // Animation Event
        public void ThrowRock()
        {
            if (attackTarget != null)
            {
                var rock = Instantiate(rockPrefab, handPos.position, quaternion.identity);
                rock.GetComponent<Rock>().target = attackTarget;
            }
        }
    }
}