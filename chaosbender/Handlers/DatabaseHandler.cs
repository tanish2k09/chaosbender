using Chaosbender.Helpers.Security;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Chaosbender.Handlers.Data
{
  class DatabaseHandler
  {
    private static MongoClient dbClient = new MongoClient(CredentialsKeeper.MongoConnection);

    public static async Task InitDB()
    {
      var cursor = await dbClient.ListDatabasesAsync();
      await cursor.ForEachAsync(db => Console.WriteLine(db["name"]));
    }
  }
}
