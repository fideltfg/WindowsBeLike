using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WindowsBeLike
{
    [RequireComponent(typeof(FullScreenRect))]
    public class Window : MonoBehaviour, IPointerDownHandler
    {
        public string Title;
        public TextMeshProUGUI titleTextBox;
        private RectTransform parentRect;

        public int windowIndex = 0;

        public virtual void Update()
        {
          

        }

        public virtual void Start()
        {
           if (titleTextBox != null)
            {
                titleTextBox.text = Title;
            }
        }


        public void OnEnable()
        {
            parentRect = transform.parent as RectTransform;
            transform.SetAsLastSibling();

           UIController.Instance.AddNewWindowToWindowList(this);

        }

        public void OnPointerDown(PointerEventData data)
        {
            if (data.button != 0) return;
            transform.SetAsLastSibling();
        }
    }
}