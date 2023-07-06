using Pooling;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    public class UIController : MonoBehaviour
    {
        public Window DefaultWindowPrefab;
        public ConsoleWindow ConsoleWindow;
        public SettingsWindow SettingsWindow;
        public ModalWindow Modal;

        public static UIController Instance { get; private set; }
        public Pooler poolerPrefab;
        Pooler pooler;
        public LayerMask UILayerMask;
        private CanvasGroup canvasGroup;
        private CanvasScaler canvasScaler;

        // provids a callback for when the window focus changes
        public event Action OnWindowFocusChange;

        public float scaleStep = 0.1f;
        public float TaskAreaHeight = 22f;
        public Color32 DefaultTextColor;
        public Color32 PlayerTextColor;
        public Color32 WarningTextColor;
        public Color32 DisabledColor;
        public Color32 DefaultBacgroundColor;

        private Window windowInFocus;
        public Window WindowInFocus
        {
            get
            {
                return windowInFocus;
            }
            set { 
                if(value != windowInFocus)
                {
                    OnWindowFocusChange?.Invoke();
                    windowInFocus = value;
                }
                
            }
        }

        [Serialize]
        public List<Window> WindowList = new List<Window>();

        private void OnEnable()
        {
            // get the pooler object from the scene 
            pooler = FindObjectOfType<Pooler>();

            // if there is no pooler in the scene, instantiate one
            if (pooler == null)
            {
                Instantiate(poolerPrefab);
            }

            Instance = this;

            // get the canvas scaler component
            canvasScaler = GetComponent<CanvasScaler>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // debug shortcut
                SetUIScale(1.0f);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                canvasScaler.scaleFactor += SettingsManager.WindowScaleStep;
            }

        }

        public static bool ObjectInLayerMask(GameObject obj, LayerMask layer)
        {
            return (layer.value & (1 << obj.layer)) > 0;
        }

        public void NewWindow()
        {
            GameObject newWindow = Pooler.root.GetPooledInstance(DefaultWindowPrefab.gameObject);
            newWindow.transform.position = transform.position;
            newWindow.transform.SetParent(transform);
            newWindow.SetActive(true);
            AddNewWindowToWindowList(newWindow.GetComponent<Window>());
        }

        public void AddNewWindowToWindowList(Window window)
        {
            if (WindowList.Contains(window) == false)
            {
                WindowList.Add(window);
                window.windowIndex = WindowList.IndexOf(window);
            }
        }

        public void DisplayModal(string question, Action yesCallback, Action noCallback, string note = "", string pos = "Yes", string neg = "No")
        {
            // Debug.Log(question);
            if (Modal != null)
            {
                Modal.RegisterOnClickYesCallback(yesCallback);
                Modal.RegisterOnClickNoCallback(noCallback);
                Modal.Question = question;
                Modal.Note = note;
                Modal.PositiveResponce = pos;
                Modal.NegativeResponce = neg;
            }
            Modal.gameObject.SetActive(true);
        }

        /// <summary>
        /// Called when the modal yes is clicked
        /// you should write your own callback
        /// </summary>
        public void ModalYesCallback()
        {
            Modal.UnregisterOnClickYesCallback(ModalYesCallback);
            Modal.UnregisterOnClickNoCallback(ModalNoCallback);
        }

        /// <summary>
        /// Called when modal no is clicked
        /// you should write your own callback
        /// </summary>
        public void ModalNoCallback()
        {
            Modal.UnregisterOnClickYesCallback(ModalYesCallback);
            Modal.UnregisterOnClickNoCallback(ModalNoCallback);
        }

        private void OnApplicationQuit()
        {
           //Serializer.Save();
        }


        public void RegisterOnWindowFocusCallback(Action callback)
        {
            // register a callback to be called when the window focus changes
            OnWindowFocusChange += callback;
        }

        public void UnregisterOnWindowFocusCallback(Action callback)
        {
            // unregister a callback to be called when the window focus changes
            OnWindowFocusChange -= callback;
        }

        public void OpenSettingsWindow()
        {
            if (SettingsWindow != null)
            {
                SettingsWindow.gameObject.SetActive(true);
            }
        }

        public void CloseSettingsWindow()
        {
            if (SettingsWindow != null)
            {
                SettingsWindow.gameObject.SetActive(false);
            }
        }

        public void ToggleSettingsWindow()
        {
            if (SettingsWindow != null)
            {
                SettingsWindow.gameObject.SetActive(!SettingsWindow.gameObject.activeSelf);
            }
        }

        public void SetUIOpacity(float value)
        {
            canvasGroup.alpha = Mathf.Clamp(value, 0.02f, 1);
            // update the control in the settings window to reflect the change
            if (SettingsWindow != null && SettingsWindow.UIOpacitySlider != null)
            {
                SettingsWindow.UIOpacitySlider.value = value;
            }
        }

        public void SetUIScale(float value)
        {
            canvasScaler.scaleFactor = Mathf.Clamp(value, 0.02f, 1);
            // update the control in the settings window to reflect the change
            if (SettingsWindow != null && SettingsWindow.UIScaleSlider != null)
            {
                SettingsWindow.UIScaleSlider.value = value;
            }
        }

        internal void OpenConsoleWindow()
        {
            if (ConsoleWindow != null)
            {
                ConsoleWindow.gameObject.SetActive(true);
            }
        }

        internal void CloseConsoleWindow()
        {
            if (ConsoleWindow != null)
            {
                ConsoleWindow.gameObject.SetActive(false);
            }
        }

        public void ToggleConsoleWindow()
        {
            if (ConsoleWindow != null)
            {
                ConsoleWindow.gameObject.SetActive(!ConsoleWindow.gameObject.activeSelf);
            }
        }


    }
}