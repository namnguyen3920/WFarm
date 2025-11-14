using UnityEngine;

public interface IWorkerStates
{
    void Enter(Worker worker);
    void Execute(Worker worker);
    void Exit(Worker worker);
}