using TMPro;
using UnityEngine;

[RequireComponent(typeof(FullScreenRect))]
public class Window : MonoBehaviour
{
    public string Title;
    public TextMeshProUGUI titleTextBox;

    public virtual void Update()
    {
        titleTextBox.text = Title;
    }
}