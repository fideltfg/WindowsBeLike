using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    public class ImageBlock : MonoBehaviour
    {

        public RectTransform parentRect;
        public Image image;

        void Start()
        {
            parentRect = GetComponentInParent<Window>().GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }

        public void Update()
        {
            float imageAspectRatio = (float)image.sprite.texture.width / (float)image.sprite.texture.height;
            float parentWidth = parentRect.rect.width - 22;
            float desiredHeight = parentWidth / imageAspectRatio;

            if (desiredHeight > parentRect.rect.height)
            {
                float desiredWidth = parentRect.rect.height * imageAspectRatio;
                image.rectTransform.sizeDelta = new Vector2(desiredWidth, parentRect.rect.height - 22);
            }
            else
            {
                image.rectTransform.sizeDelta = new Vector2(parentWidth, desiredHeight);
            }
        }
    }
}