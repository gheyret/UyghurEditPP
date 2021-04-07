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
		public abstract bool IsListed(String szWord);
		public abstract int WordCount{
			get;
		}
		public abstract bool LoadDictionary(string corpus, Uyghur.YEZIQ yeziq);
		public abstract bool LoadDictionary(Stream instr,Uyghur.YEZIQ yeziq);
		public abstract List<string> Lookup(string Soz);
		
		Dictionary<string,string> XataToghra = new Dictionary<string, string>();
		
		HashSet<string>           IshletkuchiDic = new HashSet<string>();
		HashSet<string>           XataToghraBuf  = new HashSet<string>();
		
		protected Uyghur.YEZIQ gYeziq;

		string gImlaIshletkuchi = "imla_ishletkuchi.txt";
		string gImlaXataToghra = "imla_xatatoghra.txt";
		
		
		internal UyghurSpell(){
			gImlaIshletkuchi = Path.Combine(Application.StartupPath, gImlaIshletkuchi);
			gImlaXataToghra = Path.Combine(Application.StartupPath, gImlaXataToghra);
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
				if(yeziq==Uyghur.YEZIQ.ULY){
					newsoz  = Uyghur.UEY2ULY(soz).ToLower();
				}
				else if(yeziq==Uyghur.YEZIQ.USY){
					newsoz  = Uyghur.UEY2USY(soz).ToLower();
				}
				else{
					newsoz = soz;
				}
				Add(newsoz);
			}
			
			
			if(XataToghraBuf.Count==0){
				ReadXataToghra();
			}

			foreach(string qur in XataToghraBuf){
				if(yeziq==Uyghur.YEZIQ.ULY){
					newsoz = Uyghur.UEY2ULY(qur).ToLower();
				}
				else if(yeziq==Uyghur.YEZIQ.USY){
					newsoz = Uyghur.UEY2USY(qur).ToLower();
				}
				else{
					newsoz  = qur;
				}
				string[] tx = newsoz.Split('=');
				if(tx.Length==2){
					XataToghra[tx[0].Trim()]=tx[1].Trim();
				}
				if(!IsListed(tx[1].Trim())){
					Add(tx[1].Trim());
				}
			}
			return ret;
		}
		
		void ReadIshletkuchiDic()
		{
			if(!File.Exists(gImlaIshletkuchi)) return;
			using (StreamReader sr = new StreamReader(File.OpenRead(gImlaIshletkuchi),true))
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

			if (!File.Exists(gImlaXataToghra)) return;
			XataToghra.Clear();
			using (StreamReader sr = new StreamReader(File.OpenRead(gImlaXataToghra),true))
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
				string filenm =Path.Combine(Application.StartupPath, gImlaXataToghra);
				string xt    = xata.Trim().Replace(Uyghur.Sozghuch,"").ToLower();
				string togh  = toghra.Trim().Replace(Uyghur.Sozghuch,"").ToLower();
				string bk = xt+"="+togh;
				bk= Uyghur.ToUEY(bk)?? bk;
				XataToghraBuf.Add(bk);
				try{
					File.AppendAllText(filenm,bk+System.Environment.NewLine,System.Text.Encoding.UTF8);
				}catch{
					
				}
			}
		}
		
		public string Toghrisi(string xatasi){
			string toghrisi = null;
			XataToghra.TryGetValue(xatasi.ToLower().Replace(Uyghur.Sozghuch,""), out toghrisi);
			return toghrisi;
		}
		
		public void SaveToIshletkuchi(string soz)
		{
			string filenm =Path.Combine(Application.StartupPath, gImlaIshletkuchi);
			soz = soz.ToLower().Replace(Uyghur.Sozghuch,"");
			soz = Uyghur.ToUEY(soz)?? soz;
			IshletkuchiDic.Add(soz);
			try{
				File.AppendAllText(filenm, soz+ " 1" +System.Environment.NewLine,System.Text.Encoding.UTF8);
			}catch{
				
			}
		}
	}
}