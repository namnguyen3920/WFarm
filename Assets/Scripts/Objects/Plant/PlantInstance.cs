using UnityEngine;

public class PlantInstance
{
    public PlantData PlantData { get; private set; }
    public GameObject CurrentStagePrefab => currentStage;
    public int CurrentStage { get; private set; }
    public float CurrentTime { get; private set; }
    public bool IsFullyGrown => CurrentStage >= PlantData.MaxStages - 1;
    public bool IsCompletelyHarvest { get; private set; } = false;

    private GameObject currentStage;
    private Transform fieldTransform;

    private int harvestCount = 0;
    public float deathPlantWarningTime = 30f;
    public float deathTime = 60f;
    public bool isDead = false;

    public PlantInstance(PlantData plantData, Transform parent)
    {
        PlantData = plantData;
        fieldTransform = parent;
        CurrentStage = 0;
        CurrentTime = 0f;
        SpawnStage(CurrentStage);
    }
    public void Update(float deltaTime)
    {
        if (!IsFullyGrown) 
        {

            CurrentTime += deltaTime;

            if (CurrentTime >= PlantData.stageGrowTimes[CurrentStage])
            {
                CurrentTime = 0f;
                CurrentStage++;

                SpawnStage(CurrentStage);
            }
            return;
        }
        CurrentTime += deltaTime;

        if(CurrentTime >= deathPlantWarningTime && !isDead)
        {
            isDead = true;
        }

        if (CurrentTime >= deathTime) KillPlant();

    }
    private void KillPlant()
    {
        if (CurrentStagePrefab != null)
            GameObject.Destroy(CurrentStagePrefab);

        fieldTransform = null;
        IsCompletelyHarvest = true;
    }
    public void HarvestPlant()
    {
        harvestCount++;
        CurrentTime = 0f;
        isDead = false;

        if (harvestCount >= PlantData.maxHarvest)
        {
            KillPlant();
            return;
        }

        CurrentStage = 1;
        SpawnStage(CurrentStage);

        CurrentTime = 0f;
    }
    private void SpawnStage(int stage)
    {
        if (currentStage != null) { Object.Destroy(currentStage); }
        GameObject stagePrefab = PlantData.plantGrowPrefab[stage];

        currentStage = Object.Instantiate(stagePrefab, fieldTransform);
    }
}
