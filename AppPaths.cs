/*
 * Where UyghurEdit++ keeps its own files (settings, user dictionaries, logs).
 *
 * The program folder may be read-only (for example under Program Files), so
 * these files live in %AppData%\UyghurEditPP.
 */
using System;
using System.IO;
using System.Text;

namespace UyghurEditPP
{
	public static class AppPaths
	{
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
