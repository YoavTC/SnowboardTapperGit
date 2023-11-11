using System.Collections;
using System.Xml;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DeathPanel : IPanel
{
    private void Start()
    {
        TogglePanelVisibility(false);
        SetPauseWhenActive(false);
        OnPanelVisibilityChanged += OnDeathScreenShow;
    }
    
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private Image snowBackground;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform backButton, retryButton;

    [SerializeField] private RectTransform[] buttons;

    [SerializeField] float UIMovementSpeed;
    [SerializeField] private Transform measurementObject;

    void OnDeathScreenShow(bool state)
    {
        StartCoroutine(FadeColors(endColor));
        deathText.GetComponent<RectTransform>()
            .DOAnchorPos(new Vector2(0, 670), UIMovementSpeed)
            .SetEase(Ease.InOutSine);
        
        scoreText.text = "Score:";
        scoreText.GetComponent<RectTransform>()
            .DOAnchorPos(Vector2.zero, UIMovementSpeed)
            .SetEase(Ease.InOutSine)
            .SetDelay(0.6f)
            .OnComplete(() => StartCoroutine(CountToTarget()));
        
        snowBackground.GetComponent<RectTransform>()
            .DOAnchorPos(new Vector2(0, 0), UIMovementSpeed)
            .SetEase(Ease.InOutSine);

        foreach (RectTransform button in buttons)
        {
            button.DOAnchorPosY(2000, UIMovementSpeed).SetDelay(0.5f).SetEase(Ease.InOutSine);
        }

        score = Vector2.Distance(Vector2.zero, measurementObject.position);
        
        backButton.GetComponent<RectTransform>()
            .DOAnchorPos(new Vector2(-4, 500), UIMovementSpeed)
            .SetEase(Ease.InOutSine)
            .SetDelay(1.75f);
        
        retryButton.GetComponent<RectTransform>()
            .DOAnchorPos(new Vector2(-4, 230), UIMovementSpeed)
            .SetEase(Ease.InOutSine)
            .SetDelay(1.75f);
    }

    private float score;
    [SerializeField] private AnimationCurve counterCurve;
    private Color originalScoreColor;
    
    private IEnumerator CountToTarget()
    {
        float stepThingy = score / 100;
        float currentNumber = 0;
        
        float curveProgress = 0f;
        while (currentNumber < score)
        {
            Debug.Log(curveProgress);
            curveProgress += 0.01f;
            scoreText.text = "Score:\n" + currentNumber.ToString("F" + 3);
            currentNumber += stepThingy;
            yield return HelperFunctions.GetWait(counterCurve.Evaluate(curveProgress) * 0.05f);
        }

        originalScoreColor = scoreText.color;
        
        scoreText.text = "Score:\n" + score.ToString("F" + 3);
        yield return HelperFunctions.GetWait(0.2f);
        scoreText.transform.DOScale(4, 1)
            .SetEase(Ease.InExpo)
            .OnComplete(() => scoreText.color = Color.yellow);
    }
    
    [SerializeField] private Color endColor;
    [SerializeField] private float fadeDuration;
    private IEnumerator FadeColors(Color targetColor)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            backgroundPanel.color = Color.Lerp(backgroundPanel.color, targetColor, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        
        backgroundPanel.color = targetColor;
    }

    #region Buttons
    public void BackToMenu(bool restart)
    {
        if (restart)
        {
            float outSpeed = UIMovementSpeed * 0.75f;
            StartCoroutine(FadeColors(Color.clear));
            deathText.GetComponent<RectTransform>()
                .DOAnchorPos(new Vector2(0, 1800), outSpeed)
                .SetEase(Ease.InOutSine);

            scoreText.GetComponent<RectTransform>()
                .DOAnchorPos(new Vector2(-1500, 0), outSpeed)
                .SetEase(Ease.InOutSine);

            snowBackground.GetComponent<RectTransform>()
                .DOAnchorPos(new Vector2(0, -2000), outSpeed)
                .SetEase(Ease.InOutSine);

            backButton.GetComponent<RectTransform>()
                .DOAnchorPos(new Vector2(2000, 500), outSpeed)
                .SetEase(Ease.InOutSine);

            retryButton.GetComponent<RectTransform>()
                .DOAnchorPos(new Vector2(-2000, 230), outSpeed)
                .SetEase(Ease.InOutSine);

            scoreText.transform.DOScale(3, outSpeed)
                .SetEase(Ease.InExpo)
                .OnComplete(() =>
                    scoreText.color = originalScoreColor
                );

            StartCoroutine(RestartGame());
        }
        else SceneManager.LoadScene("Scenes/MainMenu");
    }

    [SerializeField] private Spawners spawners;

    IEnumerator RestartGame()
    {
        yield return HelperFunctions.GetWait(0.8f);
        spawners.ResetGame();
        
        foreach (RectTransform button in buttons)
        {
            button.DOAnchorPosY(-170, UIMovementSpeed / 2).SetDelay(0.2f).SetEase(Ease.InOutSine);
        }
        
        gameObject.SetActive(false);
    }
    

    #endregion
}
