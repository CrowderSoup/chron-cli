﻿namespace chron_cli
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
		}

		public void Run()
		{
			if (this.Message == string.Empty)
			{
				Console.WriteLine("No Message");
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

		#endregion
	}
}
