using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] [ReadOnly] private int healthPoints = 3;
    [SerializeField] private TextMeshProUGUI healthText;

    public int health => healthPoints;

    public void RestartGame()
    {
        healthPoints = 3;
        healthText.text = "3";
    }
    private void Start()
    {
        healthPoints = 3;
    }

    public void OnDamage()
    {
        healthPoints--;
        healthText.text = healthPoints.ToString();
        Debug.Log("HP: " + healthPoints);
        if (health <= 0)
        {
            Debug.Log("Killing Player");
            KillPlayer();
        }
    }

    [SerializeField] private IPanel deathPanel;
    
    private void KillPlayer()
    {
        Debug.Log("KillPlayer()");
        AdManager.Instance.LoadAd();
        AdManager.Instance.ShowAd();
    }

    public void ShowScore()
    {
        deathPanel.TogglePanelVisibility(true);
    }
}
