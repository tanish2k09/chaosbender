using Chaosbender.Helpers.Security;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Chaosbender.Helpers.Commands
{
  class ArgumentRenderer
  {
    public static async Task<string> Command(string content)
    {
      StringBuilder builder = new StringBuilder();

      for (int index = CredentialsKeeper.getPrefix().Length;
          index < content.Length;
          ++index)
      {
        if (content[index] == ' ')
          break;

        builder.Append(content[index]);
      }

      await Task.CompletedTask;
      return builder.ToString();
    }


    /* Render method:
     * 1) Provide the command as first word
     * 2) Provide space-separated words in order
     *
     * TODO: Render codeblock flags and do sanity checks
     */
     // TODO: Render with REGEX

    public static async Task<string[]> Split(string content, string part)
    {
      /* Get arguments as simple unprocessed words
       * 
       * 1) Convert to lower case
       * 2) Remove any *extra* whitespaces
       * 3) Split on spaces
       * 4) Return words
       */
      return (await InnerTrim(content.ToLower())).Split(part);
    }

    // TODO: Remove extra whitespaces, inside and surrounding the strings
    private static async Task<string> InnerTrim(string content)
    {
      // Trim the surroundings
      content = content.Trim();

      // Trim inner extra whitespaces with StringBuilder instead of REGEX
      // For small strings for better performance
      StringBuilder builder = new StringBuilder();
      Boolean spaced = false;

      for (int index = 0; index < content.Length; ++index)
      {
        if (content[index] == ' ')
        {
          // Make sure to carry over 1 space for all spaced substrings
          if (spaced == true)
            continue;

          spaced = true;
        }

        spaced = false;
        builder.Append(content[index]);
      }

      await Task.CompletedTask;
      return builder.ToString();
    }
  }
}
