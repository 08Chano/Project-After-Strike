using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene Manager Sets the game's variables for the turnt
/// </summary>

public class SceneManager : TurnManager
{
    /// <summary>
    /// Player options
    /// </summary>
    [Header("Gameplay")]
    public int TeamCount;
    public Color TeamColour;
    public int Faction;
    public int MapDimensions;

    private void Start()
    {
        //for (int i = 0; i < 10; i++) { for (int j = 0; j < 10; j++) { Instantiate(Resources.Load("Tile", typeof(GameObject)), new Vector2(i,j), Quaternion.identity, gameObject.transform); } }
    }
}
