using System;
using UnityEngine;

namespace Script.Characters.Enemy
{
    public class Rock : MonoBehaviour
    {
        private Rigidbody rb;

        [Header("Basic Setting")] public float force;
        public GameObject target;

        private Vector3 direction;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            FlyToTarget();
        }

        public void FlyToTarget()
        {
            if (target == null)
                target = FindObjectOfType<PlayerController>().gameObject;
            direction = (target.transform.position - transform.position + Vector3.up).normalized;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}