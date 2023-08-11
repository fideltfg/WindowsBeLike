using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WindowsBeLike
{
    public class SnapWindow : Window
    {
        // The distance from the edge of the canvas in pixels that the window will snap
        public int snapDistance = 10;
        // The time (in seconds) the mouse needs to be within the snap distance for snapping to occur
        public float snapTimeThreshold = 1f;

        private float timeWithinSnapDistance;

        // Reference to the DragPanel component attached to the window
        private DragPanel dragPanel;

        // Override the Start method from the base class
        public override void Start()
        {
            base.Start();
            // Get the reference to the DragPanel component
            dragPanel = GetComponentInChildren<DragPanel>();
        }

        // Override the Update method from the base class
        public override void Update()
        {
            base.Update();

            // Check if the window is being dragged
            if (dragPanel != null && dragPanel.IsDragging)
            {
                // Convert mouse position to canvas space
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out Vector2 mousePositionOnCanvas);

                // Check how far the mouse is from the left, right, top, and bottom edges of the canvas
                float leftDistance = Mathf.Abs(mousePositionOnCanvas.x - canvasRect.rect.xMin);
                float rightDistance = Mathf.Abs(mousePositionOnCanvas.x - canvasRect.rect.xMax);
                float topDistance = Mathf.Abs(mousePositionOnCanvas.y - canvasRect.rect.yMax - UIController.Instance.TaskAreaHeight);
                float bottomDistance = Mathf.Abs(mousePositionOnCanvas.y - canvasRect.rect.yMin);

                // If the mouse is within the snap distance, increase the timer
                if (leftDistance <= snapDistance || rightDistance <= snapDistance || topDistance <= snapDistance || bottomDistance <= snapDistance)
                {
                    timeWithinSnapDistance += Time.deltaTime;

                    // If the time exceeds the threshold, snap the window to the closest edge
                    if (timeWithinSnapDistance >= snapTimeThreshold)
                    {
                        // Find the nearest edge and snap the window to it
                        if (leftDistance <= rightDistance && leftDistance <= topDistance && leftDistance <= bottomDistance)
                        {
                            SnapToLeftEdge();
                        }
                        else if (rightDistance <= leftDistance && rightDistance <= topDistance && rightDistance <= bottomDistance)
                        {
                            SnapToRightEdge();
                        }
                        else if (topDistance <= leftDistance && topDistance <= rightDistance && topDistance <= bottomDistance)
                        {
                            SnapToTopEdge();
                        }
                        else if (bottomDistance <= leftDistance && bottomDistance <= rightDistance && bottomDistance <= topDistance)
                        {
                            SnapToBottomEdge();
                        }

                        // Reset the timer after snapping
                        timeWithinSnapDistance = 0f;

                        // Set the flag to indicate that the window is within the snap distance
                    }
                }
                else
                {
                    // Reset the timer and flag if the mouse is not within the snap distance
                    timeWithinSnapDistance = 0f;
                }
            }
            else
            {
                // Reset the timer and flag if the window is not being dragged
                timeWithinSnapDistance = 0f;
            }
        }

        // Snap the window's left edge to the left edge of the canvas
        private void SnapToLeftEdge()
        {
            // Set the window's position to the calculated snap position in world space
            rectTransform.position = new Vector3(0, canvasRect.rect.height, rectTransform.position.z);
            rectTransform.sizeDelta = new Vector2(canvasRect.rect.width * .5f, canvasRect.rect.height);
        }

        // Snap the window's right edge to the right edge of the canvas
        private void SnapToRightEdge()
        {
            // Set the window's position to the calculated snap position in world space
            rectTransform.position = new Vector3(canvasRect.rect.width * .5f, canvasRect.rect.height, rectTransform.position.z);
            rectTransform.sizeDelta = new Vector2(canvasRect.rect.width * .5f, canvasRect.rect.height);
        }

        // Snap the window's top edge to the top edge of the canvas
        private void SnapToTopEdge()
        {
            FullScreenRect fsr = GetComponent<FullScreenRect>();
            fsr.ToggleFullScreen();

            // set the full screen rect to the origion position
            //fsr.SetNewOrigin(dragPanel.startPosition, dragPanel.startSizeDelta);
        }

        // Snap the window's bottom edge to the bottom edge of the canvas
        private void SnapToBottomEdge()
        {
            // Set the window's position to the calculated snap position in world space
            rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.sizeDelta.y, rectTransform.position.z);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            // Get the parent canvas of the window
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                canvasRect = parentCanvas.GetComponent<RectTransform>();
            }
        }
    }
}
