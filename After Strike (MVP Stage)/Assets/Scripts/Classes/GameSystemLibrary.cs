using UnityEngine;

public class GameSystemsLibrary : MonoBehaviour
{

    /// <summary>
    /// Player options
    /// </summary>
    [Header("Gameplay")]
    public int TeamCount;
    public Color TeamColour;
    public int Faction;
    public int MapDimensions;

    /// <summary>
    /// In game modifiers
    /// </summary>

    // Funds
    [Header("Funds")]
    public int Mod_Funds_Starting;                          //How much funds the player starts with
    public int Mod_Funds_Generated = 1000;                  //How much funds is generated per captured tile
    public int Mod_Funds_Loss;                              //How much funds is lost after each captured tile is lost
    public bool Mod_Funds_isStolen;                         //Is funds stolen when an enemy tile is captured
    public bool Mod_FundsScaling_isEnabled;                 //Does the funds gained per turn change based on factors
    public bool Mod_FundsScaling_isLinear;
    public bool Mod_FundsScaling_Duration;
    public bool Mod_FundsScaling_Modifier;
    public int Mod_Funds_Modifier;

    //Costs
    [Header("Cost")]
    public int Mod_Cost_Repair = 100;                                   //Cost of Repairs
    public int Mod_Cost_Repair_Efficency = 20;                          //How much the unit recovers per start of turn
    public int Mod_Cost_Supply_Weak = 20;                               //How much a low resupply replenishes
    public bool Mod_Cost_Scaling_isEnabled;                             //Cost changes over game time
    public bool Mod_Cost_Scaling_isLinear;                              //Is it random fluctuation or fixed
    public bool Mod_Cost_Scaling_Duration;                              //Is the change random
    public bool Mod_Cost_Scaling_Modifier;                              //
    public int Mod_Cost_ScaleCost;

    //Environmental
    [Header("Weather")]
    public bool Weather_FogOfWar_isEnabled;
    public bool Weather_isEnabled;
    public bool Weather_Changes_isEnabled;
    public int Weather_IntervalRate;
    public int Weather_Value;
    public bool Weather_isRandom;
    public int Weather_Randomness;

    [Header("Weather Hazard")]
    public bool Weather_Hazard_isEnabled;
    public bool Weather_Hazard_Weather_isEnabled;
    public bool Weather_Hazard_ExWeather_isEnabled;
    public bool Weather_Hazard_River_isEnabled;
    public bool Weather_Hazard_Terrain_isEnabled;
    public bool Weather_Hazard_Unnatural_isEnabled;

    //Difficulty
    [Header("Difficulty")]
    public bool Gameplay_DeployedReady;

    public bool Gameplay_Unit_Fatigue_isEnabled;
    public int Gameplay_Unit_Fatigue_Cap;

    public bool Gameplay_Unit_Morale_isEnabled;
    public int Gameplay_Unit_Morale_Cap;

    public bool Gameplay_Unit_Ranks_isEnabled;
    public float Gameplay_Unit_Rank1_boost = 1.1f;
    public float Gameplay_Unit_Rank2_boost = 1.2f;
    public float Gameplay_Unit_Rank3_boost = 1.25f;

    [Header("Status Effects")]
    public bool Gameplay_Unit_Inflictions_isEnabled;
    public int Gameplay_Unit_Inflictions_Severity;
    public float Gameplay_Unit_Inflictions_Sick         = 0.87f;        //ID=1 - 15% Reduction
    public float Gameplay_Unit_Inflictions_Intoxicated  = 0.909f;       //ID=2 - 10% Reduction
    public float Gameplay_Unit_Inflictions_Cold         = 2;            //ID=3 - 100% Increase
    public float Gameplay_Unit_Inflictions_Frozen       = 0.8f;         //ID=4 - 25% Reduction
    public float Gameplay_Unit_Inflictions_Sweating     = 0.769f;       //ID=5 - 30% Reduction
    public float Gameplay_Unit_Inflictions_Sunstroke    = 0.769f;       //ID=6 - 30% Reduction
    public float Gameplay_Unit_Inflictions_Poisoned     = 0.741f;       //ID=7 - 35% Reduction
    public float Gameplay_Unit_Inflictions_Delirious    = 0.667f;       //ID=8 - 50% Reduction

    [Header("Combat")]
    public bool Gameplay_Unit_Sync_isEnabled;
    public int Gameplay_Unit_Sync_Intensity;
    public float Gameplay_Unit_Defending_Mod = 1.25f;
    public float Gameplay_Unit_isDefendable_Boost = 1.2f;

    public float Gameplay_Unit_Effective_Mod= 1.35f;

    public float Gameplay_Terrain_Boost1 = 1.1f;
    public float Gameplay_Terrain_Boost2 = 1.2f;
    public float Gameplay_Terrain_Boost3 = 1.3f;

    [Header("Abilities")]
    public bool Gameplay_Unit_Commanders_isEnabled;
    public bool Gameplay_Unit_CommanderPower_isEnabled;
    public int Gameplay_Unit_CommanderPower_PassiveCharge;
    public int Gameplay_Unit_CommanderPower_ActionCharge;
    public bool Gameplay_Unit_CommanderPower_OverCharge_isEnabled;
}