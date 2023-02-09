using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WindowsBeLike;
namespace WindowsBeLike
{
    public class DefaultButton : Button
    {
        public void OnClick()
        {
            GetComponentInParent<MenuController>().ToggleMenu();
        }
    }

}