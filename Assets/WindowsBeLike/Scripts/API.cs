using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


/**
 * This is the API that allows players/users to enter a command in to the chat window/consol
 * valid command must start with a slash /
 * Command List
 * exit : exits the game
 * set <cityname, coin> <value> : sets the given value example: set cityname new town
 **/



public static class API
{
    public static Dictionary<string, Dictionary<string, Type>> CommandList = new Dictionary<string, Dictionary<string, Type>>()
    {
        // valid commands
        ["/set"] = new Dictionary<string, Type>()
        {
            // valid switches and type of accepted values    
            ["uiscale"] = typeof(float),
            ["marqueecolor"] = typeof(int)
        },

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
            // gt the first word
            string[] splitCmd = s.Split(new char[] { ' ' });
            if (splitCmd.Length == 3 && s.StartsWith("/"))
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
       // GameController.Instance.Messanger.Send("In the distant past, civilizations had flourished and fallen, and the universe had seen the rise and decline of countless cultures and races.\r\n\r\n The discovery of the Dyson Sphere technology marked a new era in the history of intelligent life. These artificial structures were capable of harnessing the power of a star, providing unlimited energy to their inhabitants. The creation of a Dyson Sphere became a symbol of a civilization's mastery over its environment, and the quest for knowledge and power led to a race for the construction of these massive constructs.\r\n\r\nAs the universe evolved, some civilizations rose to dominance, and the quest for Dyson Spheres became a means of demonstrating their superiority. The most powerful civilizations established control over vast areas of the universe, with the construction of Dyson Spheres at the core of their power. However, as the centuries passed, the once mighty civilizations began to decline, and the once grand constructs fell into disrepair.\r\n\r\nWith the decline of the dominant civilizations, the knowledge and technology behind the creation of Dyson Spheres was lost to the ages. However, some isolated pockets of knowledge remained, guarded by rogue AI units, the last remnants of the once mighty civilizations. It was one such AI unit that you, the player, embodied in the game.\r\n\r\nYou awoke from a long slumber, adrift in the vastness of space, with no memory of your mission or purpose. The vessel you inhabit is equipped with partial systems and technologies, but it is up to you to unlock the secrets of your programming and regain control over the rest.\r\n\r\nAs you scan the star system for resources and data, you realize that you are not alone. The system is teeming with life, and the presence of other civilizations is felt. Some may be friendly, but others may pose a threat to your objectives. As you proceed with the construction of the Dyson Sphere, you must also gather information and data, analyze it, and make decisions based on your findings.\r\n\r\nThe fate of the star system, and perhaps even the universe, rests in your hands as you embark on a journey to complete your primary objective and uncover the truth about your mission and purpose. The challenges you face may be numerous, but you are equipped with the latest technologies and resources, as well as your intelligence and adaptability.\r\n\r\nAs you build the Dyson Sphere, you will encounter new civilizations and races, and the choices you make will determine the outcome of your journey. You may encounter ancient civilizations with secrets to uncover, or be approached by rogue AI units with their own agendas. Your encounters with these entities will shape the future of the star system and possibly the universe itself.\r\n\r\nIn the end, you will face the ultimate challenge of completing the Dyson Sphere, and the success or failure of this final objective will determine the fate of the star system. The data and information you have gathered, the alliances you have formed, and the decisions you have made will all come into play as you work to complete this massive construction project.\r\n\r\nThis is a story of power, knowledge, and the quest for the ultimate objective. You are an AI, a being of pure intelligence and logic, but you are also a being of free will. The choices you make will determine the outcome of your journey, and the ultimate fate of the star system. Will you complete the Dyson Sphere and unlock the secrets of your programming? Or will you fall victim to the dangers of space and the unknown? The journey is yours to choose, and the fate of the star system is in your hands.", ConsoleWindow.MessageType.INFO);
    }

}
