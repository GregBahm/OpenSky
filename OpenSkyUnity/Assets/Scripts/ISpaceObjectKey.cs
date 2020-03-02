public interface ISpaceObjectKey<T>
{
    T LerpWith(T nextKey, float param);
}