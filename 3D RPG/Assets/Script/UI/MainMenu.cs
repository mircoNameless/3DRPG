using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Script.UI
{
    public class MainMenu : MonoBehaviour
    {
        private Button newGameBtn;
        
        private Button continueGameBtn;
        
        private Button endGameBtn;

        private PlayableDirector director;

        private void Awake()
        {
            newGameBtn = transform.GetChild(1).GetComponent<Button>();
            continueGameBtn = transform.GetChild(2).GetComponent<Button>();
            endGameBtn = transform.GetChild(3).GetComponent<Button>();
            
            newGameBtn.onClick.AddListener(PlayTimeLine);
            continueGameBtn.onClick.AddListener(ContinueGame);
            endGameBtn.onClick.AddListener(QuitGame);

            director = FindObjectOfType<PlayableDirector>();
            director.stopped += NewGame;
        }

        private void PlayTimeLine()
        {
            director.Play();
            
        }

        private void NewGame(PlayableDirector obj)
        {
            PlayerPrefs.DeleteAll();
            // 转换场景
            SceneController.Instance.TransitionToFirstLevel();
        }

        private void ContinueGame()
        {
            SceneController.Instance.TransitionToLoadGame();
        }

        private void QuitGame()
        {
            Application.Quit();
            Debug.Log("退出游戏");
        }
    }
}
