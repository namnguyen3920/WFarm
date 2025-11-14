using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MovingState : IWorkerStates
{
    private ITask assignedTask;
    private Vector3 targetPosition;
    public MovingState(Vector3 position)
    {
        targetPosition = position;
    }
    public MovingState(ITask task)
    {
        assignedTask = task;
        if (task != null)
            targetPosition = task.GetPosition();        
    }
    public void Enter(Worker worker)
    {
    }

    public void Execute(Worker worker)
    {

        if (assignedTask == null || !assignedTask.IsWorkable() || !assignedTask.CanWork(worker))
        {
            MoveToTarget(worker);
            assignedTask = null;
            worker.targetTask = null;
            worker.ChangeState(new IdleState());
            return;
        }

        if (assignedTask != null)
        {
            MoveToTarget(worker);
            if (Vector3.Distance(worker.transform.position, targetPosition) < 0.1f)
            {
                assignedTask.DoWork(worker);
            }
            return;
        }

        worker.ChangeState(new IdleState());


    }

    public void Exit(Worker worker)
    {
    }

    private void MoveToTarget(Worker worker)
    {
        worker.transform.position = Vector3.MoveTowards(
                worker.transform.position,
                targetPosition,
                worker.Instance.Data.moveSpeed
            );
    }
}
