namespace ToDo.Cache.Abstractions
{
    public interface ICacheIO<TCacheType, THashType> : ICacheReader<TCacheType, THashType>, ICacheWriter<TCacheType, THashType>
    {
    }
}
