using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameLib
{
    public delegate void GamePlay();
    public static bool Opt_SkipMoveAnim;
    public static bool Playing_MoveAnim;
    public static bool FreezeClicks;

    public static float CameraStep = 5f;

    // Update is called once per frame
    void Update()
    {
        
    }
}
