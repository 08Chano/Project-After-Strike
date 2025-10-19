using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProperties : MonoBehaviour
{
    List<TerrainProperty> ValidSpace = new List<TerrainProperty>();
    Stack<TerrainProperty> CP = new Stack<TerrainProperty>();

    private GameObject SkipperVar;  //Reset each time it carried out

    public bool isSelected;
    public bool ActionAvailable = true;
    public bool ActionsMove = true;
    public bool AttackChecking = false;
    public bool MoveChecking = false;
    public bool moving;

    public GameObject[] Coordinates;

    TerrainProperty SitCoordinate;

    Vector3 velocity = new Vector3();
    Vector3 TransitionNode = new Vector3();

    float VerticalAdjuster = 0;
    /// <summary>
    /// Global Events Manager
    /// </summary>
    void OnEnable()
    {
        GameManager.skipToggle += SkipMove;
    }
    void OnDisable()
    {
        GameManager.skipToggle -= SkipMove;
    }
    /// <summary>
    /// Generates an array of all of the available tiles that the unit can move to.
    /// </summary>
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
    public TerrainProperty Destination(GameObject Destination)
    {
        RaycastHit hit;
        TerrainProperty coordinate = null;
        //Debug.DrawRay(Destination.transform.position, Vector3.down, Color.red, 1);
        if (Physics.Raycast(Destination.transform.position, Vector3.down, out hit, 1))
        {
            coordinate = hit.collider.GetComponent<TerrainProperty>();
        }
        //Debug.Log(coordinate);
        return coordinate;
    }
    public void FindMoveConnect()
    {
        foreach (GameObject item in Coordinates)
        {
            item.GetComponent<TerrainProperty>().ViableMovementGrid(gameObject);
        }
    }
    public void Cast_Movement() {
        FindMoveConnect();
        GetCoordinate();

        Queue<TerrainProperty> queue = new Queue<TerrainProperty>();

        queue.Enqueue(SitCoordinate);
        SitCoordinate.isAccounted = true;

        while (queue.Count > 0)
        {
            TerrainProperty terrain = queue.Dequeue();
            ValidSpace.Add(terrain);
            terrain.isValid = true;

            if (terrain.distance < gameObject.GetComponent<UnitAttributes>().Movement)
            {
                foreach (TerrainProperty item in terrain.MovementTileLinks)
                {
                    if (!item.isAccounted)
                    {
                        item.parent = terrain;
                        item.isAccounted = true;
                        item.distance = item.TCost(gameObject.GetComponent<UnitAttributes>().UnitType) + terrain.distance;
                        if (item.distance <= gameObject.GetComponent<UnitAttributes>().FuelPool)
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

        gameObject.GetComponent<UnitAttributes>().FuelPool -= target.GetComponent<TerrainProperty>().distance;

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
            TerrainProperty terrain = target.GetComponent<TerrainProperty>();
            while (terrain != null)
            {
                CP.Push(terrain);
                terrain = terrain.parent;
            }
        }
    }
    public void Cast_AttackTiles() {
        if (gameObject.GetComponent<UnitAttributes>().LoS) {
            foreach (GameObject item in Coordinates) { item.GetComponent<TerrainProperty>().Reset(); }

            GetCoordinate();
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.forward, gameObject.GetComponent<UnitAttributes>().Range + 1);
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.right, gameObject.GetComponent<UnitAttributes>().Range + 1);
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.back, gameObject.GetComponent<UnitAttributes>().Range + 1);
            SitCoordinate.LoSAttackGrid(gameObject, Vector3.left, gameObject.GetComponent<UnitAttributes>().Range+1);

        } else {
            foreach (GameObject item in Coordinates) { item.GetComponent<TerrainProperty>().ViableTargetGrid(gameObject); }

            GetCoordinate();//Pulls current position
            Queue<TerrainProperty> queue = new Queue<TerrainProperty>();//Assigns a queue order to create an area check

            queue.Enqueue(SitCoordinate);//Assigns the current position
            SitCoordinate.isAccounted = true;//to be ignored during grid check as it's already assigned

            while (queue.Count > 0) {
                TerrainProperty terrain = queue.Dequeue();//Assigns it already
                ValidSpace.Add(terrain);
                terrain.isValid = true;

                if (terrain.Range < gameObject.GetComponent<UnitAttributes>().Range)
                {
                    //Pulls each connected tile into a loop that will then assign them within the queue
                    //This creates a loop that will run until the all connected tiles are no longer in range
                    foreach (TerrainProperty item in terrain.AttackRangeList) {
                        if (!item.isAccounted) {
                            item.parent = terrain;
                            item.isAccounted = true;
                            item.Range = terrain.Range + 1;
                            if (item.Range <= gameObject.GetComponent<UnitAttributes>().Range) {
                                queue.Enqueue(item);
                            }
                        }
                    }
                }
            }
        }
    }
    public bool ValidAttack(GameObject target) {
        bool isValid = false;

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down * 10, out hit, 10)) {
            if (hit.collider.GetComponent<TerrainProperty>() != null) {
                TerrainProperty terrain = hit.collider.GetComponent<TerrainProperty>();
                if (terrain.isHostile) {
                    isValid = true;
                    return isValid;
                }
            }
        }
        print("THis is " + isValid + " target");
        return isValid;
    }
    public void AttackPhase(GameObject target) {

        //Range Check Code

        //Combat Check

        //This Unit's attacking power
        UnitAttributes OffUnitAttributes = this.GetComponent<UnitAttributes>();
        float OffensiveUnitDef = OffUnitAttributes.EffectiveDefenceReturn();
        float OffensiveUnitAtt = OffUnitAttributes.EffectiveAttackReturn();
        //The target unit's attacking power


        UnitAttributes TarUnitAttributes = target.GetComponent<UnitAttributes>();
        float DefendingUnitDef = TarUnitAttributes.EffectiveDefenceReturn();
        if (TarUnitAttributes.isDefending) {
            //Calculate Starting Attack (before it takes damage)
            float DefendingUnitAtt = TarUnitAttributes.EffectiveAttackReturn();
            TarUnitAttributes.HealthPool -= TarUnitAttributes.DamageReturn(OffensiveUnitAtt, DefendingUnitDef, OffUnitAttributes);
            OffUnitAttributes.HealthPool -= OffUnitAttributes.DamageReturn(DefendingUnitAtt, OffensiveUnitDef, TarUnitAttributes);
        }
        else {
            //Phase 1
            TarUnitAttributes.HealthPool -= TarUnitAttributes.DamageReturn(OffensiveUnitAtt, DefendingUnitDef, OffUnitAttributes);
            //Phase 2
            if (TarUnitAttributes.HealthPool > 0) {
                float DefendingUnitAtt = TarUnitAttributes.EffectiveAttackReturn();
                OffUnitAttributes.HealthPool -= OffUnitAttributes.DamageReturn(DefendingUnitAtt, OffensiveUnitDef, TarUnitAttributes);
            } else {
                Destroy(TarUnitAttributes.gameObject);
            }
        }
        if (OffUnitAttributes.HealthPool <= 0) { Destroy(OffUnitAttributes.gameObject); }
        if (TarUnitAttributes.HealthPool <= 0) { Destroy(TarUnitAttributes.gameObject); }
        EndPhase();
    }

    private void SkipMove() {
        //Local Variables
        if (SkipperVar != null) {
            transform.position = SkipperVar.transform.position;
            transform.position += Vector3.up;
        }
        Reset();
    }
    protected void Move()
    {
        if (CP.Count > 0)
        {

            TerrainProperty terrain = CP.Peek();
            Vector3 Goal = terrain.transform.position;
            Goal += Vector3.up;

            if (Vector3.Distance(transform.position, Goal) > 0.3f) {

                Transition(Goal);
                AxisTransition();

                transform.forward = TransitionNode;
                transform.position += velocity * Time.fixedDeltaTime * 20;
                //Debug.Log("Rotate! " + transform.rotation.y);
                ////Debug.Log("Rotate! " + (Mathf.Deg2Rad* 90));
                //Debug.Log("Rotate! " + Mathf.RoundToInt(Mathf.Rad2Deg * transform.rotation.y));
                //switch (Mathf.RoundToInt(transform.rotation.y))
                //{
                //    case -270:
                //        Debug.Log("-270");
                //        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 270, 0);
                //        break;
                //    case -180:
                //        Debug.Log("-180");
                //        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 180, 0);
                //        break;
                //    case -90:
                //        Debug.Log("-90");
                //        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 90, 0);
                //        break;
                //    case 90:
                //        Debug.Log("90");
                //        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -90, 0);
                //        break;
                //    case 180:
                //        Debug.Log("180");
                //        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -180, 0);
                //        break;
                //    case 270:
                //        Debug.Log("270");
                //        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -270, 0);
                //        break;
                //    default:
                //        break;
                //}

            } else {
                transform.position = Goal;
                CP.Pop();
            }
        } else {

            Reset();
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            ActionsMove = false;
            //LevelManager.EndTurn();
        }
    }

    private void Reset() {
        //Local Variable Reset
        Debug.Log(gameObject.name);
        SkipperVar = null;
        moving = false;

        this.GetComponent<UnitActions>().MoveChecking = false;
        this.GetComponent<UnitActions>().AttackChecking = false;
        //Global Variable Reset

        GameManager.GManager.QuickCancel();
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
    public void Action_Attack() {
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
        gameObject.GetComponent<UnitAttributes>().isDefending = true;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down * 10, out hit, 10))
        {
            if (hit.collider.GetComponent<TerrainProperty>() != null)
            {
                TerrainProperty terrain = hit.collider.GetComponent<TerrainProperty>();
                if (terrain.isCapturable)
                {
                    if (!terrain.isCaptured || terrain.Heldby != gameObject.tag)
                    {
                        terrain.CapturePower += this.GetComponent<UnitAttributes>().HealthPool;
                        if (terrain.CapturePower >= 200)
                        {
                            terrain.isCaptured = true;
                            terrain.Heldby = this.tag;
                            terrain.CaptureCall(gameObject.GetComponent<UnitAttributes>().FactionSided.GetComponent<Faction>());
                        }
                    }
                }
            }
        }
        EndPhase();
    }
    public void Action_Wait()
    {
        gameObject.GetComponent<UnitAttributes>().isWaiting = true;
        EndPhase();
    }
    public void Action_Recover(int RecID, int MaxFuelPool) {
        switch (RecID)
        {
            case 1:
                //Health
                if (GetComponent<UnitAttributes>().HealthPool < 100)
                {
                    if (LevelManager.FactionQueue.Peek().Funds >= GameManager.GManager.Mod_Cost_Repair)
                    {
                        LevelManager.FactionQueue.Peek().Funds -= GameManager.GManager.Mod_Cost_Repair;
                        HealthRestore(GameManager.GManager.Mod_Cost_Repair_Efficency);
                    }
                }
                break;

            case 2:
                //Low Supplies
                FuelLoad(GameManager.GManager.Mod_Cost_Supply_Weak, MaxFuelPool);
                break;

            case 3:
                //Health and Low Supply
                if (GetComponent<UnitAttributes>().HealthPool < 100)
                {
                    if (LevelManager.FactionQueue.Peek().Funds >= GameManager.GManager.Mod_Cost_Repair)
                    {
                        LevelManager.FactionQueue.Peek().Funds -= GameManager.GManager.Mod_Cost_Repair;
                        HealthRestore(GameManager.GManager.Mod_Cost_Repair_Efficency);
                    }
                }
                FuelLoad(GameManager.GManager.Mod_Cost_Repair_Efficency, MaxFuelPool);
                break;

            case 4:
                //Health and Full Supply
                if (GetComponent<UnitAttributes>().HealthPool < 100)
                {
                    if (LevelManager.FactionQueue.Peek().Funds >= GameManager.GManager.Mod_Cost_Repair)
                    {
                        LevelManager.FactionQueue.Peek().Funds -= GameManager.GManager.Mod_Cost_Repair;
                        HealthRestore(GameManager.GManager.Mod_Cost_Repair_Efficency);
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
        gameObject.GetComponent<UnitAttributes>().FuelPool += Restoration;
        if (gameObject.GetComponent<UnitAttributes>().FuelPool > MaxFuelPool)
        {
            gameObject.GetComponent<UnitAttributes>().FuelPool = MaxFuelPool;
        }
    }
    private void HealthRestore(int Restoration)
    {
        gameObject.GetComponent<UnitAttributes>().HealthPool += Restoration;
        if (gameObject.GetComponent<UnitAttributes>().HealthPool > 100)
        {
            gameObject.GetComponent<UnitAttributes>().HealthPool = 100;
        }
    }

    public void BeginPhase()
    {

        ActionAvailable = true;
        ActionsMove = true;

        UnitAttributes unit = this.GetComponent<UnitAttributes>();
        unit.isWaiting = false;
        unit.isDefending = false;

        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void EndPhase()
    {
        ActionAvailable = false;
        ActionsMove = false;
        moving = false;

        GameManager.GManager.QuickCancel();

        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.grey;
        this.GetComponent<UnitAttributes>().FactionSided.GetComponent<Faction>().EndTurn_Faction();
    }
    public void WipeSelect() {
        isSelected = false;
        MoveChecking = false;
        AttackChecking = false;
    }
}