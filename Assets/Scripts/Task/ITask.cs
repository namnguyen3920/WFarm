using UnityEngine;

public interface ITask
{
    bool IsWorkable();

    bool IsDoing { get; set; }

    void DoWork(Worker worker);

    bool CanWork(Worker worker);

    Vector3 GetPosition();
}
