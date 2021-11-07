using System;
using Script.Character_Stats.MonoBehaviour;
using Script.Manager;
using Tools;
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
    [RequireComponent(typeof(CharacterStats))]
    public class EnemyController : MonoBehaviour, IEndGameObserver
    {
        private NavMeshAgent agent;
        private Animator anim;
        private Collider coll;
        private EnemyState enemyState;

        protected CharacterStats characterStats;

        [Header("Basic Setting")] public float sightRadius;
        public bool isGuard;
        private float speed;

        protected GameObject attackTarget;

        public float lookAtTime;
        private float remainLookAtTime;

        private float lastAttackTime;

        [Header("Patrol State")] public float patrolRange;
        private Vector3 wayPoint;
        private Vector3 guardPos;
        private Quaternion guardRotation;

        private bool isWalk;
        private bool isChase;
        private bool isFollow;
        private bool isDead;
        private bool playerDead;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            characterStats = GetComponent<CharacterStats>();
            coll = GetComponent<Collider>();
            speed = agent.speed;
            guardPos = transform.position;
            guardRotation = transform.rotation;

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

            // FIXME: 场景切换后修改掉
            GameManager.Instance.AddObserver(this);
        }

        // 切换场景时启用
        // private void OnEnable()
        // {
        //     GameManager.Instance.AddObserver(this);
        // }

        private void OnDisable()
        {
            if (GameManager.IsInitialized)
            {
                GameManager.Instance.RemoveObserver(this);
            }
        }

        private void Update()
        {
            if (characterStats.currentHealth == 0)
            {
                isDead = true;
            }

            if (!playerDead)
            {
                SwitchState();
                SwitchAnimation();
                lastAttackTime -= Time.deltaTime;
            }
        }

        private void SwitchAnimation()
        {
            anim.SetBool("Walk", isWalk);
            anim.SetBool("Chase", isChase);
            anim.SetBool("Follow", isFollow);
            anim.SetBool("Critical", characterStats.isCritical);
            anim.SetBool("Death", isDead);
        }

        private void SwitchState()
        {
            if (isDead)
            {
                enemyState = EnemyState.DEAD;
            }
            // 如果发现player 切换到CHASE
            else if (FoundPlayer())
            {
                enemyState = EnemyState.CHASE;
            }

            switch (enemyState)
            {
                case EnemyState.GUARD:
                    isChase = false;
                    if (transform.position != guardPos)
                    {
                        isWalk = true;
                        agent.isStopped = false;
                        agent.SetDestination(guardPos);

                        if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                        {
                            isWalk = false;
                            transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.1f);
                        }
                    }

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
                        agent.isStopped = false;
                        agent.SetDestination(attackTarget.transform.position);
                    }

                    // 在攻击范围内攻击

                    if (TargetInAttackRange() || TargetInSkillRange())
                    {
                        isFollow = false;
                        agent.isStopped = true;

                        if (lastAttackTime < 0)
                        {
                            lastAttackTime = characterStats.attackData.coolDown;

                            // 暴击判断
                            characterStats.isCritical = Random.value < characterStats.attackData.criticalChange;
                            // 执行攻击
                            Attack();
                        }
                    }


                    break;
                case EnemyState.DEAD:
                    coll.enabled = false;
                    agent.radius = 0f;
                    // agent.enabled = false;
                    Destroy(gameObject, 2f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Attack()
        {
            transform.LookAt(attackTarget.transform);
            
            if (TargetInSkillRange())
            {
                // 技能攻击
                anim.SetTrigger("Skill");
            }
            else if (TargetInAttackRange())
            {
                // 近身攻击
                anim.SetTrigger("Attack");
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

        private bool TargetInAttackRange()
        {
            if (attackTarget != null)
            {
                return Vector3.Distance(attackTarget.transform.position, transform.position) <=
                       characterStats.attackData.attackRange;
            }

            return false;
        }

        private bool TargetInSkillRange()
        {
            if (attackTarget != null)
            {
                return Vector3.Distance(attackTarget.transform.position, transform.position) <=
                       characterStats.attackData.skillRange;
            }

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

        // Animation Event
        void Hit()
        {
            if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
            {
                var targetStats = attackTarget.GetComponent<CharacterStats>();
                targetStats.TakeDamage(characterStats, targetStats);
            }
        }

        public void EndNotify()
        {
            // 获胜动画
            // 停止所有移动
            // 停止Agent

            anim.SetBool("Win", true);
            playerDead = true;
            isChase = false;
            isWalk = false;
            attackTarget = null;
        }
    }
}