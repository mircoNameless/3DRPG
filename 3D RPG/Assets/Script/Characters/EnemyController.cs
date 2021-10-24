using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace Script.Characters
{
    public enum EnemyState
    {
        GUARD,
        PATORL,
        CHASE,
        DEAD
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;
        private EnemyState enemyState;

        [Header("Basic Setting")] public float sightRadius;
        public bool isGuard;
        private float speed;

        private GameObject attackTarget;

        public float lookAtTime;
        private float remainLookAtTime;

        [Header("Patrol State")] public float patrolRange;
        private Vector3 wayPoint;
        private Vector3 guardPos;

        private bool isWalk;
        private bool isChase;
        private bool isFollow;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            speed = agent.speed;
            guardPos = transform.position;

            remainLookAtTime = lookAtTime;
        }

        private void Start()
        {
            if (isGuard)
            {
                enemyState = EnemyState.GUARD;
            }
            else
            {
                enemyState = EnemyState.PATORL;
                GetNewWayPoint();
            }
        }

        private void Update()
        {
            SwitchState();
            SwitchAnimation();
        }

        private void SwitchAnimation()
        {
            anim.SetBool("Walk", isWalk);
            anim.SetBool("Chase", isChase);
            anim.SetBool("Follow", isFollow);
        }

        private void SwitchState()
        {
            // 如果发现player 切换到CHASE
            if (FoundPlayer())
            {
                enemyState = EnemyState.CHASE;
            }

            switch (enemyState)
            {
                case EnemyState.GUARD:
                    break;
                case EnemyState.PATORL:

                    isChase = false;
                    agent.speed = speed * 0.5f;

                    if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;

                        if (remainLookAtTime > 0)
                        {
                            remainLookAtTime -= Time.deltaTime;
                        }
                        else
                        {
                            GetNewWayPoint();
                        }
                    }
                    else
                    {
                        isWalk = true;
                        agent.SetDestination(wayPoint);
                    }

                    break;
                case EnemyState.CHASE:
                    // todo: 追Player

                    // todo: 在攻击范围内攻击
                    // todo: 执行动画
                    isWalk = false;
                    isChase = true;

                    agent.speed = speed;
                    if (!FoundPlayer())
                    {
                        isFollow = false;
                        if (remainLookAtTime > 0)
                        {
                            agent.SetDestination(transform.position);
                            remainLookAtTime -= Time.deltaTime;
                        }
                        else if (isGuard)
                        {
                            enemyState = EnemyState.GUARD;
                        }
                        else
                        {
                            enemyState = EnemyState.PATORL;
                        }
                    }
                    else
                    {
                        isFollow = true;
                        agent.SetDestination(attackTarget.transform.position);
                    }

                    break;
                case EnemyState.DEAD:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool FoundPlayer()
        {
            var colliders = Physics.OverlapSphere(transform.position, sightRadius);

            foreach (var target in colliders)
            {
                if (target.CompareTag("Player"))
                {
                    attackTarget = target.gameObject;
                    return true;
                }
            }

            attackTarget = null;
            return false;
        }

        private void GetNewWayPoint()
        {
            remainLookAtTime = lookAtTime;

            float randomX = Random.Range(-patrolRange, patrolRange);
            float randomZ = Random.Range(-patrolRange, patrolRange);

            Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y,
                guardPos.z + randomZ);

            NavMeshHit hit;

            wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, sightRadius);
        }
    }
}