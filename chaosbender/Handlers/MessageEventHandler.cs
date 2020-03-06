using System.Threading.Tasks;

namespace Chaosbender.Handlers
{
	public class MessageEventHandler
	{
		public MessageEventHandler() { }

		public static async Task HandleCreation(DSharpPlus.Entities.DiscordMessage message)
		{
			// TODO: Enforce some basic checks before doing anything
			if (!EnforcementHandler.EnforceInitial(message))
				return;

			await CommandHandler.Execute(message);
		}
	}
}
