/// <summary>
/// Handles the full-screen behavior of a window.
/// </summary>
using System;
using UnityEngine;

namespace WindowsBeLike
{
    public class FullScreenRect : MonoBehaviour
    {
        private RectTransform rectTransform;
        internal Vector2 originalAnchoredPosition;
        internal Vector2 originalSizeDelta;
        public Vector2 targetAnchoredPosition;
        private Vector2 targetSizeDelta;
        private float duration = 0.33f;
        private float currentTime = 0f;
        private bool isAnimating = false;
        public bool isFullScreen = false;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            SetNewOrigin(rectTransform.anchoredPosition, rectTransform.sizeDelta);
        }


        /// <summary>
        /// Updates the full-screen animation each frame.
        /// </summary>
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

        /// <summary>
        /// Toggles between full-screen and normal mode.
        /// </summary>
        public void ToggleFullScreen()
        {
            transform.SetAsLastSibling();
            if (!isAnimating)
            {
                if (!isFullScreen)
                {
                    //UpdateValues();
                    SetNewOrigin(rectTransform.anchoredPosition, rectTransform.sizeDelta);
                    targetAnchoredPosition = new Vector2(0, -SettingsManager.TaskAreaHeight);
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

        internal void SetNewOrigin(Vector2 startPosition, Vector2 startSizeDelta)
        {
            originalAnchoredPosition = startPosition;
            originalSizeDelta = startSizeDelta;
        }
    }
}
