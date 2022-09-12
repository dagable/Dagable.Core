namespace Dagable.Core
{
    public interface IDagCreation<T>
    {
        string AsJson();
        T Generate();
        T Setup(int layers, int nodeCount, double probability);
    }
}