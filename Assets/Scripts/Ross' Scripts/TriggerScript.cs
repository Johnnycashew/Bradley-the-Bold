using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private GameManager gameManager;
    public enemyScript enemy;
    public int numberOfItems;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerControllerTest>();

        if (player && gameManager.inventory.Count >= numberOfItems)
        {
            enemy.isActive = true;
        }
    }
}
