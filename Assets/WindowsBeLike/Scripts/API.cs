using System;
using System.Collections.Generic;
using UnityEngine;


namespace WindowsBeLike
{

    /// <summary>
    /// This is a demo class used to demonstrate how you could tie in the UI components into your project.
    /// This is not preduction code and should nnot be used in your finished product.
    /// 
    /// This code comes with no warranty.
    /// </summary>


    public static class API
    {
        public static Dictionary<string, Dictionary<string, Type>> CommandList = new Dictionary<string, Dictionary<string, Type>>()
        {
            // valid commands
            ["/cls"] = null,
            ["/ls"] = null,

            ["/set"] = new Dictionary<string, Type>()
            {
                // valid switches and type of accepted values    
                ["uiscale"] = typeof(float),
            },
            ["/open"] = new Dictionary<string, Type>()
            {
                ["console"] = null,
                ["window"] = null,
            },
            ["/close"] = new Dictionary<string, Type>()
            {
                ["console"] = null,
            }

        };

        public static void Parse(string cmd)
        {
            if (cmd != "")
            {
                string s = cmd.ToLower();


                if (s.StartsWith("?") || s.StartsWith("/help"))
                {
                    DisplayHelp();
                    return;
                }

                // if the string starts with a known string
                // get the first word
                string[] splitCmd = s.Split(new char[] { ' ' });

                if (splitCmd.Length == 1 && s.StartsWith("/"))
                {
                    string _command = splitCmd[0];
                    if (CommandList.ContainsKey(_command)) // its a valid command
                    {
                        switch (_command)
                        {
                            case "/cls":
                                CLS();
                                break;
                        }
                    }
                }
                else if (splitCmd.Length == 3 && s.StartsWith("/"))
                {
                    string _command = splitCmd[0];//  Dictionary<string, Dictionary<string, Type>>
                    string _switch = splitCmd[1];//   Dictionary<string, Type>

                    if (CommandList.ContainsKey(_command)) // its a valid command
                    {
                        if (CommandList[_command].ContainsKey(_switch)) // its a valid switch for the given command 
                        {
                            Dictionary<string, Type> switchList = CommandList[splitCmd[0]];

                            if (switchList.ContainsKey(_switch)) // its a valid switch for the given command 
                            {
                                Type type = switchList[_switch];

                                switch (type.Name)
                                {
                                    case "Single":
                                        //Debug.Log($"Switch requires {type.Name} value!");
                                        float floatValue;
                                        if (float.TryParse(splitCmd[2], out floatValue))
                                        {
                                            switch (_command)
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


                    string _command = splitCmd[0];//  Dictionary<string, Dictionary<string, Type>>
                    string _switch = splitCmd[1];//   Dictionary<string, Type>
                    if (CommandList.ContainsKey(_command)) // its a valid command
                    {
                        if (CommandList[_command].ContainsKey(_switch)) // its a valid switch for the given command 
                        {
                            Dictionary<string, Type> switchList = CommandList[splitCmd[0]];

                            if (switchList.ContainsKey(_switch)) // its a valid switch for the given command 
                            {

                                switch (_command)
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

        private static void Close(string _switch)
        {
            switch (_switch)
            {
                case "console":
                    UIController.Instance.Console.gameObject.SetActive(false);
                    break;
            }
        }

        private static void Open(string _switch)
        {
            switch (_switch)
            {
                case "console":
                    UIController.Instance.Console.gameObject.SetActive(true);
                    break;
                case "window":
                    UIController.Instance.NewWindow();
                    break;
            }
        }

        private static void Set(string _switch, float value)
        {
            switch (_switch)
            {
                case ("uiscale"):
                    UIController.Instance.SetScaleFactor(value);
                    break;
            }
        }

        private static void DisplayHelp()
        {
            ConsoleWindow cw = UIController.Instance.Console as ConsoleWindow;
            cw.Send("Windows Be Like Help");
            cw.Send("Windows Be Like is a simple window and menu system controller or UIController that makes it easy to throw a Windows like interface into your game.\r\n\r\nJust getting here you have interacted with the core of the WBL system. The Task Menu and Window objects.\r\n\r\nThis demo scene has a WindowBeLike object that contains everything required to make it work. You can drop it into your game and modify it as you need.\r\n\r\nThe theme is simple so as to focus on the way the system works instead of how it looks. That bit is up to you. Or use it as is.\r\n\r\nLet's start with the interface.\r\nThe interface is built to mimic some of the earlier windows type desktop systems. So the buttons may be somewhat similar to modern windows, but not exactly\r\n\r\nIn the top Left of the canvas is the task button. It was the first object you clicked to open this console. The task menu is where you will go to start a task. This is where the system wide actions are centred.\r\n\r\nThis console window also has a task menu. This is based on the canvas task menu referred to as the window menu or window task menu. The actions found on the window menu are centred around the window it's attached too.\r\n\r\nNext to the window menu button is the Title Bar you can drag the title bar to reposition the window on the canvas.\r\n\r\nNext to that In the top right corner of the window is the full screen toggle button. This when clicked will cause the window it's attached to size up to fit the canvas. The window is also brought to the front if it is not currently. Clicking it again will return the window to its former size and position on the canvas.\r\n\r\nIn the lower right corner is the resize drag handle. That will pull the window to the front. and when dragged the window will resize accordingly.\r\n\r\nTo the left of the drag resize handle is the footer of the window. In this console window it is used as the input area for the console. In most windows the footer is used to display messages relevant to the window it's on.\r\n\r\nThis console is also equipped with scroll functionality that lets you see more of what's going on in here.\r\n\r\nTry resizing the window and moving it around using the controls discussed so far. When you are confident with their use, type /help commands to display a list of useful console commands to get you started. \r\n");
        }

        private static void CLS()
        {
            ConsoleWindow cw = UIController.Instance.Console as ConsoleWindow;
            cw.CLS();
        }
    }
}