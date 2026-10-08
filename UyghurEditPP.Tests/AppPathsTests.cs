using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class AppPathsTests
	{
		string gRoot;
		string gOld;
		string gNew;

		[TestInitialize]
		public void SetUp()
		{
			gRoot = Path.Combine(Path.GetTempPath(), "UyghurEditPP.Tests", Guid.NewGuid().ToString("N"));
			gOld = Path.Combine(gRoot, "program");
			gNew = Path.Combine(gRoot, "appdata", "UyghurEditPP");
			Directory.CreateDirectory(gOld);
		}

		[TestCleanup]
		public void TearDown()
		{
			if(Directory.Exists(gRoot)){
				Directory.Delete(gRoot, true);
			}
		}

		[TestMethod]
		public void MigrateFile_OldFileOnly_CopiesItAndKeepsTheOriginal()
		{
			File.WriteAllText(Path.Combine(gOld, "imla_ishletkuchi.txt"), "kitab 1\r\n");

			bool copied = AppPaths.MigrateFile("imla_ishletkuchi.txt", gOld, gNew);

			Assert.IsTrue(copied);
			Assert.AreEqual("kitab 1\r\n", File.ReadAllText(Path.Combine(gNew, "imla_ishletkuchi.txt")));
			Assert.IsTrue(File.Exists(Path.Combine(gOld, "imla_ishletkuchi.txt")));
		}

		[TestMethod]
		public void MigrateFile_NewFileExists_LeavesItAlone()
		{
			Directory.CreateDirectory(gNew);
			File.WriteAllText(Path.Combine(gOld, "uyghuredit.cfg"), "old");
			File.WriteAllText(Path.Combine(gNew, "uyghuredit.cfg"), "new");

			bool copied = AppPaths.MigrateFile("uyghuredit.cfg", gOld, gNew);

			Assert.IsFalse(copied);
			Assert.AreEqual("new", File.ReadAllText(Path.Combine(gNew, "uyghuredit.cfg")));
		}

		[TestMethod]
		public void MigrateFile_NoOldFile_DoesNothing()
		{
			bool copied = AppPaths.MigrateFile("uyghuredit.cfg", gOld, gNew);

			Assert.IsFalse(copied);
			Assert.IsFalse(File.Exists(Path.Combine(gNew, "uyghuredit.cfg")));
		}

		[TestMethod]
		public void MigrateFile_SameFolder_DoesNothing()
		{
			File.WriteAllText(Path.Combine(gOld, "uyghuredit.cfg"), "cfg");

			bool copied = AppPaths.MigrateFile("uyghuredit.cfg", gOld, gOld + Path.DirectorySeparatorChar);

			Assert.IsFalse(copied);
			Assert.AreEqual("cfg", File.ReadAllText(Path.Combine(gOld, "uyghuredit.cfg")));
		}

		[TestMethod]
		public void DataFile_IsInAppDataUyghurEditPP()
		{
			string expected = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UyghurEditPP", "uyghuredit.cfg");

			Assert.AreEqual(expected, AppPaths.DataFile(AppPaths.ConfigFileName));
		}
	}
}
