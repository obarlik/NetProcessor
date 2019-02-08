namespace BPU
{
    public enum ProcessingStatus
    {
        Ready,
        AwaitingToRun,
        Running,
        Paused,
        Halted,
        Error,
        Finished,
    }
}