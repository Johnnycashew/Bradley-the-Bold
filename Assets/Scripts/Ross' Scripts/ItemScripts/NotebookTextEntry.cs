using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Notebook Image Entry", menuName = "Notebook Entry/Text")]
public class NotebookTextEntry : ItemObject
{

    public int pageNumber;

    [TextArea(5, 10)] public string text;

    public Vector2 position;
    public Vector2 size;

    private void Awake()
    {
        type = ItemType.NotebookTextEntry;
    }

}
