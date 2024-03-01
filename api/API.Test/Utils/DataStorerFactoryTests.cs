using API.Config;
using API.Utils;
using Database.MySql;
using Database.Supabase;

namespace API.Test.Utils;

public class DataStorerFactoryTests
{
    [Test]
    [TestCase("Supabase", typeof(SupabaseStorer))]
    [TestCase("EntityFramework", typeof(MySqlDataStorer))]
    public void TheGivenUseKeywordGeneratesTheCorrectTypeOfDataStorer(string key, Type expectedType)
    {
        var factory = new DataStorerFactory(new DataStorerConfig { Use = key });
        factory.SupabaseConfig = new SupabaseConfig { Key = "key", Url = "url" };
        factory.DatabaseConfig = new DatabaseConfig { ConnectionString = "connection_string" };

        var storer = factory.GetDataStorerFromConfig();

        Assert.That(storer, Is.TypeOf(expectedType));
    }

    [Test]
    public void FactoryThrowsAnInvalidOperationExceptionIfGivenInvalidConfig()
    {
        var factory = new DataStorerFactory(new DataStorerConfig { Use = "invalid" });

        Assert.Throws<InvalidOperationException>(() => factory.GetDataStorerFromConfig());
    }
}