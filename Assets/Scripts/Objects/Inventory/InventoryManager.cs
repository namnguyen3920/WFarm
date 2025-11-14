using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton_Mono_Method<InventoryManager>
{
    public Inventory inventory = new Inventory();
    public SeedDataSO seedDatabase;
    public List<StartingItem> startItems = new List<StartingItem>();    
    public Dictionary<SeedData, int> availabelSeed = new();
    public static event Action OnInventoryChanged;
    public static event Action OnAvailableSeedChanged;

    private void OnEnable()
    {
        ItemInSlotUI.OnItemSlotClicked += HandleItemClicked;
    }

    private void OnDisable()
    {
        ItemInSlotUI.OnItemSlotClicked -= HandleItemClicked;
    }
    private void HandleItemClicked(IItemUIData item)
    {
        if (item == null) { return; }
        
        if (item.GetItemData().itemType == ItemType.Sellable && ShopManager.d_Instance.CheckFinanceCondition(item))
        {
            ShopManager.d_Instance.TemporaryItemUpdatePrice(item);
        }
        
    }
    public void AddItem(ItemData item, int amount)
    {
        inventory.AddItem(item, amount);
        if (item.itemCategory == ItemCategory.Seed)
        {
            GetAvailableSeed();
            OnAvailableSeedChanged?.Invoke();
        }
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(ItemData item, int amount)
    {
        inventory.RemoveItem(item, amount);
        OnInventoryChanged?.Invoke();
    }
    public bool HasItem(ItemData item)
    {
        return inventory.HasItem(item);
    }
    public int GetItemAmount(ItemData item)
    {
        InventoryItem itemInInventory = inventory.inventoryItems.Find(i => i.inventoryItemData._id == item._id);
        if (itemInInventory == null)
            return 0;
        return itemInInventory.inventoryIntemAmount;
    }
    public Dictionary<SeedData, int> GetAvailableSeed()
    {
        availabelSeed.Clear();

        if (seedDatabase == null) return availabelSeed;

        foreach (var item in inventory.inventoryItems)
        {
            ItemData generalItem = item.inventoryItemData;

            if (generalItem.itemCategory == ItemCategory.Seed && item.inventoryIntemAmount > 0)
            {
                SeedData seedData = seedDatabase.GetItemById(generalItem._id);

                if (seedData != null)
                {
                    availabelSeed[seedData] = item.inventoryIntemAmount;
                }
                else
                {
                    Debug.LogError($"SeedData not found for ID: {generalItem._id}. Check SeedDatabaseSO.");
                }
            }
        }
        return availabelSeed;
    }
    public void InventoryChangedCall()
    {
        OnInventoryChanged?.Invoke();
    }
    public int CountItemAmountOf<T>() where T : ItemData
    {
        int totalAmount = 0;

        foreach(var item in inventory.inventoryItems)
        {
            if(item.inventoryItemData is T)
            {
                totalAmount += item.inventoryIntemAmount;
            }
        }
        return totalAmount;
    }
    public List<InventoryItem> GetAllInventoryItems()
    {
        return inventory.inventoryItems;
    }
    public ItemData GetItemById(string id)
    {
        //ItemData itemToAdd = inventory.inventoryItems.Find(item => item.inventoryItemData._id == id).inventoryItemData;
        //return itemToAdd;
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }

        var entry = inventory.inventoryItems
            .Find(item => item.inventoryItemData != null &&
                          item.inventoryItemData._id == id);

        if (entry == null)
        {
            return null;
        }

        return entry.inventoryItemData;
    }
}
