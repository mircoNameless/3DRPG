using System;
using Script.Character_Stats.MonoBehaviour;
using UnityEngine;
using UnityEngine.AI;

namespace Script.Characters.Enemy
{
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        HitNothing,
    }

    public class Rock : MonoBehaviour
    {
        private Rigidbody rb;
        public RockStates rockStates;

        [Header("Basic Setting")] public float force;
        public int damage;
        public GameObject target;

        private Vector3 direction;
        public GameObject breakEffect;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.one;
            rockStates = RockStates.HitPlayer;
            FlyToTarget();
        }

        private void FixedUpdate()
        {
            if (rb.velocity.sqrMagnitude < 1f)
            {
                rockStates = RockStates.HitNothing;
            }
        }

        public void FlyToTarget()
        {
            if (target == null)
                target = FindObjectOfType<PlayerController>().gameObject;
            direction = (target.transform.position - transform.position + Vector3.up).normalized;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            switch (rockStates)
            {
                case RockStates.HitPlayer:
                    if (other.gameObject.CompareTag("Player"))
                    {
                        other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                        other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                        other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                        other.gameObject.GetComponent<CharacterStats>()
                            .TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());

                        rockStates = RockStates.HitNothing;
                    }

                    break;
                case RockStates.HitEnemy:
                    if (other.gameObject.GetComponent<Golem>())
                    {
                        var otherStats = other.gameObject.GetComponent<CharacterStats>();
                        otherStats.TakeDamage(damage, otherStats);

                        Instantiate(breakEffect, transform.position, Quaternion.identity);
                        Destroy(gameObject);
                    }

                    break;
                case RockStates.HitNothing:
                    break;
            }
        }
    }
}