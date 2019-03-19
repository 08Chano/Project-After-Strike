using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLib : MonoBehaviour {
    [Header("Stats")]
    [Range(0, 100)] public int HealthPool;
    [Range(0, 99)] public int MaxFuelPool;
    public int FuelPool;
    [Range(1, 100)] public int WeaknessMod;

    public GameObject FactionSided;

    [Header("Combat")]
    [Range(1, 100)] public int Attack;
    [Range(1, 100)] public int Defence;

    public bool UsesAmmo;
    [Range(1, 100)] public int AmmoAttack;
    [Range(0, 9)] public int MaxAmmo;
    [Range(0, 9)] public int Ammo;

    [Header("Unit Anti")]
    public bool Anti_ArmourType1;
    public bool Anti_ArmourType2;
    public bool Anti_ArmourType3;
    public bool Anti_ArmourType4;
    public bool Anti_ArmourType5;

    [Header("Unit Type")]
    public bool Def_ArmourType1;//Personel
    public bool Def_ArmourType2;//Soft Armour
    public bool Def_ArmourType3;//Hard Armour
    public bool Def_ArmourType4;//Thin Hull
    public bool Def_ArmourType5;//Thick Hull

    [Header("Details")]
    [Range(1, 10)] public int Range;
    public bool LoS;
    public bool Advance;//upon attacking, does it move to said tile.
    [Range(1, 10)] public int Movement;
    [Range(0, 15)] public int UnitType;

    [Header("Status")]
    public bool isDefending;
    public bool isWaiting;

    public int CommandPower;
    public int Rank;//Max is 3 for highest
    public bool Fatigue;//Inflicted after defending
    public int Morale;//0=Low, 1=Normal, 2+=High
    public int Infliction;//Weather based or inflicted through certain scenarios

}