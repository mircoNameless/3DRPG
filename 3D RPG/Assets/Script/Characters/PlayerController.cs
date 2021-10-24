using System;
using System.Collections;
using Script.Character_Stats.MonoBehaviour;
using Script.Character_Stats.ScriptableObject;
using Script.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Script.Characters
{
    public class PlayerController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;

        private CharacterStats characterStats;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            characterStats = GetComponent<CharacterStats>();
        }

        private void Start()
        {
            MouseManager.instance.ONMouseClicked += MoveTotTarget;
            MouseManager.instance.OnEnemyClicked += EventAttack;
        }


        private void Update()
        {
            SwitchAnimation();

            lastAttackTime -= Time.deltaTime;
        }

        private void SwitchAnimation()
        {
            anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        }

        private void MoveTotTarget(Vector3 target)
        {
            agent.isStopped = false;
            StopAllCoroutines();
            agent.SetDestination(target);
        }

        private GameObject attackTarget;
        private float lastAttackTime;

        private void EventAttack(GameObject target)
        {
            if (target != null)
            {
                attackTarget = target;
            }

            StartCoroutine(MoveToAttackTarget());
        }

        IEnumerator MoveToAttackTarget()
        {
            agent.isStopped = false;
            
            transform.LookAt(attackTarget.transform);

            // todo: 修改攻击范围参数
            while (Vector3.Distance(attackTarget.transform.position, transform.position) > 1f)
            {
                agent.SetDestination(attackTarget.transform.position);
                yield return null;
            }

            agent.isStopped = true;
            // Attack

            if (lastAttackTime <= 0)
            {
                anim.SetTrigger("Attack");
                lastAttackTime = 0.5f;
            }
        }
    }
}