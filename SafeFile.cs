/*
 * Writes a file without putting the existing file at risk.
 */
using System;
using System.IO;

namespace UyghurEditPP
{
	public static class SafeFile
	{
		/// <summary>
		/// Lets <paramref name="write"/> write the whole content to a temporary file in
		/// the same folder, and only then puts it in place of <paramref name="fileName"/>.
		/// If anything fails, the existing file is left as it was and the exception is
		/// passed on.
		/// </summary>
		public static void Write(string fileName, Action<Stream> write)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");
			if (write == null)
				throw new ArgumentNullException("write");

			string fullName = Path.GetFullPath(fileName);
			string folder = Path.GetDirectoryName(fullName);
			string tempName = Path.Combine(folder, "." + Path.GetFileName(fullName) + "." + Guid.NewGuid().ToString("N").Substring(0, 8) + ".tmp");
			try{
				using (FileStream fs = new FileStream(tempName, FileMode.CreateNew, FileAccess.Write, FileShare.None)) {
					write(fs);
					fs.Flush(true);
				}
				if(File.Exists(fullName)){
					// Keeps the existing file's attributes and permissions.
					File.Replace(tempName, fullName, null, true);
				}
				else{
					File.Move(tempName, fullName);
				}
			}
			finally{
				try{
					if(File.Exists(tempName)){
						File.Delete(tempName);
					}
				}
				catch(Exception ee){
					System.Diagnostics.Debug.WriteLine(ee.Message);
				}
			}
		}
	}
}
