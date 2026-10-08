using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class AppFontsTests
	{
		static string RepoRoot()
		{
			DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
			while(dir != null && !File.Exists(Path.Combine(dir.FullName, "UKIJTuz.ttf"))){
				dir = dir.Parent;
			}
			Assert.IsNotNull(dir, "UKIJTuz.ttf was not found above the test folder");
			return dir.FullName;
		}

		// The bundled files hold the family "UKIJ Tuz", and fonts made from them carry that name
		// whether or not the font is installed in Windows.
		[TestMethod]
		public void BundledFont_GivesUkijTuz()
		{
			AppFonts.LoadFrom(RepoRoot());

			Assert.IsTrue(AppFonts.UsesOwnCopy);
			using(Font f = AppFonts.Create(AppFonts.UkijTuz, 12)){
				Assert.AreEqual(AppFonts.UkijTuz, f.Name);
			}
			using(Font f = AppFonts.Create(AppFonts.UkijTuz, 12, FontStyle.Bold)){
				Assert.AreEqual(AppFonts.UkijTuz, f.Name);
				Assert.IsTrue(f.Bold);
			}
			using(Font f = AppFonts.Create("Arial", 10)){
				Assert.AreEqual("Arial", f.Name);
			}
		}
	}
}
