using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneTrigger : MonoBehaviour
{
    public GameObject trigger;
    public GameObject dialogue;

    private void OnTriggerEnter(Collider other)
    {
        trigger.SetActive(true);
        dialogue.SetActive(true);
        Destroy(this);
    }
}
