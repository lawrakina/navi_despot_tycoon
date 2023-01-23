namespace NavySpade.Common.Runtime.Api
{
    public interface ICondition
    {
        bool IsMet();
    }

    public interface ICondition<in T>
    {
        bool IsMet<T>();
    }
}