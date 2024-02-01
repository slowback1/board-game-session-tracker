namespace Database.Common.Storers;

public abstract class DbRepository 
{
    protected IDataStorer _storer { get; private set; }

    protected DbRepository(IDataStorer storer)
    {
        _storer = storer;
    }
}