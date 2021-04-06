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
			try{
				MainForm frm = new MainForm();
				frm.Show();
				if(args.Length==1){
					frm.OpenaFile(args[0]);
				}
				Application.Run(frm);
			}catch(Exception ee){
				MessageBox.Show(ee.StackTrace,"UyghurEdit++",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
	}
}
