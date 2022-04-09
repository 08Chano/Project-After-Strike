using AfterStrike.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainProperty : MonoBehaviour
{
    public TerrainType TerrainType { get => m_TerrainType; private set => m_TerrainType = value; }

    public int DefenceValue { get => m_DefenceValue; private set => m_DefenceValue = value; }

    public bool IsCapturable { get => m_IsCapturable; private set => m_IsCapturable = value; }

    public bool IsDefendable { get => m_IsDefendable; private set => m_IsDefendable = value; }

    public bool IsBase { get => m_IsBase; private set => m_IsBase = value; }

    public FactionType Heldby { get => m_Heldby; private set => m_Heldby = value; }

    public bool IsOccupied => m_IsOccupied != FactionType.Neutral || m_IsOccupied == Heldby;

    public RecoveryType RecoveryStrength { get => m_RecoveryStrength; private set => m_RecoveryStrength = value; }

    public int CapturePower
    {
        get => m_CapturePower;
        set
        {
            m_CapturePower = value;
            CapturePowerUpdated?.Invoke(CapturePower);
        }
    }

    public Action<int> CapturePowerUpdated { get => m_CapturePowerUpdated; set => m_CapturePowerUpdated = value; }


    [Header("TIle Effects")]
    [SerializeField]
    private SpriteRenderer m_MainSpriteRenderer;
    [SerializeField]
    private Material m_AttackTileMat;
    [SerializeField]
    private Material m_MoveTileMat;
    [SerializeField]
    private Material m_TargetTileMat;

    /// <summary>
    /// Adjacent tiles that the unit is able to navigate to.
    /// </summary>
    public List<TerrainProperty> MovementTileLinks = new List<TerrainProperty>();

    /// <summary>
    /// Creates an AoE field that the unit is able to attack to, doesn't care if target is available.
    /// </summary>
    public List<TerrainProperty> AttackRangeList = new List<TerrainProperty>();

    public TerrainProperty parent = null;

    public SpriteRenderer MainSpriteRenderer => m_MainSpriteRenderer;

    private Action<int> m_CapturePowerUpdated;

    public bool Occuping;
    public bool isHostile;
    public bool isOccupied;
    public bool isTarget;
    public bool isValid;
    public bool isAccounted = false;

    private int m_DefenceValue = 0;
    private bool m_IsCapturable = false;
    private bool m_IsDefendable = false;
    private TerrainType m_TerrainType = TerrainType.Void;
    private bool m_IsBase = false;
    private FactionType m_IsOccupied = FactionType.Neutral;
    private FactionType m_Heldby = FactionType.Neutral;
    private RecoveryType m_RecoveryStrength = RecoveryType.None;
    private int m_CapturePower = 0;

    [Header("Unit Movement Cost")]
    private Dictionary<MovementType, int> m_MovementCostDict = new Dictionary<MovementType, int>();

    public void SetBase(bool isSeabase)
    {
        //Sets tile as faction's base, overriding terrain values
        if (isSeabase)
        {
            TravelCost(1, 1, 1, 1, 1, 1, 1, 1, 1);
        }
        else
        {
            TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);
        }

        IsBase = true;
        IsCapturable = true;
        IsDefendable = true;
        RecoveryStrength = RecoveryType.Strong;
    }

    public void SetTerrainProperties(TerrainType terrainType)
    {
        m_TerrainType = terrainType;

        switch (terrainType)
        {
            case TerrainType.Plains:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);
                break;
            case TerrainType.Marsh:
                TravelCost(2, 1, 2, 1, 999, 999, 999, 1, 1);//Marsh
                break;
            case TerrainType.Woods:
                TravelCost(1, 2, 2, 1, 999, 999, 999, 1, 1);//Woods
                break;
            case TerrainType.River:
                TravelCost(2, 1, 1, 1, 2, 999, 999, 1, 1);//River
                break;
            case TerrainType.BrokenEarth:
                TravelCost(2, 2, 999, 1, 999, 999, 999, 1, 1);//Broken Earth
                break;
            case TerrainType.Hills:
                TravelCost(2, 2, 3, 2, 999, 999, 999, 1, 1);//Mountain H1
                break;
            case TerrainType.Mountain:
                TravelCost(999, 999, 999, 999, 999, 999, 999, 999, 999);//Mountain H2
                break;
            case TerrainType.Crags:
                TravelCost(1, 2, 2, 1, 999, 999, 999, 1, 1);//Crags
                break;
            case TerrainType.Dunes:
                TravelCost(1, 1, 2, 1, 999, 999, 999, 1, 1);//Dunes
                break;
            case TerrainType.City:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//City
                IsCapturable = true;
                break;
            case TerrainType.Town:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//Town
                break;
            case TerrainType.Factory:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//Factory
                break;
            case TerrainType.Airport:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//Airport
                break;
            case TerrainType.Harbour:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//Dock
                break;
            case TerrainType.Rubble:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//Rubble
                break;
            case TerrainType.Road:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//Road
                break;
            case TerrainType.Bridge:
                TravelCost(1, 1, 1, 1, 999, 999, 999, 1, 1);//Bridge
                break;
            case TerrainType.Sea:
                TravelCost(999, 999, 999, 999, 1, 1, 1, 1, 1);//Sea
                break;
            case TerrainType.RoughSea:
                TravelCost(999, 999, 999, 999, 2, 1, 1, 1, 1);//Rough Sea
                break;
            case TerrainType.Fog:
                TravelCost(999, 999, 999, 999, 1, 1, 1, 1, 1);//Fog
                break;
            case TerrainType.Reef:
                TravelCost(999, 999, 999, 999, 2, 3, 3, 1, 1);//Reef
                break;
            default:
                TravelCost(999, 999, 999, 999, 999, 999, 999, 999, 999);
                break;
        }
    }

    private void TravelCost(int foot, int specialist, int tiresA, int tiresB, int tank, int shipA, int shipB, int air, int sub)
    {
        m_MovementCostDict.Clear();

        m_MovementCostDict = new Dictionary<MovementType, int>();

        m_MovementCostDict.Add(MovementType.Foot, foot);
        m_MovementCostDict.Add(MovementType.Specialist, specialist);
        m_MovementCostDict.Add(MovementType.TiresOne, tiresA);
        m_MovementCostDict.Add(MovementType.TiresTwo, tiresB);
        m_MovementCostDict.Add(MovementType.Tank, tank);

        m_MovementCostDict.Add(MovementType.SmallShip, shipA);
        m_MovementCostDict.Add(MovementType.LargeShip, shipB);
        m_MovementCostDict.Add(MovementType.Submarine, sub);

        m_MovementCostDict.Add(MovementType.Air, air);
    }

    public int TCost(MovementType unitType)
    {
        return m_MovementCostDict[unitType];
    }

    public void CaptureCall(FactionClass faction)
    {
        CapturePower = 0;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = faction.inGameID_Col;
        faction.CapturedTilesAdd(this.GetComponent<TerrainProperty>());
    }

    public int distance = 0;
    public int Range = 0;

    private void OnEnable()
    {
        GameManager.CancelToggle += Reset;
    }

    private void OnDisable()
    {
        GameManager.CancelToggle -= Reset;
    }

    private void Update()
    {
        //debug.drawray(gameobject.transform.position, vector3.forward, color.red, 1);
        if (isValid)
        {
            m_MainSpriteRenderer.gameObject.SetActive(true);

            if (isHostile)
            {
                m_MainSpriteRenderer.material = m_AttackTileMat;
            }
            else if (!isOccupied && !Occuping)
            {
                if (isTarget)
                {
                    m_MainSpriteRenderer.material = m_TargetTileMat;
                }
                else
                {
                    m_MainSpriteRenderer.material = m_MoveTileMat;
                }
            }
        }
        else
        {
            m_MainSpriteRenderer.gameObject.SetActive(false);
        }
    }

    //Movement
    public void ViableMovementGrid(GameObject Searcher)
    {
        Reset();

        MovementConnections(Vector3.forward, Searcher);
        MovementConnections(Vector3.back, Searcher);
        MovementConnections(Vector3.right, Searcher);
        MovementConnections(Vector3.left, Searcher);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            if (hit.collider.tag == Searcher.tag)
            {
                Occuping = true;
            }
        }
    }

    private void MovementConnections(Vector3 Axis, GameObject Searcher)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + Axis, halfExtents);

        foreach (Collider item in colliders)
        {
            TerrainProperty terrain = item.GetComponent<TerrainProperty>();

            if (terrain != null && !terrain.isOccupied)
            {
                RaycastHit hit;

                if (!Physics.Raycast(terrain.transform.position, Vector3.up, out hit, 1))
                {
                    MovementTileLinks.Add(terrain);
                }
                else
                {
                    if (hit.collider.tag == Searcher.tag)
                    {
                        MovementTileLinks.Add(terrain);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Line of Sight based Attacks
    /// </summary>
    /// <param name="Searcher"></param>
    /// <param name="Direction"></param>
    /// <param name="Distance"></param>
    public void LoSAttackGrid(GameObject Searcher, Vector3 Direction, int Distance)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + Direction, halfExtents);
        isValid = true;
        isHostile = true;
        Range = Distance;

        foreach (Collider item in colliders)
        {
            TerrainProperty terrain = item.GetComponent<TerrainProperty>();
            if (terrain != null)
            {
                AttackRangeList.Add(terrain);
                terrain.parent = gameObject.GetComponent<TerrainProperty>();
                if (Range > 1)
                {
                    terrain.LoSAttackGrid(Searcher, Direction, Range - 1);
                }
            }
        }
    }

    //Area of attack
    public void ViableTargetGrid(GameObject Searcher)
    {
        Reset();

        TargetGridHighlighter(Vector3.forward, Searcher);
        TargetGridHighlighter(Vector3.back, Searcher);
        TargetGridHighlighter(Vector3.right, Searcher);
        TargetGridHighlighter(Vector3.left, Searcher);
    }
    public void TargetGridHighlighter(Vector3 Axis, GameObject Searcher)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + Axis, halfExtents);

        foreach (Collider item in colliders)
        {
            TerrainProperty terrain = item.GetComponent<TerrainProperty>();
            if (terrain != null)
            {
                AttackRangeList.Add(terrain);
                isHostile = true;
            }
        }
    }

    public void Reset()
    {
        AttackRangeList.Clear();
        MovementTileLinks.Clear();
        Occuping = false;
        isHostile = false;
        isTarget = false;
        isValid = false;
        isAccounted = false;
        parent = null;
        distance = 0;
        Range = 0;
    }
}