﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    public class ConsoleWindow : Window
    {
    
        public GameObject ChatPanel;
        public GameObject TextBlock;
        public GameObject ImageBlock;
        public TMP_InputField chatBox;
        public string PlayerPrefix;
        public string InfoPrefix;
        public string WaringPrefix;
        public int maxMessageCount = 100;

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
                        Send(chatBox.text, ConsoleWindow.MessageType.PLAYER);
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
            GameObject _textObject = Pooling.Pooler.root.GetPooledInstance(TextBlock);
            TextMeshProUGUI t = _textObject.GetComponent<TextMeshProUGUI>();
            _textObject.transform.SetParent(ChatPanel.transform, false);
            _textObject.gameObject.SetActive(true);
            MessageList.Add(FormatMessage(new Message()
            {
                messageText = messageText,
                // TODO change to use pooling
                textObject = t, //Instantiate(TextBlock, ChatPanel.transform).GetComponent<Text>(),
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
                    message.textObject.color = UIController.Instance.DefaultTextColor;
                    message.textObject.text = InfoPrefix + " " + message.messageText;
                    break;

                case MessageType.PLAYER:
                    message.textObject.color = UIController.Instance.PlayerTextColor;
                    message.textObject.text = PlayerPrefix + " " + message.messageText;
                    break;

                case MessageType.WARNING:
                    message.textObject.color = UIController.Instance.WarningTextColor;
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


}