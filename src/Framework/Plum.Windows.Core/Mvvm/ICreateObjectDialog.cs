namespace Plum.Windows.Mvvm
{
    public interface ICreateObjectDialog<T> where T : new()
    {
        T Object { get; set; }
    }
}