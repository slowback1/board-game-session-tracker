namespace Database.MySql.Test;

public abstract class BaseDbTest
{
    protected TestContext _context { get; private set; }
    protected MySqlDataStorer _repository { get; private set; }

    [SetUp]
    public void SetUpContext()
    {
        _context = new TestContext();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _repository = new MySqlDataStorer(_context);
    }
}