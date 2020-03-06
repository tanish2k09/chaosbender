namespace Chaosbender.Handlers
{
  class EnforcementHandler
  {
    public static bool EnforceInitial(DSharpPlus.Entities.DiscordMessage message)
    {
      // Don't listen to bot input
      // TODO: Make this configurable -> Bot allowed? Self allowed?
      if (message.Author.IsBot)
        return false;

      // TODO: Check for guild's permission allowance. Operator access might be locked.

      return true;
    }
  }
}
