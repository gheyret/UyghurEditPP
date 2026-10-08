/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2021/01/26
 * Time: 8:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
namespace UyghurEditPP
{
	public abstract class UyghurSpell{
		public abstract Int64 Add(String soz,  Int64 tekrar=1);
		public abstract bool IsListed(String szWord, bool siziqtekshur=true);
		public abstract int WordCount{
			get;
		}
		protected abstract bool LoadDictionary(string corpus, Uyghur.YEZIQ yeziq);
		protected abstract bool LoadDictionary(Stream instr,Uyghur.YEZIQ yeziq);
		public abstract List<string> Lookup(string Soz);
		
		Dictionary<string,string> XataToghra = new Dictionary<string, string>();
		
		HashSet<string>           IshletkuchiDic = new HashSet<string>();
		List<string>              XataToghraBuf  = new List<string>(); // in file order: later lines win
		
		protected Uyghur.YEZIQ gYeziq;

		string gImlaIshletkuchi = AppPaths.IshletkuchiFileName;
		string gImlaXataToghra = AppPaths.XataToghraFileName;
		// The correction list that comes with the program. The user's own corrections
		// are kept separately (gImlaXataToghra), so a newer list in a new release is used.
		string gImlaXataToghraAsasi;
		
		
		internal UyghurSpell(){
			gImlaIshletkuchi = AppPaths.DataFile(gImlaIshletkuchi);
			gImlaXataToghra = AppPaths.DataFile(gImlaXataToghra);
			gImlaXataToghraAsasi = AppPaths.ProgramFile(AppPaths.XataToghraFileName);
		}
		
		// For the tests.
		internal void SetFileNames(string ishletkuchi, string xataToghra, string xataToghraAsasi){
			gImlaIshletkuchi = ishletkuchi;
			gImlaXataToghra = xataToghra;
			gImlaXataToghraAsasi = xataToghraAsasi;
		}
		
		public string IshletkcuhiAmbarIsimi{
			get{
				return gImlaIshletkuchi;
			}
		}
		
		public string XataToghraAmbarIsimi{
			get{
				return gImlaXataToghra;
			}
		}
		
		public bool Load(Stream instr, Uyghur.YEZIQ yeziq)
		{
			gYeziq = yeziq;
			XataToghra.Clear();
			
			bool ret = LoadDictionary(instr,yeziq);
			if(IshletkuchiDic.Count==0){
				ReadIshletkuchiDic();
			}
			string newsoz;
			foreach(string soz in IshletkuchiDic){
				Add(UEYdin(soz));
			}
			
			
			if(XataToghraBuf.Count==0){
				ReadXataToghra();
			}

			foreach(string qur in XataToghraBuf){
				newsoz = UEYdin(qur);
				string[] tx = newsoz.Split('=');
				if(tx.Length==2){
					XataToghra[tx[0].Trim()]=tx[1].Trim();
//					if(!IsListed(tx[1].Trim())){
//						Add(tx[1].Trim());
//					}
				}
				else{
					System.Diagnostics.Debug.WriteLine(newsoz);
				}
			}
			return ret;
		}
		
		// The user files keep words in UEY; this converts one to the script of this dictionary.
		string UEYdin(string uey)
		{
			if(gYeziq==Uyghur.YEZIQ.ULY){
				return Uyghur.UEY2ULY(uey).ToLowerInvariant();
			}
			if(gYeziq==Uyghur.YEZIQ.USY){
				return Uyghur.UEY2USY(uey).ToLowerInvariant();
			}
			return uey;
		}
		
		/// <summary>
		/// Adds a word the user marked as correct while another script's dictionary was in
		/// use (that one saved it to the user file), so this loaded dictionary knows it too.
		/// </summary>
		public void IshletkuchiSozQosh(string soz)
		{
			soz = soz.ToLowerInvariant().Replace(Uyghur.Sozghuch,"");
			soz = Uyghur.ToUEY(soz)?? soz;
			if(IshletkuchiDic.Add(soz)){
				Add(UEYdin(soz));
			}
		}
		
