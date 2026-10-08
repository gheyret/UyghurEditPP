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
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += ThreadException;
			AppDomain.CurrentDomain.UnhandledException += UnhandledException;
			System.Windows.Threading.Dispatcher.CurrentDispatcher.UnhandledException += DispatcherUnhandledException;
			AppPaths.Prepare();
			try{
				MainForm frm = new MainForm();
				frm.Show();
				if(args.Length==1){
					frm.OpenaFile(args[0]);
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
