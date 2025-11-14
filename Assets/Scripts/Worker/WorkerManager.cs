using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WorkerManager : Singleton_Mono_Method<WorkerManager>
{
    public List<Worker> workers = new List<Worker>();
    //public List<Field> allFields = new List<Field>();
    public HashSet<Field> assignedFields = new ();

    public Worker workerPrefab;
    public InventoryManager inventory;
    public FieldManager fieldManager;
    public Transform workerSpawnStation;
    private void OnEnable()
    {
        GameSettingsManager.OnStarterDataLoaded += SpawnStarterWorker;
    }
    private void OnDisable()
    {
        GameSettingsManager.OnStarterDataLoaded -= SpawnStarterWorker;
    }
    private void Start()
    {
        fieldManager = FindAnyObjectByType<FieldManager>();
        inventory = FindAnyObjectByType<InventoryManager>();

    }
    private void Update()
    {
        GetAllFieldOnScene();
    }

    void SpawnStarterWorker(Dictionary<string, string> settings)
    {
        int workerCount = int.Parse(settings["StartWorkers"]);
        Debug.Log($"Worker Count {workerCount}");
        for (int i = 0; i < workerCount; i++)
        {
            OnSpawnMoreWorker();
        }
    }
    public void OnSpawnMoreWorker()
    {
        GameObject prefab = Instantiate(workerPrefab.gameObject, workerSpawnStation);
        Worker worker = prefab.GetComponent<Worker>();

        WorkerData workerClone = new WorkerData()
        {
            workerID = worker.workerId,
            actionDuration = worker.actionDuration,
            moveSpeed = worker.workerSpeed
        };

        WorkerInstance instance = new WorkerInstance(workerClone, worker);
        worker.Init(instance);

        workers.Add(worker);
    }
    public Field GetAvailableField()
    {
        foreach (var field in GetAllFieldOnScene())
        {
            if ((field.state == FieldStates.Empty || field.CanHarvest)
                && !IsFieldAssigned(field))
            {
                assignedFields.Add(field);
                return field;
            }
                
        }
        return null;
    }
    public List<Field> GetAllFieldOnScene()
    {
        return fieldManager.GetAllFields();
    }
    private bool IsFieldAssigned(Field field)
    {
        return assignedFields.Contains(field);
    }
}
