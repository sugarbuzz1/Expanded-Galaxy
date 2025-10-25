using PulsarModLoader.Chat.Commands.CommandRouter;
using PulsarModLoader.Utilities;
using System.Text;

namespace ExpandedGalaxy
{
    internal class ExGalCommand : ChatCommand
    {
        public override string[] CommandAliases() => new string[1]
        {
            "ExGal"
        };

        public override string Description() => "Expanded Galaxy Commands";

        public override string[][] Arguments() => new string[1][]
        {
            new string[1]
            {
                "log"
            }
        };

        public override void Execute(string arguments)
        {
            if ((object)PLServer.Instance == (object)null)
                return;

            string[] args = arguments.Split(' ');
            switch (args[0].ToLower())
            {
                case "log":
                    if (CrewLogManager.Instance.LogIndex != int.MinValue)
                        Messaging.Notification("Not on log creation screen!");
                    else
                    {
                        StringBuilder builder = new StringBuilder();
                        for (int i = 1; i < args.Length; i++)
                        {
                            builder.Append(args[i]);
                            if (i < args.Length - 1)
                                builder.Append(' ');
                        }
                        if (!(builder.Length > 0))
                            builder.Append(string.Empty);
                        CrewLogData data = CrewLogManager.Instance.TempData;
                        data.Text = builder.ToString();
                        CrewLogManager.Instance.TempData = data;
                        Messaging.Notification("Log Updated!");
                    }
                    break;
                default:
                    Messaging.Notification("Unknown Command!");
                    break;
            }
        }
    }
}
