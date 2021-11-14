using System;
using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        private Text levelText;
        private Image healthSlider;
        private Image expSlider;

        private void Awake()
        {
            levelText = transform.GetChild(2).GetComponent<Text>();
            healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }

        private void Update()
        {
            levelText.text = "Level " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
            UpdateHealth();
            UpdateExp();
        }

        private void UpdateHealth()
        {
            float sliderPercent = (float) GameManager.Instance.playerStats.currentHealth /
                                  GameManager.Instance.playerStats.maxHealth;
            healthSlider.fillAmount = sliderPercent;
        }

        private void UpdateExp()
        {
            float sliderPercent = (float) GameManager.Instance.playerStats.characterData.currentExp /
                                  GameManager.Instance.playerStats.characterData.baseExp;
            expSlider.fillAmount = sliderPercent;
        }
    }
}