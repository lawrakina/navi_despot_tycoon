namespace NavySpade.Common.Runtime.Api
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<in T>
    {
        void Execute(T argument);
    }
}