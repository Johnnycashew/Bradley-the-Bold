using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueTrigger : MonoBehaviour
{

    public DialogueTrigger dialogueTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.isTutorialEnabled)
        {
            Destroy(gameObject);
            return;
        }

        var player = other.GetComponent<PlayerControllerTest>();

        if (player)
        {
            dialogueTrigger.TriggerDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerControllerTest>();

        if (player)
        {
            Destroy(gameObject);
        }
    }

}
