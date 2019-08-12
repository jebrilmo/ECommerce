namespace Infrastructure.Interfaces.Operations
{
    public interface ISearchOperation<Filter> where Filter: IFilter
    {
        IDTO Execute(Filter filter);
    }
}
