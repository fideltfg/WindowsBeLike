using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace WindowsBeLike
{
    public class MenuController : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
    {
        [HideInInspector]
        public UIController _UIController;
        public GameObject MenuContainer;
        public GameObject Highlighter;

        private void OnEnable()
        {
            _UIController = GetComponentInParent<UIController>();
            if (MenuContainer != null)
            {
                MenuContainer.SetActive(false);
            }
            if (Highlighter != null)
            {
                Highlighter.SetActive(false);
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (GetComponentInParent<Window>())
            {
                transform.parent.SetAsLastSibling();
            }
            else
            {
                transform.SetAsLastSibling();
            }
            ToggleMenu();
        }

        public virtual void ToggleMenu()
        {
            if (MenuContainer != null)
            {
                MenuContainer.SetActive(!MenuContainer.activeSelf);
                MenuContainer.transform.SetAsLastSibling();
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (Highlighter != null)
            {
                Highlighter.SetActive(true);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (MenuContainer != null)
            {
                MenuContainer.SetActive(false);
            }
            if (Highlighter != null)
            {
                Highlighter.SetActive(false);
            }
        }

        /// <summary>
        /// Called when the exit button is clicked
        /// </summary>
        public virtual void Exit()
        {
            _UIController.DisplayModal("Are you sure you want to exit?", _UIController.ModalYesCallback, _UIController.ModalNoCallback, "This does nothing in the editor!");
        }

        public virtual void Close()
        {
            _UIController.DisplayModal("Are you sure you want to close this window?", YesCloseWindowCallback, ModalNoCallback);
        }

        public virtual void YesCloseWindowCallback()
        {
            _UIController.Modal.UnregisterOnClickYesCallback(YesCloseWindowCallback);
            _UIController.Modal.UnregisterOnClickNoCallback(ModalNoCallback);

            GetComponentInParent<Window>().CloseWindow();
        }

        public virtual void ModalNoCallback()
        {
            _UIController.Modal.UnregisterOnClickYesCallback(YesCloseWindowCallback);
            _UIController.Modal.UnregisterOnClickNoCallback(ModalNoCallback);
        }



    }

}