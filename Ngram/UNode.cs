/*
 * Created by SharpDevelop.
 * User: gheyret
 * Date: 2010/01/06
 * Time: 8:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of UNode.
	/// </summary>
	/// 
	[Serializable]
	public class UNode
	{
		public char   mHerp='\0';
		public int   mFreq=0;
		public UNode  mNext=null;
		public UNode  mAlter=null;
	}
}
