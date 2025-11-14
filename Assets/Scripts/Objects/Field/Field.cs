using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Field : MonoBehaviour, IClickable, ITask
{

    [Header("World Point Canvas")]
    [SerializeField] private RectTransform ChooseSeedCanvas;
    [SerializeField] private RectTransform plantButtonCanvas;
    [SerializeField] private RectTransform harvestButtonCanvas;
    [SerializeField] private RectTransform warningCanvas;

    [Header("World Position")]
    [SerializeField] private GameObject plantPosition;

    [Header("Field Settings")]
    public int fieldLevel = 1;    
    public FieldStates state = FieldStates.Empty;
    public FieldType fieldType = FieldType.None;

    public PlantInstance currentPlant;   

    public bool isSelecting = false;
    public SeedDataSO seedDatabase;

    [SerializeField] private List<PlantInstance> plantedPlants = new List<PlantInstance>();
    
    private InventoryManager inventory;
    private Button plantBtn;
    private Button harvestBtn;

    private void Awake()
    {
        FieldManager.d_Instance.RegisterField(this);
        plantBtn = plantButtonCanvas.GetComponentInChildren<Button>(); 
        harvestBtn = harvestButtonCanvas.GetComponentInChildren<Button>();
        plantBtn.onClick.AddListener(() => OnPlantClicked());
        harvestBtn.onClick.AddListener(() => OnHarvestClicked());
        inventory = FindAnyObjectByType<InventoryManager>();
    }
    private void Update()
    {
        CheckFieldStates();
        UpdateWarningUI();
    }    
    
    public void Plant(SeedData seed)
    {
        if (seed is SeedData) Debug.Log("Seed just plant is Seed Data");
        else Debug.Log("Seed just plant not Seed Data");

        //if (state != FieldStates.Empty) return;
        if (state == FieldStates.ReadyToHarvest || state == FieldStates.Growing) return;
        CheckFieldType(seed);
        Debug.Log("Seed in, ready to plant 2");
        PlantData plant = seedDatabase.GetPlantById(seed.plantId);
        if (plant == null)
        {
            return;
        }
        HideFieldUI();
        Debug.Log("Seed in, ready to plant 3");
        if (plant == null) return;

        plantedPlants.Clear();
        Debug.Log("Seed in, ready to plant 4");
        int seedAmount = inventory.GetItemAmount(seed);

        if (seedAmount == 0) { return; }
        
        Debug.Log("Seed in, ready to plant");
        int planted = 0;
        foreach (Transform pos in plantPosition.transform)
        {
            if (planted >= seedAmount) break;
            Debug.Log("Planted");
            PlantInstance instance = new PlantInstance(plant, pos);
            plantedPlants.Add(instance);
            TimeManager.d_Instance.RegisterPlant(instance);
            inventory.RemoveItem(seed, 1);
            planted++;
        }
    }
    public int Harvest()
    {
        if (!CanHarvest || plantedPlants.Count == 0) return 0;

        int totalHarvested = 0;
        List<PlantInstance> newPlantedPlants = new List<PlantInstance>();

        for (int i = plantedPlants.Count - 1; i >= 0; i--)
        {
            var plant = plantedPlants[i];
            if (plant.IsFullyGrown)
            {
                int amount = plant.PlantData.harvestAmount;
                inventory.AddItem(plant.PlantData.harvestProduct, amount);   
                
                plant.HarvestPlant();
                AudioManager.d_Instance.PlayHarvestSound(this.transform.position);
                totalHarvested += amount;

            }
            if (plant.IsCompletelyHarvest)
            {
                plantedPlants.RemoveAt(i);
                continue;
            }
        }

        currentPlant = plantedPlants.Count > 0 ? plantedPlants[0] : null;
        
        CheckFieldStates();
        return totalHarvested;
    }
    public void OnPlantClicked()
    {
        plantButtonCanvas.gameObject.SetActive(false);
        ChooseSeedCanvas.gameObject.SetActive(true);
    }
    public void OnHarvestClicked()
    {
        int harvested = Harvest();
        harvestButtonCanvas.gameObject.SetActive(false);
    }
    void CheckFieldStates()
    {
        if (CanHarvest && plantedPlants.Count > 0)
            state = FieldStates.ReadyToHarvest;
        else if (plantedPlants.Count > 0)
            state = FieldStates.Growing;
        else
            state = FieldStates.Empty;
    }
    private void CheckFieldType(SeedData seed)
    {
        if (seed == null) return;

        switch (seed.item)
        {
            case (Item.SeedTomato):
                fieldType = FieldType.Tomato; break;

            case (Item.SeedBlueberry):
                fieldType = FieldType.Blueberry; break;

            case (Item.SeedStrawberry):
                fieldType = FieldType.Strawberry; break;
        }
    }
    public void OnClick()
    {
        FieldManager.d_Instance.SelectingField(this);
    }

    public void ShowFieldUI()
    {
        if (state == FieldStates.Empty) plantButtonCanvas.gameObject.SetActive(true);
        else if (state == FieldStates.ReadyToHarvest) harvestButtonCanvas.gameObject.SetActive(true);
        else if(state == FieldStates.Growing) return;
    }
    public void HideFieldUI()
    {
        isSelecting = false;
        plantButtonCanvas.gameObject.SetActive(false);
        harvestButtonCanvas.gameObject.SetActive(false);
        ChooseSeedCanvas?.gameObject.SetActive(false);
    }


    // +++++++++++ AUTOMATION SYSTEM SETUP +++++++++++ //


    public bool IsWorkable()
    {
        return state == FieldStates.Empty || state == FieldStates.ReadyToHarvest;
    }
    public bool IsDoing { get; set; }
    public bool CanWork(Worker worker)
    {
        bool isSeedAvailable = InventoryManager.d_Instance.CountItemAmountOf<SeedData>() > 0;

        if (state == FieldStates.ReadyToHarvest)
        {
            return true;
        }

        if (state == FieldStates.Empty)
        {
            return isSeedAvailable;
        }

        return false;
    }
    public void DoWork(Worker worker)
    {
        if (state == FieldStates.ReadyToHarvest)
            worker.ChangeState(new HarvestingState(this));
        else if (state == FieldStates.Empty)
            worker.ChangeState(new PlantingState(this));
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    private void UpdateWarningUI()
    {
        if (plantedPlants.Count == 0)
        {
            warningCanvas.gameObject.SetActive(false);
            return;
        }

        PlantInstance plant = plantedPlants[1];

        if (plant.isDead && plant.CurrentTime >= plant.deathTime)
        {
            warningCanvas.gameObject.SetActive(false);
            plantedPlants.Clear();
            return;
        }

        bool showWarning = plant.CurrentTime >= plant.deathPlantWarningTime && plant.CurrentTime < plant.deathTime;

        warningCanvas.gameObject.SetActive(showWarning);
    }
    public bool IsPlanted => currentPlant != null || plantedPlants.Count > 0;
    public bool CanHarvest
    {
        get
        {
            if (plantedPlants.Count == 0) return false;
            foreach (var plant in plantedPlants)
            {
                if(!plant.IsFullyGrown) return false;
            }
            return true;
        }
    }
}
