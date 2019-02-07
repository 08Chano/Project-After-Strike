using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCheck : MonoBehaviour {

    public int MovementCost;

    public void MoveTo()
    {
        transform.parent.GetComponent<UnitActions>().MovementPhase(gameObject);
    }

    //void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.tag == "Terrain")
    //    {
    //        print("Terrain Detected");
    //        if (collision.GetComponent<TerrainProperty>() != null)
    //        {
    //            if (collision.GetComponent<TerrainProperty>().isOccupied == true)
    //            {
    //                gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //    for (int i = 0; i < 9; i++)
    //    {

    //        string TagLine = "Faction" + i.ToString();
    //        if (collision.tag == TagLine)
    //        {
    //            print("Unit Detected");
    //            gameObject.SetActive(false);
    //        }
    //    }
    //}
}