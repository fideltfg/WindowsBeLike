using UnityEngine.EventSystems;
using UnityEngine;

namespace WindowsBeLike
{
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private RectTransform canvasRect;
        private RectTransform parentRect;
        private Vector2 pointerOffset;
        internal Vector3 startPosition;
        internal Vector2 startSizeDelta;

        public bool IsDragging { get; private set; }

        // Called when the pointer is pressed down on the panel
        public void OnPointerDown(PointerEventData data)
        {


            // Check if the left mouse button is pressed
            if (data.button != PointerEventData.InputButton.Left) return;

            // Get the parent canvas and the parent's RectTransform for calculations
            if (parentRect == null)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    canvasRect = canvas.GetComponent<RectTransform>();
                    parentRect = transform.parent as RectTransform;
                }
            }

            startPosition = parentRect.position;
            startSizeDelta = parentRect.sizeDelta;

            // Notify the window that the pointer is down on the panel
            Window window = GetComponentInParent<Window>();
            window.OnPointerDown(data);

            // Convert the pointer position to local space of the parent RectTransform
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, data.position, data.pressEventCamera, out pointerOffset);

            // Set the dragging state to true
            IsDragging = true;
        }

        // Called when the pointer is released from the panel
        public void OnPointerUp(PointerEventData data)
        {
            // Check if the left mouse button is released
            if (data.button != PointerEventData.InputButton.Left) return;

            // Set the dragging state to false
            IsDragging = false;
        }

        // Called when the pointer is dragged while holding down on the panel
        public void OnDrag(PointerEventData data)
        {
            if (GetComponentInParent<FullScreenRect>().isFullScreen)
            {
                GetComponentInParent<FullScreenRect>().ToggleFullScreen();
            }
            // Check if the panel is currently being dragged with the left mouse button
            if (!IsDragging || data.button != PointerEventData.InputButton.Left || parentRect == null) return;

            // Convert the pointer position to local space of the canvas RectTransform
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, data.position, data.pressEventCamera, out Vector2 localPointerPosition);

            // Move the parent RectTransform based on the pointer's movement
            parentRect.localPosition = localPointerPosition - pointerOffset;
        }
    }
}
