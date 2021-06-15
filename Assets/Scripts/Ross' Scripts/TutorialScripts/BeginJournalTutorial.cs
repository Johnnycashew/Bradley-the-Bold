using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginJournalTutorial : MonoBehaviour
{

    public Dialogue dialogue;

    public PuzzleObject puzzle;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.Instance.isTutorialEnabled)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.inventory.Count == puzzle.puzzlePieces.Length && 
            GameManager.Instance.gameState == GameState.Game)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            GameManager.Instance.isJournalTutorialEnabled = true;
            Destroy(gameObject);
        }
    }
}
