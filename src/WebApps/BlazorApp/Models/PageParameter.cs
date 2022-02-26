namespace BlazorApp.Models;

public class PageParameter
{
    public string Query { get; set; }

    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string SortField { get; set; }

    public string SortOrder { get; set; }
}
