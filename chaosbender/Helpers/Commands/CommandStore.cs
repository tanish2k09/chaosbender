using Chaosbender.Data;
using System.Collections;
using System.Threading.Tasks;

namespace Chaosbender.Helpers.Commands
{ 
  class CommandStore
  {
    // TODO: Aliases are server-specific. A single static data point won't scale properly. Make them specific.
    private static Hashtable Aliases = new Hashtable();

    // Maintain a duplicate hashtable for commands, purely for O(1) search
    // The value of key isn't of significance in this table
    // TODO: Convert this Hashtable to a database table
    private static Hashtable Commands = new Hashtable()
    {
      {"alias", "alias" },
      {"ban", "ban" }
    };

    /* GetActualCommand logic:
     *
     * Commands might be aliases, a direct command, or an unidentified trigger
     * If it's an alias, find the direct command for it
     * If it's not an alias, return the trigger as-is
     * It is CommandHandler's responsibility to filter unidentified triggers in switches
     * CommandHandler should also handle reading the custom commands for appropriate perm level
     * from the database
     */
    public static string GetActualCommand(string command)
    {
      if (Aliases.ContainsKey(command)) // TODO: CRUD from db
      {
        return (string) Aliases[command];
      }

      return command;
    }

    public static async Task<string> Link(string alias, string command)
    {
      if (!Commands.ContainsKey(command))
        return Strings.CommandDNE;

      if (Aliases.ContainsKey(alias))
        return Strings.ErrorAliasLinked;

      Aliases.Add(alias, command);

      await Task.CompletedTask;
      return "The alias \"" + alias + "\" is now linked to the command \"" + command + "\""; 
    }
  }
}
