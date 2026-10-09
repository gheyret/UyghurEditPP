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
		
		// For tests: texts from the given JSON instead of langdata.json.
		internal Language(string json)
		{
			gLangJson = DynaJson.Parse(json);
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
		
		public String GetText(String key)
		{
			
			String ret=key;
			if(gLangJson!=null && (LanguaID.Equals("jpn")||LanguaID.Equals("eng"))){
				// A missing Japanese text falls back to English, a missing English text to the key.
				if(gLangJson.IsDefined(key)){
					if(LanguaID.Equals("jpn")){
						ret = Terjime(key, "jpn");
					}
					if(string.IsNullOrEmpty(ret) || LanguaID.Equals("eng")){
						ret = Terjime(key, "eng");
					}
					if(string.IsNullOrEmpty(ret)){
						ret = key;
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

		/// <summary>
		/// Puts a LEFT-TO-RIGHT MARK at both ends of each line of text, so Latin text keeps its
		/// own order (and its punctuation stays in place) inside right-to-left text. (The isolate
		/// controls LRI/PDI would be cleaner, but message boxes draw them as boxes.)
		/// </summary>
		public static string LeftToRightRun(string text)
		{
			string[] qurlar = text.Split('\n');
			for(int i = 0; i < qurlar.Length; i++){
				string qur = qurlar[i];
				bool cr = qur.EndsWith("\r", StringComparison.Ordinal);
				if(cr){
					qur = qur.Substring(0, qur.Length - 1);
				}
				if(qur.Length > 0){
					qur = "\u200E" + qur + "\u200E";
				}
				qurlar[i] = cr ? qur + "\r" : qur;
			}
			return string.Join("\n", qurlar);
		}

		// In an "uly" text, parts in {braces} (product names, file types ...) stay in Latin
		// letters in every script; only the rest is converted. In UEY they are isolated as
		// left-to-right runs so they keep their order inside the right-to-left text.
		static readonly System.Text.RegularExpressions.Regex gLatinche = new System.Text.RegularExpressions.Regex(@"\{([^{}]*)\}");

		internal static string Yeziqla(string uly, string languaID)
		{
			StringBuilder sb = new StringBuilder();
			int orun = 0;
			foreach(System.Text.RegularExpressions.Match m in gLatinche.Matches(uly)){
				sb.Append(Aylandur(uly.Substring(orun, m.Index - orun), languaID));
				if(languaID.Equals("uey")){
					sb.Append(LeftToRightRun(m.Groups[1].Value));
				}
				else{
					sb.Append(m.Groups[1].Value);
				}
				orun = m.Index + m.Length;
			}
			sb.Append(Aylandur(uly.Substring(orun), languaID));
			return sb.ToString();
		}
		
		// The text of key in the given language, or null when it has none.
		string Terjime(string key, string til)
		{
			if(!gLangJson[key].IsDefined(til)){
				return null;
			}
			return gLangJson[key][til];
		}
	}
}
