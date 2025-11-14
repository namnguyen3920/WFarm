using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InventoryItem : IItemUIData
{
    public ItemData inventoryItemData;
    public int inventoryIntemAmount;
    //public ItemType slotItemType;
    public float GetAmount() => inventoryIntemAmount;   
    public ItemData GetItemData() => inventoryItemData;
    //public ItemType GetItemType() => inventoryItemData.itemType;
}

[System.Serializable]
public class Inventory
{
    public List<InventoryItem> inventoryItems = new ();
    public void AddItem(ItemData itemType, int amount = 1)
    {
        var item = inventoryItems.Find(i => i.inventoryItemData._id == itemType._id);

        if (item == null)
        {
            inventoryItems.Add(new InventoryItem { inventoryItemData = itemType, inventoryIntemAmount = amount });
        }
        else
        {
            item.inventoryIntemAmount += amount;
        }
    }
    public void RemoveItem(ItemData itemType, int amount)
    {
        var item = inventoryItems.Find(i => i.inventoryItemData._id == itemType._id);

        if (item == null)
            return;

        item.inventoryIntemAmount -= amount;

        if (item.inventoryIntemAmount <= 0)
            inventoryItems.Remove(item);
    }
    public bool HasItem(ItemData itemType)
    {
        foreach (var item in inventoryItems)
            if (item.inventoryItemData == itemType) return true;

        return false;
    }
}
