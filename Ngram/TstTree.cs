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
	public class TstTree
	{
		private UNode m_RootNode=null;
		public int   m_SozSani=0;
		private List<string> hemmeSoz=null;
		private int   nodeCnt=0;
		private int   m_FrqTh=0;
		private StreamWriter m_Out;
		public TstTree()
		{
		}

		public int Add(String szWord)
		{
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
						if(curNode.mFreq<ushort.MaxValue)curNode.mFreq++;
						if(curNode.mFreq==1)
						{
							m_SozSani++;
						}
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

		public bool Contains(String szWord)
		{
			if (m_RootNode == null) return false;
			bool ret=_IsWordListed(m_RootNode,szWord);
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
		
		
		public int GetCount(){
			int count=0;
			if (m_RootNode == null) return count;
			_GetWordCount(m_RootNode,ref count);
			return count;
		}
		
		private void _GetWordCount(UNode curNode,ref int count)
		{
			if(curNode!=null){
				if(curNode.mHerp==0x0){
					count++;
				}
				_GetWordCount(curNode.mNext,ref count);
				_GetWordCount(curNode.mAlter,ref count);
			}
		}
		
		public int Add(String szWord, int frq)
		{
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
						curNode.mFreq+=frq;
						if(curNode.mFreq==frq)
						{
							m_SozSani++;
						}
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


		public void GetAll(int frqTh, List<string> sanduq){
			hemmeSoz=sanduq;
			m_FrqTh=frqTh;
			GetBarliqSoz(m_RootNode,"");
			return;
		}

		private void GetBarliqSoz(UNode curNode,String Soz){
			if(curNode!=null){
				if(curNode.mHerp==0x0){
					if (curNode.mFreq >= m_FrqTh) {
						hemmeSoz.Add(Soz + "\t" + curNode.mFreq);
					}
					GetBarliqSoz(curNode.mAlter, Soz);
				}
				else
				{
					GetBarliqSoz(curNode.mNext, Soz + curNode.mHerp);
					GetBarliqSoz(curNode.mAlter, Soz);
				}
			}
		}


		public void Save(StreamWriter ost,int frqTh)
		{
			m_Out=ost;
			m_FrqTh=frqTh;
			Save(m_RootNode,"");
		}

		void Save(UNode curNode,String Soz)
		{
			if(curNode!=null){
				if(curNode.mHerp==0x0){
					if (curNode.mFreq >= m_FrqTh) {
						m_Out.WriteLine(Soz + "\t" + curNode.mFreq);
					}
					Save(curNode.mAlter, Soz);
				}
				else
				{
					Save(curNode.mNext, Soz + curNode.mHerp);
					Save(curNode.mAlter, Soz);
				}
			}
		}

		public int Count{
			get{
				return m_SozSani;
			}
		}		
	}
}