namespace chron_cli
{
	using Fclp;
	using Objects;

	class Program
	{
		static void Main(string[] args)
		{
			var parser = new FluentCommandLineParser<CLIArguments>();

			parser.Setup(arg => arg.Message)
				.As('m', "message")
				.SetDefault(string.Empty);

			parser.Setup(arg => arg.SettingsFile)
				.As('s', "settings")
				.SetDefault(string.Empty);

			var result = parser.Parse(args);
			if (result.HasErrors == false)
			{
				var chron = new ChronCLI(parser.Object);
				chron.Run();
			}
		}
	}
}
