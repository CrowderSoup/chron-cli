namespace chron_cli
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Security.Principal;
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
		private string Editor { get; }

		public ChronCLI(CLIArguments args)
		{
			this.Message = args.Message;
			this.HomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			this.Editor = Environment.GetEnvironmentVariable("EDITOR");

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

			// Create the chron Directory if it doesn't exist
			Directory.CreateDirectory(this.chronDirectory);

			if (settings["chron"]["log_file"] == null)
			{
				this.LogFile = this._LogFile;
			}
			else
			{
				this.LogFile = settings["chron"]["log_file"];
			}

			// If the value from the environment is null try and get it from settings
			if (this.Editor == null && settings["editor"]["command"] != null)
			{
				this.Editor = settings["editor"]["command"];
			}
		}

		public void Run()
		{
			if (this.Message == string.Empty)
			{
				this.WriteEntry();

				if (File.Exists(@"Templates\LongEntry.txt"))
				{
					var entry = File.ReadAllText(@"Templates\LongEntry.txt");
					File.WriteAllText(@"Templates\LongEntry.txt", string.Empty);

					Console.WriteLine(entry);
				}
			}
			else
			{
				var logFilePath = $"{this.chronDirectory}/{this.LogFile}";
				var lineText = $"{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}\t{this.Message}\r\n";

				this.LogMessage(logFilePath, lineText);
			}
		}

		#region Private Methods

		private void LogMessage(string logFilePath, string lineText)
		{
			if (!File.Exists(logFilePath))
			{
				using (var streamWriter = File.CreateText(logFilePath))
				{
					streamWriter.WriteLine(lineText);
				}
			}
			else
			{
				using (var streamWriter = File.AppendText(logFilePath))
				{
					streamWriter.WriteLine(lineText);
				}
			}
		}

		private void WriteEntry()
		{
			Console.WriteLine(WindowsIdentity.GetCurrent().Name);
			Console.ReadLine();

			var process = Process.Start(new ProcessStartInfo(this.Editor, @"Templates\LongEntry.txt")
			{
				UseShellExecute = false,
				LoadUserProfile = true
			});
			process.WaitForExit();
		}

		#endregion
	}
}
