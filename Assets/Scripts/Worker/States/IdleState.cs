using System.Linq;
using UnityEngine;

public class IdleState : IWorkerStates
{
    private float searchCooldown = 0f;
    private float searchDelay = 0.3f;
    public void Enter(Worker worker)
    {
        worker.targetTask = null;
        
    }

    public void Execute(Worker worker)
    {

        //searchCooldown += Time.time;

        //if (searchCooldown < searchDelay)
        //{
        //    return;
        //}
        
        //searchCooldown = 0f;

        var allWorkables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<ITask>().Where(task => !task.IsDoing && task.IsWorkable() && task.CanWork(worker));
        
        var targetTask = allWorkables.FirstOrDefault();

        if (targetTask != null)
        {
            targetTask.IsDoing = true;
            worker.targetTask = targetTask;

            worker.ChangeState(new MovingState(targetTask));            
        }
        else
        {
            worker.ChangeState(new MovingState(worker.BackToStation()));
        }
    }

    public void Exit(Worker worker)
    {
    }
}
