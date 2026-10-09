using System;
using System.Drawing;
using System.Windows.Forms;

namespace UyghurEditPP
{
	/// <summary>
	/// The colors of the window frame around the editor: menu bar, toolbar, tab strip and
	/// status bar, with thin lines between them (in the manner of Notepad++).
	/// </summary>
	public sealed class UiTheme
	{
		public Color Bar;            // menu bar, toolbar, status bar
		public Color BarText;
		public Color BarLine;        // the line under the menu bar and the toolbar, over the status bar
		public Color MenuBack;       // drop-down menus
		public Color MenuBorder;
		public Color MenuSelected;   // the item under the mouse
		public Color MenuSelectedBorder;
		public Color Separator;
		public Color ButtonPressed;
		public Color ButtonChecked;
		public Color TabStrip;       // behind the tabs
		public Color TabActive;
		public Color TabActiveText;
		public Color TabInactive;
		public Color TabInactiveText;
		public Color TabHover;
		public Color TabBorder;
		public Color Accent;         // the line on top of the active tab
		public bool IsDark;
		// Windows of their own (OCR, About ...): the form, its buttons and text boxes.
		public Color FormBack;
		public Color FormText;
		public Color ButtonBack;
		public Color LinkText;
		// The editor (WPF). In the light theme these are left at the editor's own defaults.
		public Color EditorBack;
		public Color EditorText;
		public Color LineNumbers;
		public Color Selection;
		public Color Caret;
		public Color Misspelled;     // the wavy line under misspelled words
		public Color FoundBack;      // words marked by the find window
		public Color FoundText;

		public static readonly UiTheme Light = new UiTheme {
			Bar = Color.FromArgb(0xF5, 0xF5, 0xF5),
			BarText = Color.Black,
			BarLine = Color.FromArgb(0xD5, 0xD5, 0xD5),
			MenuBack = Color.FromArgb(0xFB, 0xFB, 0xFB),
			MenuBorder = Color.FromArgb(0xA0, 0xA0, 0xA0),
			MenuSelected = Color.FromArgb(0xD5, 0xEA, 0xF8),
			MenuSelectedBorder = Color.FromArgb(0x7A, 0xB8, 0xE6),
			Separator = Color.FromArgb(0xC8, 0xC8, 0xC8),
			ButtonPressed = Color.FromArgb(0xB5, 0xD9, 0xF2),
			ButtonChecked = Color.FromArgb(0xCC, 0xE4, 0xF7),
			TabStrip = Color.FromArgb(0xEC, 0xEC, 0xEC),
			TabActive = Color.White,
			TabActiveText = Color.Black,
			TabInactive = Color.FromArgb(0xE0, 0xE0, 0xE0),
			TabInactiveText = Color.FromArgb(0x50, 0x50, 0x50),
			TabHover = Color.FromArgb(0xF2, 0xF2, 0xF2),
			TabBorder = Color.FromArgb(0xC0, 0xC0, 0xC0),
			Accent = Color.FromArgb(0x00, 0x93, 0xE6),
			IsDark = false,
			FormBack = SystemColors.Control,
			FormText = SystemColors.ControlText,
			ButtonBack = SystemColors.Control,
			LinkText = Color.Blue,
			EditorBack = Color.White,
			EditorText = Color.Black,
			LineNumbers = Color.Gray,
			Selection = SystemColors.Highlight,
			Caret = Color.Black,
			Misspelled = Color.Red,
			FoundBack = Color.Yellow,
			FoundText = Color.Black,
		};

