
public class WorkerInstance
{
    public WorkerData Data { get; private set; }
    public Worker Worker;
    public float ActionTimer = 0f;

    public WorkerInstance(WorkerData data, Worker worker)
    {
        this.Data = data;
        this.Worker = worker;
        this.ActionTimer = 0f;
    }
    
}
