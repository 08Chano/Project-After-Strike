using AfterStrike.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : GameSystems
{

    public delegate void MenuToggle();
    public static event MenuToggle EscToggle;
    public static event MenuToggle ActToggle;
    public static event MenuToggle StatToggle;

    public delegate void Deselect();
    public static event Deselect CancelToggle;
    public static event Deselect skipToggle;

    public static bool MenuLock = true;
    public static bool FreezeClicks = false;

    //Movement Animation Options
    public static bool Opt_SkipMoveAnim = false;            //Global Options on Playing Movement Animations
    public static bool Override_SkipAnim = false;            //Skips currently playing Anime
    public static bool Playing_MoveAnim = false;            //Playing Movement Anime callout

    public static GameManager GManager;
    public static GameObject ActiveSelection;

    public GameObject HeldObject;

    private float RefreshRate = 0.03f;

    private void Awake()
    {
        if (GManager == null)
        {
            DontDestroyOnLoad(gameObject);
            GManager = this;
        }
        else if (GManager != this)
        {
            Destroy(gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!MenuLock)
        {
            BattleScreen();
        }
    }
    void BattleScreen() {
        if (Input.GetButtonDown("Escape")) { Toggle(); }
        if (Input.GetButtonDown("Switch")) { StatToggle(); }

        //Camera Controls
        if (Input.GetButton("Horizontal")) { gameObject.transform.Translate(new Vector2(Input.GetAxis("Horizontal"), 0)); }
        if (Input.GetButton("Vertical")) { gameObject.transform.Translate(new Vector2(0, Input.GetAxis("Vertical"))); }

        if (Input.GetButtonDown("Select")) {
            if (FreezeClicks) {
                //Skips Playing Animation for Battles and Moving
                skipToggle();
                skipToggle();
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down * 10, out hit, 10)) {
                if (hit.collider != null) {
                    if (EventSystem.current.IsPointerOverGameObject()) {
                    } else if (hit.collider.GetComponent<UnitActions>() != null) {
                        if (!hit.collider.GetComponent<UnitActions>().ActionAvailable && hit.collider.tag == LevelManager.DM.ActiveFactionReturn()) {
                            return; //Stops check if the unit is able to be interacted with
                        }
                        if (hit.collider.gameObject != HeldObject) {
                            //Checks if the Unit is is checking for viable targets
                            if (HeldObject != null) {
                                print("Unit is searching target = " + HeldObject.GetComponent<UnitActions>().AttackChecking);
                            }
                            if (HeldObject!= null && HeldObject.GetComponent<UnitActions>().AttackChecking) {
                                print("Unit is checking for Valid Targets");
                                UnitActions unitActions = HeldObject.GetComponent<UnitActions>();
                                if (hit.collider.tag == HeldObject.tag) {
                                    return;//Ends function check
                                } else if (hit.collider.tag != HeldObject.tag) {
                                    //Checks if selected unit is within the target space
                                    if (unitActions.ValidAttack(hit.collider.gameObject)) {
                                        HeldObject.GetComponent<UnitActions>().AttackPhase(hit.collider.gameObject);
                                        Cancel();
                                        return;
                                    }
                                }
                            } else {
                                if (HeldObject != null) {
                                    GameManager.GManager.HeldObject.GetComponent<UnitActions>().WipeSelect();
                                }
                                if (!GameScreenManager.ActionsActive) {
                                    ActToggle();
                                }
                                HeldObject = hit.collider.gameObject;
                                HeldObject.GetComponent<UnitActions>().isSelected = true;
                                return;
                            }
                        } else if (hit.collider.gameObject == HeldObject) {
                            if (HeldObject.GetComponent<UnitActions>().ActionsMove) {
                                CallAction_Move();
                            } else if (HeldObject.GetComponent<UnitActions>().ActionAvailable) {
                                CallAction_Attack();
                            }
                        }
                    } else if (hit.collider.GetComponent<TerrainProperty>() != null) {
                        TerrainProperty terrain = hit.collider.GetComponent<TerrainProperty>();
                        if (terrain.isValid && !terrain.isHostile) {
                            if (HeldObject != null) {
                                //Move Agent
                                terrain.isTarget = true;

                                HeldObject.GetComponent<UnitActions>().MovementPhase(hit.collider.gameObject);
                                GameManager.GManager.HeldObject.GetComponent<UnitActions>().WipeSelect();
                                GameManager.GManager.HeldObject.GetComponent<UnitActions>().isSelected = true;
                                //Cancel();
                                //HeldObject = null;
                            }
                        } else if (terrain.isValid && terrain.isHostile) {
                            HeldObject.GetComponent<UnitActions>().AttackPhase(hit.collider.gameObject);
                        }
                    } else {
                        Debug.Log("Unit Not Found");
                    }
                } else {
                    Debug.Log("Nothing Found");
                }
            }
        }

        if (Input.GetButtonDown("Cancel")) { Cancel(); }
        if (Input.GetButtonDown("Action1")) { CallAction_Move(); }
        if (Input.GetButtonDown("Action2")) { CallAction_Attack(); }
        if (Input.GetButtonDown("Action3")) { CallAction_Sync(); }
        if (Input.GetButtonDown("Action4")) { CallAction_Defend(); }
        if (Input.GetButtonDown("Action5")) { CallAction_Wait(); }
    }

    public void Toggle()
    {
        EscToggle();
    }
    public static void Cancel()
    {
        if (GameManager.GManager.HeldObject != null)
        {

            Debug.Log("Item is nolonger held");

            if (GameManager.GManager.HeldObject.GetComponent<UnitActions>() != null)
            {
                GameManager.GManager.HeldObject.GetComponent<UnitActions>().WipeSelect();
            }
        }
 
        if (GameScreenManager.ActionsActive) { ActToggle(); }

        GameManager.GManager.HeldObject = null;

        GameManager.GManager.QuickCancel();
    }
    public void QuickCancel()
    {

        CancelToggle();
        CancelToggle();
    }
    public void CallAction_Move()
    {
        if (HeldObject != null)
        {
            if (HeldObject.GetComponent<UnitActions>() == true)
            {
                UnitActions actions = HeldObject.GetComponent<UnitActions>();
                if (actions.ActionAvailable && actions.ActionsMove)
                {

                    Debug.Log("MOVING!!!");
                    actions.MoveChecking = true;
                }
            }
        }
    }
    public void CallAction_Attack()
    {
        if (HeldObject != null)
        {
            if (HeldObject.GetComponent<UnitActions>() == true)
            {
                UnitActions actions = HeldObject.GetComponent<UnitActions>();
                if (actions.ActionAvailable)
                {

                    Debug.Log("ATTACKING!!!");
                    actions.Action_Attack();
                    //Cancel();
                }
            }
        }
    }
    public void CallAction_Sync()
    {
        if (HeldObject != null)
        {
            if (HeldObject.GetComponent<UnitActions>() == true)
            {
                UnitActions actions = HeldObject.GetComponent<UnitActions>();
                if (actions.ActionAvailable)
                {

                    Debug.Log("SYNCHRONISING!!! Does nothing");
                    actions.Action_Synch();
                    Cancel();
                }
            }
        }
    }
    public void CallAction_Defend()
    {
        if (HeldObject != null)
        {
            print("Object not null");
            if (HeldObject.GetComponent<UnitActions>() == true)
            {
                print("Script not null");
                UnitActions actions = HeldObject.GetComponent<UnitActions>();
                if (actions.ActionAvailable)
                {

                    Debug.Log("DEFEND!!!");
                    actions.Action_Defend();
                    Cancel();
                }
            }
        }
        else
        {
            print("Object is null");
        }
    }
    public void CallAction_Wait()
    {
        if (HeldObject != null)
        {
            if (HeldObject.GetComponent<UnitActions>() == true)
            {
                UnitActions actions = HeldObject.GetComponent<UnitActions>();
                if (actions.ActionAvailable)
                {

                    Debug.Log("WAITING!!!");
                    actions.Action_Wait();
                    Cancel();
                }
            }
        }
    }
}