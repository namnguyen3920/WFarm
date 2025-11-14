using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedData", menuName = "Scriptable Objects/SeedData")]
public class SeedDataSO : GameDatabaseSO<SeedData>
{
    public PlantDataSO plantDatabase;
    public PlantData GetPlantById(string id)
    {
        return plantDatabase.GetItemById(id);
    }
}
