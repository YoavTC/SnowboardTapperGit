using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [HideInInspector] public int health = 3;
    [SerializeField] private TextMeshProUGUI healthText;

    public void RestartGame()
    {
        health = 3;
        healthText.text = "3";
    }
    
    //TEST COMMENT I GUESS

    public void OnDamage()
    {
        health--;
        healthText.text = health.ToString();
        Debug.Log("HP: " + health);
        if (health <= 1)
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
