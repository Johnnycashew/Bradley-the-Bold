using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Puzzle Piece", menuName = "Inventory System/Items/Puzzle Piece")]
public class PuzzlePieceItemObject : ItemObject
{
    [Header("Puzzle Piece Information")]
    public Sprite sprite;

    [Header("Puzzle Image Location")]
    public Quaternion spawnRotation;
    public Vector3 spawnLocation;

    private void OnEnable()
    {
        spawnLocation = new Vector3(Random.Range(-400, 400), Random.Range(-150, 150), 0);
        spawnRotation = Quaternion.Euler(0, 0, Random.Range(15.0f, 345.0f));
    }

    private void Awake()
    {
        type = ItemType.PuzzlePiece;
    }
}
