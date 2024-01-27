using TaskManagement.Domain.Common.Filtering.Models;

namespace TaskManagement.Domain.Common.Filtering.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, FilterModel filterModel)
        => source.Skip((filterModel.PageToken - 1) * filterModel.PageSize).Take(filterModel.PageSize);

    public static IEnumerable<T> ApplyPagination<T>(this IEnumerable<T> source, FilterModel filterModel)
        => source.Skip((filterModel.PageToken - 1) * filterModel.PageSize).Take(filterModel.PageSize);
}
