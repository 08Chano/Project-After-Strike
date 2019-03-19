using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLib : MonoBehaviour {

    /// <summary>
    /// In game modifiers
    /// </summary>

    // Funds
    [Header("Funds")]
    public static int Mod_Funds_Starting;                          //How much funds the player starts with
    public static int Mod_Funds_Generated = 1000;                  //How much funds is generated per captured tile
    public static int Mod_Funds_Loss;                              //How much funds is lost after each captured tile is lost
    public static bool Mod_Funds_isStolen;                         //Is funds stolen when an enemy tile is captured
    public static bool Mod_FundsScaling_isEnabled;                 //Does the funds gained per turn change based on factors
    public static bool Mod_FundsScaling_isLinear;
    public static bool Mod_FundsScaling_Duration;
    public static bool Mod_FundsScaling_Modifier;
    public static int Mod_Funds_Modifier;

    //Costs
    [Header("Cost")]
    public static int Mod_Cost_Repair = 100;                                   //Cost of Repairs
    public static int Mod_Cost_Repair_Efficency = 20;                          //How much the unit recovers per start of turn
    public static int Mod_Cost_Supply_Weak = 20;                               //How much a low resupply replenishes
    public static bool Mod_Cost_Scaling_isEnabled;                             //Cost changes over game time
    public static bool Mod_Cost_Scaling_isLinear;                              //Is it random fluctuation or fixed
    public static bool Mod_Cost_Scaling_Duration;                              //Is the change random
    public static bool Mod_Cost_Scaling_Modifier;                              //
    public static int Mod_Cost_ScaleCost;

    //Environmental
    [Header("Weather")]
    public static bool Weather_FogOfWar_isEnabled;
    public static bool Weather_isEnabled;
    public static bool Weather_Changes_isEnabled;
    public static int Weather_IntervalRate;
    public static int Weather_Value;
    public static bool Weather_isRandom;
    public static int Weather_Randomness;

    [Header("Weather Hazard")]
    public static bool Weather_Hazard_isEnabled;
    public static bool Weather_Hazard_Weather_isEnabled;
    public static bool Weather_Hazard_ExWeather_isEnabled;
    public static bool Weather_Hazard_River_isEnabled;
    public static bool Weather_Hazard_Terrain_isEnabled;
    public static bool Weather_Hazard_Unnatural_isEnabled;

    //Difficulty
    [Header("Difficulty")]
    public static bool Gameplay_DeployedReady;

    public static bool Gameplay_Unit_Fatigue_isEnabled;
    public static int Gameplay_Unit_Fatigue_Cap;

    public static bool Gameplay_Unit_Morale_isEnabled;
    public static int Gameplay_Unit_Morale_Cap;

    public static bool Gameplay_Unit_Ranks_isEnabled;
    public static float Gameplay_Unit_Rank1_boost = 1.1f;
    public static float Gameplay_Unit_Rank2_boost = 1.2f;
    public static float Gameplay_Unit_Rank3_boost = 1.25f;

    [Header("Status Effects")]
    public static bool Gameplay_Unit_Inflictions_isEnabled;
    public static int Gameplay_Unit_Inflictions_Severity;
    public static float Gameplay_Unit_Inflictions_Sick = 0.87f;            //ID=1 - 15% Reduction
    public static float Gameplay_Unit_Inflictions_Intoxicated = 0.909f;    //ID=2 - 10% Reduction
    public static float Gameplay_Unit_Inflictions_Cold = 2;                //ID=3 - 100% Increase
    public static float Gameplay_Unit_Inflictions_Frozen = 0.8f;           //ID=4 - 25% Reduction
    public static float Gameplay_Unit_Inflictions_Sweating = 0.769f;       //ID=5 - 30% Reduction
    public static float Gameplay_Unit_Inflictions_Sunstroke = 0.769f;      //ID=6 - 30% Reduction
    public static float Gameplay_Unit_Inflictions_Poisoned = 0.741f;       //ID=7 - 35% Reduction
    public static float Gameplay_Unit_Inflictions_Delirious = 0.667f;      //ID=8 - 50% Reduction

    [Header("Combat")]
    public static bool Gameplay_Unit_Sync_isEnabled;
    public static int Gameplay_Unit_Sync_Intensity;
    public static float Gameplay_Unit_Defending_Mod = 1.25f;
    public static float Gameplay_Unit_isDefendable_Boost = 1.2f;

    public static float Gameplay_Unit_Effective_Mod = 1.35f;

    public static float Gameplay_Terrain_Boost1 = 1.1f;
    public static float Gameplay_Terrain_Boost2 = 1.2f;
    public static float Gameplay_Terrain_Boost3 = 1.3f;

    [Header("Abilities")]
    public static bool Gameplay_Unit_Commanders_isEnabled;
    public static bool Gameplay_Unit_CommanderPower_isEnabled;
    public static int Gameplay_Unit_CommanderPower_PassiveCharge;
    public static int Gameplay_Unit_CommanderPower_ActionCharge;
    public static bool Gameplay_Unit_CommanderPower_OverCharge_isEnabled;

    [Header("Visuals")]
    public static Material Gameplay_Colour_Tile_Movment;
    public static Material Gameplay_Colour_Tile_Attack;
    public static Material Gameplay_Colour_Tile_Target;

}