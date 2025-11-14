using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton_Mono_Method<UIManager>
{
    [Header("Canvas")]
    [SerializeField] private RectTransform itemInventoryContainer;
    [SerializeField] private RectTransform itemStoreContainer;
    [SerializeField] private RectTransform itemInventoryStoreContainer;
    [SerializeField] private RectTransform sellItemsContainer;
    [SerializeField] private RectTransform paymentLogContainer;
    [SerializeField] private RectTransform annoucementContainer;

    [Header("GUI Prefab")]
    [SerializeField] private RectTransform itemSlotUIPrefab;
    [SerializeField] private RectTransform paymentTextPrefab;
    [SerializeField] private RectTransform annoucementTextPrefab;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private ScrollRect scrollPaymentLog;
    [SerializeField] private TextMeshProUGUI balanceText;

    private InventoryManager inventoryManager;
    private ShopManager shopManager;

    public int MaxSlotInInventory = 55;

    private void OnEnable()
    {
        InventoryManager.OnInventoryChanged += DisplayItemInInventory;
        GameManager.OnMoneyChanged += DisplayMoney;
        ShopManager.OnSellItemsChanged += UpdateSellWindowUI;
        ShopManager.OnTemporaryPriceChanged += HandleViewPriceLog;
        ShopManager.OnInsufficientMoney += HandleShopAnnoucement;
        DisplayMoney();
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= DisplayItemInInventory;
        GameManager.OnMoneyChanged -= DisplayMoney;
        ShopManager.OnSellItemsChanged -= UpdateSellWindowUI;
        ShopManager.OnTemporaryPriceChanged -= HandleViewPriceLog;
        ShopManager.OnInsufficientMoney -= HandleShopAnnoucement;
    }
    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        shopManager = FindAnyObjectByType<ShopManager>();
        InventoryManager.OnInventoryChanged += DisplayItemInInventory;
        DisplayMoney();
        DisplayItemInInventory();
        DisplayItemInStore();
    }

    private void DisplayItems<T>(RectTransform holder, List<T> database) where T : IItemUIData
    {
        foreach (Transform child in holder)
            Destroy(child.gameObject);

        foreach (var item in database)
        {
            RectTransform itemSlot = Instantiate(itemSlotUIPrefab, holder);
            ItemInSlotUI itemSlotScript = itemSlot.GetComponent<ItemInSlotUI>();
            itemSlotScript.SlotSetUP(item);
            itemSlotScript.DisplayItemAttributes(item.GetItemData(), item.GetAmount());
        }
    }
    public void DisplayItemInInventory()
    {
        if (inventoryManager == null) return;
        DisplayItems(itemInventoryContainer, inventoryManager.GetAllInventoryItems());
        DisplayItems(itemInventoryStoreContainer, inventoryManager.GetAllInventoryItems());
    }
    public void DisplayItemInStore()
    {
        if (shopManager == null) return;
        
        DisplayItems(itemStoreContainer, shopManager.GetAllItemInStore());
    }
    private void HandleViewPriceLog(int price, bool isBuy)
    {
        RectTransform textPrefab = Instantiate(paymentTextPrefab, paymentLogContainer);
        TextMeshProUGUI text = textPrefab.GetComponentInChildren<TextMeshProUGUI>();

        int itemTextPrefabCount = paymentLogContainer.childCount;
        int textPrefabMaxItem = 6;
        scrollPaymentLog.enabled = itemTextPrefabCount > textPrefabMaxItem;

        if (text != null)
        {
            string sign = isBuy ? "-" : "+";
            text.text = $"{sign}{price}";
        }
    }
    private void HandleShopAnnoucement(ItemData itemData)
    {
        foreach (Transform child in itemStoreContainer)
        {
            var itemSlot = child.GetComponent<ItemInSlotUI>();
            if (itemSlot != null && itemSlot.currentItemData.GetItemData()._id == itemData._id)
            {
                RectTransform notif = Instantiate(annoucementTextPrefab, annoucementContainer);
                var text = notif.GetComponent<TextMeshProUGUI>();
                text.text = "Insufficient Money";
                Destroy(notif.gameObject, 0.5f);
                break;
            }
        }
        if(FieldManager.d_Instance.GetAllFields().Count == 10)
        {
            RectTransform notif = Instantiate(annoucementTextPrefab, annoucementContainer);
            var text = notif.GetComponent<TextMeshProUGUI>();
            text.text = "Max Field Amount Reached";
            Destroy(notif.gameObject, 0.5f);
        }
    }
    public void ClearPaymentLog()
    {
        foreach (Transform child in paymentLogContainer)
            Destroy(child.gameObject);
    }
    public void UpdateSellWindowUI()
    {
        DisplayItems(sellItemsContainer, shopManager.sellItemsList);
    }
    public void DisplayMoney()
    {
        moneyText.SetText(GameManager.d_Instance.money.ToString());
        balanceText.SetText(GameManager.d_Instance.money.ToString());
    }
    public void ShowUI(RectTransform component)
    {
        component.gameObject.SetActive(true);
    }
    public void HideUI(RectTransform component)
    {
        component.gameObject.SetActive(false);
    }
}
