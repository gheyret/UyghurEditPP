/*
 * Where UyghurEdit++ keeps its own files (settings, user dictionaries, logs).
 *
 * The program folder may be read-only (for example under Program Files), so
 * these files live in %AppData%\UyghurEditPP. Files left in the program folder
 * by older versions are copied there once.
 */
using System;
using System.IO;
using System.Text;

namespace UyghurEditPP
{
	public static class AppPaths
	{
		public const string ConfigFileName      = "uyghuredit.cfg";
		public const string IshletkuchiFileName = "imla_ishletkuchi.txt";
		public const string XataToghraFileName  = "imla_xatatoghra.txt";

		static readonly string[] gKonaHojjetler = { ConfigFileName, IshletkuchiFileName, XataToghraFileName };

		/// <summary>
		/// %AppData%\UyghurEditPP
		/// </summary>
		public static string DataFolder{
			get{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UyghurEditPP");
			}
		}

		public static string LogFolder{
			get{
				return Path.Combine(DataFolder, "logs");
			}
		}

		public static string ProgramFolder{
			get{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		public static string DataFile(string fileName)
		{
			return Path.Combine(DataFolder, fileName);
		}

		/// <summary>
		/// Creates the data folder and copies settings and user dictionaries that
		/// older versions saved next to the program. Never throws; failures are logged.
		/// </summary>
		public static void Prepare()
		{
			try{
				Directory.CreateDirectory(DataFolder);
			}
			catch(Exception ee){
				ErrorLog.Write(ee);
				return;
			}
			foreach(string name in gKonaHojjetler){
				try{
					MigrateFile(name, ProgramFolder, DataFolder);
				}
				catch(Exception ee){
					ErrorLog.Write(ee);
				}
			}
		}

		/// <summary>
		/// Copies oldFolder\fileName to newFolder\fileName when the new file does not
		/// exist yet and the old one does. The old file is left in place.
		/// Returns true when a file was copied.
		/// </summary>
		public static bool MigrateFile(string fileName, string oldFolder, string newFolder)
		{
			string oldPath = Path.Combine(oldFolder, fileName);
			string newPath = Path.Combine(newFolder, fileName);
			if(File.Exists(newPath) || !File.Exists(oldPath)){
				return false;
			}
			if(string.Equals(Path.GetFullPath(oldPath), Path.GetFullPath(newPath), StringComparison.OrdinalIgnoreCase)){
				return false;
			}
			Directory.CreateDirectory(newFolder);
			File.Copy(oldPath, newPath, false);
			return true;
		}
	}

	/// <summary>
	/// Appends errors to %AppData%\UyghurEditPP\logs\error-yyyyMMdd.log.
	/// </summary>
	public static class ErrorLog
	{
		static readonly object gLock = new object();

		public static string FileName{
			get{
				return Path.Combine(AppPaths.LogFolder, "error-" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + ".log");
			}
		}

		/// <summary>
		/// Writes the exception to the log file. Never throws.
		/// Returns the log file name, or null when it could not be written.
		/// </summary>
		public static string Write(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
			try{
				lock(gLock){
					string fileName = FileName;
					Directory.CreateDirectory(Path.GetDirectoryName(fileName));
					string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + Environment.NewLine + ex + Environment.NewLine + Environment.NewLine;
					File.AppendAllText(fileName, text, Encoding.UTF8);
					return fileName;
				}
			}
			catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee);
				return null;
			}
		}
	}
}
