using UnityEngine;
using UnityEngine.EventSystems;

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

    void Awake()
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
    }

    void Update()
    {
        maxWidth = canvasRectTransform.sizeDelta.x - parentRectTransform.anchoredPosition.x;
        maxHeight = canvasRectTransform.sizeDelta.y - parentRectTransform.anchoredPosition.y;
    }


    public void OnPointerDown(PointerEventData data)
    {
        panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, data.position, data.pressEventCamera, out pointerOffset);
    }

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
            Vector2 newSize = parentRectTransform.sizeDelta + new Vector2(delta.x, -delta.y);// * 2;
            newSize.x = Mathf.Clamp(newSize.x, minWidth, maxWidth);
            newSize.y = Mathf.Clamp(newSize.y, minHeight, maxHeight);
            parentRectTransform.sizeDelta = newSize;
            pointerOffset = localPointerPosition;
        }
        GetComponentInParent<FullScreenRect>().isFullScreen = false;
    }

    Vector2 ClampToWindow(PointerEventData data)
    {
        Vector2 rawPointerPosition = data.position;

        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
        float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

        Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
        return newPointerPosition;
    }
}
