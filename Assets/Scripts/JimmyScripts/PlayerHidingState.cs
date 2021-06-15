using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHidingState : State
{
    public PlayerControllerTest player;

    public PlayerHidingState(PlayerControllerTest playerScript)
    {
        player = playerScript;
    }

    public void action()
    {
        player.HidingState();
    }
}
