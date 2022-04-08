using AfterStrike.Enum;
using UnityEngine;

namespace AfterStrike.Class.Unit
{
    public class UnitAttributes : MonoBehaviour
    {
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

        public ArmourTypes Anti_Armour;
        public ArmourTypes ArmourType;

        [Header("Details")]
        [Range(1, 10)] public int Range;
        public bool LoS;
        public bool Advance;//upon attacking, does it move to said tile.
        [Range(1, 10)]
        public int Movement;
        public MovementType MoveType;
        public UnitType UnitType;

        [Header("Status")]
        public bool isDefending;
        public bool isWaiting;

        public int CommandPower;
        public int Rank;//Max is 3 for highest
        public bool Fatigue;//Inflicted after defending
        public int Morale;//0=Low, 1=Normal, 2+=High
        public int Infliction;//Weather based or inflicted through certain scenarios

        //Calculations for attack and defence after modifiers
        public float EffectiveAttackReturn()
        {
            float EFR = 0; //Effective Raw
            if (UsesAmmo && Ammo > 0) { EFR = AmmoAttack; } else { EFR = Attack; }
            switch (Rank)
            {
                case 1:
                    EFR = EFR * GameManager.GManager.Gameplay_Unit_Rank1_boost;
                    break;
                case 2:
                    EFR = EFR * GameManager.GManager.Gameplay_Unit_Rank2_boost;
                    break;
                case 3:
                    EFR = EFR * GameManager.GManager.Gameplay_Unit_Rank3_boost;
                    break;
                default:
                    break;
            }
            EFR = EFR * (HealthPool / WeaknessMod); //Percentage Strength

            return EFR;
        }
        public float EffectiveDefenceReturn()
        {
            float EFR = 0;
            //Terrain Modification
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, Vector3.down * 10, out hit, 10))
            {
                if (hit.collider.GetComponent<TerrainProperty>() != null)
                {
                    TerrainProperty terrain = hit.collider.GetComponent<TerrainProperty>();
                    //Checks if Unit is defending at all
                    if (terrain.IsDefendable && this.isDefending)
                    {
                        EFR = Defence * GameManager.GManager.Gameplay_Unit_Defending_Mod * GameManager.GManager.Gameplay_Unit_isDefendable_Boost;
                    }
                    else if (this.isDefending)
                    {
                        EFR = Defence * GameManager.GManager.Gameplay_Unit_Defending_Mod;
                    }
                    else
                    {
                        EFR = Defence;
                    }
                    //Checks for terrain bonuses
                    switch (terrain.DefenceValue)
                    {
                        case 1:
                            EFR = EFR * GameManager.GManager.Gameplay_Terrain_Boost1;
                            break;
                        case 2:
                            EFR = EFR * GameManager.GManager.Gameplay_Terrain_Boost2;
                            break;
                        case 3:
                            EFR = EFR * GameManager.GManager.Gameplay_Terrain_Boost3;
                            break;
                        default:
                            break;
                    }
                }
            }
            //Rank modification
            switch (Rank)
            {
                case 1:
                    EFR = EFR * GameManager.GManager.Gameplay_Unit_Rank1_boost;
                    break;
                case 2:
                    EFR = EFR * GameManager.GManager.Gameplay_Unit_Rank2_boost;
                    break;
                case 3:
                    EFR = EFR * GameManager.GManager.Gameplay_Unit_Rank3_boost;
                    break;
                default:
                    break;
            }

            EFR = EFR * (HealthPool / WeaknessMod); //Percentage Strength
            return EFR;
        }

        public int DamageReturn(float AStrength, float DStrength, UnitAttributes Aggressor)
        {
            //AStrength and DStrength are the values pushed through to account simultanious attack
            //AStrength and DStrength is the calculated before it's pulled, factored weakness mod
            //Values fed are either their return current values, or held values from GM.

            if (isDefending) { DStrength = DStrength * 1.25f; }
            if (CommandPower != 0) { DStrength = DStrength * (1 + (CommandPower / 100)); }

            if (GameManager.GManager.Gameplay_Unit_Fatigue_isEnabled) { if (Fatigue) { DStrength = DStrength * 0.9f; } }
            if (GameManager.GManager.Gameplay_Unit_Morale_isEnabled) { if (Morale < 1) { DStrength = DStrength * 0.9f; } else if (Morale == 1) { DStrength = DStrength * 1f; } else if (Morale > 1) { DStrength = DStrength * 1.1f; } }
            if (GameManager.GManager.Gameplay_Unit_Ranks_isEnabled) { if (Morale < 1) { DStrength = DStrength * 1.1f; } else if (Morale == 1) { DStrength = DStrength * 1.2f; } else if (Morale > 1) { DStrength = DStrength * 1.25f; } }
            if (GameManager.GManager.Gameplay_Unit_Inflictions_isEnabled)
            {
                switch (Infliction)
                {
                    case 1:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Sick * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    case 2:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Intoxicated * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    case 3:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Cold * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    case 4:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Frozen * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    case 5:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Sweating * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    case 6:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Sunstroke * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    case 7:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Poisoned * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    case 8:
                        DStrength += DStrength * (GameManager.GManager.Gameplay_Unit_Inflictions_Delirious * GameManager.GManager.Gameplay_Unit_Inflictions_Severity);
                        break;
                    default:
                        break;
                }
            }

            if (Aggressor.Anti_Armour == ArmourType) 
                AStrength = AStrength * 1.35f;

            //Modifier Stack
            float DamageValue = 50 * (Mathf.Pow(AStrength / DStrength, 0.366f));

            return Mathf.FloorToInt(DamageValue);
        }

        public void OnDestroy()
        {
            //Wipe any further attachments from this, such as tile icons added for player vision and markers that are attached to it
            Reset();
            FactionSided.GetComponent<Faction>().UnitRemove(this);
            //To Be Added
            //-Unit markers
            //-Death animation
            //-List wiping
        }
        public void Reset()
        {
            isDefending = false;
            isWaiting = false;
        }
    }
}