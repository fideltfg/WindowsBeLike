/// <summary>
/// The API class provides a set of static methods for parsing and executing commands in a WindowsBeLike interface.
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WindowsBeLike
{
    /// <summary>
    /// Static class that represents the API for the WindowsBeLike interface.
    /// </summary>
    public static class API
    {
        /// <summary>
        /// Represents the command history.
        /// </summary>
        public static CommandHistory commandHistory = new CommandHistory();

        /// <summary>
        /// Dictionary containing the list of valid commands and their associated switches and types.
        /// </summary>
        public static Dictionary<string, Dictionary<string, Type>> CommandList = new Dictionary<string, Dictionary<string, Type>>()
        {
            ["/cls"] = null,
            ["/ls"] = null,

            ["/set"] = new Dictionary<string, Type>()
            {
                ["uiscale"] = typeof(float),
            },
            ["/open"] = new Dictionary<string, Type>()
            {
                ["console"] = null,
                ["settings"] = null,
            },
            ["/close"] = new Dictionary<string, Type>()
            {
                ["console"] = null,
                ["settings"] = null,
            }
        };

        /// <summary>
        /// Parses the given command and executes the corresponding action.
        /// </summary>
        /// <param name="cmd">The command to parse and execute.</param>
        public static void Parse(string cmd)
        {
            if (cmd != "")
            {
                string s = cmd;

                // add the command to the history
                commandHistory.AddCommand(s);

                if (s.StartsWith("?") || s.StartsWith("/help"))
                {
                    DisplayHelp(s);
                    return;
                }

                string[] splitCmd = s.Split(new char[] { ' ' });

                if (splitCmd.Length == 1 && s.StartsWith("/"))
                {
                    string _command = splitCmd[0];
                    if (CommandList.ContainsKey(_command))
                    {
                        switch (_command.ToLower())
                        {
                            case "/cls":
                                CLS();
                                break;
                        }
                    }
                }
                else if (splitCmd.Length == 3 && s.StartsWith("/"))
                {
                    string _command = splitCmd[0];
                    string _switch = splitCmd[1];

                    if (CommandList.ContainsKey(_command))
                    {
                        if (CommandList[_command].ContainsKey(_switch))
                        {
                            Dictionary<string, Type> switchList = CommandList[splitCmd[0]];

                            if (switchList.ContainsKey(_switch))
                            {
                                Type type = switchList[_switch];

                                switch (type.Name)
                                {
                                    case "Single":
                                        float floatValue;
                                        if (float.TryParse(splitCmd[2], out floatValue))
                                        {
                                            switch (_command.ToLower())
                                            {
                                                case "/set":
                                                    Set(_switch, floatValue);
                                                    break;
                                            }
                                        }
                                        break;
                                    case "Int32":
                                        Debug.Log("The value is an int");
                                        break;
                                    case "Boolean":
                                        Debug.Log("The value is a bool");
                                        break;
                                    case "String":
                                        Debug.Log("The value is a string");
                                        break;
                                    default:
                                        Debug.Log("The value type is unknown");
                                        break;
                                }
                            }
                        }
                    }
                }
                else if (splitCmd.Length == 2 && s.StartsWith("/"))
                {
                    string _command = splitCmd[0];
                    string _switch = splitCmd[1];
                    if (CommandList.ContainsKey(_command))
                    {
                        if (CommandList[_command].ContainsKey(_switch))
                        {
                            Dictionary<string, Type> switchList = CommandList[splitCmd[0]];

                            if (switchList.ContainsKey(_switch))
                            {
                                switch (_command.ToLower())
                                {
                                    case "/open":
                                        Open(_switch);
                                        break;
                                    case "/close":
                                        Close(_switch);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Closes the specified window.
        /// </summary>
        /// <param name="_switch">The window to close.</param>
        private static void Close(string _switch)
        {
            switch (_switch.ToLower())
            {
                case "console":
                    UIController.Instance.CloseConsoleWindow();
                    break;
                case "settings":
                    UIController.Instance.CloseSettingsWindow();
                    break;
            }
        }

        /// <summary>
        /// Opens the specified window.
        /// </summary>
        /// <param name="_switch">The window to open.</param>
        private static void Open(string _switch)
        {
            Debug.Log($"Open {_switch}");
            switch (_switch.ToLower())
            {
                case "console":
                    UIController.Instance.OpenConsoleWindow();
                    break;
                case "settings":
                    UIController.Instance.OpenSettingsWindow();
                    break;
            }
        }

        /// <summary>
        /// Sets the specified switch value.
        /// </summary>
        /// <param name="_switch">The switch to set.</param>
        /// <param name="value">The value to set.</param>
        private static void Set(string _switch, float value)
        {
            switch (_switch.ToLower())
            {
                case ("uiscale"):
                    UIController.Instance.SetUIScale(value);
                    break;
                case ("UIOpacity"):
                    UIController.Instance.SetUIOpacity(value);
                    break;
            }
        }

        /// <summary>
        /// Displays the help information.
        /// </summary>
        /// <param name="s">The help command.</param>
        private static void DisplayHelp(string s)
        {
            ConsoleWindow cw = UIController.Instance.ConsoleWindow as ConsoleWindow;
            switch (s)
            {
                default:
                    cw.Send("Windows Be Like Help");
                    cw.Send("Windows Be Like is a simple window and menu system controller or UIController that makes it easy to throw a Windows like interface into your game.\r\n\r\nJust getting here you have interacted with the core of the WBL system. The Task Menu and Window objects.\r\n\r\nThis demo scene has a WindowBeLike object that contains everything required to make it work. You can drop it into your game and modify it as you need.\r\n\r\nThe theme is simple so as to focus on the way the system works instead of how it looks. That bit is up to you. Or use it as is.\r\n\r\nLet's start with the interface.\r\nThe interface is built to mimic some of the earlier windows type desktop systems. So the buttons may be somewhat similar to modern windows, but not exactly\r\n\r\nIn the top Left of the canvas is the task button. It was the first object you clicked to open this console. The task menu is where you will go to start a task. This is where the system wide actions are centred.\r\n\r\nThis console window also has a task menu. This is based on the canvas task menu referred to as the window menu or window task menu. The actions found on the window menu are centred around the window it's attached too.\r\n\r\nNext to the window menu button is the Title Bar you can drag the title bar to reposition the window on the canvas.\r\n\r\nNext to that In the top right corner of the window is the full screen toggle button. This when clicked will cause the window it's attached to size up to fit the canvas. The window is also brought to the front if it is not currently. Clicking it again will return the window to its former size and position on the canvas.\r\n\r\nIn the lower right corner is the resize drag handle. That will pull the window to the front. and when dragged the window will resize accordingly.\r\n\r\nTo the left of the drag resize handle is the footer of the window. In this console window it is used as the input area for the console. In most windows the footer is used to display messages relevant to the window it's on.\r\n\r\nThis console is also equipped with scroll functionality that lets you see more of what's going on in here.\r\n\r\nTry resizing the window and moving it around using the controls discussed so far. When you are confident with their use, type /help <command> to display a list of useful console commands to get you started. \r\n");
                    break;
            }
        }

        /// <summary>
        /// Clears the console window.
        /// </summary>
        private static void CLS()
        {
            UIController.Instance.ConsoleWindow.CLS();
        }
    }
}

