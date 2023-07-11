using UnityEngine;

public class SetAnchoredPositionToMouse : MonoBehaviour
{
    private RectTransform rectTransform;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to local coordinates relative to the RectTransform's parent
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, mousePosition, null, out Vector2 localPoint);

        // Set the anchored position of the RectTransform to the local point
        rectTransform.anchoredPosition = localPoint;
    }
}
