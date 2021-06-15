using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle", menuName = "Puzzle")]
public class PuzzleObject : ScriptableObject
{
    [Header("Pieces")]
    public PuzzlePieceItemObject[] puzzlePieces;

    [Header("Placement")]
    [Tooltip("This is an array of where the target placement for corresponding pieces are supposed to go.")]
    public Vector3[] pieceLocations;
    [Tooltip("This is the distance from the target location for pieces when they are trying to be placed.")]
    public float acceptableDistance;
    [Tooltip("This is the rotation for the piece when it is trying to be placed.")]
    public float acceptableRotation;

    [Header("Completion")]
    public ItemObject[] notebookEntries;
    public Sprite finishedPuzzleImage;
    public Sprite puzzleResultImage;
}
