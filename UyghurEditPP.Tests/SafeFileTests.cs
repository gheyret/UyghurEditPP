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

		[TestInitialize]
		public void SetUp()
		{
			gFolder = Path.Combine(Path.GetTempPath(), "UyghurEditPP.Tests", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(gFolder);
			gFile = Path.Combine(gFolder, "test.txt");
		}

		[TestCleanup]
		public void TearDown()
		{
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

		static byte[] Combine(byte[] a, byte[] b)
		{
			byte[] r = new byte[a.Length + b.Length];
			Buffer.BlockCopy(a, 0, r, 0, a.Length);
			Buffer.BlockCopy(b, 0, r, a.Length, b.Length);
			return r;
		}
	}
}
