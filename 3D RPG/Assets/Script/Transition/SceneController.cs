using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using Script.Transition;
using Tools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    private GameObject player;
    private NavMeshAgent playerAgent;

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionType.DifferentScene:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        player = GameManager.Instance.playerStats.gameObject;
        playerAgent = player.GetComponent<NavMeshAgent>();
        playerAgent.enabled = false;
        
        player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);

        playerAgent.enabled = true;
        
        yield return null;
    }

    private TranstionDestination GetDestination(DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TranstionDestination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
            {
                return entrances[i];
            }
        }
        
        return null;
    }
}