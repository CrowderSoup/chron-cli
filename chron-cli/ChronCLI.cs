namespace chron_cli
{
	using System;
	using Objects;

	public class ChronCLI
	{
		private string Message { get; }

		public ChronCLI(CLIArguments args)
		{
			this.Message = args.Message;
		}

		public void Run()
		{
			if (this.Message == string.Empty)
			{
				Console.WriteLine("No Message");
			}
			else
			{
				Console.WriteLine(this.Message);
			}

			Console.ReadLine();
		}
	}
}
