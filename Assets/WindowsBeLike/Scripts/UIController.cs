using Pooling;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    public class UIController : MonoBehaviour
    {
        public Window DefaultWindowPrefab;
        public Window ConsoleWindow;
        public static UIController Instance { get; private set; }
        public Pooler poolerPrefab;
        Pooler pooler;
        public LayerMask UILayerMask;
        private CanvasScaler canvasScaler;
        public float scaleStep = 0.1f;
        public float TaskAreaHeight = 22f;
        public Color32 DefaultTextColor;
        public Color32 PlayerTextColor;
        public Color32 WarningTextColor;
        public Color32 DisabledColor;
        public Color32 DefaultBacgroundColor;

        Dictionary<string, Window> windowList = new Dictionary<string, Window>();

        private void OnEnable()
        {
            pooler = FindObjectOfType<Pooler>();
            if (pooler == null)
            {
                Instantiate(poolerPrefab);
            }
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
                ConsoleWindow.gameObject.SetActive(!ConsoleWindow.gameObject.activeSelf);
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

        public void NewWindow()
        {
            GameObject newWindow = Pooler.root.GetPooledInstance(DefaultWindowPrefab.gameObject);
            newWindow.transform.SetParent(transform);
            newWindow.SetActive(true);
        }

    }
}