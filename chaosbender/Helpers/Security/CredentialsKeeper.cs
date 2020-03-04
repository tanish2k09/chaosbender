using Chaosbender.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Chaosbender.Helpers.Security
{
  // TODO: Make credentials CRUD-able
  class CredentialsKeeper
  {
    private static ulong DevID;
    private static string Prefix = "t;";
    private static string Token;

    // This struct might show warnings about no initialized value
    // It is assigned by the JSON read operation in ReadCreds()
    private struct CredsJson
    {
      [JsonProperty("Token")]
      public string Token;

      [JsonProperty("DevID")]
      public ulong DevID;
    }

    public static string getPrefix()
    {
      return Prefix;
    }

    public static string getToken()
    {
      return Token;
    }

    public static Boolean IsDev(ulong id)
    {
      return id == DevID;
    }

    public static Boolean IsAdmin(ulong id)
    {
      return IsDev(id); // || other admins <- TODO
    }

    public static Boolean IsOperator(ulong id)
    {
      return true; // admins or actual operators <- TODO
    }

    public static async Task<Boolean> ReadCreds()
    {
      // Read credentials as Token and DevID into a struct object from creds.json
      string info = "";
      using (FileStream fs = File.OpenRead(Strings.CredsPath))
      using (StreamReader sr = new StreamReader(fs))
        info = await sr.ReadToEndAsync();

      CredsJson creds = JsonConvert.DeserializeObject<CredsJson>(info);
      DevID = creds.DevID;
      Token = creds.Token;
      return true;
    }
  }
}
