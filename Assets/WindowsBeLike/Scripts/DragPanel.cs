using UnityEngine.EventSystems;
using UnityEngine;

namespace WindowsBeLike
{
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private Vector2 pointerOffset;
        private RectTransform canvasRect;
        private RectTransform parentRect;

        public void OnPointerDown(PointerEventData data)
        {
            if (data.button != 0) return;

            if(parentRect == null)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    canvasRect = canvas.transform as RectTransform;
                    parentRect = transform.parent as RectTransform;
                }
            }


            parentRect.SetAsLastSibling();

            // make the windows follow the mouse pointer at the point the user clicked.
            // without this the window will follow at the pivot point
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, data.position, data.pressEventCamera, out pointerOffset);
        }

        public void OnDrag(PointerEventData data)
        {
            if (data.button != 0) return;
            if (parentRect == null)
                return;

            Vector2 pointerPostion = ClampToWindow(data);

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, pointerPostion, data.pressEventCamera, out localPointerPosition
            ))
            {
                // Clamp the position of the panel to the bounds of the canvas
                Vector2 minPosition = canvasRect.rect.min - parentRect.rect.min;
                Vector2 maxPosition = canvasRect.rect.max - parentRect.rect.max;
                maxPosition.y -= UIController.Instance.TaskAreaHeight;
                parentRect.localPosition = Vector3.Max(minPosition, Vector3.Min(maxPosition, localPointerPosition - pointerOffset));
            }
        }

        Vector2 ClampToWindow(PointerEventData data)
        {
            Vector2 rawPointerPosition = data.position;

            Vector3[] canvasCorners = new Vector3[4];
            canvasRect.GetWorldCorners(canvasCorners);

            float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
            float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

            Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
            return newPointerPosition;
        }
    } 

}