using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "Scriptable Objects/ItemDatabaseSO")]
public class ItemDatabaseSO : GameDatabaseSO<ItemData>
{
    public SeedDataSO seedDatabase;
    public PlantDataSO plantDatabase;
    public ShopDataSO shopDatabase;
    public GeneralItemSO generalDatabase;
    public ItemData GetItemByID(string id)
    {
        var seedItem = seedDatabase.itemList.Find(x => x._id == id);
        if (seedItem != null) return seedItem;

        var plantItem = plantDatabase.itemList.Find(x => x._id == id);
        if (plantItem != null) return plantItem;

        var shopItem = shopDatabase.itemList.Find(x => x._id == id);
        if (shopItem != null) return shopItem;

        var generalItem = generalDatabase.itemList.Find(x => x._id == id);
        if (generalItem != null) return generalItem;

        return null;
    }
}
 