		/// <summary>
		/// Adds a correction the user chose while another script's dictionary was in use.
		/// </summary>
		public void XataToghraQosh(string xata, string toghra)
		{
			string bk = xata.Trim().Replace(Uyghur.Sozghuch,"").ToLowerInvariant()+"="+toghra.Trim().Replace(Uyghur.Sozghuch,"").ToLowerInvariant();
			bk = Uyghur.ToUEY(bk)?? bk;
			XataToghraBuf.Add(bk);
			string[] tx = UEYdin(bk).Split('=');
			if(tx.Length==2){
				XataToghra[tx[0].Trim()]=tx[1].Trim();
			}
		}
		
		void ReadIshletkuchiDic()
		{
			if(!File.Exists(gImlaIshletkuchi)) return;
			using (StreamReader sr = new StreamReader(OpenShared(gImlaIshletkuchi),true))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
				{
					string[] lineParts = line.Split(null);
					if (lineParts.Length >= 1)
					{
						string key = lineParts[0];
						IshletkuchiDic.Add(key);
					}
				}
			}
		}
		
		void ReadXataToghra()
		{
			XataToghra.Clear();
			// The user's corrections come last, so they win over the shipped list.
			ReadXataToghra(gImlaXataToghraAsasi);
			ReadXataToghra(gImlaXataToghra);
		}
		
		void ReadXataToghra(string filenm)
		{
			if (!File.Exists(filenm)) return;
			using (StreamReader sr = new StreamReader(OpenShared(filenm),true))
			{
				String qur;
				while ((qur = sr.ReadLine()) != null)
				{
					XataToghraBuf.Add(qur);
				}
			}
			return;
		}
		
		public void SaveToXataToghra(string xata, string toghra)
		{
			if(!XataToghra.ContainsKey(xata))
			{
				string filenm =gImlaXataToghra;
				string xt    = xata.Trim().Replace(Uyghur.Sozghuch,"").ToLowerInvariant();
				string togh  = toghra.Trim().Replace(Uyghur.Sozghuch,"").ToLowerInvariant();
				string bk = xt+"="+togh;
				XataToghra[xt]=togh;				
				bk= Uyghur.ToUEY(bk)?? bk;
				XataToghraBuf.Add(bk);
				try{
					AppendLine(filenm, bk);
				}catch(Exception ee){
					SaveFailed(filenm, ee);
				}
			}
		}
		
		public string Toghrisi(string xatasi){
			string toghrisi = null;
			XataToghra.TryGetValue(xatasi.ToLowerInvariant().Replace(Uyghur.Sozghuch,""), out toghrisi);
			return toghrisi;
		}
		
		public void SaveToIshletkuchi(string soz)
		{
			string filenm =gImlaIshletkuchi;
			soz = soz.ToLowerInvariant().Replace(Uyghur.Sozghuch,"");
			soz = Uyghur.ToUEY(soz)?? soz;
			IshletkuchiDic.Add(soz);
			try{
				AppendLine(filenm, soz+ " 1");
			}catch(Exception ee){
				SaveFailed(filenm, ee);
			}
		}

		// Another dictionary may be reading the same user file on a background thread, so
		// readers allow writers and the other way round.
		static Stream OpenShared(string filenm)
		{
			return new FileStream(filenm, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		}
		
		static void AppendLine(string filenm, string line)
		{
			byte[] data = System.Text.Encoding.UTF8.GetBytes(line + System.Environment.NewLine);
			for(int tekrar = 0; ; tekrar++){
				try{
					bool yengi = !File.Exists(filenm);
					using(FileStream fs = new FileStream(filenm, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)){
						if(yengi && fs.Length == 0){
							byte[] bom = System.Text.Encoding.UTF8.GetPreamble(); // as File.AppendAllText(..., UTF8) wrote it
							fs.Write(bom, 0, bom.Length);
						}
						fs.Write(data, 0, data.Length);
					}
					return;
				}
				catch(IOException){
					if(tekrar >= 1) throw;
					System.Threading.Thread.Sleep(100);
				}
			}
		}
		
		void SaveFailed(string filenm, Exception ee)
		{
			ErrorLog.Write(ee);
			CenteredMessageBox.Show(CenteredMessageBox.MainWindow, MainForm.gLang.GetText("Could not save the word to the user dictionary:") + Environment.NewLine + filenm + Environment.NewLine + Environment.NewLine + ee.Message,
			                "UyghurEdit++", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
	}
}