using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : State
{
    public PlayerControllerTest player;

    public PlayerDeadState(PlayerControllerTest playerScript)
    {
        player = playerScript;
    }

    public void action()
    {
        player.DeadState();
    }
}
