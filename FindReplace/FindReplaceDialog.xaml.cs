/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 01/15/2021
 * Time: 17:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Media;
namespace UyghurEditPP.FindReplace
{
	/// <summary>
	/// Interaction logic for FindReplaceDialog.xaml
	/// </summary>
	public partial class FindReplaceDialog : Window
	{
		private static string textToFind = "";
		private static bool caseSensitive = false;
		private static bool wholeWord = true;
		private static bool useRegex = false;
		private static bool useWildcards = false;
		private static bool searchUp = false;
		private TextEditor  curEditor;
		private int         repCount = 0;
		KUNUPKA       gKunupka = KUNUPKA.System;
		
		bool          gLastIsFind = true;
		
		public FindReplaceDialog(TextEditor editor)
		{
			InitializeComponent();
			Editor = editor;
			cbCaseSensitive.IsChecked = caseSensitive;
			cbWholeWord.IsChecked = wholeWord;
			cbRegex.IsChecked = useRegex;
			cbWildcards.IsChecked = useWildcards;
			cbSearchUp.IsChecked = searchUp;
			
			this.FontFamily = new System.Windows.Media.FontFamily("UKIJ Tuz");
			this.FontSize = 14;
			repCount = 0;
			this.txtFind.PreviewTextInput += UserControl_TextInput;
			this.txtReplace.PreviewTextInput += UserControl_TextInput;
			UpdateMessages();
		}

		public void UpdateMessages(){
			this.Title          = MainForm.gLang.GetText("Izdesh we Almashturush");
			this.labIzde1.Text = MainForm.gLang.GetText("Izdeydighan tékist:");
			this.labRep.Text   = MainForm.gLang.GetText("Orunbasar tékist:");
			this.butFind1.Content = MainForm.gLang.GetText("Izde");
			this.butCount.Content = MainForm.gLang.GetText("Sani");
			
			this.butRep.Content = MainForm.gLang.GetText("Almashtur");
			this.butRepAll.Content = MainForm.gLang.GetText("Hemmini Almashtur");
			
			this.cbCaseSensitive.Content = MainForm.gLang.GetText("Chong-kichik yézilishini perqlendürsun");
			this.cbWholeWord.Content     = MainForm.gLang.GetText("Pütün söz");

			this.cbRegex.Content     = MainForm.gLang.GetText("Muntizim ipade")+" (Regular Expression)";
			this.cbWildcards.Content     = MainForm.gLang.GetText("Alahide belgiler") + "(\\t,\\n,\\r...)";
			this.cbSearchUp.Content     = MainForm.gLang.GetText("Üstige qarap izdisun");
			
			this.ToolTip = "<Ctrl>+<K> " + MainForm.gLang.GetText("ni bassa kunupka almiship, Uyghurche kirgüzgili bolidu");
			this.labKun.Text = "<Ctrl>+<K> " + MainForm.gLang.GetText("ni bassa kunupka almishidu. Uyghurche kirgüzgili bolidu");
		}
		
