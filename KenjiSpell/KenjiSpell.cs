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
	public class KenjiSpell: UyghurSpell,IComparer<NamzatQelip>
	{
		private UNode m_RootNode=null;
		private  int  m_SozSani=0;
		private int   nodeCnt=0;
		

		HashSet<string>   tmpNam = new HashSet<string>();
		List<NamzatQelip> _namzatlar = new List<NamzatQelip>();
		
		public KenjiSpell()
		{
		}

		public override Int64 Add(String soz,  Int64 tekrar=1)
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

		public override bool IsListed(String szWord)
		{
			if (m_RootNode == null) return false;
			bool ret=_IsWordListed(m_RootNode,szWord.Trim().Replace(Uyghur.Sozghuch,"").ToLower());
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
		
		public override int WordCount{
			get{
				return m_SozSani;
			}
		}
		
		public override bool LoadDictionary(string corpus, Uyghur.YEZIQ yeziq)
		{
			if (!File.Exists(corpus)) return false;
			return LoadDictionary(File.OpenRead(corpus), yeziq);
		}
		
		public override bool LoadDictionary(Stream instr,Uyghur.YEZIQ yeziq)
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

		
		
		private void _GetSuggestions(String qelip)
		{
			_GetSuggestions(m_RootNode,qelip+'\0',0,"");
			//System.Diagnostics.Debug.WriteLine(qelip + " --> " + Namzatlar.Count);
			return;
		}

		private void _GetSuggestions(UNode curNode,String Soz,int pos,String namzat)
		{
			if (curNode==null) return;
			char Herp=Soz[pos];
			if(curNode.mHerp==Herp||Herp=='?'||Herp=='*')
			{
				if(curNode.mHerp==0x0 && Herp=='\0')
				{
					if(!tmpNam.Contains(namzat))
					{
						tmpNam.Add(namzat);
						NamzatQelip nm = new NamzatQelip();
						nm.soz = namzat;
						nm.tekrar = curNode.mFreq;
						_namzatlar.Add(nm);
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
					if(!tmpNam.Contains(Soz))tmpNam.Add(Soz);
				}
				_SearchAll(curNode.mNext, Soz+curNode.mHerp);
				_SearchAll(curNode.mAlter, Soz);
			}
		}
		
		
		//Namzat Sozlerni tepip chiqidu
		public override List<string> Lookup(string Soz)
		{
			tmpNam.Clear();
			_namzatlar.Clear();
			List<string> Namzatlar = new List<string>();
			if (m_RootNode == null) return Namzatlar;
			char[] herpler;
			String yasSoz;
			Soz = Soz.Trim().Replace(Uyghur.Sozghuch,"").ToLower();
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

			//Yuqiridiki barliq sozuq tawushlar xata dep qarap, andin bir bir herpning arqisidin birdin soz chushup qalghan dep qarisaq
			//Herp Bir Herpning aldida birdin herp chushup qalghan dep perez qilsaq
			string tmp;
			for(i=lenSoz;i>=0;i--)
			{
				tmp = yasSoz.Insert(i,"?");
				_GetSuggestions(tmp);
				if(i<yasSoz.Length){
					tmp = yasSoz.Remove(i,1);
					_GetSuggestions(tmp);
					tmp = yasSoz.Remove(i,1).Insert(i,"?");
					_GetSuggestions(tmp);
				}
			}
			
			
			for(i=lenSoz-1;i>=0;i--)
			{
				yasSoz = Soz.Insert(i,"?"); // Bir herp chsuhup qalghan
				_GetSuggestions(yasSoz);

				herpler=Soz.ToCharArray();
				herpler[i]='?';    //Birdin herp hata
				yasSoz=new String(herpler);
				_GetSuggestions(yasSoz);

				yasSoz = yasSoz.Insert(i,"?");
				_GetSuggestions(yasSoz);
				
				
				if((i-1)>=0){
					herpler[i-1]='?';  //xoshana ikki herp xata
					yasSoz=new String(herpler);
					_GetSuggestions(yasSoz);
				}
				
				if((i-2)>=0){
					herpler=Soz.ToCharArray();
					herpler[i]='?';    //Birdin herp hata
					herpler[i-2]='?';  //birin herpni atalap xatamu qaraydu
					yasSoz=new String(herpler);
					_GetSuggestions(yasSoz);
				}

				if((i-3)>=0){
					herpler=Soz.ToCharArray();
					herpler[i]='?';    //Birdin herp hata
					herpler[i-3]='?';  //birin herpni atalap xatamu qaraydu
					yasSoz=new String(herpler);
					_GetSuggestions(yasSoz);
				}
			}
			
			foreach(NamzatQelip qlp in _namzatlar)
			{
				short dist= (short)GetDistance(Soz,qlp.soz);
				qlp.ariliq = dist;
			}
			_namzatlar.Sort(this);
			
			foreach(NamzatQelip qlp in _namzatlar)
			{
				if(Namzatlar.Count>=10){
					break;
				}
				Namzatlar.Add(qlp.soz);
			}

			i=1;
			while(i<lenSoz && lenSoz-i>=3)
			{
				yasSoz=Soz.Substring(0,lenSoz-i);
				if(IsListed(yasSoz)){
					if(!Namzatlar.Contains(yasSoz)){
						Namzatlar.Add(yasSoz);
					}
					break;
				}
				i++;
			}
			return Namzatlar;
		}
		
		public int Compare(NamzatQelip a,NamzatQelip b){
			if(a.ariliq==b.ariliq)
			{
				return b.tekrar-a.tekrar;
			}
			return a.ariliq-b.ariliq;
		}
		
		
		int GetDistance(string s, string t)
		{
			var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };
			
			int[,] matrix = new int[bounds.Height, bounds.Width];
			
			for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
			for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };
			
			for (int height = 1; height < bounds.Height; height++)
			{
				for (int width = 1; width < bounds.Width; width++)
				{
					int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
					int insertion = matrix[height, width - 1] + 1;
					int deletion = matrix[height - 1, width] + 1;
					int substitution = matrix[height - 1, width - 1] + cost;
					
					int distance = Math.Min(insertion, Math.Min(deletion, substitution));
					
					if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
					{
						distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
					}
					
					matrix[height, width] = distance;
				}
			}
			
			return matrix[bounds.Height - 1, bounds.Width - 1];
		}
		
		/// <summary>
		///     Calculate the difference between 2 strings using the Levenshtein distance algorithm
		/// </summary>
		/// <param name="source1">First string</param>
		/// <param name="source2">Second string</param>
		/// <returns></returns>
		public int LevenshteinDistance(string source1, string source2) //O(n*m)
		{
			var source1Length = source1.Length;
			var source2Length = source2.Length;

			var matrix = new int[source1Length + 1, source2Length + 1];

			// First calculation, if one entry is empty return full length
			if (source1Length == 0)
				return source2Length;

			if (source2Length == 0)
				return source1Length;

			// Initialization of matrix with row size source1Length and columns size source2Length
			for (var i = 0; i <= source1Length; matrix[i, 0] = i++){}
			for (var j = 0; j <= source2Length; matrix[0, j] = j++){}

			// Calculate rows and collumns distances
			for (var i = 1; i <= source1Length; i++)
			{
				for (var j = 1; j <= source2Length; j++)
				{
					var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;
					matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),matrix[i - 1, j - 1] + cost);
				}
			}
			// return result
			return matrix[source1Length, source2Length];
		}
	}
	
	public class NamzatQelip{
		public string soz=null;
		public short  tekrar=-1;
		public short  ariliq=-1;
	}
}