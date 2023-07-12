/// <summary>
/// Handles the sizing of an Image component within a window.
/// </summary>
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

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        void Start()
        {
            parentRect = GetComponentInParent<Window>().GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }

        /// <summary>
        /// Updates the size of the image based on the parent window's size.
        /// </summary>
        public void Update()
        {
            float imageAspectRatio = (float)image.sprite.texture.width / (float)image.sprite.texture.height;
            float parentWidth = parentRect.rect.width - 22;
            float desiredHeight = parentWidth / imageAspectRatio;

            if (desiredHeight > parentRect.rect.height)
            {
                float desiredWidth = parentRect.rect.height * imageAspectRatio;
                image.rectTransform.sizeDelta = new Vector2(desiredWidth, parentRect.rect.height);
            }
            else
            {
                image.rectTransform.sizeDelta = new Vector2(parentWidth, desiredHeight);
            }
        }
    }
}
