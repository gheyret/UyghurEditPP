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
			System.Diagnostics.Debug.WriteLine("Buyruq Quri: " +args.Length);
			MainForm frm = new MainForm();
			frm.Show();
			if(args.Length==1){
				frm.OpenaFile(args[0]);
			}
			Application.Run(frm);
//			KenjiSpell kImla = new KenjiSpell();
//			kImla.LoadDictionary("uyghur_imla.txt",Uyghur.YEZIQ.ULY);
//			System.Diagnostics.Debug.WriteLine(kImla.WordCount);
//			if(kImla.IsListed("dédeklermu")){
//				System.Diagnostics.Debug.WriteLine("bügün barken");
//			}
//			else{
//				System.Diagnostics.Debug.WriteLine("bügün yoqken");
//			}
//			if(kImla.IsListed("bugun")){
//				System.Diagnostics.Debug.WriteLine("bugun barken");
//			}
//			else{
//				System.Diagnostics.Debug.WriteLine("bugun yoqken");
//				List<string> namzat = kImla.Lookup("aghzini");
//				foreach(string nn in namzat){
//					System.Diagnostics.Debug.WriteLine(nn);
//				}
//			}
		}
	}
}
