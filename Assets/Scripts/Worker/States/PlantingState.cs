using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlantingState : IWorkerStates
{
    private Field assignedField;    
    private SeedData choseSeed;
    private float timer;
    private Dictionary<SeedData, int> seeds = new Dictionary<SeedData, int>();
    public PlantingState(Field field)
    {
        assignedField = field;
    }
    public void Enter(Worker worker)
    {
        seeds = InventoryManager.d_Instance.GetAvailableSeed();
        if (seeds.Count == 0)
        {

            if (assignedField != null)
            {
                assignedField.IsDoing = false;
            }

            worker.targetTask = null;
            worker.ChangeState(new IdleState());
            return;
        }
        if (assignedField == null)
        {
            return;
        }

        choseSeed = seeds.Keys.FirstOrDefault();
        
        timer = 0f;

        worker.UpdateProgress(0, worker.Instance.Data.actionDuration);
    }

    public void Execute(Worker worker)
    {
        if(assignedField == null) { return; }

        worker.Instance.Data.isWorking = true;

        timer += Time.deltaTime;

        worker.UpdateProgress(timer, worker.Instance.Data.actionDuration);

        if (timer < worker.Instance.Data.actionDuration) { return; }

        assignedField.Plant(choseSeed);
        worker.HideProgress();
        worker.ChangeState(new IdleState());


    }

    public void Exit(Worker worker)
    {
        worker.Instance.Data.isWorking = false;
        assignedField.IsDoing = false;
        assignedField = null;
    }
}
