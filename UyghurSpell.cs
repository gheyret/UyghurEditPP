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
		protected Uyghur.YEZIQ gYeziq;

		public bool LoadXataToghra(string xatatoghra, Uyghur.YEZIQ yeziq)
		{
			if (!File.Exists(xatatoghra)) return false;
			XataToghra.Clear();
			using (StreamReader sr = new StreamReader(File.OpenRead(xatatoghra),System.Text.Encoding.UTF8))
			{
				String qur;
				while ((qur = sr.ReadLine()) != null)
				{
					if(yeziq==Uyghur.YEZIQ.ULY){
						qur = Uyghur.UEY2ULY(qur).ToLower();
					}
					else if(yeziq==Uyghur.YEZIQ.USY){
						qur = Uyghur.UEY2USY(qur).ToLower();
					}
					string[] tx = qur.Split('=');
					if(tx.Length==2){
						XataToghra[tx[0].Trim()]=tx[1].Trim();
					}
				}
			}
			return true;
		}
		
		public void AddXataToghra(string xata, string toghra){
			XataToghra[xata.Trim().Replace(Uyghur.Sozghuch,"").ToLower()]=toghra.Trim().Replace(Uyghur.Sozghuch,"").ToLower();
		}
		
		public string Toghrisi(string xatasi){
			string toghrisi = null;
			XataToghra.TryGetValue(xatasi.ToLower().Replace(Uyghur.Sozghuch,""), out toghrisi);
			return toghrisi;
		}
	}
}