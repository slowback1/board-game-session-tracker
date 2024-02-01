using Database.Common.Storers;

namespace Database.Common.Test.Storers;

public abstract class BaseDbTest
{
    protected TestDataStorer DataStorer = new TestDataStorer();
}