using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalTutorialScript : MonoBehaviour
{

    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.isJournalTutorialEnabled)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            GameManager.Instance.isJournalTutorialEnabled = false;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Message)
        {
            Cursor.visible = false;
            return;
        }
        else
        {
            Cursor.visible = true;
            Destroy(this);
        }
    }

}
