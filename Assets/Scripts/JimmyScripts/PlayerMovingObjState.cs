using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingObjState : State
{
    public PlayerControllerTest player;

    public PlayerMovingObjState(PlayerControllerTest playerScript)
    {
        player = playerScript;
    }

    public void action()
    {
        player.MovingObjState();
    }
}
