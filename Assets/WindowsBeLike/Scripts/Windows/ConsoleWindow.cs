/// <summary>
/// Represents a console window in the WindowsBeLike interface.
/// </summary>
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    /// <summary>
    /// Represents a console window that displays messages and accepts user input.
    /// </summary>
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

        /// <summary>
        /// Enum defining the types of messages for the console window.
        /// </summary>
        public enum MessageType
        {
            PLAYER,
            WARNING,
            INFO
        }

        [SerializeField]
        List<Message> MessageList = new List<Message>();

        /// <summary>
        /// Called when the console window is started.
        /// </summary>
        public override void Start()
        {
            base.Start();
            ConsoleIntro();
        }

        void Awake()
        {
            chatBox.ActivateInputField();
        }

        /// <summary>
        /// Called when the console window is enabled.
        /// </summary>
        public override void OnEnable()
        {
            base.OnEnable();
            chatBox.ActivateInputField();
        }

        /// <summary>
        /// Closes the console window.
        /// </summary>
        public override void CloseWindow()
        {
            chatBox.DeactivateInputField();
            base.CloseWindow();
        }

        void OnDisable()
        {
            chatBox.DeactivateInputField();
        }

        /// <summary>
        /// Clears the console window.
        /// </summary>
        public void CLS()
        {
            for (int i = 0; i < MessageList.Count; i++)
            {
                MessageList[i].textObject.gameObject.SetActive(false);
            }
            MessageList.Clear();
            ConsoleIntro();
        }

        /// <summary>
        /// Sends a message to the console window.
        /// </summary>
        /// <param name="messageText">The text of the message.</param>
        /// <param name="messageType">The type of the message (default is INFO).</param>
        public void Send(string messageText, MessageType messageType = MessageType.INFO)
        {
            if (MessageList.Count >= maxMessageCount)
            {
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
                textObject = t,
                messageType = messageType
            }));

            // needed to get the scroll view to refresh correctly.
            Canvas.ForceUpdateCanvases();

            // set focus back to the input box
            chatBox.ActivateInputField();
        }

        /// <summary>
        /// Formats the message based on its type and prefixes.
        /// </summary>
        /// <param name="message">The message to format.</param>
        /// <returns>The formatted message.</returns>
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

        void ConsoleIntro()
        {
            string text = "Welcome to Windows Be Like! Type /help or another valid command.";
            Send(text);
        }

        /// <summary>
        /// Called when input ends in the console window.
        /// </summary>
        public void OnInputEnd()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (chatBox.isFocused == false)
                {
                    if (chatBox.text.Length > 0)
                    {
                        // repeat the input back in the messenger window.
                        Send(chatBox.text, MessageType.PLAYER);
                        API.Parse(chatBox.text);
                        chatBox.text = "";
                    }
                }
                chatBox.ActivateInputField();
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
}
