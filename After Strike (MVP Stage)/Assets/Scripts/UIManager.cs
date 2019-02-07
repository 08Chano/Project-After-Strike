using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Profiler;
    public static bool ActionsActive;

    [Header("Menu Containers")]
    public GameObject[] MenuHeader;
    public GameObject[] MenuContents;

    [Header("Top Left Containers")]
    public GameObject Sprite_Viewer;            //Top Left for terrain Sprite Display
    public GameObject Terrain_Viewer;           //Top Left for terrain Sprite Display
    public GameObject Unit_Viewer;              //Top Left for Unit Sprite Display
    public GameObject[] Unit_Statistics;        //Top Left for Unit's value display
    public GameObject[] Unit_InfoTab;           //Top Left container for the unit's toggle for unit values
    [Header("Actions Menu")]
    public GameObject[] Unit_ActionTab;         //
    public GameObject Indicator_AttackIcon;     //
    public GameObject Indicator_AttackValue;    //
    [Header("Top Right Containers")]
    public Image[] SpriteHolders;               //
    public Text UI_Resource;                    //
    public Text UI_Turn;                        //
    public int PhaseValue;                      //

    private GameObject icons;
    private float RefreshRate = 0.03f;

    void OnEnable()
    {
        GameManager.EscToggle += Toggler_Headers;
        GameManager.ActToggle += Toggler_Action;
        GameManager.StatToggle += Toggler_Unit;
    }
    void OnDisable()
    {
        GameManager.EscToggle -= Toggler_Headers;
        GameManager.ActToggle -= Toggler_Action;
        GameManager.StatToggle -= Toggler_Unit;
    }

    private void Awake()
    {
        if (Profiler == null)
        {
            print("Profiler set");
            Profiler = this;
        }
        else if (Profiler != this)
        {
            Destroy(gameObject);
        }
        icons = GameObject.Find("Commander Icon").gameObject;

        foreach (GameObject item in Unit_ActionTab)
        {
            Button itemised = item.GetComponent<Button>();
            switch (item.name)
            {
                case "Move":
                    itemised.onClick.AddListener(GameManager.GManager.CallAction_Move);
                    break;
                case "Attack":
                    itemised.onClick.AddListener(GameManager.GManager.CallAction_Attack);
                    break;
                case "Sync":
                    itemised.onClick.AddListener(GameManager.GManager.CallAction_Sync);
                    break;
                case "Defend":
                    itemised.onClick.AddListener(GameManager.GManager.CallAction_Defend);
                    break;
                case "Wait":
                    itemised.onClick.AddListener(GameManager.GManager.CallAction_Wait);
                    break;
                default:
                    break;
            }
        }
        foreach (GameObject item in MenuContents)
        {
            Button itemised = item.GetComponent<Button>();
            switch (item.name)
            {
                case "Resume":
                    itemised.onClick.AddListener(GameManager.GManager.Toggle);
                    break;
                case "QuickSave":
                    //Quick save function
                    //itemised.onClick.AddListener(GameManager.GManager.CallAction_Attack);
                    break;
                case "Options":
                    //Overlay the Menu options
                    //itemised.onClick.AddListener(GameManager.GManager.CallAction_Sync);
                    break;
                case "ExitGame":
                    //itemised.onClick.AddListener(GameManager.GManager.CallAction_Defend);
                    break;
                case "EndTurn":
                    itemised.onClick.AddListener(LevelManager.DM.EndTurn);
                    break;
                default:
                    break;
            }
        }
    }
    private void Start()
    {
        InvokeRepeating("Update_UI_Unit", RefreshRate, RefreshRate);
        InvokeRepeating("Update_UI_Terrain", RefreshRate, RefreshRate);
    }

    void Update_UI_Unit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        RaycastHit UnitRayHit;
        //        Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down * 10, Color.blue, 3);
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down * 10, out UnitRayHit, 9))
        {
            //Debug.Log("Unit is not held!");
            if (UnitRayHit.collider.tag.Contains("Faction"))
            {
                UnitAttributes TargetUnit = UnitRayHit.collider.GetComponent<UnitAttributes>();

                if (!Unit_Viewer.activeSelf)
                {
                    //Sets the Unit Display to active if unit is hovered over with mouse
                    Unit_Viewer.SetActive(true);
                    Sprite_Viewer.SetActive(true);
                    Unit_Viewer.GetComponent<Image>().sprite = TargetUnit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                }


                for (int i = 0; i < Unit_Statistics.Length; i++)
                {
                    //Debug.Log("Updating!");
                    switch (Unit_Statistics[i].name)
                    {

                        case "HP Text":
                            Unit_Statistics[i].GetComponent<Text>().text = Mathf.RoundToInt(TargetUnit.HealthPool / 10).ToString() + "/10";
                            break;

                        case "HP Bar":
                            Unit_Statistics[i].GetComponent<Slider>().value = TargetUnit.HealthPool;
                            break;

                        case "Ammo Text":
                            Unit_Statistics[i].GetComponent<Text>().text = TargetUnit.Ammo.ToString() + "/" + TargetUnit.MaxAmmo.ToString();
                            break;

                        case "Ammo Bar":
                            Unit_Statistics[i].GetComponent<Slider>().value = TargetUnit.Ammo;
                            Unit_Statistics[i].GetComponent<Slider>().maxValue = TargetUnit.MaxAmmo;
                            break;

                        case "Fuel Text":
                            Unit_Statistics[i].GetComponent<Text>().text = TargetUnit.FuelPool.ToString() + "/" + TargetUnit.MaxFuelPool.ToString();
                            break;

                        case "Fuel Bar":
                            Unit_Statistics[i].GetComponent<Slider>().value = TargetUnit.FuelPool;
                            Unit_Statistics[i].GetComponent<Slider>().maxValue = TargetUnit.MaxFuelPool;
                            break;

                        case "Attack Meter":
                            Unit_Statistics[i].GetComponent<Slider>().value = TargetUnit.Attack;
                            break;

                        case "Attack Value":
                            Unit_Statistics[i].GetComponent<Text>().text = TargetUnit.Attack.ToString();
                            break;

                        case "Defence Meter":
                            Unit_Statistics[i].GetComponent<Slider>().value = TargetUnit.Defence;
                            break;

                        case "Defence Value":
                            Unit_Statistics[i].GetComponent<Text>().text = TargetUnit.Defence.ToString();
                            break;

                        case "Movement Meter":
                            Unit_Statistics[i].GetComponent<Slider>().value = TargetUnit.Movement;
                            break;

                        case "Movement Value":
                            Unit_Statistics[i].GetComponent<Text>().text = TargetUnit.Movement.ToString();
                            break;

                        default:
                            break;
                    }
                }
                if (Indicator_AttackIcon.activeSelf) { Indicator_AttackIcon.SetActive(false); }
                if (GameManager.GManager.HeldObject && GameManager.GManager.HeldObject.GetComponent<UnitAttributes>() != null)
                {

                    UnitAttributes SelectedUnit = GameManager.GManager.HeldObject.GetComponent<UnitAttributes>();

                    if (UnitRayHit.collider.tag.Contains("Faction") && UnitRayHit.collider.gameObject != GameManager.GManager.HeldObject) {

                        if (!Indicator_AttackIcon.activeSelf) { Indicator_AttackIcon.SetActive(true); }

                        //int DamageValue = ((SelectedUnit.Attack * SelectedUnit.HealthPool) / SelectedUnit.WeaknessMod) - ((TargetUnit.Defence*TargetUnit.HealthPool)/TargetUnit.WeaknessMod);
                        float effectiveAttack = 0;


                        if (SelectedUnit.UsesAmmo && SelectedUnit.Ammo > 0) {
                            effectiveAttack = SelectedUnit.AmmoAttack * (SelectedUnit.HealthPool / SelectedUnit.WeaknessMod);
                        } else {
                            effectiveAttack = SelectedUnit.Attack * (SelectedUnit.HealthPool / SelectedUnit.WeaknessMod);
                        }

                        float effectiveDefence = TargetUnit.Defence * (TargetUnit.HealthPool / TargetUnit.WeaknessMod);


                        //Uses the unit's damage calculation to display these values
                        int damageDisplay = TargetUnit.DamageReturn(effectiveAttack,effectiveDefence, SelectedUnit);

                        //Sets Text
                        Indicator_AttackValue.GetComponent<Text>().text = damageDisplay.ToString() + "%";

                        if (damageDisplay >= 50) {
                            Indicator_AttackIcon.GetComponent<Image>().color = Color.blue;
                        } else if (damageDisplay < 50 && damageDisplay >= 25) {
                            Indicator_AttackIcon.GetComponent<Image>().color = Color.magenta;
                        } else {
                            Indicator_AttackIcon.GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }
        }
        else
        {
            if (Unit_Viewer)
            {
                //disables the Unit Display if unit isn't under mouse
                Unit_Viewer.SetActive(false);
                Sprite_Viewer.SetActive(false);
            }
        }
    }
    void Update_UI_Terrain()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        RaycastHit hit;
        int TerrainMask = 1 << 9;
        //Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down * 10, Color.blue, 3);
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, 10, TerrainMask))
        {
            if (hit.collider.tag.Contains("Land"))
            {
                if (hit.collider.GetComponent<TerrainProperty>() != null)
                {
                    TerrainProperty terrain = hit.collider.GetComponent<TerrainProperty>();
                    Terrain_Viewer.GetComponent<Image>().sprite = terrain.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    Terrain_Viewer.GetComponent<Image>().color = terrain.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                    if (terrain.isCapturable)
                    {
                        if (terrain.CapturePower > 0)
                        {
                            Terrain_Viewer.transform.GetChild(0).GetComponent<Slider>().value = terrain.CapturePower;
                            Terrain_Viewer.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Capturing " + terrain.name + "!";

                            RaycastHit UnitDetection;
                            int UnitMask = 1 << 8;
                            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out UnitDetection, 10, UnitMask)) {
                                if (UnitDetection.collider.tag.Contains("Faction") && UnitDetection.collider.GetComponent<UnitAttributes>()!=null) {
                                    //Change the silder's fore-colour to capturing Unit's Faction Colour
                                    Terrain_Viewer.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = UnitDetection.collider.GetComponent<UnitAttributes>().FactionSided.GetComponent<Faction>().inGameID_Col;
                                    //Change the silder's background colour to the 
                                    Terrain_Viewer.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = terrain.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                                }
                            }
                        }
                        else
                        {
                            Terrain_Viewer.transform.GetChild(0).GetComponent<Slider>().value = 200;
                            //Changes name to match it's been taken or not
                            if (terrain.isCaptured)
                            {
                                Terrain_Viewer.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = terrain.Heldby + "'s " + terrain.name;
                                Terrain_Viewer.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = terrain.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                                Terrain_Viewer.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = terrain.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                            }
                            else
                            {
                                Terrain_Viewer.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Neutral " + terrain.name;
                                Terrain_Viewer.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
                            }
                        }
                    }
                    else
                    {
                        //Default Response for terrain if it's not able to be captured
                        Terrain_Viewer.transform.GetChild(0).GetComponent<Slider>().value = 200;
                        Terrain_Viewer.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = terrain.name;
                        Terrain_Viewer.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
                    }
                }
            }
        }
    }
    void Toggler_Headers()
    {
        foreach (GameObject item in MenuHeader)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
            }
            else
            {
                item.SetActive(true);
            }
        }
    }
    void Toggler_Unit()
    {
        foreach (GameObject item in Unit_InfoTab)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
            }
            else
            {
                item.SetActive(true);
            }
        }
    }
    void Toggler_Action()
    {
        foreach (GameObject item in Unit_ActionTab)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
                ActionsActive = false;
            }
            else
            {
                item.SetActive(true);
                ActionsActive = true;
            }
        }
    }


    public static void UpdateProfiles(Faction faction)
    {
        if (UIManager.Profiler != null)
        {
            for (int i = 0; i < Profiler.SpriteHolders.Length; i++)
            {
                Profiler.SpriteHolders[i].color = faction.inGameID_Col;
            }

            if (Profiler.icons != null)
            {
                Profiler.icons.GetComponent<Image>().sprite = faction.sprite_Commander;
            }

            Profiler.UI_Resource.text = faction.Funds.ToString();
            Profiler.UI_Turn.text = Profiler.PhaseValue.ToString();
        }
        else
        {
            print("Profiler doesn't exist");
        }
    }
}