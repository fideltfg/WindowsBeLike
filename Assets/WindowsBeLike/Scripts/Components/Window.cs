/// <summary>
/// Represents a window in the WindowsBeLike interface.
/// </summary>
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WindowsBeLike
{
    /// <summary>
    /// Represents a window in the WindowsBeLike interface.
    /// </summary>
    [RequireComponent(typeof(FullScreenRect))]
    public class Window : MonoBehaviour, IPointerDownHandler
    {
        public string Title;
        public TextMeshProUGUI titleTextBox;
        public DragCorner DragCorner;
        public DragPanel DragPanel;
        public Button FullscreenButton;
        public float minWidth = 200f;
        public float minHeight = 75f;
        public bool Dragable = true;
        public bool Resizeable = true;
        public bool AllowFullscreen = true;
        public int windowIndex = 0;

        private RectTransform rectTransform; // Cached reference to RectTransform component

        public virtual void Start()
        {
            if (titleTextBox != null)
            {
                titleTextBox.text = Title;
            }

            rectTransform = GetComponent<RectTransform>(); // Get the reference to the RectTransform

            if (DragCorner != null)
            {
                if (Resizeable)
                {
                    DragCorner.SetMinHeight(minHeight);
                    DragCorner.SetMinWidth(minWidth);
                }
                else
                {
                    DragCorner.enabled = false;
                }
            }
        }

        public virtual void Update()
        {
            if (DragPanel != null)
            {
                DragPanel.enabled = Dragable;
            }

            if (DragCorner != null)
            {
                DragCorner.enabled = Resizeable;
            }

            if (FullscreenButton != null)
            {
                FullscreenButton.enabled = AllowFullscreen;
            }
        }

        /// <summary>
        /// Clamps the window position within the screen boundaries.
        /// </summary>
        /// <param name="screenSize">The size of the screen.</param>
        public void ClampToScreen(Vector2 screenSize)
        {
            Vector3 pos = rectTransform.position;
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            float screenW = screenSize.x;
            float screenH = screenSize.y;

            pos.x = Mathf.Clamp(pos.x, width * 0.5f, screenW - width * 0.5f);
            pos.y = Mathf.Clamp(pos.y, height * 0.5f, screenH - height * 0.5f);

            rectTransform.position = pos;
        }

        public virtual void OnEnable()
        {
            transform.SetAsLastSibling();

            if (UIController.Instance != null)
            {
                UIController.Instance.AddNewWindowToWindowList(this);
            }
            else if (GetComponentInParent<UIController>() != null)
            {
                GetComponentInParent<UIController>().AddNewWindowToWindowList(this);
            }

            UIController.OnResolutionChanged += ClampToScreen;
        }

        /// <summary>
        /// Called when the pointer is down on the window.
        /// </summary>
        public void OnPointerDown(PointerEventData data)
        {
            if (data.button != 0) return;
            transform.SetAsLastSibling();
            UIController.Instance.WindowInFocus = this;
            Debug.Log("Window in focus: " + UIController.Instance.WindowInFocus.Title);
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public virtual void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
