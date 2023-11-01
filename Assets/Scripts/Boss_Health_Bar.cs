using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Boss_Health_Bar : MonoBehaviour
{
    public GameObject bossSliderContainer;
    private Slider bossSlider;
    public string bossName;
    private TextMeshProUGUI bossHealthBarName;
    private float bossHealth;
    private Transform canvas;
    private bool bossHealthBarActive = true;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        var container = Instantiate(bossSliderContainer, canvas);
        bossHealthBarName = container.GetComponentInChildren<TextMeshProUGUI>();
        bossSlider = container.GetComponentInChildren<Slider>();
        bossHealthBarName.text = bossName;
        container.SetActive(true);
    }

    void Update()
    {
        bossHealth = GetComponent<Enemy_Health>().currentHealth;

        float maxBossHealth = GetComponent<Enemy_Health>().totalHealth;
        bossSlider.value = bossHealth / maxBossHealth;

        if(bossHealth <= 10 && bossHealthBarActive) {

            DeactivateHealthBar();
        }
    }

    private void DeactivateHealthBar() {
        GameObject.FindGameObjectWithTag("BossHealthBar").SetActive(false);
        bossHealthBarActive = false;
    }


}