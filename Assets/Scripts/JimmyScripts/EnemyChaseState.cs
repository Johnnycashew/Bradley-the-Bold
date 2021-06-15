using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State
{
    public enemyScript enemy;

    public EnemyChaseState(enemyScript enemyScript)
    {
        enemy = enemyScript;
    }

    public void action()
    {
        enemy.ChasingState();
    }
}
