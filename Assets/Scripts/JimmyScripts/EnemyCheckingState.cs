using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckingState : State
{
    public enemyScript enemy;

    public EnemyCheckingState(enemyScript enemyScript)
    {
        enemy = enemyScript;
    }

    public void action()
    {
        enemy.CheckingState();
    }
}
