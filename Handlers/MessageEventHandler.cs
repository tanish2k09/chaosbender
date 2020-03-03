using System;
using System.Threading.Tasks;

namespace Chaosbender.Handlers
{
	public class MessageEventHandler
	{
		public MessageEventHandler() { }

		public static async Task HandleCreation(DSharpPlus.Entities.DiscordMessage message)
		{
			// TODO: Implement EnforcementHandler to enforce some policies before execution

			await CommandHandler.Execute(message);
		}
	}
}
