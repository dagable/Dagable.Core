namespace Dagable.Core
{
    public interface IDagCreation<T>
    {
        string AsJson();
        T Generate();
    }
}