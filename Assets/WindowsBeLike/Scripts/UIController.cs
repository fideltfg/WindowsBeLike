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

        public LayerMask UILayerMask;
        public float scaleStep = 0.1f;
        public float TaskAreaHeight = 22f;
        public Color32 DefaultTextColor;
        public Color32 PlayerTextColor;
        public Color32 WarningTextColor;
        public Color32 DisabledColor;
        public Color32 DefaultBacgroundColor;


        private CanvasGroup canvasGroup;
        private CanvasScaler canvasScaler;
        private int screenWidth;
        private int screenHeight;
        private Window windowInFocus;

        // provides a callback for when the resolution changes
        public static event Action<Vector2> OnResolutionChanged;
        // provids a callback for when the window focus changes
        public static event Action OnWindowFocusChange;

        public Window WindowInFocus
        {
            get
            {
                return windowInFocus;
            }
            set
            {
                if (value != windowInFocus)
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
            Instance = this;

            // get the canvas scaler component
            canvasScaler = GetComponent<CanvasScaler>();
            canvasGroup = GetComponent<CanvasGroup>();

            // Store the current screen resolution
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            // Subscribe to the resolution changed event
            OnResolutionChanged += OnResolutionChangedCallback;

        }

        private void OnDisable()
        {
            OnResolutionChanged -= OnResolutionChangedCallback;
        }

        private void Update()
        {
            if (Screen.width != screenWidth || Screen.height != screenHeight)
            {
                OnResolutionChanged?.Invoke(new Vector2(Screen.width, Screen.height));
            }
        }

        private void OnResolutionChangedCallback(Vector2 newScreenSize)
        {
            if (newScreenSize.x != screenWidth || newScreenSize.y != screenHeight)
            {
                screenHeight = (int)newScreenSize.y;
                screenWidth = (int)newScreenSize.x;
            }
        }
        public static bool ObjectInLayerMask(GameObject obj, LayerMask layer)
        {
            return (layer.value & (1 << obj.layer)) > 0;
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