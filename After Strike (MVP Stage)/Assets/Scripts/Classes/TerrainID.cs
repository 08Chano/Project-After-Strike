using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainID : MonoBehaviour
{
    [Header("Unit Effects")]
    public int DefenceValue = 0;
    public bool isCapturable = false;
    public bool isDefendable = false;

    [Header("Terrain Attributes")]
    public bool isBase = false;
    public bool isCaptured = false;
    public bool isDefended = false;
    public string Heldby;
    private bool HasSuppliesStrong;
    private bool HasSuppliesWeak;
    private bool HasRepairs;
    public int CapturePower;

    [Header("Unit Movement Cost")]
    public int MoveCost_Foot;
    public int MoveCost_Mech;
    public int MoveCost_Vehicle;
    public int MoveCost_Tracks;

    public int MoveCost_LightBoat;
    public int MoveCost_MediumBoat;
    public int MoveCost_HeavyBoat;

    public int MoveCost_Jet;
    public int MoveCost_Rotor;

    [Header("TIle Effects")]
    protected Material TileMat_Attack;
    protected Material TileMat_Move;
    protected Material TileMat_Target;

    public void SetBase(string faction)
    {
        //Sets tile as faction's base, overriding terrain values
        if (gameObject.tag == "Sea")
        {
            TravelCost(1, 1, 1, 1, 1, 1, 1, 1, 1, "Sea HQ");
        }
        else
        {
            TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "HQ");
        }
        isBase = true;
        isCapturable = true;
        isDefendable = true;
        HasRepairs = true;
        HasSuppliesStrong = true;
    }

    public void TerrainType(int Identity)
    {

        TileMat_Attack = GameManager.GManager.Gameplay_Colour_Tile_Attack;
        TileMat_Move = GameManager.GManager.Gameplay_Colour_Tile_Movment;
        TileMat_Target = GameManager.GManager.Gameplay_Colour_Tile_Target;

        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load("Terrain/TerrainIcon" + Identity, typeof(Sprite)) as Sprite;
        switch (Identity)
        {
            case 0:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Plains");//Plains
                break;
            case 1:
                TravelCost(2, 1, 2, 1, 999, 999, 999, 1, 1, "Marsh");//Marsh
                break;
            case 2:
                TravelCost(1, 2, 2, 1, 999, 999, 999, 1, 1, "Woods");//Woods
                break;
            case 3:
                TravelCost(2, 1, 1, 1, 2, 999, 999, 1, 1, "River");//River
                break;
            case 4:
                TravelCost(2, 2, 999, 1, 999, 999, 999, 1, 1, "Broken Earth");//Broken Earth
                break;
            case 5:
                TravelCost(2, 2, 3, 2, 999, 999, 999, 1, 1, "Mountain H1");//Mountain H1
                break;
            case 6:
                TravelCost(999, 999, 999, 999, 999, 999, 999, 999, 999, "Mountain H2");//Mountain H2
                break;
            case 7:
                TravelCost(1, 2, 2, 1, 999, 999, 999, 1, 1, "Crags");//Crags
                break;
            case 8:
                TravelCost(1, 1, 2, 1, 999, 999, 999, 1, 1, "Dunes");//Dunes
                break;
            case 9:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "city");//City
                isCapturable = true;
                //isDefendable = false;
                break;
            case 10:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Town");//Town
                break;
            case 11:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Facotry");//Factory
                break;
            case 12:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Airport");//Airport
                break;
            case 13:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Hanger");//Hanger
                break;
            case 14:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Dock");//Dock
                break;
            case 15:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Port");//Port
                break;
            case 16:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Rubble");//Rubble
                break;
            case 17:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Road");//Road
                break;
            case 18:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1, "Bridge");//Bridge
                break;
            case 19:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 999, 1, "Tunnel");//Tunnel
                break;
            case 20:
                TravelCost(999, 999, 999, 999, 1, 1, 1, 1, 1, "Sea");//Sea
                break;
            case 21:
                TravelCost(999, 999, 999, 999, 2, 1, 1, 1, 1, "Rough Sea");//Rough Sea
                break;
            case 22:
                TravelCost(999, 999, 999, 999, 1, 1, 1, 1, 1, "Fog");//Fog
                break;
            case 23:
                TravelCost(999, 999, 999, 999, 2, 3, 3, 1, 1, "Reef");//Reef
                break;
            default:
                TravelCost(999, 999, 999, 999, 999, 999, 999, 999, 999, "Mountain H3");
                break;
        }
    }
    private void TravelCost(int foot, int mech, int vehicle, int track, int lboat, int mboat, int hboat, int jet, int rotor, string NewName)
    {
        MoveCost_Foot = foot;
        MoveCost_Mech = mech;
        MoveCost_Vehicle = vehicle;
        MoveCost_Tracks = track;
        MoveCost_LightBoat = lboat;
        MoveCost_MediumBoat = mboat;
        MoveCost_HeavyBoat = hboat;
        MoveCost_Jet = jet;
        MoveCost_Rotor = rotor;
        this.name = NewName;

    }
    public int TCost(int UnitType)
    {
        int travelCost;
        switch (UnitType)
        {
            case 0:
                travelCost = MoveCost_Foot;
                return travelCost;
            case 1:
                travelCost = MoveCost_Mech;
                return travelCost;
            case 2:
                travelCost = MoveCost_Vehicle;
                return travelCost;
            case 3:
                travelCost = MoveCost_Tracks;
                return travelCost;
            case 4:
                travelCost = MoveCost_LightBoat;
                return travelCost;
            case 5:
                travelCost = MoveCost_MediumBoat;
                return travelCost;
            case 6:
                travelCost = MoveCost_HeavyBoat;
                return travelCost;
            case 7:
                travelCost = MoveCost_Jet;
                return travelCost;
            case 8:
                travelCost = MoveCost_Rotor;
                return travelCost;
            default:
                travelCost = 999;
                return travelCost;
        }
    }
    public int RecoverID()
    {
        if (HasRepairs)
        {
            if (HasSuppliesStrong)
            {
                return 4;
            }
            else if (HasSuppliesWeak)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if (HasSuppliesStrong)
            {
                return 5;
            }
            else if (HasSuppliesWeak)
            {
                return 2;
            }
        }
        return 0;
    }

    public void CaptureCall(Faction faction)
    {
        CapturePower = 0;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = faction.inGameID_Col;
        faction.CapturedTilesAdd(this.GetComponent<TerrainProperty>());
    }
}