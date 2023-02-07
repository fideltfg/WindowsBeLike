using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace WindowsBeLike
{
    public class MenuController : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
    {
        RectTransform targetWindow;
        UIController _UIController;
        public GameObject SubItem;
        public GameObject Highlighter;

        private void OnEnable()
        {

            _UIController = GetComponentInParent<UIController>();
            targetWindow = GetComponentInParent<RectTransform>();
            transform.SetAsLastSibling();
            if (SubItem != null)
            {
                SubItem.SetActive(false);
            }
            if (Highlighter != null)
            {
                Highlighter.SetActive(false);
            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (transform.parent != null)
            {
                transform.parent.SetAsLastSibling();
            }
            else
            {
                transform.SetAsLastSibling();
            }
       
            if (SubItem != null)
            {
                SubItem.SetActive(!SubItem.activeSelf);
                SubItem.transform.SetAsLastSibling();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Highlighter != null)
            {
                Highlighter.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (SubItem != null)
            {
                SubItem.SetActive(false);
            }
            if (Highlighter != null)
            {
                Highlighter.SetActive(false);
            }
        }

        public void SetScaleFactor(float value)
        {
            _UIController.SetScaleFactor(value);
        }

        public void ToggleConsoleWindow()
        {
            _UIController.ToggleConsoleWindow();
        }
    }

}