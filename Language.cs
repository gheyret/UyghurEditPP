/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2009/06/15
 * Time: 10:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of Language.
	/// </summary>

	public class Language
	{
		dynamic gLangJson = null;
		public Language()
		{
			LoadLanguageData();
			LanguaID ="uly";
		}
		
		public String LanguaID{
			get;
			set;
		}

		void LoadLanguageData(){
			try{
				String strRes ="UyghurEditPP.langdata.json";
				System.Reflection.Assembly asm =System.Reflection.Assembly.GetExecutingAssembly();
				Stream stm=asm.GetManifestResourceStream(strRes);
				if(stm==null) return;
				System.IO.StreamReader rd=new StreamReader(stm,System.Text.Encoding.UTF8);
				String jsonstr = rd.ReadToEnd();
				gLangJson = DynaJson.Parse(jsonstr);
				rd.Close();
				stm.Close();
			}catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee.Message);
			}
		}
		
		StringBuilder gBuf = new StringBuilder();
		List<string> gTmp = new List<string>();
		public String GetText(String key)
		{
			
			String ret=key;
			if(gLangJson!=null && (LanguaID.Equals("jpn")||LanguaID.Equals("eng"))){
				if(gLangJson.IsDefined(key)){
					ret = gLangJson[key][LanguaID];
					if(string.IsNullOrEmpty(ret)){
						ret = key;
					}
				}
				else{
					if(gTmp.Contains(key)==false){
						gTmp.Add(key);
						string str= String.Format("\"{0}\":{{\"jpn\":\"\",\"eng\":\"\"}},",key);
						gBuf.AppendLine(str);
					}
				}
			}
			else{
				// Newer messages use their English text as the key and keep the
				// Uyghur (ULY) text in an "uly" field. Until that field is filled in,
				// the English text is shown as is.
				if(gLangJson!=null && gLangJson.IsDefined(key) && gLangJson[key].IsDefined("uly")){
					string uly = gLangJson[key]["uly"];
					if(string.IsNullOrEmpty(uly)){
						return key;
					}
					return Yeziqla(uly, LanguaID);
				}
				ret = Aylandur(key, LanguaID);
			}
			return ret;
		}

		static string Aylandur(string uly, string languaID)
		{
			if(languaID.Equals("uey")){
				return Uyghur.ULY2UEY(uly).Replace("🠊", "🠈");
			}
			if(languaID.Equals("usy")){
				return Uyghur.ULY2USY(uly);
			}
			return uly;
		}

		// In an "uly" text, parts in {braces} (product names, file types ...) stay in Latin
		// letters in every script; only the rest is converted. In UEY they are wrapped in
		// LEFT-TO-RIGHT MARKs so they keep their order inside the right-to-left text.
		static readonly System.Text.RegularExpressions.Regex gLatinche = new System.Text.RegularExpressions.Regex(@"\{([^{}]*)\}");

		internal static string Yeziqla(string uly, string languaID)
		{
			StringBuilder sb = new StringBuilder();
			int orun = 0;
			foreach(System.Text.RegularExpressions.Match m in gLatinche.Matches(uly)){
				sb.Append(Aylandur(uly.Substring(orun, m.Index - orun), languaID));
				if(languaID.Equals("uey")){
					sb.Append('‎').Append(m.Groups[1].Value).Append('‎');
				}
				else{
					sb.Append(m.Groups[1].Value);
				}
				orun = m.Index + m.Length;
			}
			sb.Append(Aylandur(uly.Substring(orun), languaID));
			return sb.ToString();
		}
		
		public void Save(string filename){
			string jstr = "{" + gBuf.ToString()+ "}";
			File.WriteAllText(filename,jstr,Encoding.UTF8);
		}
	}
}
