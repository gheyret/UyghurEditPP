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
			get{ return 14; }
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
			base.OnPaintBackground(pevent);
			for (int id = 0; id < this.TabCount; id++)
				DrawTabBackground(pevent.Graphics, id);

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
		
		private void DrawTabBackground(Graphics graphics, int id)
		{
			if (id == SelectedIndex){
				graphics.FillRectangle(Brushes.White, GetTabRect(id));
			}
			else if (id == HotTabIndex)
			{
				Rectangle rc = GetTabRect(id);
				rc.Width--;
				rc.Height--;
				graphics.DrawRectangle(Pens.DarkGray, rc);
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

			if (tabImage != null)
			{
				textrect.Width -= tabImage.Width;
				textrect.X += tabImage.Width;
			}

			Color frColor = id == SelectedIndex ? Color.Black : this.ForeColor;
			Color bkColor = id == SelectedIndex ? Color.White : this.BackColor;

			using (Bitmap bm = new Bitmap(contentRect.Width, contentRect.Height))
			{
				using (Graphics bmGraphics = Graphics.FromImage(bm))
				{
					TextRenderer.DrawText(bmGraphics, this.TabPages[id].Text, this.Font, textrect, frColor, bkColor);
					if (selectedOrHot)
					{
						Rectangle closeRect = new Rectangle(contentRect.Right - CloseButtonHeight, 0, CloseButtonHeight, CloseButtonHeight);
						closeRect.Offset(-2, (contentRect.Height - closeRect.Height) / 2);
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
			using (Font closeFont = new Font("Tahoma", 9, FontStyle.Bold))
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
	
	class MyRenderer : ToolStripProfessionalRenderer
	{
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			var btn = e.Item as ToolStripButton;
			if (btn != null && btn.CheckOnClick && btn.Checked)
			{
				Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
				Color c = Color.FromArgb(0, 147, 230); //Your custom color here
				var brush = new SolidBrush(c);
				e.Graphics.FillRectangle(brush, bounds);
			}
			else base.OnRenderButtonBackground(e);
		}
	}
}
