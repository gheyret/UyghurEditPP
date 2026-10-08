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
		};

		public static UiTheme Current = Light;
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
			if(e.Item.Enabled && !(e.ToolStrip is ToolStripDropDown)){
				e.TextColor = gTheme.BarText;
			}
			base.OnRenderItemText(e);
		}
	}
}
