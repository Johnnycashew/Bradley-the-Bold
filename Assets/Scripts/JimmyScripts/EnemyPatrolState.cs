using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : State
{
    public enemyScript enemy;

    public EnemyPatrolState(enemyScript enemyScript)
    {
        enemy = enemyScript;
    }

    public void action()
    {
        enemy.PatrolState();
    }
}
