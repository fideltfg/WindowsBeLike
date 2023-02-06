using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour
{
    bool leftButtonPressed = false;
    public bool LeftButtonPressed
    {
        get { return leftButtonPressed; }
        private set
        {
            if (value != leftButtonPressed)
            {
                leftButtonPressed = value;
            }
        }
    }

    bool leftButtonReleased = false;
    bool wasDragging = false;
    public bool LeftButtonReleased
    {
        get { return leftButtonReleased; }
        private set
        {
            // if the value chages
            if (value != leftButtonReleased)
            {
                // set the new value
                leftButtonReleased = value;

                if (leftButtonReleased == true)
                {
                    // invoke the callback
                    OnLeftButtonReleaseCallbackList?.Invoke(wasDragging);
                }
            }
        }
    }

    public bool LeftButtonHeld;

    bool leftButtonDoubleClick = false;
    public bool LeftButtonDoubleClick
    {
        get { return leftButtonDoubleClick; }
        private set
        {
            // if the value changes
            if (value != leftButtonDoubleClick)
            {
                // set the new value
                leftButtonDoubleClick = value;

                if (value == true)
                {
                    // invloke the callback
                    OnLeftDoubleClickCallbackList?.Invoke();
                    ActionOverUI = OverUI;
                }
                else
                {
                    ActionOverUI = false;
                }
            }
        }
    }

    public bool RightButtonPressed;
    public bool RightButtonHeld;
    public bool RightButtonReleased;
    public bool RightButtonDoubleClick;
    public bool MiddleButtonPressed;
    public bool MiddleButtonHeld;
    public bool MiddleButtonReleased;

    public bool ActionOverUI = false;
    public bool isDragging = false;
    public bool IsDragging
    {
        get
        {
            return isDragging;
        }
        set
        {
            isDragging = value;
            if (value == true)
            {
                ActionOverUI = OverUI;
            }
            else
            {
                ActionOverUI = false;
            }

        }
    }
    public bool OverUI = false;
    public float ScrollWheel;
    /// <summary>
    /// 
    /// </summary>
    public Vector3 MouseDownScreenPoint;
    public Vector3 DragEndPosition;
    ///
    public Vector3 CurrentPosition;
    public float DragThreshold = 0.1f;
    public float DoubleClickTime = 0.2f;
    private float leftClickTime;
    private float rightClickTime;
    private Vector3 previousMousePosition;

    public LayerMask UILayerMask;

    public static Mouse Instance;

    public GameObject ObjectMouseIsOver;

    private void Awake()
    {
        Instance = this;
    }


    public Action OnLeftDoubleClickCallbackList;
    public Action<bool> OnLeftButtonReleaseCallbackList;

    public Action OnDoubleClickRightCallbackList;

    public void RegisterOnDoubleClickLeftCallback(Action callback)
    {
        OnLeftDoubleClickCallbackList += callback;
    }
    public void UnregisterOnDoubleClickLeftCallback(Action callback)
    {
        OnLeftDoubleClickCallbackList -= callback;
    }

    public void RegisterOnDoubleClickRightCallback(Action callback)
    {
        OnDoubleClickRightCallbackList += callback;
    }
    public void UnregisterOnDoubleClickRightCallback(Action callback)
    {
        OnDoubleClickRightCallbackList -= callback;
    }

    public void RegisterOnLeftButtonReleaseCallback(Action<bool> callback)
    {
        OnLeftButtonReleaseCallbackList += callback;
    }
    public void UnregisterOnLeftButtonReleaseCallback(Action<bool> callback)
    {
        OnLeftButtonReleaseCallbackList -= callback;
    }

    void Update()
    {
        CurrentPosition = Input.mousePosition;
        OverUI = IsPointerOverUIElement();
        ScrollWheel = Input.mouseScrollDelta.y;
        LeftButtonPressed = Input.GetMouseButtonDown(0);
        LeftButtonHeld = Input.GetMouseButton(0);
        LeftButtonReleased = Input.GetMouseButtonUp(0);
        RightButtonPressed = Input.GetMouseButtonDown(1);
        RightButtonHeld = Input.GetMouseButton(1);
        RightButtonReleased = Input.GetMouseButtonUp(1);
        MiddleButtonPressed = Input.GetMouseButtonDown(2);
        MiddleButtonHeld = Input.GetMouseButton(2);
        MiddleButtonReleased = Input.GetMouseButtonUp(2);

        if (LeftButtonPressed)
        {
            wasDragging = false;
            LeftButtonDoubleClick = Time.time - leftClickTime < DoubleClickTime;
            leftClickTime = Time.time;
            MouseDownScreenPoint = Input.mousePosition;
            previousMousePosition = Input.mousePosition;
        }

        if (LeftButtonHeld && !IsDragging)
        {
            if (Vector3.Distance(previousMousePosition, Input.mousePosition) > DragThreshold)
            {
                wasDragging = false;
                IsDragging = true;
            }
        }

        if (LeftButtonReleased)
        {
            DragEndPosition = Input.mousePosition;

            wasDragging = IsDragging;
            IsDragging = false;
        }

        if (RightButtonPressed)
        {
            // RightButtonDoubleClick = Time.time - rightClickTime < DoubleClickTime;
            rightClickTime = Time.time;
            MouseDownScreenPoint = Input.mousePosition;
            previousMousePosition = Input.mousePosition;
        }

        if (RightButtonHeld && !IsDragging)
        {
            if (Vector3.Distance(previousMousePosition, Input.mousePosition) > DragThreshold)
            {
                IsDragging = true;
            }
        }

        if (RightButtonReleased)
        {
            DragEndPosition = Input.mousePosition;
            IsDragging = false;
        }

        if (MiddleButtonHeld && !IsDragging)
        {
            if (Vector3.Distance(previousMousePosition, Input.mousePosition) > DragThreshold)
            {
                IsDragging = true;
            }
        }

        if (MiddleButtonReleased)
        {
            DragEndPosition = Input.mousePosition;
            IsDragging = false;
        }

        // must be at the end of the update method.
        previousMousePosition = Input.mousePosition;
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        if (raysastResults.Count > 0)
        {
            ObjectMouseIsOver = raysastResults[0].gameObject;
        }
        else
        {
            ObjectMouseIsOver = null;
            return false;
        }

        for (int index = 0; index < raysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raysastResults[index];

            if (UIController.ObjectInLayerMask(curRaysastResult.gameObject, UILayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
