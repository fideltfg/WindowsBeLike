using UnityEngine;
namespace WindowsBeLike
{
    public class RectTransformResize : MonoBehaviour
    {
        public RectTransform TargetRect;

        public void Update()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(TargetRect.rect.width - 20, rectTransform.sizeDelta.y);
        }
    }
}
