using Chaosbender.Data;
using System.Collections;
using System.Threading.Tasks;

namespace Chaosbender.Helpers.Commands
{ 
  class CommandStore
  {
    // TODO: Aliases are server-specific. A single static data point won't scale properly. Make them specific.
    private static Hashtable Aliases = new Hashtable() 
    {
    };

    // Maintain a duplicate hashtable for commands, purely for O(1) search
    // The value of key isn't of significance in this table
    private static Hashtable Commands = new Hashtable()
    {
      {"alias", "alias" },
      {"ban", "ban" }
    };

    public static string GetActualCommand(string command)
    {
      if (Commands.ContainsKey(command))
      {
        return command;
      }
      else if (Aliases.ContainsKey(command)) 
      {
        return (string) Aliases[command];
      }

      return null;
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
