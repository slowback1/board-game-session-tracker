using Database.Common.Storers;

namespace Database.MySql;

public partial class MySqlDataStorer : IDataStorer
{
    private readonly ApplicationDbContext _context;

    public MySqlDataStorer(ApplicationDbContext context)
    {
        _context = context;
    }
}