		void window1_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.K && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				if(gKunupka == KUNUPKA.System){
					gKunupka = KUNUPKA.Uyghur;
				}
				else
				{
					gKunupka = KUNUPKA.System;
				}
				e.Handled = true;
			}
			this.labKun.Text = MainForm.gLang.GetText("<Ctrl>+<K> ni bassa kunupka almishidu. Uyghurche kirgüzgili bolidu");
		}
		
		
		private void UserControl_TextInput(object sender,TextCompositionEventArgs e)
		{
			string newtxt="";
			TextBox curBox = sender as TextBox;
			if(gKunupka == KUNUPKA.Uyghur){
				newtxt = Uyghur.KeyToUEY(e.Text);
				if(newtxt.Length>0)
				{
					e.Handled = true;
					InputText(curBox,newtxt);
				}
			}
		}
		
		private void InputText(TextBox curBox,string newtxt)
		{
			int oldpos = curBox.CaretIndex;
			string txt = curBox.Text;
			int inc = 0;
			if(Uyghur.IsUSozuq(newtxt[0]))
			{
				if((oldpos==0) || (oldpos>0 && (Uyghur.IsUSozuq(txt[oldpos-1])||!Uyghur.IsUyghurcheHerp(txt[oldpos-1])))
				  )
				{
					txt = txt.Insert(txtFind.CaretIndex,Uyghur.UYG_UN_HM_6+"");
					inc++;
				}
			}
			txt = txt.Insert(oldpos+inc,newtxt);
			inc++;
			curBox.Text = txt;
			curBox.CaretIndex =  oldpos+inc;
		}
		
		public TextEditor Editor{
			set{
				curEditor=value;
				repCount = 0;
			}
			get{
				return curEditor;
			}
		}
		
		
		public void ShowMe()
		{
			UpdateMessages();
			Show();
			Topmost=true;
			Activate();
		}
		
		public void HideMe()
		{
			this.Hide();
			repCount = 0;
		}
		
		private void Window_Closed(object sender, System.EventArgs e)
		{
			textToFind = txtFind.Text;
			caseSensitive = (cbCaseSensitive.IsChecked == true);
			wholeWord = (cbWholeWord.IsChecked == true);
			useRegex = (cbRegex.IsChecked == true);
			useWildcards = (cbWildcards.IsChecked == true);
			searchUp = (cbSearchUp.IsChecked == true);
		}

		public void FindNextClick(object sender, RoutedEventArgs e)
		{
			gLastIsFind = true;
			if (!FindNext(txtFind.Text)){
				SystemSounds.Beep.Play();
			}
		}


		public void CountClick(object sender, RoutedEventArgs e)
		{
			Regex regex = GetRegEx(txtFind.Text, true);
			MatchCollection cnt = regex.Matches(Editor.Text,0);
			labKun.Text = MainForm.gLang.GetText("Sani: ") + cnt.Count;
		}
		
		public void ContinueLastOperation(){
			if(gLastIsFind){
				FindNextClick(null,null);
			}
			else{
				ReplaceClick(null,null);
			}
		}
		
		
		private void ReplaceClick(object sender, RoutedEventArgs e)
		{
			gLastIsFind = false;
			Regex regex = GetRegEx(txtFind.Text);
			//string input = editor.Text.Substring(editor.SelectionStart, editor.SelectionLength);
			Match match = regex.Match(Editor.Text,Editor.SelectionStart);
			if (match.Success)
			{
				Editor.Document.Replace(match.Index, txtFind.Text.Length, txtReplace.Text);
				Editor.CaretOffset　= match.Index+txtReplace.Text.Length;
				Editor.BringCaretToView();
				Editor.Focus();
				repCount++;
				labKun.Text = MainForm.gLang.GetText("Jemiy ") + repCount + MainForm.gLang.GetText(" qétim almashturuldi");
			}

			if (!FindNext(txtFind.Text) && !match.Success){
				SystemSounds.Beep.Play();
			}
		}

		private void ReplaceAllClick(object sender, RoutedEventArgs e)
		{
			string msg = MainForm.gLang.GetText("Rastla barliq «") + txtFind.Text + MainForm.gLang.GetText("» ni «") +  txtReplace.Text + MainForm.gLang.GetText("» gha alamshturamsiz?");
			if (MessageBox.Show(msg, MainForm.gLang.GetText("Hemmini Almashturush"), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				Regex regex = GetRegEx(txtFind.Text, true);
				int offset = 0; //Editor.CaretOffset;
				Editor.BeginChange();
				//MatchCollection finds =  regex.Matches(Editor.Text, offset);
				Match match = regex.Match(Editor.Text, offset);
				while(match.Success)
				{
					Editor.Document.Replace(match.Index, match.Length, txtReplace.Text);
					offset = match.Index+ txtReplace.Text.Length;
					match = regex.Match(Editor.Text, offset);
					repCount++;
				}
				Editor.EndChange();
				Editor.CaretOffset = offset;
				Editor.BringCaretToView();
				labKun.Text = MainForm.gLang.GetText("Jemiy ") + repCount + MainForm.gLang.GetText(" qétim almashturuldi");
			}
		}

		private bool FindNext(string textToFind)
		{
			Regex regex = GetRegEx(textToFind);
			int start = regex.Options.HasFlag(RegexOptions.RightToLeft) ?	Editor.SelectionStart : Editor.SelectionStart + Editor.SelectionLength;
			Match match = regex.Match(Editor.Text, start);

			if (!match.Success)  // start again from beginning or end
			{
				if (regex.Options.HasFlag(RegexOptions.RightToLeft))
					match = regex.Match(Editor.Text, Editor.Text.Length);
				else
					match = regex.Match(Editor.Text, 0);
			}

			if (match.Success)
			{
				Editor.Select(match.Index, match.Length);
				Editor.CaretOffset = match.Index;
				Editor.BringCaretToView();
			}

			return match.Success;
		}

		private Regex GetRegEx(string textToFind, bool leftToRight = false)
		{
			RegexOptions options = RegexOptions.None;
			if (cbSearchUp.IsChecked == true && !leftToRight)
				options |= RegexOptions.RightToLeft;
			if (cbCaseSensitive.IsChecked == false)
				options |= RegexOptions.IgnoreCase;

			if (cbRegex.IsChecked == true)
			{
				return new Regex(textToFind, options);
			}
			else
			{
				string pattern = Regex.Escape(textToFind);
				if (cbWildcards.IsChecked == true)
					pattern = pattern.Replace("\\*", ".*").Replace("\\?", ".");
				if (cbWholeWord.IsChecked == true)
					pattern = "\\b" + pattern + "\\b";
				return new Regex(pattern, options);
			}
		}
	}
}