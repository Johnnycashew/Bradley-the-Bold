using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : State
{
    public PlayerControllerTest player;

    public PlayerNormalState(PlayerControllerTest playerScript)
    {
        player = playerScript;
    }

    public void action()
    {
        player.NormalState();
    }
}