		public static readonly UiTheme Dark = new UiTheme {
			Bar = Color.FromArgb(0x2B, 0x2B, 0x2B),
			BarText = Color.FromArgb(0xE0, 0xE0, 0xE0),
			BarLine = Color.FromArgb(0x44, 0x44, 0x44),
			MenuBack = Color.FromArgb(0x2B, 0x2B, 0x2B),
			MenuBorder = Color.FromArgb(0x5A, 0x5A, 0x5A),
			MenuSelected = Color.FromArgb(0x3F, 0x4A, 0x55),
			MenuSelectedBorder = Color.FromArgb(0x55, 0x66, 0x77),
			Separator = Color.FromArgb(0x50, 0x50, 0x50),
			ButtonPressed = Color.FromArgb(0x26, 0x4F, 0x78),
			ButtonChecked = Color.FromArgb(0x2F, 0x4A, 0x63),
			TabStrip = Color.FromArgb(0x25, 0x25, 0x26),
			TabActive = Color.FromArgb(0x1E, 0x1E, 0x1E),
			TabActiveText = Color.White,
			TabInactive = Color.FromArgb(0x2D, 0x2D, 0x2D),
			TabInactiveText = Color.FromArgb(0xB4, 0xB4, 0xB4),
			TabHover = Color.FromArgb(0x3A, 0x3A, 0x3A),
			TabBorder = Color.FromArgb(0x44, 0x44, 0x44),
			Accent = Color.FromArgb(0x00, 0x93, 0xE6),
			IsDark = true,
			FormBack = Color.FromArgb(0x20, 0x20, 0x20),
			FormText = Color.FromArgb(0xE0, 0xE0, 0xE0),
			ButtonBack = Color.FromArgb(0x33, 0x33, 0x33),
			LinkText = Color.FromArgb(0x4E, 0xA6, 0xEA),
			EditorBack = Color.FromArgb(0x1E, 0x1E, 0x1E),
			EditorText = Color.FromArgb(0xDC, 0xDC, 0xDC),
			LineNumbers = Color.FromArgb(0x85, 0x85, 0x85),
			Selection = Color.FromArgb(0x26, 0x4F, 0x78),
			Caret = Color.White,
			Misspelled = Color.FromArgb(0xF1, 0x4C, 0x4C),
			FoundBack = Color.FromArgb(0x80, 0x60, 0x00),
			FoundText = Color.White,
		};

		public static UiTheme Current = Light;

		// The values of the "theme" setting.
		public const string LightSetting = "light";
		public const string DarkSetting = "dark";
		public const string SystemSetting = "system";

		/// <summary>The theme for a setting; anything unknown is the light theme.</summary>
		public static UiTheme FromSetting(string setting)
		{
			if(DarkSetting.Equals(setting)){
				return Dark;
			}
			if(SystemSetting.Equals(setting)){
				return WindowsUsesDarkMode() ? Dark : Light;
			}
			return Light;
		}

