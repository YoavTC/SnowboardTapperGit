using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : IPanel
{
    private void Start()
    {
        soundManager = SoundManager.Instance;
        SetPauseWhenActive(true);
        TogglePanelVisibility(false);

        OnPanelVisibilityChanged += OnPanelPauseChange;
        
        soundManager.PlaySound("mainTheme", true);
    }

    private SoundManager soundManager;

    [SerializeField] private AudioClip mainTheme, clickSound;

    [SerializeField] private Sprite pauseSprite, resumeSprite;
    [SerializeField] private Button button;

    void OnPanelPauseChange(bool state)
    {
        Debug.Log("state: " + state);
        button.image.sprite = state ? pauseSprite : resumeSprite;
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}