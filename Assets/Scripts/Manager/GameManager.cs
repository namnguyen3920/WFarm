using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StartingItem
{
    public ItemData itemData;
    public int itemQuantity;

    public StartingItem(ItemData item, int quantity)
    {
        this.itemData = item;
        this.itemQuantity = quantity;
    }
}

public class GameManager : Singleton_Mono_Method<GameManager>
{

    [Header("UI Canvas")]
    public RectTransform startGameCanvas;
    public RectTransform endGameCanvas;


    [Header("Settings")]
    public int money = 0;
    public int initialSeedAmount = 10;

    [Header("List")]
    public List<StartingItem> startItems = new List<StartingItem>();

    [Header("ScriptableObject References")]
    public ItemDatabaseSO itemDatabase;

    public static event Action OnMoneyChanged;

    private void Start()
    {
        LoadStartingSettings();
        InitConfig();
        ShowStartCanvas();
    }
    private void Update()
    {
        if (money == 100000) ShowEndCanvas();
    }
    private void InitConfig()
    {
        foreach(var item in startItems)
        {
            InventoryManager.d_Instance.inventory.AddItem(item.itemData, item.itemQuantity);
        }
    }
    private void LoadStartingSettings()
    {
        string starting_itemPath = Path.Combine(Application.streamingAssetsPath, "csv/starting_item.csv");

        LoadStartItems(starting_itemPath);
    }
    public void MoneyChange(int amount)
    {
        money += amount;        
        OnMoneyChanged?.Invoke();
    }
    private void LoadStartItems(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }
        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(",");
            string id = values[0].Trim();
            if (!int.TryParse(values[1], out int amount))
            {
                continue;
            }
            ItemData itemData = itemDatabase.GetItemByID(id);
            if(itemData == null)
            {
                continue;
            }
            startItems.Add(new StartingItem(itemData, amount));
        }        
    }

    
    public void HideStartCanvas()
    {
        startGameCanvas.gameObject.SetActive(false);
        ResumeGame();
    }
    public void HideEndCanvas()
    {
        endGameCanvas.gameObject.SetActive(false);
        ResumeGame();
    }
    public void StartOverGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowStartCanvas()
    {
        startGameCanvas.gameObject.SetActive(true);
        PauseGame();
    }
    private void ShowEndCanvas()
    {
        endGameCanvas.gameObject.SetActive(true);
        PauseGame();
    }
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
