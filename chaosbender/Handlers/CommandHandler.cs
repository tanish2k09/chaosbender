using Chaosbender.Data;
using Chaosbender.Helpers.Commands;
using Chaosbender.Helpers.Security;
using System.Threading.Tasks;

namespace Chaosbender.Handlers
{
  class CommandHandler
  {
    public static async Task Execute(DSharpPlus.Entities.DiscordMessage message)
    {
      /* Execute is a generic method to streamline all perm types.
       * Choose the proper method to use
       * Use the user perms to trigger a method instead of triggering a method and checking perms
       * Reduces permission-checks' code length and increases maintainability
       * 
       * NOTE: Execute functions cascade down the permissions ladder until a command hit is found
       */

      string command = await ArgumentRenderer.Command(message.Content);
      command = CommandStore.GetActualCommand(command);

      if (CredentialsKeeper.IsDev(message.Author.Id))
        await ExecuteDev(message, command);
      else if (CredentialsKeeper.IsAdmin(message.Author.Id))
        await ExecuteAdmin(message, command);
      else if (CredentialsKeeper.IsOperator(message.Author.Id))
        await ExecuteOperator(message, command);
      else
        await ExecutePublic(message, command);
    }

    /* All commands executable by the developers only will be listed here */
    private static async Task ExecuteDev(DSharpPlus.Entities.DiscordMessage message, string command)
    {
      switch(command)
      {
        default:
          await ExecuteAdmin(message, command);
          break;
      }
    }

    /* All commands executable by admins will be listed here */
    private static async Task ExecuteAdmin(DSharpPlus.Entities.DiscordMessage message, string command)
    {
      switch (command)
      {
        case "alias":
          await SetupAlias(message);
          break;

        case "ban":
          await Ban(message);
          break;

        default:
          await ExecuteOperator(message, command);
          break;
      }
    }

    /* All commands executable by operators will be listed here */
    private static async Task ExecuteOperator(DSharpPlus.Entities.DiscordMessage message, string command)
    {
      switch (command)
      {
        default:
          await ExecutePublic(message, command);
          break;
      }
    }

    /* All commands executable by most users will be listed here */
    private static async Task ExecutePublic(DSharpPlus.Entities.DiscordMessage message, string command)
    {
      switch (command)
      {
        default:
          await Reply(message, Strings.Noop);
          break;
      }
    }

    public static async Task Reply(DSharpPlus.Entities.DiscordMessage message, string response)
    {
      if (response.Length > 0)
        await message.RespondAsync(response);
    }

    // TODO: Make bans robust enough to see if we wanna ban a role, a member, multiple members.
    // TODO: Check perms and stuff
    public static async Task Ban(DSharpPlus.Entities.DiscordMessage message)
    {
      await message.Channel.Guild.BanMemberAsync(message.MentionedUsers[0].Id, 0, "Testing...");
      await Reply(message, Strings.Banned);
    }

    private static async Task SetupAlias(DSharpPlus.Entities.DiscordMessage message)
    {
      // Make sure there are exactly 2 parts, an alias and the command.
      if (
        (await ArgumentRenderer.Split(message.Content, " as "))
        .Length != 2)
      {
        await Reply(message, Strings.WrongSyntax);
        return;
      }

      // The first part also contains "<prefix><alias>" word, strip that
      string[] parts = await ArgumentRenderer.Split(message.Content, " ");
      if (parts.Length != 4)
      {
        await Reply(message, Strings.WordsNotSentences);
        return;
      }

      await Reply(message, await CommandStore.Link(parts[1], parts[3]));
    }
  }
}
