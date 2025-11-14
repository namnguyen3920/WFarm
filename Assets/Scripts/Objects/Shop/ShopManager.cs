using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class StoreItem : IItemUIData
{
    public ItemData itemData;
    public int itemPrice;
    public int quantity = 1;

    public StoreItem(ItemData item, int price, int quantity)
    {
        this.itemData = item;
        this.itemPrice = price;
        this.quantity = quantity;
    }

    public float GetAmount() => itemPrice;
    public ItemData GetItemData() => itemData;
}
[System.Serializable]
public class SellItem : IItemUIData
{
    public ItemData itemData;
    public int amount;
    public int sellPrice;

    public float GetAmount() => amount;

    public ItemData GetItemData() => itemData;
}
public class ShopManager : Singleton_Mono_Method<ShopManager>
{

    [Header("PaymentCanvasComponent")]
    [SerializeField] private RectTransform paymentTextPrefab;

    [Header("ScriptableObject References")]
    public GeneralItemSO generalItemDatabase;
    public List<StoreItem> storeItemList = new();

    public List<IItemUIData> sellItemsList = new();
    public static event Action OnSellItemsChanged;
    public static event Action<ItemData> OnInsufficientMoney;

    public static event Action<int, bool> OnTemporaryPriceChanged;

    private void Awake()
    {
        LoadStoreItem();
    }
    private void OnEnable()
    {
        ItemInSlotUI.OnStoreItemClicked += HandleItemClicked;
    }

    private void OnDisable()
    {
        ItemInSlotUI.OnStoreItemClicked -= HandleItemClicked;
    }
    public void OnConfirmClicked()
    {
        int totalMoney = CalculateTotalMoney();

        foreach(var item in sellItemsList)
        {
            ItemData itemData = item.GetItemData();
            int amount = 1;

            if(item is SellItem sellItem) amount = sellItem.amount;

            if(itemData.itemType == ItemType.Sellable) InventoryManager.d_Instance.RemoveItem(itemData, amount);
            if(itemData.itemType == ItemType.Buyable) HandleItemPurchase(itemData, amount);
        }
        sellItemsList.Clear();
        OnSellItemsChanged?.Invoke();
        GameManager.d_Instance.MoneyChange(totalMoney);
        UIManager.d_Instance.ClearPaymentLog();
        AudioManager.d_Instance.PlayBuyingSound(this.transform.position);
    }
    private int CalculateTotalMoney()
    {
        int total = 0;

        foreach(var item in sellItemsList)
        {            
            ItemData itemData = item.GetItemData();
            int price = GetItemSellPrice(itemData);
            int amount = 1;

            if (item is SellItem sellItem) amount = sellItem.amount;

            if(itemData.itemType == ItemType.Sellable) total += price * amount;
            if(itemData.itemType == ItemType.Buyable) total -= price * amount;
        }
        return total;
    }

    private void HandleItemClicked(IItemUIData item)
    {
        if (item == null) { return; }

        if (item.GetItemData().itemType == ItemType.Buyable && CheckFinanceCondition(item))
        {
            TemporaryItemUpdatePrice(item);
        }
    }
    private void HandleItemPurchase(ItemData itemData, int amount)
    {
        if (itemData == null) return;

        if (itemData.itemCategory == ItemCategory.Seed)
        {
            InventoryManager.d_Instance.AddItem(itemData, amount);
            return;
        }

        if (itemData.itemCategory == ItemCategory.Equipment)
        {
            switch (itemData._id)
            {
                case "worker":
                    WorkerManager.d_Instance.OnSpawnMoreWorker();
                    break;
                case "field":
                    FieldManager.d_Instance.OnSpawnMoreField();
                    break;
                default:
                    break;
            }
            return;
        }
    }
    public bool CheckFinanceCondition(IItemUIData item)
    {
        if (item == null) return false;

        ItemData itemData = item.GetItemData();
        int price = GetItemSellPrice(itemData);

        if (item is InventoryItem inventoryItem && itemData.itemType == ItemType.Sellable)
        {
            if (inventoryItem.inventoryIntemAmount <= 0) return false;

            inventoryItem.inventoryIntemAmount--;

            if (sellItemsList.Find(sell => sell.GetItemData()._id == itemData._id) is SellItem currentSellItem)
            {
                currentSellItem.amount++;
            }
            else
            {
                SellItem newSellItem = new SellItem
                {
                    itemData = itemData,
                    amount = 1
                };
                sellItemsList.Add(newSellItem);
            }

            InventoryManager.d_Instance.InventoryChangedCall();
            OnSellItemsChanged?.Invoke();
            return true;
        }

        if (item is StoreItem storeItem && itemData.itemType == ItemType.Buyable)
        {
            bool canBuy = GameManager.d_Instance.money >= price;

            if (!canBuy)
            {
                OnInsufficientMoney?.Invoke(itemData);
                return false;
            }
            int amountToSell = storeItem.quantity;
            if (sellItemsList.Find(sell => sell.GetItemData()._id == itemData._id) is SellItem currentSellItem)
            {
                currentSellItem.amount += amountToSell;
            }
            else
            {
                SellItem newBuyItem = new SellItem
                {
                    itemData = itemData,
                    amount = amountToSell
                };
                sellItemsList.Add(newBuyItem);
            }

            OnSellItemsChanged?.Invoke();
            return true;
        }

        return false;
    }
    public void TemporaryItemUpdatePrice(IItemUIData item)
    {
        if (item == null) return;

        int price = GetItemSellPrice(item.GetItemData());
        Debug.Log($"Item price {price}");
        bool isBuy = false;

        if (item is InventoryItem invItem && item.GetItemData().itemType == ItemType.Sellable)
        {
            isBuy = false;
        }
        else if (item is StoreItem storeItem && item.GetItemData().itemType == ItemType.Buyable)
        {
            isBuy = true;
        }
        else
        {
            return;
        }

        OnTemporaryPriceChanged?.Invoke(price, isBuy);
    }
    private void LoadStoreItem()
    {
        string store_itemPath = Path.Combine(Application.streamingAssetsPath, "csv/store_item.csv");

        string[] lines = File.ReadAllLines(store_itemPath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(",");
            string id = values[0].Trim();
            int quantity = int.Parse(values[2]);
            if (!int.TryParse(values[1], out int amount))
            {
                continue;
            }
            GeneralItemData itemData = generalItemDatabase.GetItemById(id);
            if (itemData == null)
            {
                continue;
            }
            storeItemList.Add(new StoreItem(itemData, amount, quantity));
        }
    }
    public int GetItemSellPrice(ItemData item)
    {
        switch (item)
        {
            case PlantData plant:
                return plant.harvestProduct.productPrice;
            case GeneralItemData general:
                return general.productPrice;
            default:
                return item.productPrice;
        }
    }
    public List<StoreItem> GetAllItemInStore()
    {
        return storeItemList;
    }
}
