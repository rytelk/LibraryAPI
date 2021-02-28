namespace Library.Infrastructure.Services
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
    }
}