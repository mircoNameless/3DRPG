using System;
using System.Collections;
using Script.Character_Stats.MonoBehaviour;
using Script.Character_Stats.ScriptableObject;
using Script.Manager;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script.Characters
{
    public class PlayerController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;

        private CharacterStats characterStats;

        private bool isDead;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            characterStats = GetComponent<CharacterStats>();
        }

        private void Start()
        {
            MouseManager.Instance.ONMouseClicked += MoveTotTarget;
            MouseManager.Instance.OnEnemyClicked += EventAttack;

            GameManager.Instance.RegisterPlay(characterStats);
        }


        private void Update()
        {
            isDead = characterStats.currentHealth == 0;

            if (isDead)
            {
                GameManager.Instance.NotifyObservers();
            }

            SwitchAnimation();

            lastAttackTime -= Time.deltaTime;
        }

        private void SwitchAnimation()
        {
            anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
            anim.SetBool("Death", isDead);
        }

        private void MoveTotTarget(Vector3 target)
        {
            StopAllCoroutines();
            if (isDead) return;

            agent.isStopped = false;
            agent.SetDestination(target);
        }

        private GameObject attackTarget;
        private float lastAttackTime;

        private void EventAttack(GameObject target)
        {
            if (isDead) return;
            
            if (target != null)
            {
                attackTarget = target;
                characterStats.isCritical = Random.value < characterStats.attackData.criticalChange;
                StartCoroutine(MoveToAttackTarget());
            }
        }

        IEnumerator MoveToAttackTarget()
        {
            agent.isStopped = false;

            transform.LookAt(attackTarget.transform);

            // todo: 修改攻击范围参数
            while (Vector3.Distance(attackTarget.transform.position, transform.position) >
                   characterStats.attackData.attackRange)
            {
                agent.SetDestination(attackTarget.transform.position);
                yield return null;
            }

            agent.isStopped = true;
            // Attack

            if (lastAttackTime <= 0)
            {
                anim.SetBool("Critical", characterStats.isCritical);
                anim.SetTrigger("Attack");
                lastAttackTime = characterStats.attackData.coolDown;
            }
        }

        //Animation Event
        void Hit()
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}