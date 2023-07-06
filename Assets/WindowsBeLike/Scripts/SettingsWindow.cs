using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    public class SettingsWindow : Window
    {

        // add a slider to set the background opacity of windows
        public Slider UIOpacitySlider;
        public Slider UIScaleSlider;

        public void OnUIScaleSliderChange()
        {
            UIController.Instance.SetUIScale(UIScaleSlider.value);
        }

        public void OnUIOpacitySliderChange()
        {
            UIController.Instance.SetUIOpacity(UIOpacitySlider.value);
        }

    }
}
