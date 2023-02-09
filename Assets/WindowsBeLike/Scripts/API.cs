using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


namespace WindowsBeLike
{
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
                    string _value = splitCmd[2]; // type

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
                            else
                            {
                                //
                            }
                        }
                        else
                        {
                            //   GameController.Instance.Messanger.Send("Command requires a valid switch!");
                        }
                    }
                    else
                    {
                        //   GameController.Instance.Messanger.Send("Unknown command: " + splitCmd[0]);
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
        }


        private static void CLS()
        {
            ConsoleWindow cw = UIController.Instance.Console as ConsoleWindow;
            cw.CLS();
        }
    }

}