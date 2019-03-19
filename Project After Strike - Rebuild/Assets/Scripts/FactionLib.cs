using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store of all of the IDs and locations of what the base factions have
/// Unit Orders Land - > 3 Foot Soldiers, 2 Mechs, 5 Tracks, and 6 Vehicles
/// Unit Orders Air  - > 3 Jet-planes, and 3 Rotor-planes
/// Unit Orders Sea  - > 1 Light Boat, 4 Heavy Boats, and 2 Submarines
/// </summary>

public class FactionLib : MonoBehaviour {

    //GameSystems
    public string inGameID_ID;
    public Color inGameID_Col;

    public bool isNPC;
    public bool isFriendly;
    public Sprite sprite_Commander;
    public Sprite sprite_Faction;

    //In game Content
    public string Faction_Name;

    public int Funds;

    public List<Controller_Tile> lTile= new List<Controller_Tile>();
    public List<Controller_Unit> lUnit = new List<Controller_Unit>();
}