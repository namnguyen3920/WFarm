using System;
using System.Xml.Linq;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [Header("Worker Attributes")]
    public string workerId;
    public float actionDuration = 10f;
    public float workerSpeed = 2f;


    [Header("timer UI")]
    public RectTransform timerCanvas;
    public RectTransform progressFill;

    private const float progressMaxWidth = 7.55f;

    public WorkerManager Manager { get; private set; }
    [NonSerialized] public WorkerInstance Instance;

    public GameObject prefab;
    public IWorkerStates currentState;
    public ITask targetTask;
    public InventoryManager inventory;
  
    private void Start()
    {
        inventory = FindAnyObjectByType<InventoryManager>();
        ChangeState(new IdleState());
    }
    public void Init(WorkerInstance instance)
    {
        Instance = instance;
    }
    public void Update()
    {
        currentState?.Execute(this);
    }
    public void UpdateProgress(float current, float total)
    {
        if (total <= 0)
        {
            timerCanvas.gameObject.SetActive(false);
            return;
        }

        timerCanvas.gameObject.SetActive(true);

        float t = Mathf.Clamp01(current / total);

        progressFill.sizeDelta = new Vector2(progressMaxWidth * t, progressFill.sizeDelta.y);
    }

    public void HideProgress()
    {
        timerCanvas.gameObject.SetActive(false);
    }
    public Vector3 BackToStation()
    {
        if (Manager == null)
            Manager = WorkerManager.d_Instance;

        return Manager.workerSpawnStation.position;
    }
    public void ChangeState(IWorkerStates newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        newState.Enter(this);
    }
}
