using System;
using System.Threading.Tasks;
using Chaosbender.Handlers.Data;
using Chaosbender.Data;
using Chaosbender.Handlers;
using Chaosbender.Helpers.Security;
using DSharpPlus;

namespace Chaosbender
{
  class Program
  {
    private static DiscordShardedClient discord;

    static void Main(string[] args)
    {
      MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private static async Task MainAsync(string[] args)
    {
      await CredentialsKeeper.ReadCreds(args[0]);
      await DatabaseHandler.InitDB();

      discord = new DiscordShardedClient(new DiscordConfiguration
      {
        Token = CredentialsKeeper.Token,
        TokenType = TokenType.Bot
      });

      CredentialsKeeper.WipeToken();

      discord.MessageCreated += async e =>
      {
        if (e.Message.Content.StartsWith(CredentialsKeeper.Prefix))
          await MessageEventHandler.HandleCreation(e.Message);
      };

      await discord.ConnectAsync();

      Console.WriteLine(Strings.Ready);
      await Task.Delay(-1);
    }
  }
}
