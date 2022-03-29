using AfterStrike.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static Queue<Faction> FactionQueue = new Queue<Faction>();

    public static LevelManager DM;

    private void OnEnable()
    {
//        Debug.Log("something's enabled");
        SceneManager.sceneLoaded += StartGame;
    }
    private void OnDisable()
    {
//        Debug.Log("something's disabled");
        SceneManager.sceneLoaded -= StartGame;
    }

    private void Awake()
    {
        if (DM == null)
        {
            DM = this;
        }
        else if (DM != this)
        {
            Destroy(gameObject);
        }

        GameManager.MenuLock = false;
    }
    private void Start()
    {
        Debug.Log("Game has started");
    }

    private GameObject[] Team;
    public GameObject[,] Map;
    private int turntimer;

    /// <summary>
    /// Controller for the game's main system
    /// </summary>


    public void StartGame(Scene scene, LoadSceneMode loadScene) {
        //Test
        //print("Something's supposed to happen in here: " + scene.name);

        //Command line
        int bounds = 0;
        if (GameManager.GManager != null) {
            bounds = GameManager.GManager.MapDimensions;
        }
        if (bounds < 10) {
            bounds = 10;
        }

        //Actual Function
        if (scene.name.Contains("Main")) {
//            print("This looks right");
            Map = new GameObject[bounds, bounds];

            for (int i = 0; i < bounds; i++) {
                for (int j = 0; j < bounds; j++) {
                    Map[i, j] = Instantiate(Resources.Load("Tile", typeof(GameObject)), new Vector3(i, 0, j), Quaternion.identity, gameObject.transform) as GameObject;
                    if (i == 0 || j == 0 || i == bounds -1 || j == bounds - 1) {
                        Map[i, j].GetComponent<TerrainProperty>().TerrainType(20);
                    } else {
                        Map[i, j].GetComponent<TerrainProperty>().TerrainType(0);
                    }
                }
            }

            //Test
//            print("Let's get to work! Total Teams are: " + GameManager.GManager.TeamCount);
            int spacer = 1;
            GameObject temp;

            switch (GameManager.GManager.TeamCount)
            {
                case 2:
//                    print("Two Teams!");

                    Map[spacer, spacer].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(spacer, 1, spacer), Quaternion.identity) as GameObject;
                    temp.tag = "Faction1";

                    Map[bounds - (spacer + 1), bounds - (spacer + 1)].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(bounds - (spacer + 1), 1, bounds - (spacer + 1)), Quaternion.identity) as GameObject;
                    temp.tag = "Faction2";
                    break;
                case 3:
//                    print("Three Teams!");

                    Map[spacer, spacer].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(spacer, 1, spacer), Quaternion.identity) as GameObject;
                    temp.tag = "Faction1";

                    Map[spacer, bounds - (spacer + 1)].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(spacer, 1, bounds - (spacer + 1)), Quaternion.identity) as GameObject;
                    temp.tag = "Faction2";

                    Map[bounds - (spacer + 1), spacer].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(bounds - (spacer + 1), 1, spacer), Quaternion.identity) as GameObject;
                    temp.tag = "Faction3";
                    break;
                case 4:
//                    print("Four Teams!");

                    Map[spacer, spacer].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(spacer, 1, spacer), Quaternion.identity) as GameObject;
                    temp.tag = "Faction1";

                    Map[spacer, bounds - (spacer + 1)].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(spacer, 1, bounds - (spacer + 1)), Quaternion.identity) as GameObject;
                    temp.tag = "Faction2";

                    Map[bounds - (spacer + 1), spacer].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(bounds - (spacer + 1), 1, spacer), Quaternion.identity) as GameObject;
                    temp.tag = "Faction3";

                    Map[bounds - (spacer + 1), bounds - (spacer + 1)].GetComponent<TerrainProperty>().TerrainType(9);
                    temp = Instantiate(Resources.Load("Infantry", typeof(GameObject)), new Vector3(bounds - (spacer + 1), 1, bounds - (spacer + 1)), Quaternion.identity) as GameObject;
                    temp.tag = "Faction4";
                    break;
                default:
                    break;
            }

            //Spawns the main faction gameobject that acts a controller
            for (int i = 0; i < GameManager.GManager.TeamCount; i++) {
                GameObject newfaction = Instantiate(Resources.Load("FactionContainer", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                Faction nfaction = newfaction.GetComponent<Faction>();
                nfaction.tag = "Faction" + (i + 1);
                nfaction.inGameID_ID = nfaction.tag;
                if (i == 0) {
                    nfaction.FactionGenerator(Random.Range(0, 6), GameManager.GManager.TeamColour);
                } else {
                    nfaction.FactionGenerator(Random.Range(0, 6), Color.white);
                }
                FactionQueue.Enqueue(newfaction.GetComponent<Faction>());
            }

//            print("there are " + FactionQueue + "in the queue and " + FactionQueue.Peek().Faction_Name + "is first");

            //Reset's Camera Position
            GameManager.GManager.transform.position = new Vector3(0, 10, 0);
            GameManager.GManager.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            print("Colours pulled");
            GameManager.GManager.Gameplay_Colour_Tile_Movment = Resources.Load("Colours/Colour_Tile_Movment", typeof(Material)) as Material;
            print(GameManager.GManager.Gameplay_Colour_Tile_Movment);
            GameManager.GManager.Gameplay_Colour_Tile_Attack = Resources.Load("Colours/Colour_Tile_Attack", typeof(Material)) as Material;
            print(GameManager.GManager.Gameplay_Colour_Tile_Attack);
            GameManager.GManager.Gameplay_Colour_Tile_Target = Resources.Load("Colours/Colour_Tile_Target", typeof(Material)) as Material;
            print(GameManager.GManager.Gameplay_Colour_Tile_Target);


            //Starts the game's turn management
            QuickCycle();
            BeginTurn();
        }
    }
    
    public string ActiveFactionReturn() {
        string CurrentFaction = "";
        CurrentFaction = FactionQueue.Peek().tag;
        return CurrentFaction;
    }

    private void QuickCycle() {
        for (int i = 0; i < FactionQueue.Count; i++) {
            if (FactionQueue.Peek().TerrainList.Count == 0) {
                if (FactionQueue.Peek().UnitList.Count == 0) {
                    FactionQueue.Peek().FindThings();
                }
            }
            Faction faction = FactionQueue.Dequeue();
            FactionQueue.Enqueue(faction);
        }
    }

    public void BeginTurn() {
        if (FactionQueue.Peek().TerrainList.Count == 0) {
            if (FactionQueue.Peek().UnitList.Count == 0) {
                FactionQueue.Peek().FindThings();
            }
        }
        FactionQueue.Peek().StartTurn_Faction();
    }

    public void EndTurn() {
        print("Ending Turn!");
        GameManager.GManager.QuickCancel();
        Faction faction = FactionQueue.Dequeue();
        FactionQueue.Enqueue(faction);
        LevelManager.DM.turntimer++;
        if (LevelManager.DM.turntimer++ > GameManager.GManager.TeamCount) {
            GameScreenManager.Profiler.PhaseValue++;
            LevelManager.DM.turntimer = 0;
        }
        BeginTurn();
    }
    public void EndScenario() {
        print("Someone has Lost!");
    }
}