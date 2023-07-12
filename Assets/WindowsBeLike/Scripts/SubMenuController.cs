/// <summary>
/// Controller for a sub menu in the WindowsBeLike interface.
/// </summary>
using UnityEngine.EventSystems;

namespace WindowsBeLike
{
    /// <summary>
    /// Represents a sub menu controller that handles pointer events and visibility of the menu container.
    /// </summary>
    public class SubMenuController : MenuController, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
    {
        /// <summary>
        /// Called when the pointer enters the sub menu.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (MenuContainer != null)
            {
                MenuContainer.SetActive(true);
            }
        }

        /// <summary>
        /// Called when the pointer exits the sub menu.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
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
