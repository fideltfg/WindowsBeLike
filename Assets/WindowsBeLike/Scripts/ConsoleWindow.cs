using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// NOTE: DO NOT USER MESSANGER IN POOLING CLASS. CAUSES STACK OVER FLOW.


public class ConsoleWindow : Window
{
    public int maxMessageCount = 100;
    public GameObject ChatPanel;
    public GameObject TextObject;
    public TMP_InputField chatBox;

    public Color32 PlayerTextColor;
    public Color32 InfoTextColor;
    public Color32 WarningTextColor;

    public string PlayerPrefix;
    public string InfoPrefix;
    public string WaringPrefix;


    public enum MessageType
    {
        PLAYER, WARNING, INFO
    }

    [SerializeField]
    List<Message> MessageList = new List<Message>();

    public override void Update()
    {
        base.Update();
        // TODO move this to somplace more sensible
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (chatBox.isFocused == false)
            {
                if (chatBox.text.Length > 0)
                {
                    // repeat the input back in the messanger window.

                    API.Parse(chatBox.text);
                    chatBox.text = "";
                }
            }
        }

    }

    public void Send(string messageText, MessageType type = MessageType.INFO)
    {
        if (MessageList.Count >= maxMessageCount)
        {
            // Destroy(MessageList[0].textObject.gameObject);
            MessageList[0].textObject.gameObject.SetActive(false);
            MessageList.RemoveAt(0);
        }
        GameObject _textObject = Pooling.Pooler.root.GetPooledInstance(TextObject);
        TextMeshProUGUI t = _textObject.GetComponent<TextMeshProUGUI>();
        _textObject.transform.SetParent(ChatPanel.transform, false);
        _textObject.gameObject.SetActive(true);
        MessageList.Add(FormatMessage(new Message()
        {
            messageText = messageText,
            // TODO change to use pooling
            textObject = t, //Instantiate(TextObject, ChatPanel.transform).GetComponent<Text>(),
            messageType = type
        }));
        // needed to get the scroll view to refresh corretly.
        Canvas.ForceUpdateCanvases();
      //  ChatPanel.GetComponentInParent<ScrollRect>().scroll
    }

    private Message FormatMessage(Message message)
    {
        switch (message.messageType)
        {
            case MessageType.INFO:
                message.textObject.color = InfoTextColor;
                message.textObject.text = InfoPrefix + " " + message.messageText;
                break;

            case MessageType.PLAYER:
                message.textObject.color = PlayerTextColor;
                message.textObject.text = PlayerPrefix + " " + message.messageText;
                break;

            case MessageType.WARNING:
                message.textObject.color = WarningTextColor;
                message.textObject.text = WaringPrefix + " " + message.messageText;
                break;
        }

        return message;
    }

    public void OnInputChange()
    {
        Send("Input End", MessageType.WARNING);
    }

}

[Serializable]
public class Message
{
    public string messageText;
    public TextMeshProUGUI textObject;
    public ConsoleWindow.MessageType messageType;


}

