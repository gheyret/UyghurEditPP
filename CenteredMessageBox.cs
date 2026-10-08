/*
 * Shows message boxes centered on the window they belong to.
 *
 * MessageBox.Show places the box where Windows decides (usually the middle of the
 * screen), even when an owner window is given. A thread-local CBT hook catches the
 * moment the message box is activated, moves it to the center of the owner window
 * (kept inside the working area of the owner's monitor), and removes itself.
 */
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace UyghurEditPP
{
	public static class CenteredMessageBox
	{
		public static DialogResult Show(IWin32Window owner, string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
		{
			IntPtr ownerHandle = owner != null ? owner.Handle : IntPtr.Zero;
			return Run(ownerHandle, () => owner != null
				? MessageBox.Show(owner, text, caption, buttons, icon)
				: MessageBox.Show(text, caption, buttons, icon));
		}

		/// <summary>
		/// The main window, for message boxes shown from code that has no window of its own.
		/// </summary>
		public static IWin32Window MainWindow
		{
			get{
				foreach(Form form in Application.OpenForms){
					// Not from another thread (an error there): its handle may not be used here.
					if(form is MainForm && !form.IsDisposed && form.IsHandleCreated && !form.InvokeRequired){
						return form;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Runs show (which shows one message box, for example a WPF MessageBox) and centers
		/// that message box on ownerHandle.
		/// </summary>
		public static T Run<T>(IntPtr ownerHandle, Func<T> show)
		{
			if(ownerHandle == IntPtr.Zero){
				return show();
			}
			Hook hook = new Hook(ownerHandle);
			try{
				return show();
			}
			finally{
				hook.Dispose();
			}
		}

		sealed class Hook : IDisposable
		{
			readonly IntPtr gOwner;
			readonly HookProc gProc;   // kept in a field so the delegate is not collected
			IntPtr gHook;

			public Hook(IntPtr owner)
			{
				gOwner = owner;
				gProc = CbtProc;
				gHook = SetWindowsHookEx(WH_CBT, gProc, IntPtr.Zero, GetCurrentThreadId());
			}

			IntPtr CbtProc(int nCode, IntPtr wParam, IntPtr lParam)
			{
				IntPtr hook = gHook;
				if(nCode == HCBT_ACTIVATE && IsMessageBox(wParam)){
					try{
						Center(wParam, gOwner);
					}
					catch(Exception ee){
						System.Diagnostics.Debug.WriteLine(ee);
					}
					Dispose();  // only the first message box
				}
				return CallNextHookEx(hook, nCode, wParam, lParam);
			}

			public void Dispose()
			{
				if(gHook != IntPtr.Zero){
					UnhookWindowsHookEx(gHook);
					gHook = IntPtr.Zero;
				}
			}
		}

		static bool IsMessageBox(IntPtr hwnd)
		{
			StringBuilder sb = new StringBuilder(16);
			GetClassName(hwnd, sb, sb.Capacity);
			return sb.ToString() == "#32770";
		}

		static void Center(IntPtr box, IntPtr owner)
		{
			RECT o, b;
			if(IsIconic(owner) || !IsWindowVisible(owner) || !GetWindowRect(owner, out o) || !GetWindowRect(box, out b)){
				return;
			}
			Rectangle area = Screen.FromHandle(owner).WorkingArea;
			Point p = CenterIn(new Rectangle(o.Left, o.Top, o.Right - o.Left, o.Bottom - o.Top), new Size(b.Right - b.Left, b.Bottom - b.Top), area);
			SetWindowPos(box, IntPtr.Zero, p.X, p.Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
		}

		/// <summary>
		/// The top-left corner that centers a box of the given size on owner, moved as little
		/// as needed to stay inside area (the working area of the owner's monitor).
		/// </summary>
		internal static Point CenterIn(Rectangle owner, Size box, Rectangle area)
		{
			int x = owner.Left + (owner.Width - box.Width) / 2;
			int y = owner.Top + (owner.Height - box.Height) / 2;
			x = Math.Max(area.Left, Math.Min(x, area.Right - box.Width));
			y = Math.Max(area.Top, Math.Min(y, area.Bottom - box.Height));
			return new Point(x, y);
		}

		const int WH_CBT = 5;
		const int HCBT_ACTIVATE = 5;
		const uint SWP_NOSIZE = 0x0001;
		const uint SWP_NOZORDER = 0x0004;
		const uint SWP_NOACTIVATE = 0x0010;

		delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		[StructLayout(LayoutKind.Sequential)]
		struct RECT { public int Left, Top, Right, Bottom; }

		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);
		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr hhk);
		[DllImport("user32.dll")]
		static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
		[DllImport("kernel32.dll")]
		static extern uint GetCurrentThreadId();
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
		[DllImport("user32.dll")]
		static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
		[DllImport("user32.dll")]
		static extern bool IsIconic(IntPtr hWnd);
		[DllImport("user32.dll")]
		static extern bool IsWindowVisible(IntPtr hWnd);
		[DllImport("user32.dll")]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
	}
}
