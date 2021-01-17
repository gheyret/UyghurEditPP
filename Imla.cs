/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2020/12/07
 * Time: 10:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using UyghurEditPP.Rendering;
using UyghurEditPP.Document;
using System.Windows.Media;
using System.Windows;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of SinaqReng.
	/// </summary>
	public class Imla:DocumentColorizingTransformer
	{
		readonly TextDecorationCollection gCollection;
		public Imla(){
			SpellCheker = null;
			gCollection = new TextDecorationCollection();
			TextDecoration dec = new TextDecoration();
			dec.Pen = new Pen { Thickness = 2, DashStyle = DashStyles.Dash, Brush = new SolidColorBrush(Colors.Red) };
			dec.PenThicknessUnit = TextDecorationUnit.FontRecommended;
			gCollection.Add(dec);
			gCollection.Freeze();
		}
		
		public IEnumerable<Match> FindWords(string text)
		{
			if(WordFinder==null) yield return null;
			foreach (Match m in WordFinder.Matches(text))
			{
				yield return m;
			}
		}
		
		public string Selection{
			get;
			set;
		}
		
		public int SelectionOffset{
			get;
			set;
		}
		
		public Regex WordFinder{
			get;
			set;
		}
		
		public SymSpell SpellCheker{
			get;
			set;
		}
		
		protected override void ColorizeLine(DocumentLine line)
		{
			if (line.Length == 0)
				return;

			int lineStartOffset = line.Offset;
			string text = CurrentContext.Document.GetText(line).ToLower();
			if(WordFinder!=null && SpellCheker!=null)
			{
				foreach(Match soz in FindWords(text))
				{
					if(soz!=null && SpellCheker.IsListed(soz.Value.Replace(Uyghur.Sozghuch,""))==false)
					{
						ChangeLinePart(lineStartOffset + soz.Index,lineStartOffset + soz.Index +soz.Value.Length ,ApplyChanges);
					}
				}
			}
			
			if(!string.IsNullOrEmpty(Selection) && Selection.Length>0){
				int start = 0;
				int index;
				while ((index = text.IndexOf(Selection, start, StringComparison.Ordinal)) >= 0)
				{
					if((lineStartOffset + index)!=SelectionOffset){
						ChangeLinePart(lineStartOffset + index, lineStartOffset + index + Selection.Length, HilightSelection);
					}
					start = index + 1; // search for next occurrence
				}
			}
		}
		
		void ApplyChanges(VisualLineElement element)
		{
			element.TextRunProperties.SetTextDecorations(gCollection);
		}
		
		void HilightSelection(VisualLineElement element)
		{
			element.TextRunProperties.SetBackgroundBrush(Brushes.Yellow);
		}
	}
}
