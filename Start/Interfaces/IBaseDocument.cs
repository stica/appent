namespace Start.Common.Interfaces
{
    public interface IBaseDocument<T>
    {
        string JSONDocument { get; set; }

        T Document { get; set; }
    }
}
