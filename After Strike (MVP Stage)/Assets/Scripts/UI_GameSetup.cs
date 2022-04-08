using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameSetup : MonoBehaviour
{

    public Dropdown[] DropdownContainer;

    public void StartGame()
    {
        foreach (Dropdown item in DropdownContainer)
        {
            if (item.IsActive())
            {
                switch (item.name)
                {
                    case "Teams":
                        //GameManager.GManager.ResourceCall_TotalPlayers(item.value);
                        //Debug.Log("Teams is here :" + item.value);
                        GameManager.GManager.TeamCount = item.value + 2;
                        break;

                    case "Colour":
                        //GameManager.GManager.ResourceCall_Faction(item.value);
                        //Debug.Log("Colour is here :" + item.options[item.value]);
                        //Debug.Log("Colour is here :" + item.transform.GetChild(0).GetComponent<Text>().text);
                        string itemised = item.transform.GetChild(0).GetComponent<Text>().text;
                        itemised.ToLower();
                        //print(itemised);
                        if (itemised.Contains("Grey"))
                        {
                            GameManager.GManager.TeamColour = new Color(0.5f, 0.5f, 0.5f);
                        }
                        else if (itemised.Contains("Red"))
                        {
                            GameManager.GManager.TeamColour = Color.red;
                        }
                        else if (itemised.Contains("Blue"))
                        {
                            GameManager.GManager.TeamColour = Color.blue;
                        }
                        else if (itemised.Contains("Yellow"))
                        {
                            GameManager.GManager.TeamColour = Color.yellow;
                        }
                        else if (itemised.Contains("Green"))
                        {
                            GameManager.GManager.TeamColour = Color.green;
                        }
                        else if (itemised.Contains("White"))
                        {
                            GameManager.GManager.TeamColour = new Color(0.9f, 0.9f, 0.9f);
                        }
                        else if (itemised.Contains("Black"))
                        {
                            GameManager.GManager.TeamColour = Color.black;
                        }
                        else if (itemised.Contains("Magenta"))
                        {
                            GameManager.GManager.TeamColour = Color.magenta;
                        }
                        else if (itemised.Contains("Orange"))
                        {
                            GameManager.GManager.TeamColour = new Color(1, 0.5f, 0);
                        }
                        else if (itemised.Contains("Maroon"))
                        {
                            GameManager.GManager.TeamColour = new Color(0.5f, 0, 0);
                        }
                        else
                        {
                            GameManager.GManager.TeamColour = Color.grey;
                        }
                        break;

                    case "Faction":
                        //GameManager.GManager.ResourceCall_Faction(item.value);
                        Debug.Log("Faction is here :" + item.value);
                        break;

                    case "MapSize":
                        switch (item.value)
                        {
                            case 0:
                                GameManager.GManager.MapDimensions = 10;
                                break;
                            case 1:
                                GameManager.GManager.MapDimensions = 15;
                                break;
                            case 2:
                                GameManager.GManager.MapDimensions = 20;
                                break;
                            case 3:
                                GameManager.GManager.MapDimensions = 25;
                                break;
                            default:
                                GameManager.GManager.MapDimensions = 10;
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        SceneManager.LoadScene(1);
        //GameManager.GManager.GameSetup(DropdownContainer[0].value, DropdownContainer[1].value, DropdownContainer[3].value);
    }
}