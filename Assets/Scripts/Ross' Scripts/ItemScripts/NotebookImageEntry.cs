using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Notebook Image Entry", menuName = "Notebook Entry/Image")]
public class NotebookImageEntry : ItemObject
{

    public int pageNumber;

    public Sprite sprite;

    public Vector2 position;
    public Vector2 size;

    private void Awake()
    {
        type = ItemType.NotebookImageEntry;
    }

}