		/// <summary>
		/// Whether apps are set to dark mode in the Windows settings. Read the same way as
		/// Windows Forms in .NET 9+ does (AppsUseLightTheme = 0 means dark; see dotnet/winforms,
		/// Application.cs); never dark with a high contrast theme.
		/// </summary>
		public static bool WindowsUsesDarkMode()
		{
			if(SystemInformation.HighContrast){
				return false;
			}
			try{
				object value = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1);
				return value is int && (int)value == 0;
			}
			catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee);
				return false;
			}
		}

		[System.Runtime.InteropServices.DllImport("dwmapi.dll")]
		static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);
		const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

		/// <summary>
		/// Dark or light title bar. Only on Windows 11 (build 22000) or later, where this window
		/// attribute is documented; Windows 10 keeps the light title bar.
		/// </summary>
		public static void SetTitleBar(IntPtr hwnd, bool dark)
		{
			if(hwnd == IntPtr.Zero || Environment.OSVersion.Version.Major < 10 || Environment.OSVersion.Version.Build < 22000){
				return;
			}
			int value = dark ? 1 : 0;
			try{
				DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
			}
			catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee);
			}
		}
	}

	/// <summary>The toolbar and menu colors of a theme.</summary>
	class UiColorTable : ProfessionalColorTable
	{
		readonly UiTheme gTheme;

		public UiColorTable(UiTheme theme)
		{
			gTheme = theme;
			UseSystemColors = false;
		}

		public override Color MenuStripGradientBegin { get { return gTheme.Bar; } }
		public override Color MenuStripGradientEnd { get { return gTheme.Bar; } }
		public override Color ToolStripGradientBegin { get { return gTheme.Bar; } }
		public override Color ToolStripGradientMiddle { get { return gTheme.Bar; } }
		public override Color ToolStripGradientEnd { get { return gTheme.Bar; } }
		public override Color ToolStripBorder { get { return gTheme.BarLine; } }
		public override Color StatusStripGradientBegin { get { return gTheme.Bar; } }
		public override Color StatusStripGradientEnd { get { return gTheme.Bar; } }

		public override Color ToolStripDropDownBackground { get { return gTheme.MenuBack; } }
		public override Color ImageMarginGradientBegin { get { return gTheme.MenuBack; } }
		public override Color ImageMarginGradientMiddle { get { return gTheme.MenuBack; } }
		public override Color ImageMarginGradientEnd { get { return gTheme.MenuBack; } }
		public override Color MenuBorder { get { return gTheme.MenuBorder; } }
		public override Color MenuItemBorder { get { return gTheme.MenuSelectedBorder; } }
		public override Color MenuItemSelected { get { return gTheme.MenuSelected; } }
		public override Color MenuItemSelectedGradientBegin { get { return gTheme.MenuSelected; } }
		public override Color MenuItemSelectedGradientEnd { get { return gTheme.MenuSelected; } }
		public override Color MenuItemPressedGradientBegin { get { return gTheme.MenuBack; } }
		public override Color MenuItemPressedGradientMiddle { get { return gTheme.MenuBack; } }
		public override Color MenuItemPressedGradientEnd { get { return gTheme.MenuBack; } }

		public override Color ButtonSelectedBorder { get { return gTheme.MenuSelectedBorder; } }
		public override Color ButtonSelectedGradientBegin { get { return gTheme.MenuSelected; } }
		public override Color ButtonSelectedGradientMiddle { get { return gTheme.MenuSelected; } }
		public override Color ButtonSelectedGradientEnd { get { return gTheme.MenuSelected; } }
		public override Color ButtonSelectedHighlight { get { return gTheme.MenuSelected; } }
		public override Color ButtonPressedGradientBegin { get { return gTheme.ButtonPressed; } }
		public override Color ButtonPressedGradientMiddle { get { return gTheme.ButtonPressed; } }
		public override Color ButtonPressedGradientEnd { get { return gTheme.ButtonPressed; } }
		public override Color ButtonPressedHighlight { get { return gTheme.ButtonPressed; } }
		public override Color ButtonCheckedGradientBegin { get { return gTheme.ButtonChecked; } }
		public override Color ButtonCheckedGradientMiddle { get { return gTheme.ButtonChecked; } }
		public override Color ButtonCheckedGradientEnd { get { return gTheme.ButtonChecked; } }
		public override Color ButtonCheckedHighlight { get { return gTheme.ButtonChecked; } }
		public override Color CheckBackground { get { return gTheme.ButtonChecked; } }
		public override Color CheckSelectedBackground { get { return gTheme.MenuSelected; } }
		public override Color CheckPressedBackground { get { return gTheme.ButtonPressed; } }

		public override Color SeparatorDark { get { return gTheme.Separator; } }
		public override Color SeparatorLight { get { return gTheme.Separator; } }
		public override Color GripDark { get { return gTheme.Separator; } }
		public override Color GripLight { get { return gTheme.Bar; } }
	}

	/// <summary>
	/// Flat bars with a one-pixel line between them: under the menu bar and the toolbar, and
	/// over the status bar. Drop-down menus keep their usual border.
	/// </summary>
	class UiRenderer : ToolStripProfessionalRenderer
	{
		readonly UiTheme gTheme;

		public UiRenderer(UiTheme theme) : base(new UiColorTable(theme))
		{
			gTheme = theme;
			RoundedEdges = false;
		}

		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if(e.ToolStrip is StatusStrip){
				using(SolidBrush back = new SolidBrush(gTheme.Bar)){
					e.Graphics.FillRectangle(back, e.AffectedBounds);
				}
				return;
			}
			base.OnRenderToolStripBackground(e);
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			ToolStrip strip = e.ToolStrip;
			if(strip is ToolStripDropDown){
				base.OnRenderToolStripBorder(e);
				return;
			}
			using(Pen pen = new Pen(gTheme.BarLine)){
				if(strip is StatusStrip){
					e.Graphics.DrawLine(pen, 0, 0, strip.Width, 0);
				}
				else{
					e.Graphics.DrawLine(pen, 0, strip.Height - 1, strip.Width, strip.Height - 1);
				}
			}
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if(e.Item.Enabled){
				e.TextColor = gTheme.BarText;
			}
			else if(gTheme.IsDark){
				e.TextColor = Color.FromArgb(0x80, 0x80, 0x80);
			}
			base.OnRenderItemText(e);
		}

		// Black glyph icons (wrap, writing direction, the script buttons ...) are drawn in the
		// text color in the dark theme; colored icons are drawn as they are.
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			Image glyph = gTheme.IsDark && e.Image != null ? LightGlyph(e.Image, e.Item == null || e.Item.Enabled) : null;
			if(glyph == null){
				base.OnRenderItemImage(e);
				return;
			}
			e.Graphics.DrawImage(glyph, e.ImageRectangle);
		}

		// One recolored pair per toolbar / menu image, dropped with the image: the number of
		// images is fixed, so this stays small.
		static readonly System.Runtime.CompilerServices.ConditionalWeakTable<Image, Image[]> gGlyphs =
			new System.Runtime.CompilerServices.ConditionalWeakTable<Image, Image[]>();

		// The image recolored to the text color (or a dimmed gray when disabled), or null if it
		// is not a dark one-color glyph.
		Image LightGlyph(Image image, bool enabled)
		{
			Image[] cached = gGlyphs.GetValue(image, img => MakeGlyphs(img, gTheme.BarText, Color.FromArgb(0x70, 0x70, 0x70)));
			return cached == null ? null : cached[enabled ? 0 : 1];
		}

		static Image[] MakeGlyphs(Image image, Color text, Color disabled)
		{
			Bitmap src = image as Bitmap;
			if(src == null || src.Width > 64 || src.Height > 64){
				return null;
			}
			int opaque = 0, dark = 0;
			for(int y = 0; y < src.Height; y++){
				for(int x = 0; x < src.Width; x++){
					Color c = src.GetPixel(x, y);
					if(c.A < 32 || (c.R == 255 && c.G == 0 && c.B == 255)){
						continue; // transparent, or the magenta used as transparent color
					}
					opaque++;
					int max = Math.Max(c.R, Math.Max(c.G, c.B)), min = Math.Min(c.R, Math.Min(c.G, c.B));
					if(max < 120 && max - min < 40){
						dark++;
					}
				}
			}
			if(opaque == 0 || dark < opaque * 0.9){
				return null;
			}
			return new Image[] { Recolor(src, text), Recolor(src, disabled) };
		}

		// Darker pixels become more opaque in the new color: a black glyph turns into a
		// glyph of that color with the same smooth edges.
		static Bitmap Recolor(Bitmap src, Color color)
		{
			Bitmap dst = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			for(int y = 0; y < src.Height; y++){
				for(int x = 0; x < src.Width; x++){
					Color c = src.GetPixel(x, y);
					if(c.A == 0 || (c.R == 255 && c.G == 0 && c.B == 255)){
						continue;
					}
					double darkness = 1.0 - (c.R * 0.299 + c.G * 0.587 + c.B * 0.114) / 255.0;
					int alpha = (int)Math.Round(c.A * darkness);
					dst.SetPixel(x, y, Color.FromArgb(alpha, color.R, color.G, color.B));
				}
			}
			return dst;
		}

		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			if(e.Item == null || e.Item.Enabled){
				e.ArrowColor = gTheme.BarText;
			}
			base.OnRenderArrow(e);
		}

		// The check mark of a menu item; drawn as lines in the dark theme, where the usual
		// black mark would not show.
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			if(!gTheme.IsDark){
				base.OnRenderItemCheck(e);
				return;
			}
			Rectangle rc = e.ImageRectangle;
			rc.Inflate(1, 1);
			using(SolidBrush back = new SolidBrush(gTheme.ButtonChecked)){
				e.Graphics.FillRectangle(back, rc);
			}
			float w = Math.Max(1.5f, rc.Height / 8f);
			System.Drawing.Drawing2D.SmoothingMode old = e.Graphics.SmoothingMode;
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			using(Pen pen = new Pen(gTheme.BarText, w)){
				e.Graphics.DrawLines(pen, new[] {
					new PointF(rc.Left + rc.Width * 0.25f, rc.Top + rc.Height * 0.52f),
					new PointF(rc.Left + rc.Width * 0.43f, rc.Top + rc.Height * 0.70f),
					new PointF(rc.Left + rc.Width * 0.76f, rc.Top + rc.Height * 0.32f) });
			}
			e.Graphics.SmoothingMode = old;
		}
	}
}
