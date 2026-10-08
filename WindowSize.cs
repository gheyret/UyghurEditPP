using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace UyghurEditPP
{
	/// <summary>
	/// The main window's size is saved in logical units (pixels at 100%, 96 DPI), the units the
	/// older, DPI-unaware versions saved it in, and turned into device pixels for the monitor the
	/// window opens on. Saving device pixels would make the size grow or shrink from launch to
	/// launch when the window moves between monitors with different DPI. The position stays in
	/// screen pixels: it is what tells which monitor the window is on.
	/// </summary>
	public static class WindowSize
	{
		public const int LogicalDpi = 96;

		public static Size ToLogical(Size device, int dpi)
		{
			return new Size(Scale(device.Width, LogicalDpi, dpi), Scale(device.Height, LogicalDpi, dpi));
		}

		public static Size ToDevice(Size logical, int dpi)
		{
			return new Size(Scale(logical.Width, dpi, LogicalDpi), Scale(logical.Height, dpi, LogicalDpi));
		}

		static int Scale(int value, int to, int from)
		{
			if(from <= 0 || to <= 0){
				return value;
			}
			return (int)Math.Round(value * (double)to / from, MidpointRounding.AwayFromZero);
		}

		[DllImport("user32.dll")] static extern IntPtr MonitorFromPoint(POINT pt, uint flags);
		[DllImport("shcore.dll")] static extern int GetDpiForMonitor(IntPtr monitor, int type, out uint dpiX, out uint dpiY);
		[StructLayout(LayoutKind.Sequential)] struct POINT { public int X, Y; }
		const uint MONITOR_DEFAULTTONEAREST = 2;
		const int MDT_EFFECTIVE_DPI = 0;

		/// <summary>
		/// The DPI of the monitor at a screen point (GetDpiForMonitor, Windows 8.1 or later:
		/// https://learn.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getdpiformonitor );
		/// fallback when it cannot be read.
		/// </summary>
		public static int DpiAt(Point screenPoint, int fallback)
		{
			try{
				IntPtr monitor = MonitorFromPoint(new POINT { X = screenPoint.X, Y = screenPoint.Y }, MONITOR_DEFAULTTONEAREST);
				uint x, y;
				if(monitor != IntPtr.Zero && GetDpiForMonitor(monitor, MDT_EFFECTIVE_DPI, out x, out y) == 0 && x > 0){
					return (int)x;
				}
			}
			catch(DllNotFoundException){
			}
			catch(EntryPointNotFoundException){
			}
			return fallback;
		}
	}
}
