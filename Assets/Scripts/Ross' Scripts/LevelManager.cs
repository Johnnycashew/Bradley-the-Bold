using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private bool isReady = false;

    private GameManager gameManager;

    public bool isCursorLocked = true;

    public Dialogue dialogue;

    public PuzzleObject requiredPuzzle;

    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState != GameState.Game) return;

        if (Input.GetKeyDown(KeyCode.R) &&
            isReady)
        {
            if (!isCursorLocked) Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(sceneName);
        }

        if (gameManager.puzzlesSolved.ContainsKey(requiredPuzzle) &&
            !isReady)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            isReady = true;
        }
    }
}
