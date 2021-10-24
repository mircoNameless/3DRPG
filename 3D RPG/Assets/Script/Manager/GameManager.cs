using System.Collections.Generic;
using Script.Character_Stats.MonoBehaviour;
using Tools;
using UnityEngine;

namespace Script.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public CharacterStats playerStats;

        private List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

        public void RegisterPlay(CharacterStats player)
        {
            playerStats = player;
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
    }
}
