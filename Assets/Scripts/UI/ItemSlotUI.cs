using System;
using System.Xml.Xsl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemAmount;
    public static event Action<IItemUIData> OnItemSlotClicked;
    public static event Action<IItemUIData> OnStoreItemClicked;

    public IItemUIData currentItemData;

    private void OnEnable()
    {
        itemAmount = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SlotSetUP(IItemUIData item)
    {
        this.currentItemData = item;
    }
    public void DisplayItemAttributes(ItemData itemData, float amount)
    {
        if (itemData != null)
        {
            itemIcon.sprite = itemData.itemSprite;
            itemAmount.text = amount.ToString();
        }        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.d_Instance.PlayItemClickSound(this.transform.position);
        if (currentItemData is InventoryItem inventoryItem)
            OnItemSlotClicked?.Invoke(inventoryItem);
        
        else if (currentItemData is StoreItem storeItem)
            OnStoreItemClicked?.Invoke(storeItem);        
    }
}
