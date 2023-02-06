using UnityEngine;

public class FullScreenRect : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private Vector2 originalSizeDelta;
    private Vector2 targetAnchoredPosition;
    private Vector2 targetSizeDelta;
    private float duration = 0.33f;
    private float currentTime = 0f;
    private bool isAnimating = false;
    public bool isFullScreen = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateValues();
    }

    public void UpdateValues()
    {
        originalAnchoredPosition = rectTransform.anchoredPosition;
        originalSizeDelta = rectTransform.sizeDelta;
    }

    private void Update()
    {
        if (isAnimating)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetAnchoredPosition, t);
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, targetSizeDelta, t);
            if (currentTime >= duration)
            {
                rectTransform.anchoredPosition = targetAnchoredPosition;
                rectTransform.sizeDelta = targetSizeDelta;
                isAnimating = false;
            }
        }
    }

    public void ToggleFullScreen()
    {
        if (!isAnimating)
        {
            if (!isFullScreen)
            {
                UpdateValues();
                targetAnchoredPosition = Vector2.zero;
                targetSizeDelta = new Vector2(Screen.width, Screen.height);
                isFullScreen = true;
            }
            else
            {
                targetAnchoredPosition = originalAnchoredPosition;
                targetSizeDelta = originalSizeDelta;
                isFullScreen = false;
            }
            currentTime = 0f;
            isAnimating = true;
        }
    }
}
