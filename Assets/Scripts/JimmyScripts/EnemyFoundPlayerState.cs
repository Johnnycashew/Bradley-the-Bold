using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFoundPlayerState : State
{
    public enemyScript enemy;

    public EnemyFoundPlayerState(enemyScript enemyScript)
    {
        enemy = enemyScript;
    }

    public void action()
    {
        enemy.FoundPlayer();
    }
}
