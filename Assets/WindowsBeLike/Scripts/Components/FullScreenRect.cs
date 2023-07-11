using UnityEngine;
using WindowsBeLike;
namespace WindowsBeLike
{
    public class FullScreenRect : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Vector2 originalAnchoredPosition;
        private Vector2 originalSizeDelta;
        public Vector2 targetAnchoredPosition;
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
            transform.SetAsLastSibling();
            if (!isAnimating)
            {
                if (!isFullScreen)
                {
                    UpdateValues();
                   // targetAnchoredPosition = new Vector2(0, -UIController.Instance.TaskAreaHeight);
                    targetAnchoredPosition = new Vector2(0, -SettingsManager.TaskAreaHeight);

                    //targetSizeDelta = new Vector2(Screen.width, Screen.height - UIController.Instance.TaskAreaHeight);
                    targetSizeDelta = new Vector2(Screen.width, Screen.height - SettingsManager.TaskAreaHeight);

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
}