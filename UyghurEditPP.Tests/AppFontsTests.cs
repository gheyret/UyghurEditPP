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

		[TestMethod]
		public void FolderUri_KeepsHashAndPercentInTheName()
		{
			Uri uri = AppFonts.FolderUri(@"C:\Programs\Uyghur#Edit 100%\bin");
			Assert.AreEqual(@"C:\Programs\Uyghur#Edit 100%\bin\", uri.LocalPath);
			Assert.AreEqual("", uri.Fragment);
		}

		// The editor's font is found in a program folder whose name contains '#'.
		[TestMethod]
		public void WpfFont_LoadsFromAFolderWithHash()
		{
			string folder = Path.Combine(Path.GetTempPath(), "UyghurEditPP.Tests", "font#" + Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(folder);
			try{
				File.Copy(Path.Combine(RepoRoot(), "UKIJTuz.ttf"), Path.Combine(folder, "UKIJTuz.ttf"));
				AppFonts.LoadFrom(folder);
				string found = null;
				Exception error = null;
				System.Threading.Thread t = new System.Threading.Thread(() => {
					try{
						System.Windows.Media.FontFamily family = AppFonts.Wpf(AppFonts.UkijTuz);
						foreach(System.Windows.Media.Typeface face in family.GetTypefaces()){
							System.Windows.Media.GlyphTypeface glyphs;
							if(face.TryGetGlyphTypeface(out glyphs)){
								found = glyphs.FontUri.LocalPath;
							}
						}
					}
					catch(Exception ee){ error = ee; }
				});
				t.SetApartmentState(System.Threading.ApartmentState.STA);
				t.Start();
				t.Join();
				Assert.IsNull(error, error == null ? "" : error.ToString());
				Assert.IsNotNull(found, "no typeface found");
				Assert.IsTrue(found.StartsWith(folder, StringComparison.OrdinalIgnoreCase), found);
			}
			finally{
				AppFonts.LoadFrom(RepoRoot());
			}
		}
	}
}
