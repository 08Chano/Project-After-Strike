using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AfterStrike.UI;
using AfterStrike.Class.Unit;

namespace AfterStrike.Manager
{
    public class GameScreenManager : MonoBehaviour
    {
        public static GameScreenManager Profiler;
        public static bool ActionsActive;

        [Header("Menu Containers")]
        public GameObject[] MenuHeader;
        public GameObject[] MenuContents;

        [Header("Top Left Containers")]
        public GameObject Sprite_Viewer;            //Top Left for terrain Sprite Display
        [SerializeField] private TileProfilerUIController TileProfiler;              //Top Left for Unit Sprite Display

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
            GameManager.StatToggle += TileProfiler.ToggleStatsView;
        }
        void OnDisable()
        {
            GameManager.EscToggle -= Toggler_Headers;
            GameManager.ActToggle -= Toggler_Action;
            GameManager.StatToggle -= TileProfiler.ToggleStatsView;
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

                    //Sets the Unit Display to active if unit is hovered over with mouse
                    TileProfiler.UpdateUnitPreviewWindow(TargetUnit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);

                    // Updates the Unit Stats preview
                    TileProfiler.UpdateUnitStats(TargetUnit);

                    if (Indicator_AttackIcon.activeSelf) { Indicator_AttackIcon.SetActive(false); }

                    if (GameManager.GManager.HeldObject && GameManager.GManager.HeldObject.GetComponent<UnitAttributes>() != null)
                    {

                        UnitAttributes SelectedUnit = GameManager.GManager.HeldObject.GetComponent<UnitAttributes>();

                        if (UnitRayHit.collider.tag.Contains("Faction") && UnitRayHit.collider.gameObject != GameManager.GManager.HeldObject)
                        {

                            if (!Indicator_AttackIcon.activeSelf) { Indicator_AttackIcon.SetActive(true); }

                            //int DamageValue = ((SelectedUnit.Attack * SelectedUnit.HealthPool) / SelectedUnit.WeaknessMod) - ((TargetUnit.Defence*TargetUnit.HealthPool)/TargetUnit.WeaknessMod);
                            float effectiveAttack = 0;


                            if (SelectedUnit.UsesAmmo && SelectedUnit.Ammo > 0)
                            {
                                effectiveAttack = SelectedUnit.AmmoAttack * (SelectedUnit.HealthPool / SelectedUnit.WeaknessMod);
                            }
                            else
                            {
                                effectiveAttack = SelectedUnit.Attack * (SelectedUnit.HealthPool / SelectedUnit.WeaknessMod);
                            }

                            float effectiveDefence = TargetUnit.Defence * (TargetUnit.HealthPool / TargetUnit.WeaknessMod);


                            //Uses the unit's damage calculation to display these values
                            int damageDisplay = TargetUnit.DamageReturn(effectiveAttack, effectiveDefence, SelectedUnit);

                            //Sets Text
                            Indicator_AttackValue.GetComponent<Text>().text = damageDisplay.ToString() + "%";

                            if (damageDisplay >= 50)
                            {
                                Indicator_AttackIcon.GetComponent<Image>().color = Color.blue;
                            }
                            else if (damageDisplay < 50 && damageDisplay >= 25)
                            {
                                Indicator_AttackIcon.GetComponent<Image>().color = Color.magenta;
                            }
                            else
                            {
                                Indicator_AttackIcon.GetComponent<Image>().color = Color.red;
                            }
                        }
                    }
                }
            }
            else
            {
                TileProfiler.UpdateUnitPreviewWindow(null);
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
                        
                        TileProfiler.UpdateTerrainPreviewWindow(terrain);
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


        public static void UpdateProfiles(FactionClass faction)
        {
            if (GameScreenManager.Profiler != null)
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
}