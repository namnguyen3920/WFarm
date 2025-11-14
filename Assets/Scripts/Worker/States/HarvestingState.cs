using UnityEngine;

public class HarvestingState : IWorkerStates
{
    private Field assignedField;
    private float timer;
    public HarvestingState(Field field)
    {
        assignedField = field;
    }
    public void Enter(Worker worker)
    {
        timer = 0f;
        worker.UpdateProgress(0, worker.Instance.Data.actionDuration);
    }

    public void Execute(Worker worker)
    {
        if (assignedField == null) { return; }

        worker.Instance.Data.isWorking = true;

        timer += Time.deltaTime;

        worker.UpdateProgress(timer, worker.Instance.Data.actionDuration);

        if (timer < worker.Instance.Data.actionDuration) { return; }

        assignedField.Harvest();

        assignedField.IsDoing = false;
        assignedField = null;
        worker.HideProgress();
        worker.ChangeState(new IdleState());


    }

    public void Exit(Worker worker)
    {
        worker.Instance.Data.isWorking = false;
    }
}
