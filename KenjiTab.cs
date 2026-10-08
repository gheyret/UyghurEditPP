using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UyghurEditPP
{
	public delegate void delgRemoveTab(int TabIndex);
	public class KenjiTab : System.Windows.Forms.TabControl
	{
		private int _hotTabIndex = -1;
		public delgRemoveTab  RemoveTab=null;
		public KenjiTab(): base()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
		}

		#region Properties
		
		private int CloseButtonHeight
		{
			// 14 pixels at 100%, scaled to the monitor's DPI.
			get{ return LogicalToDeviceUnits(14); }
		}

		private int HotTabIndex
		{
			get{ return _hotTabIndex; }
			set
			{
				if (_hotTabIndex != value)
				{
					_hotTabIndex = value;
					this.Invalidate();
				}
			}
		}

		#endregion

		#region Overridden Methods
		
//		protected override void  OnCreateControl()
//		{
//			base.OnCreateControl();
//			this.OnFontChanged(EventArgs.Empty);
//		}

//		protected override void OnFontChanged(EventArgs e)
//		{
//			base.OnFontChanged(e);
//			IntPtr hFont = this.Font.ToHfont();
//			SendMessage(this.Handle, WM_SETFONT, hFont, new IntPtr(-1));
//			SendMessage(this.Handle, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
//			this.UpdateStyles();
//		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			TCHITTESTINFO HTI = new TCHITTESTINFO(e.X, e.Y);
			HotTabIndex = SendMessage(this.Handle, TCM_HITTEST, IntPtr.Zero, ref HTI);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			HotTabIndex = -1;
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			UiTheme theme = UiTheme.Current;
			using (SolidBrush strip = new SolidBrush(theme.TabStrip))
				pevent.Graphics.FillRectangle(strip, ClientRectangle);
			if (this.TabCount > 0 && this.Alignment == TabAlignment.Top)
			{
				// The line under the tabs; the active tab is drawn over it, so it stays open below.
				int y = GetTabRect(0).Bottom - 1;
				using (Pen line = new Pen(theme.TabBorder))
					pevent.Graphics.DrawLine(line, 0, y, Width, y);
			}
			for (int id = 0; id < this.TabCount; id++)
				if (id != SelectedIndex)
					DrawTabBackground(pevent.Graphics, id);
			if (SelectedIndex >= 0 && SelectedIndex < this.TabCount)
				DrawTabBackground(pevent.Graphics, SelectedIndex);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			for (int id = 0; id < this.TabCount; id++)
				DrawTabContent(e.Graphics, id);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == TCM_SETPADDING)
				m.LParam = MAKELPARAM(this.Padding.X + CloseButtonHeight / 2, this.Padding.Y);

			if (m.Msg == WM_MOUSEDOWN && !this.DesignMode && HotTabIndex>=0)
			{
				Point pt = this.PointToClient(Cursor.Position);
				Rectangle closeRect = GetCloseButtonRect(HotTabIndex);
				if (closeRect.Contains(pt))
				{
					//TabPages.RemoveAt(HotTabIndex);
					//m.Msg = WM_NULL;
					if(RemoveTab!=null){
						RemoveTab(HotTabIndex);
					}
				}
			}
			base.WndProc(ref m);
		}

		#endregion

		#region Private Methods
		
		private IntPtr MAKELPARAM(int lo, int hi)
		{
			return new IntPtr((hi << 16) | (lo & 0xFFFF));
		}
		
		// A tab with a thin frame; the active one is white, open below, with a colored line on top.
		private void DrawTabBackground(Graphics graphics, int id)
		{
			UiTheme theme = UiTheme.Current;
			Rectangle rc = GetTabRect(id);
			bool active = id == SelectedIndex;
			Color back = active ? theme.TabActive : (id == HotTabIndex ? theme.TabHover : theme.TabInactive);
			using (SolidBrush brush = new SolidBrush(back))
				graphics.FillRectangle(brush, rc);
			using (Pen border = new Pen(theme.TabBorder))
			{
				graphics.DrawLine(border, rc.Left, rc.Top, rc.Left, rc.Bottom - 1);
				graphics.DrawLine(border, rc.Right - 1, rc.Top, rc.Right - 1, rc.Bottom - 1);
				graphics.DrawLine(border, rc.Left, rc.Top, rc.Right - 1, rc.Top);
			}
			if (active)
			{
				using (SolidBrush accent = new SolidBrush(theme.Accent))
					graphics.FillRectangle(accent, rc.Left, rc.Top, rc.Width, LogicalToDeviceUnits(3));
			}
		}
		
		private void DrawTabContent(Graphics graphics, int id)
		{
			bool selectedOrHot = id == this.SelectedIndex || id == this.HotTabIndex;
			bool vertical = this.Alignment >= TabAlignment.Left;
			
			Image tabImage = null;
			
			if (this.ImageList != null)
			{
				TabPage page = this.TabPages[id];
				if (page.ImageIndex > -1 && page.ImageIndex < this.ImageList.Images.Count)
					tabImage = this.ImageList.Images[page.ImageIndex];

				if (page.ImageKey.Length > 0 && this.ImageList.Images.ContainsKey(page.ImageKey))
					tabImage = this.ImageList.Images[page.ImageKey];
			}

			Rectangle tabRect = GetTabRect(id);
			Rectangle contentRect = vertical ? new Rectangle(0, 0, tabRect.Height, tabRect.Width) : new Rectangle(Point.Empty, tabRect.Size);
			Rectangle textrect = contentRect;
			textrect.Width -= FontHeight;
			// Keep clear of the frame and the line on top (the text fills its box with the tab color).
			int edge = LogicalToDeviceUnits(3);
			textrect.Inflate(-1, -edge);

			if (tabImage != null)
			{
				textrect.Width -= tabImage.Width;
				textrect.X += tabImage.Width;
			}

			UiTheme theme = UiTheme.Current;
			Color frColor = id == SelectedIndex ? theme.TabActiveText : theme.TabInactiveText;
			Color bkColor = id == SelectedIndex ? theme.TabActive : (id == this.HotTabIndex ? theme.TabHover : theme.TabInactive);

			using (Bitmap bm = new Bitmap(contentRect.Width, contentRect.Height))
			{
				using (Graphics bmGraphics = Graphics.FromImage(bm))
				{
					TextRenderer.DrawText(bmGraphics, this.TabPages[id].Text, this.Font, textrect, frColor, bkColor);
					if (selectedOrHot)
					{
						Rectangle closeRect = new Rectangle(contentRect.Right - CloseButtonHeight, 0, CloseButtonHeight, CloseButtonHeight);
						closeRect.Offset(-LogicalToDeviceUnits(2), (contentRect.Height - closeRect.Height) / 2);
						DrawCloseButton(bmGraphics, closeRect);
					}
					if (tabImage != null)
					{
						Rectangle imageRect = new Rectangle(Padding.X, 0, tabImage.Width, tabImage.Height);
						imageRect.Offset(0, (contentRect.Height - imageRect.Height) / 2);
						bmGraphics.DrawImage(tabImage, imageRect);
					}
				}

				graphics.DrawImage(bm, tabRect);
				
			}
		}

		private void DrawCloseButton(Graphics graphics, Rectangle bounds)
		{
			graphics.FillRectangle(Brushes.Red, bounds);
			using (Font closeFont = new Font("Tahoma", LogicalToDeviceUnits(12), FontStyle.Bold, GraphicsUnit.Pixel)) // 9 pt at 100%
				TextRenderer.DrawText(graphics, "X", closeFont, bounds, Color.White, Color.Red, TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);

		}
		
		private Rectangle GetCloseButtonRect(int id)
		{
			Rectangle tabRect = GetTabRect(id);
			Rectangle closeRect = new Rectangle(tabRect.Left, tabRect.Top, CloseButtonHeight, CloseButtonHeight);

			switch (Alignment)
			{
				case TabAlignment.Left:
					closeRect.Offset((tabRect.Width - closeRect.Width) / 2, 0);
					break;
				case TabAlignment.Right:
					closeRect.Offset((tabRect.Width - closeRect.Width) / 2, tabRect.Height - closeRect.Height);
					break;
				default:
					closeRect.Offset(tabRect.Width - closeRect.Width, (tabRect.Height - closeRect.Height) / 2);
					break;
			}

			return closeRect;
		}

		#endregion

		#region Interop

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, ref TCHITTESTINFO lParam);

		[StructLayout(LayoutKind.Sequential)]
		private struct TCHITTESTINFO
		{
			public Point pt;
			public TCHITTESTFLAGS flags;
			public TCHITTESTINFO(int x, int y)
			{
				pt = new Point(x, y);
				flags = TCHITTESTFLAGS.TCHT_NOWHERE;
			}
		}

		[Flags()]
		private enum TCHITTESTFLAGS
		{
			TCHT_NOWHERE = 1,
			TCHT_ONITEMICON = 2,
			TCHT_ONITEMLABEL = 4,
			TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
		}

		private const int WM_NULL = 0x0;
		private const int WM_SETFONT = 0x30;
		private const int WM_FONTCHANGE = 0x1D;
		private const int WM_MOUSEDOWN = 0x201;

		private const int TCM_FIRST = 0x1300;
		private const int TCM_HITTEST = TCM_FIRST + 13;
		private const int TCM_SETPADDING = TCM_FIRST + 43;
		
		#endregion

	}
}
