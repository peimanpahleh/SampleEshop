namespace BlazorApp.Models;

public class PagedList<TEntity> where TEntity : class
{
    public long TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<TEntity> Result { get; set; } = new();
}
