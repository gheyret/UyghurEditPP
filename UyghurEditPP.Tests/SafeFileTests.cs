using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class SafeFileTests
	{
		string gFolder;
		string gFile;
		Action<string, string, string> gOriginalReplace;
		Action<Exception> gOriginalLog;
		int gLogged;

		[TestInitialize]
		public void SetUp()
		{
			gFolder = Path.Combine(Path.GetTempPath(), "UyghurEditPP.Tests", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(gFolder);
			gFile = Path.Combine(gFolder, "test.txt");
			gOriginalReplace = SafeFile.ReplaceFile;
			gOriginalLog = SafeFile.Log;
			gLogged = 0;
			SafeFile.Log = ee => gLogged++;
		}

		[TestCleanup]
		public void TearDown()
		{
			SafeFile.ReplaceFile = gOriginalReplace;
			SafeFile.Log = gOriginalLog;
			if(Directory.Exists(gFolder)){
				foreach(string f in Directory.GetFiles(gFolder)){
					File.SetAttributes(f, FileAttributes.Normal);
				}
				Directory.Delete(gFolder, true);
			}
		}

		static Action<Stream> Bytes(byte[] data)
		{
			return s => s.Write(data, 0, data.Length);
		}

		void AssertOnlyTargetLeft()
		{
			CollectionAssert.AreEqual(new[] { gFile }, Directory.GetFiles(gFolder));
		}

		[TestMethod]
		public void Write_NewFile_CreatesIt()
		{
			byte[] data = Encoding.UTF8.GetBytes("yéngi");

			SafeFile.Write(gFile, Bytes(data));

			CollectionAssert.AreEqual(data, File.ReadAllBytes(gFile));
			AssertOnlyTargetLeft();
		}

		[TestMethod]
		public void Write_ExistingFile_ReplacesContent()
		{
			File.WriteAllText(gFile, "a much longer old content that must not survive");
			byte[] data = Encoding.UTF8.GetBytes("new");

			SafeFile.Write(gFile, Bytes(data));

			CollectionAssert.AreEqual(data, File.ReadAllBytes(gFile));
			AssertOnlyTargetLeft();
		}

		[TestMethod]
		public void Write_WriterFails_KeepsOriginalAndRemovesTempFile()
		{
			File.WriteAllText(gFile, "original");

			Assert.ThrowsException<InvalidOperationException>(() =>
				SafeFile.Write(gFile, s => {
					s.WriteByte(1);
					throw new InvalidOperationException("disk full");
				}));

			Assert.AreEqual("original", File.ReadAllText(gFile));
			AssertOnlyTargetLeft();
		}

		[TestMethod]
		public void Write_WriterFailsForNewFile_LeavesNothing()
		{
			Assert.ThrowsException<IOException>(() =>
				SafeFile.Write(gFile, s => { throw new IOException("failed"); }));

			Assert.AreEqual(0, Directory.GetFiles(gFolder).Length);
		}

		// The bytes, including a BOM or its absence, are written exactly as given.
		[DataTestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public void Write_KeepsBomAsWritten(bool bom)
		{
			File.WriteAllText(gFile, "old");
			Encoding enc = new UTF8Encoding(bom);

			SafeFile.Write(gFile, s => {
				StreamWriter w = new StreamWriter(s, enc);
				w.Write("ئۇيغۇر");
				w.Flush();
			});

			byte[] expected = Combine(enc.GetPreamble(), enc.GetBytes("ئۇيغۇر"));
			CollectionAssert.AreEqual(expected, File.ReadAllBytes(gFile));
		}

		[TestMethod]
		public void Write_ExistingReadOnlyFile_ThrowsAndKeepsOriginal()
		{
			File.WriteAllText(gFile, "original");
			File.SetAttributes(gFile, FileAttributes.ReadOnly);

			Assert.ThrowsException<UnauthorizedAccessException>(() =>
				SafeFile.Write(gFile, Bytes(Encoding.UTF8.GetBytes("new"))));

			Assert.AreEqual("original", File.ReadAllText(gFile));
			AssertOnlyTargetLeft();
		}

		// ReplaceFileW failed without touching anything (for example a file system that
		// does not support it): the content is copied over the file instead.
		[TestMethod]
		public void Write_ReplaceUnsupported_FallsBackToCopy()
		{
			File.WriteAllText(gFile, "original");
			SafeFile.ReplaceFile = (src, dst, bak) => { throw new IOException("not supported"); };

			SafeFile.Write(gFile, Bytes(Encoding.UTF8.GetBytes("new")));

			Assert.AreEqual("new", File.ReadAllText(gFile));
			Assert.AreEqual(1, gLogged);
			AssertOnlyTargetLeft();
		}

		[TestMethod]
		public void Write_ReplacePlatformNotSupported_FallsBackToCopy()
		{
			File.WriteAllText(gFile, "original");
			SafeFile.ReplaceFile = (src, dst, bak) => { throw new PlatformNotSupportedException(); };

			SafeFile.Write(gFile, Bytes(Encoding.UTF8.GetBytes("new")));

			Assert.AreEqual("new", File.ReadAllText(gFile));
			AssertOnlyTargetLeft();
		}

		// ERROR_UNABLE_TO_MOVE_REPLACEMENT_2: the original was already renamed to the
		// backup name and the new content is still under the temporary name.
		[TestMethod]
		public void Write_ReplaceFailsHalfWay_PutsNewContentInPlace()
		{
			File.WriteAllText(gFile, "original");
			SafeFile.ReplaceFile = (src, dst, bak) => {
				File.Move(dst, bak);
				throw new IOException("unable to move replacement");
			};

			SafeFile.Write(gFile, Bytes(Encoding.UTF8.GetBytes("new")));

			Assert.AreEqual("new", File.ReadAllText(gFile));
			Assert.AreEqual(1, gLogged);
			AssertOnlyTargetLeft();
		}

		[TestMethod]
		public void Write_Success_LeavesNoBackup()
		{
			File.WriteAllText(gFile, "original");
			string backup = null;
			SafeFile.ReplaceFile = (src, dst, bak) => { backup = bak; gOriginalReplace(src, dst, bak); };

			SafeFile.Write(gFile, Bytes(Encoding.UTF8.GetBytes("new")));

			Assert.IsNotNull(backup);
			Assert.AreEqual("new", File.ReadAllText(gFile));
			Assert.AreEqual(0, gLogged);
			AssertOnlyTargetLeft();
		}

		// The fallback copy cannot even open the file (locked by another program):
		// the original is untouched, so the temporary file is removed.
		[TestMethod]
		public void Write_ReplaceAndCopyFailOnLockedFile_KeepsOriginal()
		{
			File.WriteAllText(gFile, "original");
			SafeFile.ReplaceFile = (src, dst, bak) => { throw new IOException("sharing violation"); };

			using (new FileStream(gFile, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				Assert.ThrowsException<IOException>(() =>
					SafeFile.Write(gFile, Bytes(Encoding.UTF8.GetBytes("new"))));
			}

			Assert.AreEqual("original", File.ReadAllText(gFile));
			AssertOnlyTargetLeft();
		}

		static byte[] Combine(byte[] a, byte[] b)
		{
			byte[] r = new byte[a.Length + b.Length];
			Buffer.BlockCopy(a, 0, r, 0, a.Length);
			Buffer.BlockCopy(b, 0, r, a.Length, b.Length);
			return r;
		}
	}
}
