using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : UnitProperties
{
    public GameObject[] TileSet;
    //[Range(0, 15)] public int 

    private bool MoveGrid;

    private void Awake() {
        CastArea();
    }

    private void FixedUpdate() {
        if (!GameManager.Playing_MoveAnim && isSelected) {
            if (ActionAvailable && ActionsMove && MoveChecking) {
                Cast_Movement();
            } else if (ActionAvailable && AttackChecking) {
                Cast_AttackTiles();
            }
        } else {
            if (isSelected || moving) {
                Move();
            }
        }
    }

    public void DamagePhase(GameObject Target, int CharacterAttack, int Character)
    {
        //Target.GetComponent<UnitProperty>.HealthPool -= (Attack * (HealthPool / 10)
    }

    public void Special()
    {

    }

    public void MergeAction(GameObject Target, int OtherHealth)
    {
    }

    public void MergeReceiver()
    {

    }
}