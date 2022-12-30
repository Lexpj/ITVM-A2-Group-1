using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    private int health = 100;
    private string healthState = "";
    private bool canBeCaptured = false;
    private float healthCounter = 0f;
    private Transform lastPosBeforeKnock;
    private bool savedPos;
    [SerializeField] private GameObject kindOfPlayer;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthStatus;

    [SerializeField] GameObject knocked;
    [SerializeField] GameObject injured;

    // Start is called before the first frame update
    void Start()
    {
        healthState = "Healthy";
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealthState();
        if (healthState == "Knocked")
        {
            rechargeHealth();
            knocked.SetActive(true);
            injured.SetActive(false);
        }
        if(healthState == "Injured")
        {
            knocked.SetActive(false);
            injured.SetActive(true);
        }
        if(healthState == "Healthy")
        {
            knocked.SetActive(false);
            injured.SetActive(false);
        }
        if (kindOfPlayer.CompareTag("Player"))
        {
            healthStatus.text = healthState;
            slider.value = health;
        }
    }

    public string GetHealthState()
    {
        return healthState;
    }

    public void Damage(int DamageAmount)
    {
        if (healthState != "Knocked")
        {
            health = health - DamageAmount;

            if (health <= 0)
            {
                health = 0;
            }
        }
    }

    public void BringToInjured()
    {
        health = 50;
    }

    public Transform lastPos()
    {
        return lastPosBeforeKnock;
    }

    private void CheckHealthState()
    {
        if (health == 100)
        {
            healthState = "Healthy";
        }
        if (health == 50)
        {
            healthState = "Injured";
        }
        if (health < 50)
        {
            if (!savedPos)
            {
                savedPos = true;
                lastPosBeforeKnock = transform;
            }
            healthState = "Knocked";
        }
    }

    private void rechargeHealth()
    {
        if (health < 50)
        {
            healthCounter += Time.deltaTime;
            if (healthCounter >= 0.1)
            {
                health++;
                healthCounter = 0f;
            }
        }
    }
}
