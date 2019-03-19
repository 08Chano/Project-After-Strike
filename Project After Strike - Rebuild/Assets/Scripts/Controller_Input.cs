using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Input : InputLib
{
    public delegate void UIToggle();
    public static event UIToggle CancelToggle;
    public static event UIToggle SkipToggle;

    GameObject MCam;

    private void Start()
    {
        MCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        //Camera Moving within battle scene
        if (Input.GetAxis("Horizontal") > 0) { MCam.transform.transform.Translate(GameManager.CameraStep * Time.deltaTime, 0,0); }
        if (Input.GetAxis("Horizontal") < 0) { MCam.transform.transform.Translate(-GameManager.CameraStep * Time.deltaTime, 0, 0); }
        if (Input.GetAxis("Vertical") > 0) { MCam.transform.transform.Translate(0, GameManager.CameraStep * Time.deltaTime, 0); }
        if (Input.GetAxis("Vertical") < 0) { MCam.transform.transform.Translate(0, -GameManager.CameraStep * Time.deltaTime, 0); }
        //End of Camera Controls

        if (Input.GetButtonDown("Cancel")) { Cancel(); }
        if (Input.GetButtonDown("Action1")) { CallAction_Move(); }
        if (Input.GetButtonDown("Action2")) { CallAction_Attack(); }
        if (Input.GetButtonDown("Action3")) { CallAction_Sync(); }
        if (Input.GetButtonDown("Action4")) { CallAction_Defend(); }
        if (Input.GetButtonDown("Action5")) { CallAction_Wait(); }

    }

    private void Cancel()
    {
        CancelToggle();
        print("CANCEL");
    }

    private void CallAction_Move()
    {
        print("Action1");
    }

    private void CallAction_Attack()
    {
        print("Action2");
    }

    private void CallAction_Sync()
    {
        print("Action3");
    }

    private void CallAction_Defend()
    {
        print("Action4");
    }

    private void CallAction_Wait()
    {
        print("Action5");
    }
}
