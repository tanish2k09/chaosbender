using System;
using System.Threading.Tasks;
using Chaosbender.Data;
using Chaosbender.Handlers;
using Chaosbender.Helpers.Security;
using DSharpPlus;

namespace Chaosbender
{
  class Program
  {
    private static DiscordClient discord;

    static void Main(string[] args)
    {
      MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private static async Task MainAsync(string[] args)
    {
      await CredentialsKeeper.ReadCreds();

      discord = new DiscordClient(new DiscordConfiguration
      {
        Token = CredentialsKeeper.getToken(),
        TokenType = TokenType.Bot
      });

      discord.MessageCreated += async e =>
      {
        if (e.Message.Content.StartsWith(CredentialsKeeper.getPrefix()))
          await MessageEventHandler.HandleCreation(e.Message);
      };

      await discord.ConnectAsync();

      Console.WriteLine(Strings.Ready);
      await Task.Delay(-1);
    }
  }
}
