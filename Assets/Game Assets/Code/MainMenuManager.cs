using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton, settingsButton, creditsButton;
    [SerializeField] private AudioClip click;

    [SerializeField] private Transform playButtonParent;
    [SerializeField] private float buttonLoopDuration;

    [SerializeField] private TextMeshProUGUI versionText;

    private void Start()
    {
        Debug.Log(Application.targetFrameRate);
        Application.targetFrameRate = 140;

        playButtonParent.DOScale(new Vector3(2,2,2), buttonLoopDuration).SetLoops(-1, LoopType.Yoyo);
        versionText.text = "build: "  + Application.version;
    }
    
    public void OnPressPlay()
    {
        SoundManager.Instance.PlaySound("click");
        SceneManager.LoadScene("Scenes/SampleScene");
    }
}
