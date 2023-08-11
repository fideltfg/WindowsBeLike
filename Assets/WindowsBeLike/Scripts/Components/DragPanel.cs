using UnityEngine.EventSystems;
using UnityEngine;

namespace WindowsBeLike
{
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        // Reference to the dummy object assigned in UIController
        GameObject DummyObject
        {
            get
            {
                return UIController.Instance.WindowDragOutline;
            }
        }

        private RectTransform canvasRect;
        private RectTransform parentRectTransform;
        private Vector2 pointerOffset;

        public bool IsDragging { get; private set; }

        public void OnPointerDown(PointerEventData data)
        {
            if (data.button != PointerEventData.InputButton.Left) return;

            if (parentRectTransform == null)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    canvasRect = canvas.GetComponent<RectTransform>();
                    parentRectTransform = transform.parent as RectTransform;
                }
            }

            // Set the position of the dummy object to match the parent window
            DummyObject.transform.position = parentRectTransform.position;

            // Ensure the dummy object is the last sibling and visible on top
            DummyObject.transform.SetAsLastSibling();

          

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out pointerOffset);

            IsDragging = true;
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.button != PointerEventData.InputButton.Left) return;

            // Move the parent window to the location where the dummy object was dropped
            parentRectTransform.position = DummyObject.transform.position;

            // Disable the dummy object until the next drag
            DummyObject.SetActive(false);

            IsDragging = false;
        }

        public void OnDrag(PointerEventData data)
        {
            if (!IsDragging || data.button != PointerEventData.InputButton.Left || parentRectTransform == null) return;

            // Enable the dummy object
            DummyObject.SetActive(true);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, data.position, data.pressEventCamera, out Vector2 localPointerPosition);

            // Move the dummy object based on the pointer's movement
            DummyObject.transform.localPosition = localPointerPosition - pointerOffset;
            DummyObject.GetComponent<RectTransform>().sizeDelta = parentRectTransform.sizeDelta;

            // Ensure the dummy object remains the last sibling and visible on top
            DummyObject.transform.SetAsLastSibling();
        }
    }
}
