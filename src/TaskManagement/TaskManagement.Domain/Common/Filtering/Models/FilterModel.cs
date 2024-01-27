namespace TaskManagement.Domain.Common.Filtering.Models;

public class FilterModel
{
    public int PageSize { get; set; } = 10;

    public int PageToken { get; set; } = 1;
}
