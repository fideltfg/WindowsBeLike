using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WindowsBeLike
{
    [RequireComponent(typeof(FullScreenRect))]
    public class Window : MonoBehaviour
    {
        public string Title;
        public TextMeshProUGUI titleTextBox;
        private RectTransform parentRect;

        public virtual void Update()
        {
            titleTextBox.text = Title;
        }

        public virtual void OnAwake()
        {
            parentRect = transform.parent as RectTransform;
        }

        public virtual void OnPointerDown(PointerEventData data)
        {
          //  if (data.button != 0) return;
            parentRect.SetAsLastSibling();
        }
    }
}