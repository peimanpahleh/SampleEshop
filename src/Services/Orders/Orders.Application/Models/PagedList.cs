namespace Orders.Application.Models;

public class PagedList<TEntity> where TEntity : class
{
    public long TotalItems { get; set; }
    public int TotalPages { get; set; }
    public IReadOnlyList<TEntity> Result { get; set; }
}
