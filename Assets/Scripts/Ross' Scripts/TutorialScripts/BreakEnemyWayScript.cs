using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreakEnemyWayScript : MonoBehaviour
{

    private void Start()
    {
        if (!GameManager.Instance.isTutorialEnabled)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerControllerTest>();

        if (player)
        {
            var enemy = FindObjectOfType<enemyScript>();
            enemy.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            Destroy(gameObject);
        }
    }

}
