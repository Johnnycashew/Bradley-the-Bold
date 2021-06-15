using UnityEngine;

public enum ItemType
{
    Document,
    PuzzlePiece,
    NotebookImageEntry,
    NotebookTextEntry,
}

public class ItemObject : ScriptableObject
{
    [Header("Basic Item Information")]
    public ItemType type;
}
