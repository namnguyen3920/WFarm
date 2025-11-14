using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    Seed,
    Plant,
    Product,
    Equipment
}
public enum ItemType
{
    None,
    Sellable,
    ReadOnly,
    Buyable,
    Special
}
[System.Serializable]
public class ItemData
{
    public string _id;
    public string displayName;
    public GameObject itemPrefab;
    public Sprite itemSprite;
    public int productPrice;

    public ItemCategory itemCategory;
    public Item item;
    public ItemType itemType;
}

[System.Serializable]
public class SeedData : ItemData
{
    public string plantId;
    public int priceSeed;
}
[System.Serializable]
public class PlantData : ItemData
{
    public List<GameObject> plantGrowPrefab;
    public float[] stageGrowTimes;

    public int maxHarvest = 40;
    public ItemData harvestProduct;
    public int harvestAmount = 1;

    public int MaxStages { get { return plantGrowPrefab.Count; } }
}

[System.Serializable]
public class WorkerData : ItemData
{
    public string workerID;
    public SeedData preferredSeed;
    public float actionDuration;
    public float moveSpeed;
    public bool isWorking = false;
}

[System.Serializable]
public class GeneralItemData : ItemData 
{
    public string itemDataIdLink;
}

[System.Serializable]
public class ShopItemData : ItemData
{
    public string targetId;
    public int quantity;
}

[System.Serializable]
public class AnimalData : ItemData { }