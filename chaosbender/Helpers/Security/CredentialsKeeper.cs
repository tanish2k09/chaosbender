using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Chaosbender.Helpers.Security
{
  class CredentialsKeeper
  {
    public static string Token { get; private set; }
    public static string SelfDB { get; private set; }
    public static ulong DevID { get; private set; }
    public static ulong SelfID { get; private set; }
    public static string MongoConnection { get; private set; }

    // TODO: Allow custom prefixes guild-wise
    public static string Prefix { get; private set; } = "t;";

    // This struct might show warnings about no initialized value
    // It is assigned by the JSON read operation in ReadCreds()
#pragma warning disable 0649
    private struct CredsJson
    {
      [JsonProperty("Token")]
      public string Token;

      [JsonProperty("DevID")]
      public ulong DevID;

      [JsonProperty("SelfID")]
      public ulong SelfID;

      [JsonProperty("SelfDB")]
      public string SelfDB;

      [JsonProperty("MongoConnection")]
      public string MongoConnection;
    }
#pragma warning restore 0649

    public static bool IsDev(ulong id)
    {
      return id == DevID;
    }

    public static bool IsAdmin(ulong id)
    {
      return IsDev(id); // || other admins <- TODO
    }

    public static bool IsOperator(ulong id)
    {
      return true; // admins or actual operators <- TODO
    }

    public static async Task<bool> ReadCreds(string path)
    {
      // Read credentials as Token and DevID into a struct object from creds.json
      string info = "";
      using (FileStream fs = File.OpenRead(path))
      using (StreamReader sr = new StreamReader(fs))
        info = await sr.ReadToEndAsync();

      CredsJson creds = JsonConvert.DeserializeObject<CredsJson>(info);
      DevID = creds.DevID;
      Token = creds.Token;
      SelfID = creds.SelfID;
      SelfDB = creds.SelfDB;
      MongoConnection = creds.MongoConnection;
      return true;
    }

    public static void WipeToken()
    {
      Token = "";
    }

    public static bool IsSelf(ulong id)
    {
      return id == SelfID;
    }
  }
}
