namespace IcucApp.Core
{
    public interface IMapper<in TSource, out TDest>
    {
        TDest Map(TSource source);
    }
}