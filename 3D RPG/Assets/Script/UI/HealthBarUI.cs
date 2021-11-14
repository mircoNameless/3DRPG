using System;
using System.Collections;
using System.Collections.Generic;
using Script.Character_Stats.MonoBehaviour;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;
    public Transform barPoint;
    public bool alwaysVisible;

    public float visibleTime;
    private float timeLeft;

    public Image healthSlider;
    private Transform uiBar;
    private Transform cam;

    private CharacterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.updateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                uiBar = Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = uiBar.GetChild(0).GetComponent<Image>();
                uiBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(uiBar.gameObject);
        }

        uiBar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float sliderPercent = (float) currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        if (uiBar != null)
        {
            uiBar.position = barPoint.position;
            uiBar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
            {
                uiBar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
    }
}