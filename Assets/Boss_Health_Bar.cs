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

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        var container = Instantiate(bossSliderContainer, canvas);
        bossHealthBarName = GetComponentInChildren<TextMeshProUGUI>();
        bossSlider = container.GetComponentInChildren<Slider>();
    }

    void Update()
    {
        bossHealth = GetComponent<Enemy_Health>().currentHealth;
                bossHealthBarName.text = bossName;

        float maxBossHealth = GetComponent<Enemy_Health>().totalHealth;
        bossSlider.value = bossHealth / maxBossHealth;
    }
}
