namespace Infrastructure.Interfaces
{
    public interface IFilter
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}
