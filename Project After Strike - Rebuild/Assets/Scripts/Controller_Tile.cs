using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Tile: TileProperty {

    public List<Controller_Tile> MovementTileLinks = new List<Controller_Tile>();//Adjacent tiles that the unit is able to navigate to
    public List<Controller_Tile> AttackRangeList = new List<Controller_Tile>();//Creates an AoE field that the unit is able to attack to, doesn't care if target is available

    public Controller_Tile parent = null;

    public int TerrainID;

    public bool Occuping;
    public bool isHostile;
    public bool isOccupied;
    public bool isTarget;
    public bool isValid;
    public bool isAccounted = false;

    public int distance = 0;
    public int Range = 0;

    void OnEnable()
    {
        Controller_Input.CancelToggle += Reset;
    }
    void OnDisable()
    {
        Controller_Input.CancelToggle -= Reset;
    }

    private void Awake()
    {
        TerrainType(0);
    }

    private void Update()
    {
        Debug.DrawRay(gameObject.transform.position, Vector3.back, Color.red, 1);
        if (isValid) {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            //if (isHostile) {
            //    gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            //} else if (!isOccupied && !Occuping) {
            //    if (isTarget) {
            //        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            //    } else {
            //        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            //    }
            //}
        } else {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }


    //Movement
    public void ViableMovementGrid(GameObject Searcher) {

        Reset();
        MovementConnections(Vector2.up, Searcher);
        MovementConnections(Vector2.down, Searcher);
        MovementConnections(Vector2.right, Searcher);
        MovementConnections(Vector2.left, Searcher);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 1))
        {
            if (hit.collider.tag == Searcher.tag)
            {
                Occuping = true;
            }
        }
    }
    public void MovementConnections(Vector3 Axis, GameObject Searcher)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + Axis, halfExtents);
        foreach (Collider item in colliders)
        {
            Controller_Tile terrain = item.GetComponent<Controller_Tile>();
            if (terrain != null && !terrain.isOccupied)
            {
                RaycastHit hit;
                if (!Physics.Raycast(terrain.transform.position, Vector3.back, out hit, 1))
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

    //Line of Sight based Attacks
    public void LoSAttackGrid(GameObject Searcher, Vector3 Direction, int Distance)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + Direction, halfExtents);
        isValid = true;
        isHostile = true;
        Range = Distance;

        foreach (Collider item in colliders)
        {
            Controller_Tile terrain = item.GetComponent<Controller_Tile>();
            if (terrain != null)
            {
                AttackRangeList.Add(terrain);
                terrain.parent = gameObject.GetComponent<Controller_Tile>();
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
            Controller_Tile terrain = item.GetComponent<Controller_Tile>();
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
        isOccupied = false;
        isTarget = false;
        isValid = false;
        isAccounted = false;
        parent = null;
        distance = 0;
        Range = 0;
    }
}