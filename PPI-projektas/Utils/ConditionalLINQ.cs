namespace PPI_projektas.Utils;

public static class ConditionalLINQ
{
    public static IQueryable<T> If<T>(this IQueryable<T> query, bool condition, params Func<IQueryable<T>, IQueryable<T>>[] transforms)
    {
        return condition
            ? transforms.Aggregate(query, (current, transform) => transform.Invoke(current))
            : query;
    }

    public static IEnumerable<T> If<T>(this IEnumerable<T> query, bool condition, params Func<IEnumerable<T>, IEnumerable<T>>[] transforms)
    {
        return condition
            ? transforms.Aggregate(query, (current, transform) => transform.Invoke(current))
            : query;
    }
}