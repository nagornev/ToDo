namespace ToDo.Microservices.Cache
{
    public interface ICacheIO<TCacheType, THashType> : ICacheReader<TCacheType, THashType>, ICacheWriter<TCacheType, THashType>
    {
    }
}
