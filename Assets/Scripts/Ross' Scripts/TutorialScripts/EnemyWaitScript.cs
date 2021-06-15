using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaitScript : MonoBehaviour
{

    private void Start()
    {
        if (!GameManager.Instance.isTutorialEnabled)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<enemyScript>();

        if (enemy)
        {
            other.GetComponent<NavMeshAgent>().isStopped = true;
            Destroy(gameObject);
        }
    }

}
