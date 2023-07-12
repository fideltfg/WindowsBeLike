/// <summary>
/// Handles the dragging behavior of the corner of a window.
/// </summary>
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WindowsBeLike
{
    public class DragCorner : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private Vector2 pointerOffset;
        private RectTransform canvasRectTransform;
        private RectTransform panelRectTransform;
        private RectTransform parentRectTransform;
        private float minWidth = 200f;
        private float minHeight = 75f;
        private float maxWidth;
        private float maxHeight;

        /// <summary>
        /// Called when the pointer is pressed down on the drag corner.
        /// </summary>
        /// <param name="data">The pointer event data.</param>
        public void OnPointerDown(PointerEventData data)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvasRectTransform = canvas.transform as RectTransform;
                panelRectTransform = transform as RectTransform;
                parentRectTransform = transform.parent.GetComponent<RectTransform>();
                maxWidth = canvasRectTransform.sizeDelta.x - parentRectTransform.anchoredPosition.x;
                maxHeight = canvasRectTransform.sizeDelta.y - parentRectTransform.anchoredPosition.y;
            }
            panelRectTransform.SetAsLastSibling();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, data.position, data.pressEventCamera, out pointerOffset);
        }

        /// <summary>
        /// Called when the pointer is dragged while holding down on the drag corner.
        /// </summary>
        /// <param name="data">The pointer event data.</param>
        public void OnDrag(PointerEventData data)
        {
            if (panelRectTransform == null)
                return;

            Vector2 pointerPostion = ClampToWindow(data);

            Vector2 localPointerPosition;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, pointerPostion, data.pressEventCamera, out localPointerPosition
            ))
            {
                Vector2 delta = localPointerPosition - pointerOffset;
                Vector2 newSize = parentRectTransform.sizeDelta + new Vector2(delta.x, -delta.y);
                newSize.x = Mathf.Clamp(newSize.x, minWidth, maxWidth);
                newSize.y = Mathf.Clamp(newSize.y, minHeight, maxHeight - 20);
                parentRectTransform.sizeDelta = newSize;
                pointerOffset = localPointerPosition;
            }
            GetComponentInParent<FullScreenRect>().isFullScreen = false;
        }

        /// <summary>
        /// Clamps the pointer position to the window boundaries.
        /// </summary>
        /// <param name="data">The pointer event data.</param>
        /// <returns>The clamped pointer position.</returns>
        private Vector2 ClampToWindow(PointerEventData data)
        {
            Vector2 rawPointerPosition = data.position;

            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(canvasCorners);

            float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
            float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

            return new Vector2(clampedX, clampedY);
        }

        /// <summary>
        /// Sets the minimum height for resizing the window.
        /// </summary>
        /// <param name="minHeight">The minimum height value.</param>
        internal void SetMinHeight(float minHeight)
        {
            this.minHeight = minHeight;
        }

        /// <summary>
        /// Sets the minimum width for resizing the window.
        /// </summary>
        /// <param name="minWidth">The minimum width value.</param>
        internal void SetMinWidth(float minWidth)
        {
            this.minWidth = minWidth;
        }
    }
}
