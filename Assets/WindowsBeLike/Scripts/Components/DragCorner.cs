using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WindowsBeLike
{
    public class DragCorner : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private Vector2 pointerOffset;
        private RectTransform canvasRectTransform;
        private RectTransform dummyRectTransform;
        private RectTransform parentRectTransform;
        private float minWidth = 200f;
        private float minHeight = 75f;
        private float maxWidth;
        private float maxHeight;
        public Image CornerDragCursor;

        GameObject DummyObject
        {
            get
            {
                return UIController.Instance.WindowDragOutline;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvasRectTransform = canvas.transform as RectTransform;
                dummyRectTransform = DummyObject.transform as RectTransform;
                parentRectTransform = transform.parent.GetComponent<RectTransform>();
                maxWidth = canvasRectTransform.sizeDelta.x - parentRectTransform.anchoredPosition.x;
                maxHeight = canvasRectTransform.sizeDelta.y - parentRectTransform.anchoredPosition.y;
            }
            dummyRectTransform.SetAsLastSibling();
            DummyObject.GetComponent<RectTransform>().sizeDelta = parentRectTransform.sizeDelta;
            DummyObject.transform.position = parentRectTransform.position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, data.position, data.pressEventCamera, out pointerOffset);
        }

        public void OnDrag(PointerEventData data)
        {
            if (dummyRectTransform == null) { return; }

            DummyObject.SetActive(true);
            LockCursorWithinBounds();

            GetComponentInParent<FullScreenRect>().isFullScreen = false;

            Vector2 pointerPostion = ClampToWindow(data); 

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, pointerPostion, data.pressEventCamera, out Vector2 localPointerPosition
            ))
            {
                Vector2 delta = localPointerPosition - pointerOffset;
                Vector2 newSize = dummyRectTransform.sizeDelta + new Vector2(delta.x, -delta.y);
                newSize.x = Mathf.Clamp(newSize.x, minWidth, maxWidth);
                newSize.y = Mathf.Clamp(newSize.y, minHeight, maxHeight - 20);
                dummyRectTransform.sizeDelta = newSize;
                pointerOffset = localPointerPosition;
            }
            GetComponentInParent<FullScreenRect>().isFullScreen = false;
        }

        public void OnPointerUp(PointerEventData data)
        {
            DummyObject.SetActive(false);
            //   CornerDragCursor.gameObject.SetActive(false);
            UnlockCursor();
            StartCoroutine(LerpToSize(dummyRectTransform.sizeDelta));
        }

        private System.Collections.IEnumerator LerpToSize(Vector2 targetSize)
        {
            float lerpTime = 0.15f;
            float startTime = Time.time;
            Vector2 initialSize = parentRectTransform.sizeDelta;

            while (Time.time - startTime < lerpTime)
            {
                float t = (Time.time - startTime) / lerpTime;
                parentRectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, t);
                yield return null;
            }

            parentRectTransform.sizeDelta = targetSize;
        }

        void LockCursorWithinBounds()
        {
          //  Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        void UnlockCursor()
        {
           // Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private Vector2 ClampToWindow(PointerEventData data)
        {
            Vector2 rawPointerPosition = data.position;
            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(canvasCorners);
            float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
            float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);
            return new Vector2(clampedX, clampedY);
        }

        internal void SetMinHeight(float minHeight)
        {
            this.minHeight = minHeight;
        }

        internal void SetMinWidth(float minWidth)
        {
            this.minWidth = minWidth;
        }
    }
}
