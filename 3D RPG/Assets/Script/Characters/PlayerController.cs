using System;
using Script.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Script.Characters
{
    public class PlayerController : MonoBehaviour
    {
        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            MouseManager.instance.ONMouseClicked += MoveTotTarget;
        }

        private void MoveTotTarget(Vector3 target)
        {
            agent.SetDestination(target);
        }
    }
}