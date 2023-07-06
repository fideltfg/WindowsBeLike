using System;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WindowsBeLike
{
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


        public virtual void Start()
        {
            if (titleTextBox != null)
            {
                titleTextBox.text = Title;
            }

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


        public virtual void OnEnable()
        {
            // parentRect = transform.parent as RectTransform;
            transform.SetAsLastSibling();
            if (UIController.Instance != null)
            {
                UIController.Instance.AddNewWindowToWindowList(this);
            }
            else if (GetComponentInParent<UIController>() != null)
            {
                GetComponentInParent<UIController>().AddNewWindowToWindowList(this);
            }


        }

        public void OnPointerDown(PointerEventData data)
        {
            if (data.button != 0) return;
            transform.SetAsLastSibling();
        }

        public virtual void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}