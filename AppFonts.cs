using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UyghurEditPP
{
	/// <summary>
	/// The UKIJ Tuz font that comes with the program. When it is not installed in Windows, the
	/// copies next to UyghurEditPP.exe are loaded for this program only (nothing is installed),
	/// so menus, windows and the editor still show it.
	///  - GDI (what Windows Forms draws text with): AddFontResourceEx with FR_PRIVATE
	///    https://learn.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-addfontresourceexw
	///  - GDI+ (System.Drawing.Font objects): a PrivateFontCollection
	///  - WPF (the editor, the find window): a FontFamily pointing at the program folder
	/// </summary>
	public static class AppFonts
	{
		public const string UkijTuz = "UKIJ Tuz";
		static readonly string[] gFiles = { "UKIJTuz.ttf", "UKIJTuzBold.ttf" };

		static PrivateFontCollection gCollection;
		static FontFamily gFamily;

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
		static extern int AddFontResourceEx(string name, uint fl, IntPtr res);
		const uint FR_PRIVATE = 0x10;

		/// <summary>True when UKIJ Tuz is not installed and the program's own copy is used.</summary>
		public static bool UsesOwnCopy{
			get{ return gFamily != null; }
		}

		/// <summary>Loads the program's copy of UKIJ Tuz if Windows does not have it. Never throws.</summary>
		public static void Load()
		{
			try{
				if(IsInstalled(UkijTuz)){
					return;
				}
				LoadFrom(AppPaths.ProgramFolder);
			}
			catch(Exception ee){
				ErrorLog.Write(ee);
			}
		}

		internal static void LoadFrom(string folder)
		{
			PrivateFontCollection collection = new PrivateFontCollection();
			foreach(string name in gFiles){
				string path = Path.Combine(folder, name);
				if(!File.Exists(path)){
					continue;
				}
				collection.AddFontFile(path);
				AddFontResourceEx(path, FR_PRIVATE, IntPtr.Zero);
			}
			foreach(FontFamily family in collection.Families){
				if(family.Name == UkijTuz){
					gCollection = collection; // the families live as long as the collection
					gFamily = family;
				}
			}
		}

		static bool IsInstalled(string name)
		{
			using(InstalledFontCollection installed = new InstalledFontCollection()){
				foreach(FontFamily family in installed.Families){
					if(string.Equals(family.Name, name, StringComparison.OrdinalIgnoreCase)){
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>A font by name; UKIJ Tuz comes from the program's copy when needed.</summary>
		public static Font Create(string name, float size, FontStyle style = FontStyle.Regular, GraphicsUnit unit = GraphicsUnit.Point)
		{
			if(gFamily != null && UkijTuz.Equals(name, StringComparison.OrdinalIgnoreCase)){
				return new Font(gFamily, size, style, unit);
			}
			return new Font(name, size, style, unit);
		}

		/// <summary>
		/// Gives the controls that asked for UKIJ Tuz (in their designer code) the program's copy;
		/// without it Windows Forms would fall back to Microsoft Sans Serif.
		/// </summary>
		public static void Fix(Control root)
		{
			if(gFamily == null){
				return;
			}
			Font f = root.Font;
			if(UkijTuz.Equals(f.OriginalFontName, StringComparison.OrdinalIgnoreCase) && f.Name != UkijTuz){
				root.Font = new Font(gFamily, f.Size, f.Style, f.Unit);
			}
			foreach(Control child in root.Controls){
				Fix(child);
			}
		}

		/// <summary>The WPF font family for a name; UKIJ Tuz from the program folder when needed.</summary>
		public static System.Windows.Media.FontFamily Wpf(string name)
		{
			if(gFamily != null && UkijTuz.Equals(name, StringComparison.OrdinalIgnoreCase)){
				string folder = AppPaths.ProgramFolder;
				if(!folder.EndsWith("\\", StringComparison.Ordinal)){
					folder += "\\";
				}
				return new System.Windows.Media.FontFamily(new Uri(folder), "./#" + UkijTuz);
			}
			return new System.Windows.Media.FontFamily(name);
		}
	}
}
