using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Modal : MonoBehaviour
{
    [Header("Resources")]
    public GameObject modal;
    public TextMeshProUGUI Title,Description;
    public Image Icon;
    [Header("Values")]
    public string title;
    public string description;
    [SerialisedField] public Sprite icon;

    void LateUpdate() {
        Title.text = title;
        Description.text = description;
        Icon.sprite = icon;
    }

    public void Launch(){
        modal.GetComponent<Animator>().Play("open");
    }
    
    public void Close(){
        modal.GetComponent<Animator>().Play("close");
    }
}

