/// <summary>
/// Controller for the user interface in the WindowsBeLike interface.
/// </summary>
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    /// <summary>
    /// Represents the controller for the user interface in the WindowsBeLike interface.
    /// </summary>
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

        /// <summary>
        /// Gets or sets the window currently in focus.
        /// </summary>
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

        /// <summary>
        /// Checks if an object is in the specified layer mask.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <param name="layer">The layer mask to compare against.</param>
        /// <returns>True if the object is in the layer mask, otherwise false.</returns>
        public static bool ObjectInLayerMask(GameObject obj, LayerMask layer)
        {
            return (layer.value & (1 << obj.layer)) > 0;
        }

        /// <summary>
        /// Adds a new window to the window list.
        /// </summary>
        /// <param name="window">The window to add.</param>
        public void AddNewWindowToWindowList(Window window)
        {
            if (WindowList.Contains(window) == false)
            {
                WindowList.Add(window);
                window.windowIndex = WindowList.IndexOf(window);
            }
        }

        /// <summary>
        /// Displays a modal window with the specified question, callbacks, note, and button labels.
        /// </summary>
        /// <param name="question">The question to display.</param>
        /// <param name="yesCallback">The callback for the Yes button.</param>
        /// <param name="noCallback">The callback for the No button.</param>
        /// <param name="note">The note to display.</param>
        /// <param name="pos">The label for the positive response button (default is "Yes").</param>
        /// <param name="neg">The label for the negative response button (default is "No").</param>
        public void DisplayModal(string question, Action yesCallback, Action noCallback, string note = "", string pos = "Yes", string neg = "No")
        {
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
        /// Called when the Yes button is clicked in the modal window.
        /// You should write your own callback.
        /// </summary>
        public void ModalYesCallback()
        {
            Modal.UnregisterOnClickYesCallback(ModalYesCallback);
            Modal.UnregisterOnClickNoCallback(ModalNoCallback);
        }

        /// <summary>
        /// Called when the No button is clicked in the modal window.
        /// You should write your own callback.
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

        /// <summary>
        /// Registers a callback to be called when the window focus changes.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        public void RegisterOnWindowFocusCallback(Action callback)
        {
            OnWindowFocusChange += callback;
        }

        /// <summary>
        /// Unregisters a callback from being called when the window focus changes.
        /// </summary>
        /// <param name="callback">The callback to unregister.</param>
        public void UnregisterOnWindowFocusCallback(Action callback)
        {
            OnWindowFocusChange -= callback;
        }

        /// <summary>
        /// Opens the settings window.
        /// </summary>
        public void OpenSettingsWindow()
        {
            if (SettingsWindow != null)
            {
                SettingsWindow.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Closes the settings window.
        /// </summary>
        public void CloseSettingsWindow()
        {
            if (SettingsWindow != null)
            {
                SettingsWindow.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Toggles the visibility of the settings window.
        /// </summary>
        public void ToggleSettingsWindow()
        {
            if (SettingsWindow != null)
            {
                SettingsWindow.gameObject.SetActive(!SettingsWindow.gameObject.activeSelf);
            }
        }

        /// <summary>
        /// Sets the opacity of the UI.
        /// </summary>
        /// <param name="value">The opacity value (between 0 and 1).</param>
        public void SetUIOpacity(float value)
        {
            canvasGroup.alpha = Mathf.Clamp(value, 0.02f, 1);
            // update the control in the settings window to reflect the change
            if (SettingsWindow != null && SettingsWindow.UIOpacitySlider != null)
            {
                SettingsWindow.UIOpacitySlider.value = value;
            }
        }

        /// <summary>
        /// Sets the scale of the UI.
        /// </summary>
        /// <param name="value">The scale value (between 0 and 1).</param>
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

        /// <summary>
        /// Toggles the visibility of the console window.
        /// </summary>
        public void ToggleConsoleWindow()
        {
            if (ConsoleWindow != null)
            {
                ConsoleWindow.gameObject.SetActive(!ConsoleWindow.gameObject.activeSelf);
            }
        }
    }
}
