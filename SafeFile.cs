/*
 * Writes a file without putting the existing file at risk.
 */
using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UyghurEditPP.Tests")]

namespace UyghurEditPP
{
	public static class SafeFile
	{
		// Seams for the tests: the file operations and the error log.
		internal static Action<string, string, string> ReplaceFile = (source, destination, backup) => File.Replace(source, destination, backup, true);
		internal static Action<string, string> MoveFile = File.Move;
		internal static Action<Exception> Log = ee => ErrorLog.Write(ee);

		/// <summary>
		/// Lets <paramref name="write"/> write the whole content to a temporary file in
		/// the same folder, and only then puts it in place of <paramref name="fileName"/>.
		/// The temporary file is removed only once the new content is in place, or when
		/// the existing file has not been touched; otherwise it is kept and its name is
		/// given in the exception.
		/// </summary>
		public static void Write(string fileName, Action<Stream> write)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");
			if (write == null)
				throw new ArgumentNullException("write");

			string fullName = Path.GetFullPath(fileName);
			string folder = Path.GetDirectoryName(fullName);
			string prefix = Path.Combine(folder, "." + Path.GetFileName(fullName) + "." + Guid.NewGuid().ToString("N").Substring(0, 8));
			string tempName = prefix + ".tmp";
			string backupName = prefix + ".bak";

			try{
				using (FileStream fs = new FileStream(tempName, FileMode.CreateNew, FileAccess.Write, FileShare.None)) {
					write(fs);
					fs.Flush(true);
				}
			}
			catch{
				DeleteQuietly(tempName);
				throw;
			}

			if(!File.Exists(fullName)){
				File.Move(tempName, fullName);
				return;
			}

			try{
				// Keeps the existing file's attributes and permissions.
				ReplaceFile(tempName, fullName, backupName);
			}
			catch(UnauthorizedAccessException){
				// Read-only file or no permission: the existing file was not touched.
				DeleteQuietly(tempName);
				throw;
			}
			catch(Exception ee){
				Recover(fullName, tempName, backupName, ee);
				return;
			}
			DeleteQuietly(backupName);
		}

		// ReplaceFileW (used by File.Replace) can fail half way. With a backup name given,
		// the documented outcomes are: both files keep their names (ERROR_UNABLE_TO_MOVE_REPLACEMENT,
		// ERROR_UNABLE_TO_REMOVE_REPLACED and any other error), or the original has already been
		// renamed to the backup name and the new content is still under the temporary name
		// (ERROR_UNABLE_TO_MOVE_REPLACEMENT_2).
		// https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-replacefilew
		static void Recover(string fullName, string tempName, string backupName, Exception replaceError)
		{
			Log(replaceError);
			if(!File.Exists(fullName)){
				// The original is only under the backup name: finish the job.
				try{
					MoveFile(tempName, fullName);
				}
				catch(Exception moveError){
					// Whatever blocked the replace (for example a virus scanner holding the
					// temporary file) may still be there. Put the original back instead.
					Log(moveError);
					try{
						MoveFile(backupName, fullName);
					}
					catch(Exception restoreError){
						Log(restoreError);
						throw new IOException("The file could not be saved. The original content is in " + backupName + " and the new content is in " + tempName, moveError);
					}
					DeleteQuietly(tempName);
					throw new IOException("The file could not be saved; it was left unchanged.", moveError);
				}
				DeleteQuietly(backupName);
				return;
			}

			// The original is untouched. Some file systems (for example some network drives)
			// do not support replacing; copy the new content over the file instead, as files
			// were saved before. The temporary file stays until the copy has succeeded.
			FileInfo before = new FileInfo(fullName);
			long length = before.Length;
			DateTime written = before.LastWriteTimeUtc;
			try{
				File.Copy(tempName, fullName, true);
			}
			catch(Exception copyError){
				FileInfo after = new FileInfo(fullName);
				if(after.Exists && after.Length == length && after.LastWriteTimeUtc == written){
					// The copy did not start, so the original is still complete.
					DeleteQuietly(tempName);
					throw;
				}
				throw new IOException("The file could not be saved completely. The new content is kept in " + tempName, copyError);
			}
			DeleteQuietly(tempName);
			DeleteQuietly(backupName);
		}

		static void DeleteQuietly(string fileName)
		{
			try{
				if(File.Exists(fileName)){
					File.Delete(fileName);
				}
			}
			catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee.Message);
			}
		}
	}
}
