using System;
using Script.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Script.Characters
{
    public class PlayerController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            MouseManager.instance.ONMouseClicked += MoveTotTarget;
        }

        private void Update()
        {
            SwitchAnimation();
        }

        private void SwitchAnimation()
        {
            anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        }

        private void MoveTotTarget(Vector3 target)
        {
            agent.SetDestination(target);
        }
    }
}