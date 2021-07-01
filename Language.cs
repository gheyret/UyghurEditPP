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
				if(LanguaID.Equals("uey")){
					ret = Uyghur.ULY2UEY(key);
				}
				else if(LanguaID.Equals("usy")){
					ret = Uyghur.ULY2USY(key);
				}
			}
			return ret;
		}
		
		public void Save(string filename){
			string jstr = "{" + gBuf.ToString()+ "}";
			File.WriteAllText(filename,jstr,Encoding.UTF8);
		}
	}
}
