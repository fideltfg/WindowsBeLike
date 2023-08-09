/// <summary>
/// Represents the settings window in the WindowsBeLike interface.
/// </summary>
using UnityEngine.UI;

namespace WindowsBeLike
{
    /// <summary>
    /// Represents the settings window in the WindowsBeLike interface.
    /// </summary>
    public class SettingsWindow : SnapWindow
    {
        // add a slider to set the background opacity of windows
        public Slider UIOpacitySlider;
        public Slider UIScaleSlider;

        /// <summary>
        /// Called when the UIScaleSlider value changes.
        /// </summary>
        public void OnUIScaleSliderChange()
        {
            UIController.Instance.SetUIScale(UIScaleSlider.value);
        }

        /// <summary>
        /// Called when the UIOpacitySlider value changes.
        /// </summary>
        public void OnUIOpacitySliderChange()
        {
            UIController.Instance.SetUIOpacity(UIOpacitySlider.value);
        }
    }
}
