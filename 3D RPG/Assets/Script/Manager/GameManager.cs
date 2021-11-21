using System.Collections.Generic;
using Cinemachine;
using Script.Character_Stats.MonoBehaviour;
using Script.Transition;
using Tools;
using UnityEngine;

namespace Script.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public CharacterStats playerStats;

        private CinemachineFreeLook followCamera;

        private List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void RegisterPlay(CharacterStats player)
        {
            playerStats = player;

            followCamera = FindObjectOfType<CinemachineFreeLook>();

            if (followCamera != null)
            {
                followCamera.Follow = playerStats.transform.GetChild(2);
                followCamera.LookAt = playerStats.transform.GetChild(2);
            }
        }

        public void AddObserver(IEndGameObserver gameObserver)
        {
            endGameObservers.Add(gameObserver);
        }

        public void RemoveObserver(IEndGameObserver gameObserver)
        {
            endGameObservers.Remove(gameObserver);
        }

        public void NotifyObservers()
        {
            foreach (var observer in endGameObservers)
            {
                observer.EndNotify();
            }
        }

        public Transform GetEntrance()
        {
            foreach (var item in FindObjectsOfType<TranstionDestination>())
            {
                if (item.destinationTag == DestinationTag.Enter)
                {
                    return item.transform;
                }
            }

            return null;
        }
    }
}
