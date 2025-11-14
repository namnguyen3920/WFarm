using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameDatabaseSO<T> : ScriptableObject where T : ItemData
{
    public List<T> itemList = new();
    
    public T GetItemById(string id)
    {
        return itemList.Find(item => item._id == id);
    }
    public List<T> GetAllItems()
    {
        return itemList;
    }
    public T GetItemByType(Item type)
    {
        return itemList.Find(data => data.item == type);
    }
}
