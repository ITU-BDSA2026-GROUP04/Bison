namespace SimpleDB;
public interface IDatabaseRepository<T> // added "public", because without it it's only visible within the SimpleDB project. 
{
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
}