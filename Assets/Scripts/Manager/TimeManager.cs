using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : Singleton_Mono_Method<TimeManager>
{
    private List<PlantInstance> growingPlant = new();

    private void Update()
    {
        float delta = Time.deltaTime;

        foreach (var plant in growingPlant)
        {
            plant.Update(delta);
        }
    }

    public void RegisterPlant(PlantInstance plant)
    {
        if (!growingPlant.Contains(plant))
        {
            growingPlant.Add(plant);
        }
    }
}
