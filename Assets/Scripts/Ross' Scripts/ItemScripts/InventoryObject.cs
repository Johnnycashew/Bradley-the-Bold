using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<ItemObject> items = new List<ItemObject>();

    public void AddItem(ItemObject _item)
    {
        items.Add(_item);
    }

}
