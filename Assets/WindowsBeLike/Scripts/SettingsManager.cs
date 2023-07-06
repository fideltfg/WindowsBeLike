using System;

using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static float windowOpacity = 1;
    public static float WindowOpacity
    {
        get { return windowOpacity; }
        set
        {
            if (windowOpacity != value)
            {
                OnWindowOpacityChange?.Invoke();
                windowOpacity = value;
            }
        }
    }

    private static float windowScale = 1;
    public static float WindowScale
    {
        get { return windowScale; }
        set
        {
            if (windowScale != value)
            {
                OnWindowScaleChange?.Invoke();
                windowScale = value;
            }
            windowScale = value;
        }
    }

    private static float windowScaleStep = 1;
    public static float WindowScaleStep
    {
        get { return windowScaleStep; }
        set { windowScaleStep = value; }
    }

    private static float taskAreaHeight = 22f;
    public static float TaskAreaHeight
    {
        get { return taskAreaHeight; }
        set
        {
            if (taskAreaHeight != value)
            {
                OnTaskAreaHeightChange?.Invoke();
                taskAreaHeight = value;
            }
        }
    }


    public static event Action OnWindowOpacityChange;
    public static event Action OnWindowScaleChange;
    public static event Action OnTaskAreaHeightChange;
    public static event Action OnTaskScaleChange;


}
