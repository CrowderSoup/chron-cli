namespace chron_cli
{
	using System;
	using System.IO;
	using IniParser;
	using Objects;

	public class ChronCLI
	{
		private string _SettingsFile
		{
			get { return ".chron-cli"; }
		}
		private string _chronDirectory
		{
			get { return Path.Combine(this.HomePath, "chron"); }
		}
		private string _LogFile
		{
			get { return "log.txt"; }
		}

		private string Message { get; }
		private string HomePath { get; }
		private string SettingsFilePath { get; }
		private string chronDirectory { get; }
		private string LogFile { get; }

		public ChronCLI(CLIArguments args)
		{
			this.Message = args.Message;
			this.HomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

			if (args.SettingsFile == string.Empty)
			{
				this.SettingsFilePath = Path.Combine(this.HomePath, this._SettingsFile);
			}
			else
			{
				this.SettingsFilePath = Path.Combine(this.HomePath, args.SettingsFile);
			}

			var iniParser = new FileIniDataParser();
			var settings = iniParser.ReadFile(this.SettingsFilePath);

			if (settings["chron"]["directory"] == null)
			{
				this.chronDirectory = this._chronDirectory;
			}
			else
			{
				this.chronDirectory = settings["chron"]["directory"];
			}

			if (settings["chron"]["log_file"] == null)
			{
				this.LogFile = this._LogFile;
			}
			else
			{
				this.LogFile = settings["chron"]["log_file"];
			}
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
