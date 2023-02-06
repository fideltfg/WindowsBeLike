using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject ConsoleWindow;

    public GameObject InfoPanel;
    public static UIController Instance { get; private set; }
    public LayerMask UILayerMask;
    private CanvasScaler canvasScaler;
    public float scaleStep = 0.1f;

    private void OnEnable()
    {
        Instance = this;
        canvasScaler = GetComponent<CanvasScaler>();
    }

    public void UpdateInfoPanel(Selectable selectable)
    {
        if (selectable == null) return;
        //    InfoPanel.GetComponent<InfoPanel>().Populate(selectable);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // debug shortcut
            SetScaleFactor(1.0f);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // debug shortcut
            canvasScaler.scaleFactor += scaleStep;
        }

    }

    public void SetScaleFactor(float value)
    {
        canvasScaler.scaleFactor = Mathf.Clamp(value, 0.1f, 1);
    }

    public void ToggleConsoleWindow()
    {
        if (ConsoleWindow != null)
        {
            ConsoleWindow.SetActive(!ConsoleWindow.activeSelf);
        }
    }

/*    public void ResizeImageToFitParent(RectTransform imageRectTransform)
    {
        float parentWidth = imageRectTransform.parent.GetComponent<RectTransform>().rect.width;
        float aspectRatio = imageRectTransform.rect.height / imageRectTransform.rect.width;
        imageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth);
        imageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentWidth * aspectRatio);
    }*/

    public static bool ObjectInLayerMask(GameObject obj, LayerMask layer)
    {
        return (layer.value & (1 << obj.layer)) > 0;
    }

}