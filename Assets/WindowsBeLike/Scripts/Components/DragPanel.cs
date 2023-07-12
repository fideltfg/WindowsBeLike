/// <summary>
/// Handles the dragging behavior of the panel within a window.
/// </summary>
using UnityEngine.EventSystems;
using UnityEngine;

namespace WindowsBeLike
{
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private Vector2 pointerOffset;
        private RectTransform canvasRect;
        private RectTransform parentRect;

        /// <summary>
        /// Called when the pointer is pressed down on the panel.
        /// </summary>
        /// <param name="data">The pointer event data.</param>
        public void OnPointerDown(PointerEventData data)
        {
            if (data.button != 0) return;

            if (parentRect == null)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    canvasRect = canvas.transform as RectTransform;
                    parentRect = transform.parent as RectTransform;
                }
            }

            parentRect.SetAsLastSibling();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, data.position, data.pressEventCamera, out pointerOffset);
        }

        /// <summary>
        /// Called when the pointer is dragged while holding down on the panel.
        /// </summary>
        /// <param name="data">The pointer event data.</param>
        public void OnDrag(PointerEventData data)
        {
            if (data.button != 0) return;
            if (parentRect == null)
                return;

            FullScreenRect fsr = GetComponentInParent<FullScreenRect>();

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, data.position, data.pressEventCamera, out localPointerPosition
            ))
            {
                float height = SettingsManager.TaskAreaHeight;
                if (parentRect.rect.height > canvasRect.rect.height - height)
                {
                    parentRect.sizeDelta = new Vector2(parentRect.rect.width, canvasRect.rect.height - height);
                }

                Vector2 minPosition = canvasRect.rect.min - parentRect.rect.min;
                Vector2 maxPosition = canvasRect.rect.max - parentRect.rect.max;
                maxPosition.y -= height;
                parentRect.localPosition = Vector3.Max(minPosition, Vector3.Min(maxPosition, localPointerPosition - pointerOffset));
            }
        }
    }
}
