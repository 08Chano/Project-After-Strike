using System.Collections.Generic;
using UnityEngine;

public class UnitProperty : UnitLib
{
    List<Controller_Tile> ValidSpace = new List<Controller_Tile>();
    Stack<Controller_Tile> CP = new Stack<Controller_Tile>();

    private GameObject SkipperVar;  //Reset each time it carried out

    public bool isSelected;
    public bool ActionAvailable = true;
    public bool ActionsMove = true;
    public bool AttackChecking = false;
    public bool MoveChecking = false;
    public bool moving;

    public GameObject[] Coordinates;

    Controller_Tile SitCoordinate;

    Vector3 velocity = new Vector3();
    Vector3 TransitionNode = new Vector3();

    float VerticalAdjuster = 0;

    void OnEnable()
    {
        Controller_Input.SkipToggle += SkipMove;
    }
    void OnDisable()
    {
        Controller_Input.SkipToggle -= SkipMove;
    }
    protected void CastArea()
    {
        Coordinates = GameObject.FindGameObjectsWithTag("Land");
        VerticalAdjuster = GetComponent<Collider>().bounds.extents.y;
    }
    public void GetCoordinate()
    {
        SitCoordinate = Destination(gameObject);
        SitCoordinate.isOccupied = true;
    }
    public Controller_Tile Destination(GameObject Destination)
    {
        RaycastHit hit;
        Controller_Tile coordinate = null;
        //Debug.DrawRay(Destination.transform.position, Vector3.down, Color.red, 1);
        if (Physics.Raycast(Destination.transform.position, Vector3.down, out hit, 1))
        {
            coordinate = hit.collider.GetComponent<Controller_Tile>();
        }
        //Debug.Log(coordinate);
        return coordinate;
    }
    public void FindMoveConnect()
    {
        foreach (GameObject item in Coordinates)
        {
            item.GetComponent<Controller_Tile>().ViableMovementGrid(gameObject);
        }
    }
    public void Cast_Movement()
    {
        FindMoveConnect();
        GetCoordinate();

        Queue<Controller_Tile> queue = new Queue<Controller_Tile>();

        queue.Enqueue(SitCoordinate);
        SitCoordinate.isAccounted = true;

        while (queue.Count > 0)
        {
            Controller_Tile terrain = queue.Dequeue();
            ValidSpace.Add(terrain);
            terrain.isValid = true;

            if (terrain.distance < gameObject.GetComponent<Controller_Unit>().Movement)
            {
                foreach (Controller_Tile item in terrain.MovementTileLinks)
                {
                    if (!item.isAccounted)
                    {
                        item.parent = terrain;
                        item.isAccounted = true;
                        item.distance = item.TCost(gameObject.GetComponent<Controller_Unit>().UnitType) + terrain.distance;
                        if (item.distance <= gameObject.GetComponent<Controller_Unit>().FuelPool)
                        {
                            queue.Enqueue(item);
                        }
                    }
                }
            }
        }
    }
    public void MovementPhase(GameObject target)
    {
        SkipperVar = target;
        CP.Clear();
        moving = true;

        gameObject.GetComponent<Controller_Unit>().FuelPool -= target.GetComponent<Controller_Tile>().distance;

        if (GameManager.Opt_SkipMoveAnim)
        {
            Debug.Log("Skipping Move Anim");
            transform.position = target.transform.position;
            transform.position += Vector3.up;
        }
        else
        {
            Debug.Log("Using Move Anim");
            //Systems Control
            GameManager.Playing_MoveAnim = true;
            GameManager.FreezeClicks = true;
            Controller_Tile terrain = target.GetComponent<Controller_Tile>();
            while (terrain != null)
            {
                CP.Push(terrain);
                terrain = terrain.parent;
            }
        }
    }
    public void Cast_AttackTiles()
    {
        if (gameObject.GetComponent<Controller_Unit>().LoS)
        {
            foreach (GameObject item in Coordinates) { item.GetComponent<Controller_Tile>().Reset(); }

            GetCoordinate();
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.forward, gameObject.GetComponent<Controller_Unit>().Range + 1);
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.right, gameObject.GetComponent<Controller_Unit>().Range + 1);
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.back, gameObject.GetComponent<Controller_Unit>().Range + 1);
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.left, gameObject.GetComponent<Controller_Unit>().Range + 1);

        }
        else
        {
            foreach (GameObject item in Coordinates) { item.GetComponent<Controller_Tile>().ViableTargetGrid(gameObject); }

            GetCoordinate();//Pulls current position
            Queue<Controller_Tile> queue = new Queue<Controller_Tile>();//Assigns a queue order to create an area check

            queue.Enqueue(SitCoordinate);//Assigns the current position
            SitCoordinate.isAccounted = true;//to be ignored during grid check as it's already assigned

            while (queue.Count > 0)
            {
                Controller_Tile terrain = queue.Dequeue();//Assigns it already
                ValidSpace.Add(terrain);
                terrain.isValid = true;

                if (terrain.Range < gameObject.GetComponent<Controller_Unit>().Range)
                {
                    //Pulls each connected tile into a loop that will then assign them within the queue
                    //This creates a loop that will run until the all connected tiles are no longer in range
                    foreach (Controller_Tile item in terrain.AttackRangeList)
                    {
                        if (!item.isAccounted)
                        {
                            item.parent = terrain;
                            item.isAccounted = true;
                            item.Range = terrain.Range + 1;
                            if (item.Range <= gameObject.GetComponent<Controller_Unit>().Range)
                            {
                                queue.Enqueue(item);
                            }
                        }
                    }
                }
            }
        }
    }
    public bool ValidAttack(GameObject target)
    {
        bool isValid = false;

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down * 10, out hit, 10))
        {
            if (hit.collider.GetComponent<Controller_Tile>() != null)
            {
                Controller_Tile terrain = hit.collider.GetComponent<Controller_Tile>();
                if (terrain.isHostile)
                {
                    isValid = true;
                    return isValid;
                }
            }
        }
        print("THis is " + isValid + " target");
        return isValid;
    }
    public void AttackPhase(GameObject target)
    {

        //Range Check Code

        //Combat Check

        //This Unit's attacking power
        Controller_Unit OffController_Unit = this.GetComponent<Controller_Unit>();
        float OffensiveUnitDef = OffController_Unit.EffectiveDefenceReturn();
        float OffensiveUnitAtt = OffController_Unit.EffectiveAttackReturn();
        //The target unit's attacking power


        Controller_Unit TarController_Unit = target.GetComponent<Controller_Unit>();
        float DefendingUnitDef = TarController_Unit.EffectiveDefenceReturn();
        if (TarController_Unit.isDefending)
        {
            //Calculate Starting Attack (before it takes damage)
            float DefendingUnitAtt = TarController_Unit.EffectiveAttackReturn();
            TarController_Unit.HealthPool -= TarController_Unit.DamageReturn(OffensiveUnitAtt, DefendingUnitDef, OffController_Unit);
            OffController_Unit.HealthPool -= OffController_Unit.DamageReturn(DefendingUnitAtt, OffensiveUnitDef, TarController_Unit);
        }
        else
        {
            //Phase 1
            TarController_Unit.HealthPool -= TarController_Unit.DamageReturn(OffensiveUnitAtt, DefendingUnitDef, OffController_Unit);
            //Phase 2
            if (TarController_Unit.HealthPool > 0)
            {
                float DefendingUnitAtt = TarController_Unit.EffectiveAttackReturn();
                OffController_Unit.HealthPool -= OffController_Unit.DamageReturn(DefendingUnitAtt, OffensiveUnitDef, TarController_Unit);
            }
            else
            {
                Destroy(TarController_Unit.gameObject);
            }
        }
        if (OffController_Unit.HealthPool <= 0) { Destroy(OffController_Unit.gameObject); }
        if (TarController_Unit.HealthPool <= 0) { Destroy(TarController_Unit.gameObject); }
        EndPhase();
    }

    private void SkipMove()
    {
        //Local Variables
        if (SkipperVar != null)
        {
            transform.position = SkipperVar.transform.position;
            transform.position += Vector3.up;
        }
        Reset();
    }
    protected void Move()
    {
        if (CP.Count > 0)
        {

            Controller_Tile terrain = CP.Peek();
            Vector3 Goal = terrain.transform.position;
            Goal += Vector3.up;

            if (Vector3.Distance(transform.position, Goal) > 0.3f)
            {

                Transition(Goal);
                AxisTransition();

                transform.forward = TransitionNode;
                transform.position += velocity * Time.fixedDeltaTime * 20;
            }
            else
            {
                transform.position = Goal;
                CP.Pop();
            }
        }
        else
        {

            Reset();
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            ActionsMove = false;
            //LevelManager.EndTurn();
        }
    }

    private void Reset()
    {
        //Local Variable Reset
        Debug.Log(gameObject.name);
        SkipperVar = null;
        moving = false;

        this.GetComponent<Controller_Unit>().MoveChecking = false;
        this.GetComponent<Controller_Unit>().AttackChecking = false;
        //Global Variable Reset

        GameManager.QuickCancel();
        GameManager.FreezeClicks = false;
        GameManager.Playing_MoveAnim = false;
    }
    private void Transition(Vector3 Goal)
    {
        TransitionNode = Goal - transform.position;
        TransitionNode.Normalize();
    }
    private void AxisTransition()
    {
        velocity = TransitionNode * 2;
    }

    /// <summary>
    /// End of Movement Code
    /// </summary>
    public void Action_Attack()
    {
        //Needs to cast an area
        AttackChecking = true;
        //Play Animations and change UI (TBWO)
    }
    public void Action_Synch()
    {
        EndPhase();
    }

    public void Action_Defend()
    {
        gameObject.GetComponent<Controller_Unit>().isDefending = true;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down * 10, out hit, 10))
        {
            if (hit.collider.GetComponent<Controller_Tile>() != null)
            {
                Controller_Tile terrain = hit.collider.GetComponent<Controller_Tile>();
                if (terrain.isCapturable)
                {
                    if (!terrain.isCaptured || terrain.Heldby != gameObject.tag)
                    {
                        terrain.CapturePower += this.GetComponent<Controller_Unit>().HealthPool;
                        if (terrain.CapturePower >= 200)
                        {
                            terrain.isCaptured = true;
                            terrain.Heldby = this.tag;
                            terrain.CaptureCall(gameObject.GetComponent<Controller_Unit>().FactionSided.GetComponent<Faction>());
                        }
                    }
                }
            }
        }
        EndPhase();
    }
    public void Action_Wait()
    {
        gameObject.GetComponent<Controller_Unit>().isWaiting = true;
        EndPhase();
    }
    public void Action_Recover(int RecID, int MaxFuelPool)
    {
        switch (RecID)
        {
            case 1:
                //Health
                if (GetComponent<Controller_Unit>().HealthPool < 100)
                {
                    if (LevelManager.FactionQueue.Peek().Funds >= Controller_Unit.Mod_Cost_Repair)
                    {
                        LevelManager.FactionQueue.Peek().Funds -= Controller_Unit.Mod_Cost_Repair;
                        HealthRestore(Controller_Unit.Mod_Cost_Repair_Efficency);
                    }
                }
                break;

            case 2:
                //Low Supplies
                FuelLoad(Controller_Unit.Mod_Cost_Supply_Weak, MaxFuelPool);
                break;

            case 3:
                //Health and Low Supply
                if (GetComponent<Controller_Unit>().HealthPool < 100)
                {
                    if (LevelManager.FactionQueue.Peek().Funds >= Controller_Unit.Mod_Cost_Repair)
                    {
                        LevelManager.FactionQueue.Peek().Funds -= Controller_Unit.Mod_Cost_Repair;
                        HealthRestore(Controller_Unit.Mod_Cost_Repair_Efficency);
                    }
                }
                FuelLoad(Controller_Unit.Mod_Cost_Repair_Efficency, MaxFuelPool);
                break;

            case 4:
                //Health and Full Supply
                if (GetComponent<Controller_Unit>().HealthPool < 100)
                {
                    if (LevelManager.FactionQueue.Peek().Funds >= Controller_Unit.Mod_Cost_Repair)
                    {
                        LevelManager.FactionQueue.Peek().Funds -= Controller_Unit.Mod_Cost_Repair;
                        HealthRestore(Controller_Unit.Mod_Cost_Repair_Efficency);
                    }
                }
                FuelLoad(MaxFuelPool, MaxFuelPool);

                break;

            case 5:
                //Full Supply
                FuelLoad(MaxFuelPool, MaxFuelPool);
                break;

            default:
                break;
        }
    }
    //Calls these functions
    private void FuelLoad(int Restoration, int MaxFuelPool)
    {
        gameObject.GetComponent<Controller_Unit>().FuelPool += Restoration;
        if (gameObject.GetComponent<Controller_Unit>().FuelPool > MaxFuelPool)
        {
            gameObject.GetComponent<Controller_Unit>().FuelPool = MaxFuelPool;
        }
    }
    private void HealthRestore(int Restoration)
    {
        gameObject.GetComponent<Controller_Unit>().HealthPool += Restoration;
        if (gameObject.GetComponent<Controller_Unit>().HealthPool > 100)
        {
            gameObject.GetComponent<Controller_Unit>().HealthPool = 100;
        }
    }

    public void BeginPhase()
    {

        ActionAvailable = true;
        ActionsMove = true;

        Controller_Unit unit = this.GetComponent<Controller_Unit>();
        unit.isWaiting = false;
        unit.isDefending = false;

        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void EndPhase()
    {
        ActionAvailable = false;
        ActionsMove = false;
        moving = false;

        Controller_Unit.QuickCancel();

        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.grey;
        this.GetComponent<Controller_Unit>().FactionSided.GetComponent<Faction>().EndTurn_Faction();
    }
    public void WipeSelect()
    {
        isSelected = false;
        MoveChecking = false;
        AttackChecking = false;
    }
}