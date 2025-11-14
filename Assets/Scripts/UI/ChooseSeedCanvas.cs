using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSeedCanvas : MonoBehaviour
{
    public Dictionary<SeedData, int> currentAvailableSeed = new();
    [SerializeField] RectTransform seedSlotPrefab;
    [SerializeField] RectTransform content;

    private void OnEnable()
    {
        InventoryManager.OnAvailableSeedChanged += GetAvailableSeed;
        GetAvailableSeed();
    }

    private void OnDisable()
    {
        InventoryManager.OnAvailableSeedChanged -= GetAvailableSeed;
    }
    private void GetAvailableSeed()
    {
        Debug.Log($"CurrenAvailableSeed call at Choose Seed Canvas {currentAvailableSeed.Count}");
        currentAvailableSeed.Clear();

        currentAvailableSeed = InventoryManager.d_Instance.GetAvailableSeed();

        DisplayCurrentSeed();
    }
    private void DisplayCurrentSeed()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var seed in currentAvailableSeed)
        {
            RectTransform itemSlotInstance = Instantiate(seedSlotPrefab, content);
            
            ItemInSlot itemSlotScript = itemSlotInstance.GetComponent<ItemInSlot>();
            if (itemSlotScript != null) 
            {
                itemSlotScript.DisplayItemAttributes(seed.Key, seed.Value);
            }
        }
    }
}
