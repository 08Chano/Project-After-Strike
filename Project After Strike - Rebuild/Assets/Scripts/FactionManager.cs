using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : FactionLib
{
    public void FactionGenerator(int FactionID, Color FactionColor)
    {
        //Assigns colour
        if (FactionColor == Color.white)
        {
            //Uses Defualt is none is specified
            switch (FactionID)
            {
                case 0:
                    //"The State":
                    inGameID_Col = Color.blue;
                    break;
                case 1:
                    //"People's Kingdom":
                    inGameID_Col = Color.green;
                    break;
                case 2:
                    //"Analytica":
                    inGameID_Col = Color.red;
                    break;
                case 3:
                    //"Old Faith":
                    inGameID_Col = Color.blue;
                    break;
                case 4:
                    //"Thanatos' Suffering":
                    inGameID_Col = new Color(128, 0, 128);
                    break;
                case 5:
                    //"CyberNetics":
                    inGameID_Col = Color.cyan;
                    break;
                default:
                    break;
            }
        }
        else
        {
            inGameID_Col = FactionColor;
        }
        //Assigns the faction's name and values
        switch (FactionID)
        {
            case 0:
                //"The State":
                Faction_Name = "The State";
                sprite_Commander = Resources.Load("FactionIcons/TheState/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/TheState/Faction", typeof(Sprite)) as Sprite;
                break;
            case 1:
                //"People's Kingdom":
                Faction_Name = "People's Kingdom";
                sprite_Commander = Resources.Load("FactionIcons/PeoplesKingdom/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/PeoplesKingdom/Faction", typeof(Sprite)) as Sprite;
                break;
            case 2:
                //"Analytica":
                Faction_Name = "Analytica";
                sprite_Commander = Resources.Load("FactionIcons/Analytica/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/Analytica/Faction", typeof(Sprite)) as Sprite;
                break;
            case 3:
                //"Old Faith":
                Faction_Name = "Old Faith";
                sprite_Commander = Resources.Load("FactionIcons/OldFaith/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/OldFaith/Faction", typeof(Sprite)) as Sprite;
                break;
            case 4:
                //"Thanatos' Suffering":
                Faction_Name = "Thanatos' Suffering";
                sprite_Commander = Resources.Load("FactionIcons/ThanatosSuffering/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/ThanatosSuffering/Faction", typeof(Sprite)) as Sprite;
                break;
            case 5:
                //"CyberNetics":
                Faction_Name = "CyberNetics";
                sprite_Commander = Resources.Load("FactionIcons/CyberNetics/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/CyberNetics/Faction", typeof(Sprite)) as Sprite;
                break;
            default:
                break;
        }
    }

    public void StartTurn_Faction()
    {
        UIManager.UpdateProfiles(this.GetComponent<FactionManager>());
        Funds += lTile.Count * GameManager.Mod_Funds_Generated;

        print(Faction_Name + " is Starting their turn. Their funds are at: " + Funds + "and have " + lTile.Count + " tiles under their control, producing: " + GameManager.GManager.Mod_Funds_Generated);

        foreach (Controller_Unit item in lUnit)
        {
            if (item != null)
            {
                if (item.isWaiting)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(item.transform.position, Vector3.down * 10, out hit, 10))
                    {
                        if (hit.collider.GetComponent<Controller_Tile>() != null)
                        {
                            Controller_Tile terrain = hit.collider.GetComponent<Controller_Tile>();
                            if (terrain.isCaptured)
                            {
                                if (item.tag == terrain.Heldby)
                                {
                                    item.GetComponent<Controller_Unit>().Action_Recover(terrain.RecoverID(), item.MaxFuelPool);
                                }
                            }
                        }
                    }
                }
                else if (item.isDefending)
                {
                    item.FuelPool -= (item.FuelPool / 10);
                }
                item.GetComponent<Controller_Unit>().BeginPhase();
            }
            else
            {
                lUnit.Remove(item);
            }
        }
    }

    public void EndTurn_Faction()
    {
        foreach (Controller_Unit item in lUnit)
        {
            if (!item.isDefending || !item.isWaiting)
            {
                TurnManager.DM.EndTurn();
            }
        }
    }

    //Only called if this faction has no Units or Tiles under it's name
    public void FindThings()
    {
        GameObject[] findableUnits;
        findableUnits = GameObject.FindGameObjectsWithTag(this.tag);
        foreach (GameObject item in findableUnits)
        {
            if (item.GetComponent<Controller_Unit>() != null)
            {
                if (item.tag == this.tag && item.GetComponent<Controller_Unit>().FactionSided == null)
                {
                    item.GetComponent<Controller_Unit>().FactionSided = this.gameObject;
                    item.transform.parent = this.transform;
                    item.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.grey;
                    lUnit.Add(item.GetComponent<Controller_Unit>());
                }
            }
        }
    }

    public void CapturedTilesAdd(Controller_Tile terrain)
    {
        if (!lTile.Contains(terrain))
        {
            if (terrain.isCapturable)
            {
                lTile.Add(terrain);
            }
        }
    }
    public void CapturedTilesRemove(Controller_Tile terrain)
    {
        if (lTile.Contains(terrain))
        {
            lTile.Remove(terrain);
        }
    }
    public void UnitAdd(Controller_Unit unit)
    {
        if (!lUnit.Contains(unit))
        {
            lUnit.Add(unit);
        }
    }
    public void UnitRemove(Controller_Unit unit)
    {
        if (lUnit.Contains(unit))
        {
            lUnit.Remove(unit);
        }
        if (lUnit.Count == 0)
        {
            TurnManager.EndScenario();
        }
    }
}