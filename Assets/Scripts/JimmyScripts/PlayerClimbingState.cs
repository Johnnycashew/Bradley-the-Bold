using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : State
{
    public PlayerControllerTest player;

    public PlayerClimbingState(PlayerControllerTest playerScript)
    {
        player = playerScript;
    }

    public void action()
    {
        player.ClimbingState();
    }
}
