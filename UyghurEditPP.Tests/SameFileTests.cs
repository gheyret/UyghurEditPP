using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class SameFileTests
	{
		[TestMethod]
		public void SameFile_IgnoresCaseAndRelativeParts()
		{
			Assert.IsTrue(MainForm.SameFile(@"C:\Hojjetler\a.txt", @"c:\HOJJETLER\A.TXT"));
			Assert.IsTrue(MainForm.SameFile(@"C:\Hojjetler\x\..\a.txt", @"C:\Hojjetler\a.txt"));
			string full = Path.Combine(Directory.GetCurrentDirectory(), "b.txt");
			Assert.IsTrue(MainForm.SameFile("b.txt", full));
			Assert.IsTrue(MainForm.SameFile(@".\B.TXT", full));
		}

		[TestMethod]
		public void SameFile_DifferentOrEmpty()
		{
			Assert.IsFalse(MainForm.SameFile(@"C:\a.txt", @"C:\b.txt"));
			Assert.IsFalse(MainForm.SameFile("", ""));
			Assert.IsFalse(MainForm.SameFile(null, @"C:\a.txt"));
		}
	}
}
