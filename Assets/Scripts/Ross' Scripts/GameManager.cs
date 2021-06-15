using System.Collections.Generic;
using UnityEngine;

//  Track our Game's State
public enum GameState
{
    Game,
    Menu,
    Message,
}

public class GameManager : Singleton<GameManager>
{
    [Header("State of Game")]
    public GameState gameState;

    [Header("Inventory")]
    public List<ItemObject> inventory;

    [Header("Puzzle Interaction")]
    public PuzzleObject puzzleToSolve;
    
    public Dictionary<PuzzleObject, bool> puzzlesSolved = new Dictionary<PuzzleObject, bool>();

    [Header("Tutorial Information")]
    public bool isTutorialEnabled;
    public bool isJournalTutorialEnabled;

    private void Update()
    {
        if (gameState == GameState.Game) Cursor.visible = false;
    }

    private void OnApplicationQuit()
    {
        ClearData();
    }

    //  Clears the player's Inventory and Puzzle information
    public void ClearData()
    {
        inventory.Clear();
        puzzlesSolved.Clear();
        NotebookManager.Instance.ClearData();

        isJournalTutorialEnabled = false;
        isTutorialEnabled = true;
    }

}
