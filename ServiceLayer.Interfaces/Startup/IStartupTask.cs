namespace ServiceLayer.Interfaces.Startup
{
    public interface IStartupTask
    {
        int Order { get; }
        void Execute();
    }
}
