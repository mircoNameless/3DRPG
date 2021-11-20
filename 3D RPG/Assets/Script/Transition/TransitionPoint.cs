using System;
using UnityEngine;

namespace Script.Transition
{
    public enum TransitionType
    {
        SameScene,
        DifferentScene
    }
    
    public class TransitionPoint : MonoBehaviour
    {
        
        [Header("Transition info")] public string sceneName;
        public TransitionType transitionType;
        public DestinationTag destinationTag;

        private bool canTrans;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && canTrans)
            {
                // TODO
                SceneController.Instance.TransitionToDestination(this);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canTrans = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canTrans = false;
            }
        }
    }
}
