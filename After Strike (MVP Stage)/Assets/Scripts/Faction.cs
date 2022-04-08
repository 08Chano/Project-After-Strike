using AfterStrike.Class.Unit;
using AfterStrike.Enum;
using AfterStrike.Manager;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
{

    //GameSystems
    public string inGameID_ID;
    public Color inGameID_Col;

    public bool isNPC;
    public bool isFriendly;
    public Sprite sprite_Commander;
    public Sprite sprite_Faction;
    public FactionType m_FactionType;
    //In game Content
    public string Faction_Name;

    public int Funds;

    public List<TerrainProperty> TerrainList = new List<TerrainProperty>();
    public List<UnitAttributes> UnitList = new List<UnitAttributes>();

    public void FactionGenerator(FactionType FactionID, Color FactionColor)
    {
        //Assigns colour
        if (FactionColor == Color.white)
        {
            //Uses Defualt is none is specified
            switch (FactionID)
            {
                case FactionType.Neutral:
                    break;
                case FactionType.TheState:
                    inGameID_Col = Color.blue;
                    break;
                case FactionType.PeoplesKingdom:
                    inGameID_Col = Color.green;
                    break;
                case FactionType.Analytica:
                    inGameID_Col = Color.red;
                    break;
                case FactionType.OldFaith:
                    inGameID_Col = Color.blue;
                    break;
                case FactionType.Occult:
                    inGameID_Col = new Color(128, 0, 128);
                    break;
                case FactionType.CyberStream:
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
            case FactionType.Neutral:
                break;
            case FactionType.TheState:
                Faction_Name = "The State";
                sprite_Commander = Resources.Load("FactionIcons/TheState/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/TheState/Faction", typeof(Sprite)) as Sprite;
                break;
            case FactionType.PeoplesKingdom:
                Faction_Name = "People's Kingdom";
                sprite_Commander = Resources.Load("FactionIcons/PeoplesKingdom/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/PeoplesKingdom/Faction", typeof(Sprite)) as Sprite;
                break;
            case FactionType.Analytica:
                Faction_Name = "Analytica";
                sprite_Commander = Resources.Load("FactionIcons/Analytica/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/Analytica/Faction", typeof(Sprite)) as Sprite;
                break;
            case FactionType.OldFaith:
                Faction_Name = "Old Faith";
                sprite_Commander = Resources.Load("FactionIcons/OldFaith/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/OldFaith/Faction", typeof(Sprite)) as Sprite;
                break;
            case FactionType.Occult:
                Faction_Name = "Thanatos' Suffering";
                sprite_Commander = Resources.Load("FactionIcons/ThanatosSuffering/Commander", typeof(Sprite)) as Sprite;
                sprite_Faction = Resources.Load("FactionIcons/ThanatosSuffering/Faction", typeof(Sprite)) as Sprite;
                break;
            case FactionType.CyberStream:
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
        GameScreenManager.UpdateProfiles(this.GetComponent<Faction>());
        Funds += TerrainList.Count * GameManager.GManager.Mod_Funds_Generated;

        print(Faction_Name + " is Starting their turn. Their funds are at: " + Funds + "and have " + TerrainList.Count + " tiles under their control, producing: " + GameManager.GManager.Mod_Funds_Generated);

        foreach (UnitAttributes item in UnitList)
        {
            if (item != null)
            {
                if (item.isWaiting)
                {
                    RaycastHit hit;

                    if (Physics.Raycast(item.transform.position, Vector3.down * 10, out hit, 10))
                    {
                        if (hit.collider.GetComponent<TerrainProperty>() != null)
                        {
                            TerrainProperty terrain = hit.collider.GetComponent<TerrainProperty>();

                            if (terrain.Heldby == m_FactionType)
                            {
                                item.GetComponent<UnitActions>().Action_Recover(terrain.RecoveryStrength, item.MaxFuelPool);
                            }
                        }
                    }
                }
                else if (item.isDefending)
                {
                    item.FuelPool -= (item.FuelPool / 10);
                }
                item.GetComponent<UnitProperties>().BeginPhase();
            }
            else
            {
                UnitList.Remove(item);
            }
        }
    }

    public void EndTurn_Faction()
    {
        foreach (UnitAttributes item in UnitList)
        {
            if (!item.isDefending || !item.isWaiting)
            {
                LevelManager.DM.EndTurn();
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
            if (item.GetComponent<UnitAttributes>() != null)
            {
                if (item.tag == this.tag && item.GetComponent<UnitAttributes>().FactionSided == null)
                {
                    item.GetComponent<UnitAttributes>().FactionSided = this.gameObject;
                    item.transform.parent = this.transform;
                    item.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.grey;
                    UnitList.Add(item.GetComponent<UnitAttributes>());
                }
            }
        }
    }

    public void CapturedTilesAdd(TerrainProperty terrain)
    {
        if (!TerrainList.Contains(terrain))
        {
            if (terrain.IsCapturable)
            {
                TerrainList.Add(terrain);
            }
        }
    }

    public void CapturedTilesRemove(TerrainProperty terrain)
    {
        if (TerrainList.Contains(terrain))
        {
            TerrainList.Remove(terrain);
        }
    }

    public void UnitAdd(UnitAttributes unit)
    {
        if (!UnitList.Contains(unit))
        {
            UnitList.Add(unit);
        }
    }

    public void UnitRemove(UnitAttributes unit)
    {
        if (UnitList.Contains(unit))
        {
            UnitList.Remove(unit);
        }
        if (UnitList.Count == 0)
        {
            LevelManager.DM.EndScenario();
        }
    }
}