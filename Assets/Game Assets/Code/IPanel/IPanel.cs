using UnityEngine;
public abstract class IPanel : MonoBehaviour
{
    public event System.Action<bool> OnPanelVisibilityChanged;
    public bool isVisible { get; private set; }
    private bool pauseWhenActive;
    public Transform[] backgroundObjects;

    public void SetPauseWhenActive(bool state) { pauseWhenActive = state; }

    public void TogglePanelVisibility()
    {
        if (isVisible) Hide();
        else Show();
        OnPanelVisibilityChanged?.Invoke(!isVisible);
    }

    public void TogglePanelVisibility(bool state)
    {
        Debug.Log("TogglePanelVisibility");
        if (state) Show();
        else Hide();
        OnPanelVisibilityChanged?.Invoke(state);
    }

    private void Hide()
    {
        isVisible = false;
        if (pauseWhenActive) Time.timeScale = 1f;
        transform.gameObject.SetActive(false);
        
        foreach (Transform temp in backgroundObjects)
        {
            temp.gameObject.SetActive(true);
        }
        
        SetActiveRecursively(transform, false);
    }

    private void Show()
    {
        isVisible = true;
        if (pauseWhenActive) Time.timeScale = 0f;
        transform.gameObject.SetActive(true);
        
        foreach (Transform temp in backgroundObjects)
        {
            temp.gameObject.SetActive(false);
        }
        
        SetActiveRecursively(transform, true);
    }
    
    private void SetActiveRecursively(Transform parent, bool state)
    {
        transform.gameObject.SetActive(state);
        foreach (Transform child in parent)
        {
            SetActiveRecursively(child, state);
        }
    }
}