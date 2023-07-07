using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace WindowsBeLike
{
    public class SubMenuController : MenuController, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
    {


        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (MenuContainer != null)
            {
                MenuContainer.SetActive(true);
            }

        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (MenuContainer != null)
            {
                MenuContainer.SetActive(false);
            }
        }
    }

}