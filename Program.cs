/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2020/11/16
 * Time: 8:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace UyghurEditPP
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			UsePerMonitorDpi();
			AppFonts.Load();
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += ThreadException;
			AppDomain.CurrentDomain.UnhandledException += UnhandledException;
			System.Windows.Threading.Dispatcher.CurrentDispatcher.UnhandledException += DispatcherUnhandledException;
			AppPaths.Prepare();
			try{
				MainForm frm = new MainForm();
				frm.Show();
				// Every file given (e.g. several files dropped on the exe, or "Open with" on a
				// selection), each in its own tab; with more than one, none was opened before.
				foreach(string arg in args){
					// A file that cannot be opened (no permission, locked ...) is reported, and
					// the program still starts with the others.
					try{
						frm.OpenaFile(arg);
					}
					catch(Exception ee){
						ShowError(ee);
					}
				}
				Application.Run(frm);
			}catch(Exception ee){
				ShowError(ee);
			}
		}

		static void ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			ShowError(e.Exception);
		}

		static void DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			ShowError(e.Exception);
			e.Handled = true;
		}

		static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			ShowError(ex ?? new Exception(Convert.ToString(e.ExceptionObject)));
		}

		/// <summary>
		/// Logs the exception and tells the user where the log is.
		/// </summary>
		static void ShowError(Exception ex)
		{
			string logFile = ErrorLog.Write(ex);
			// While the message box is open, messages are still processed and the same
			// error can happen again; log those, but do not stack up message boxes.
			if(System.Threading.Interlocked.CompareExchange(ref gShowingError, 1, 0) != 0){
				return;
			}
			try{
				string msg = Text("An unexpected error occurred.") + Environment.NewLine + Environment.NewLine + CenteredMessageBox.LeftToRight(ex.Message);
				if(logFile!=null){
					msg += Environment.NewLine + Environment.NewLine + Text("Details were saved to:") + Environment.NewLine + CenteredMessageBox.LeftToRight(logFile);
				}
				CenteredMessageBox.Show(CenteredMessageBox.MainWindow, msg,"UyghurEdit++",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee);
			}
			finally{
				System.Threading.Interlocked.Exchange(ref gShowingError, 0);
			}
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern bool SetProcessDpiAwarenessContext(IntPtr value);
		static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = new IntPtr(-4);

		// app.config asks Windows Forms for Per-Monitor V2, but Windows Forms applies it only when
		// it creates its first window; the WPF editor is created before that and sets the process
		// to system DPI aware first. So set it here, before any window (Windows 10 1703 or later;
		// on older Windows the call is missing and the program stays as before).
		static void UsePerMonitorDpi()
		{
			try{
				SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
			}
			catch(EntryPointNotFoundException){
			}
		}

		// 1 while an error message is shown (errors can come from other threads too).
		static int gShowingError = 0;

		// The error may come from MainForm itself, so fall back to the English key.
		static string Text(string key)
		{
			try{
				return MainForm.gLang.GetText(key);
			}
			catch(Exception){
				return key;
			}
		}
	}
}
