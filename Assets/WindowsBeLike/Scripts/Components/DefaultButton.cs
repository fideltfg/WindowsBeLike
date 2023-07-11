using UnityEngine.UI;
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