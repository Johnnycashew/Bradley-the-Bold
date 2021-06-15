using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Document", menuName = "Inventory System/Items/Document")]
public class DocumentItemObject : ItemObject
{
    [Header("Document Information")]
    Image image;

    private void Awake()
    {
        type = ItemType.Document;
    }
}
