/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2021/01/22
 * Time: 9:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

/*
 * Created by SharpDevelop.
 * User: gheyret
 * Date: 2010/01/05
 * Time: 16:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of TstTree.
	/// </summary>
	public class KenjiSpell
	{
		private UNode m_RootNode=null;
		private  int  m_SozSani=0;
		private int   nodeCnt=0;
		Dictionary<string,string> XataToghra = new Dictionary<string, string>();
		List<string> Namzatlar = new List<string>();
		public KenjiSpell()
		{
		}

		public int Add(String soz,  Int64 tekrar=1)
		{
			string szWord = soz.Replace(Uyghur.Sozghuch,"").Trim().ToLower();
			if(szWord.Trim().Length==0){
				return 0;
			}
			szWord+='\0';
			if(m_RootNode==null)
			{
				m_RootNode=new UNode();
				m_RootNode.mHerp=szWord[0];
				nodeCnt++;
			}

			UNode curNode=m_RootNode;
			int pos=0;
			char Herp=szWord[pos];
			while(true){
				if(curNode.mHerp==Herp)
				{
					if(Herp=='\0')
					{
						if(curNode.mFreq==0){
							m_SozSani++;
						}
						curNode.mFreq = (curNode.mFreq+tekrar)<short.MaxValue?(short)(curNode.mFreq+tekrar):short.MaxValue;
						break;
					}
					pos++;
					Herp=szWord[pos];
					if(curNode.mNext==null)
					{
						curNode.mNext=new UNode();
						curNode.mNext.mHerp=Herp;
						nodeCnt++;
					}
					curNode=curNode.mNext;
				}
				else{
					if(curNode.mAlter==null){
						curNode.mAlter=new UNode();
						curNode.mAlter.mHerp=Herp;
						nodeCnt++;
					}
					curNode=curNode.mAlter;
				}
			}
			return curNode.mFreq;
		}

		public bool IsListed(String szWord)
		{
			if (m_RootNode == null) return false;
			bool ret=_IsWordListed(m_RootNode,szWord.ToLower());
			return ret;
		}
		
		private bool _IsWordListed(UNode node,String szWord)
		{
			UNode curNode=node;
			szWord+='\0';
			int len=szWord.Length;
			int pos=0;
			char Herp;
			bool ret=false;
			Herp=szWord[pos];
			while(curNode!=null)
			{
				if(curNode.mHerp==Herp)
				{
					if(curNode.mHerp=='\0')
					{
						ret = true;
						break;
					}
					pos++;
					Herp=szWord[pos];
					curNode = curNode.mNext;
				}
				else
				{
					curNode=curNode.mAlter;
				}
			}
			return ret;
		}
		

		public int WordCount{
			get{
				return m_SozSani;
			}
		}
		
		public bool LoadDictionary(string corpus, Uyghur.YEZIQ yeziq)
		{
			
			if (!File.Exists(corpus)) return false;
			return LoadDictionary(File.OpenRead(corpus), yeziq);
		}
		
		public bool LoadDictionary(Stream instr,Uyghur.YEZIQ yeziq)
		{
			using (StreamReader sr = new StreamReader(instr,System.Text.Encoding.UTF8))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
				{
					string[] lineParts = line.Split(null);
					if (lineParts.Length >= 2)
					{
						string key = lineParts[0];
						if(yeziq==Uyghur.YEZIQ.ULY){
							key  = Uyghur.UEY2ULY(key).ToLower();
						}
						else if(yeziq==Uyghur.YEZIQ.USY){
							key  = Uyghur.UEY2USY(key).ToLower();
						}
						Int64 count;
						if (Int64.TryParse(lineParts[1], out count))
						{
							Add(key, count);
						}
					}
				}
			}
			return true;
		}

		
		public bool LoadXataToghra(string xatatoghra, Uyghur.YEZIQ yeziq)
		{
			if (!File.Exists(xatatoghra)) return false;
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
		
		
		private void _GetSuggestions(String qelip)
		{
			_GetSuggestions(m_RootNode,qelip+'\0',0,"");
			//System.Diagnostics.Debug.WriteLine(qelip + " --> " + Namzatlar.Count);
			return;
		}

		public List<string> TestNamzat(String qelip)
		{
			Namzatlar.Clear();
			if (m_RootNode != null){
				_GetSuggestions(m_RootNode,qelip+'\0',0,"");
			}
			return Namzatlar;
		}


		private void _GetSuggestions(UNode curNode,String Soz,int pos,String namzat)
		{
			if (curNode==null) return;
			char Herp=Soz[pos];
			if(curNode.mHerp==Herp||Herp=='?'||Herp=='*')
			{
				if(curNode.mHerp==0x0 && Herp=='\0')
				{
					if(!Namzatlar.Contains(namzat)){
						Namzatlar.Add(namzat);
					}
				}
				else if(Herp=='*'){
					_SearchAll(curNode,namzat);
				}
				else{
					_GetSuggestions(curNode.mNext, Soz, pos+1,namzat+curNode.mHerp);
					_GetSuggestions(curNode.mAlter, Soz, pos,namzat);
				}
			}
			else
			{
				_GetSuggestions(curNode.mAlter, Soz, pos,namzat);
			}
		}
		
		
		private void _SearchAll(UNode curNode,String Soz){
			if(curNode!=null){
				if(curNode.mHerp==0x0){
					if(!Namzatlar.Contains(Soz))Namzatlar.Add(Soz);
				}
				_SearchAll(curNode.mNext, Soz+curNode.mHerp);
				_SearchAll(curNode.mAlter, Soz);
			}
		}
		
		//Namzat Sozlerni tepip chiqidu
		public List<string> Lookup(string Soz)
		{
			Namzatlar.Clear();
			if (m_RootNode == null) return Namzatlar;
			char[] herpler;
			String yasSoz;
			Soz = Soz.ToLower();
			int lenSoz=Soz.Length;
			int i;

			//Barliq sozuq tashuwshlarni xatalashqan dep perez qilsaq
			herpler = Soz.ToCharArray();
			for(i = 0;i<herpler.Length;i++){
				if(Uyghur.IsSozuq(herpler[i])){
					herpler[i]='?';
				}
			}
			yasSoz=new String(herpler);
			_GetSuggestions(yasSoz);
			//if(Namzatlar.Count!=0) return Namzatlar;

			//Yuqiridiki barliq sozuq tawushlar xata dep qarap, andin bir bir herpning arqisidin birdin soz chushup qalghan dep qarisaq
			//Herp Bir Herpning aldida birdin herp chushup qalghan dep perez qilsaq
			for(i=lenSoz;i>=0;i--)
			{
				_GetSuggestions(yasSoz.Insert(i,"?"));
			}

			//Her bir herp xatalashqan dep perz qilghanda
			for(i=lenSoz-1;i>=0;i--)
			{
				herpler=Soz.ToCharArray();
				herpler[i]='?';
				yasSoz=new String(herpler);
				_GetSuggestions(yasSoz);
			}

			//Her bir herpni artuqche daep perez qilghanda
			for(i=lenSoz-1;i>=0;i--)
			{
				yasSoz=Soz.Remove(i,1);
				_GetSuggestions(yasSoz);
			}
			
			//Herp Bir Herpning aldida birdin herp chushup qalghan dep perez qilsaq
			for(i=lenSoz;i>=0;i--)
			{
				yasSoz = Soz.Insert(i,"?");
				_GetSuggestions(yasSoz);
			}
			
			//Her bir herpning iki yenidiki herp xata
			for(i=lenSoz-2;i>=1;i--)
			{
				herpler=Soz.ToCharArray();
				herpler[i-1]='?';
				herpler[i+1]='?';
				yasSoz=new String(herpler);
				_GetSuggestions(yasSoz);
			}
			
			//Qoshna ikki herp xatalashqan dep perz qilghanda
			for(i=lenSoz-2;i>=0;i--)
			{
				herpler=Soz.ToCharArray();
				herpler[i]='?';
				herpler[i+1]='?';
				yasSoz=new String(herpler);
				_GetSuggestions(yasSoz);
			}

			//Her Bir Herpning ikki yenidin birdin herp chushup qalghan
			for(i=lenSoz;i>=1;i--)
			{
				yasSoz = Soz.Insert(i-1,"?").Insert(i+1,"?");
				_GetSuggestions(yasSoz);
			}
			
			//Her ikki Herpning ikki yenidin birdin herp chushup qalghan
			for(i=lenSoz-1;i>=1;i--)
			{
				yasSoz = Soz.Insert(i+1,"?").Insert(i-1,"?");
				_GetSuggestions(yasSoz);
			}

			if(Namzatlar.Count!=0) return Namzatlar;
			
			//Qoshna 3 herp xatalashqan dep perz qilghanda
			for(i=lenSoz-3;i>=0;i--)
			{
				herpler=Soz.ToCharArray();
				herpler[i]='?';
				herpler[i+1]='?';
				herpler[i+2]='?';
				yasSoz=new String(herpler);
				_GetSuggestions(yasSoz);
			}
									
			//Qoshna ikki herp esli birla herp idi
			for(i=lenSoz-2;i>=0;i--)
			{
				yasSoz=Soz.Remove(i,2).Insert(i,"?");
				_GetSuggestions(yasSoz);
			}

			//Oz-ara Qoshna ikki herp artuqche daep perez qilghanda
			for(i=lenSoz-2;i>=0;i--)
			{
				yasSoz=Soz.Remove(i,2);
				_GetSuggestions(yasSoz);
			}

			//Qoshna 3 herp esli ikki herp idi
			for(i=lenSoz-3;i>=0;i--)
			{
				yasSoz=Soz.Remove(i,3).Insert(i,"??");
				_GetSuggestions(yasSoz);
			}
			
			//Belkim qoshumche xata, shunga eng keynidin 4 heripni ochuruwetip korup baqidu
			//Oz-ara Qoshna 3 herp artuqche daep perez qilghanda
			if(lenSoz>6){
				for(i=1;i<4;i++)
				{
					yasSoz=Soz.Substring(0,lenSoz-i);
					_GetSuggestions(yasSoz);
				}
			}
			return Namzatlar;
		}
	}
}