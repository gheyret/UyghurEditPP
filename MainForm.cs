/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2020/11/11
 * Time: 9:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using UyghurEditPP.Document;
using UyghurEditPP.FindReplace;
using System.Windows.Forms.Integration;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	/// 
	
	public partial class MainForm : Form
	{
		public TextEditor  gEditor;
		int         gFileNum = 1;
		
		public static Language gLang = new Language();

		Imla          gImla;
		List<String>  gIzlar=new List<String>();
		Dictionary<string, int>     gIzOffset = new Dictionary<string, int>();
		
		KUNUPKA       gKunupka = KUNUPKA.System;
		//Regex    gReg = new Regex(@"[’\w-[_\d]]+([-]+[’\w-[_\d]]+)*"); //@"([’\w-[_\d]]+)"
		//ئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا
		//Buning siziqchidin nechchisi bolup ketismu boliwetidiken
		//Regex         gUyghurcheSoz= new Regex("[ئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا]+([-]+[ئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا]+)*",RegexOptions.Compiled);
		// //Qoshma soz otturisida peqet we peqetla birla siziqche bolushi kerek
		//Buningda siziqchining aldi keynige boshluq kirip qalsimu qoshma soz dep hokum qilidu
		//Emma imla ambirida qoshma sozdiki siziqchening aldi keynide boshluq yoq
		//Regex         gUyghurcheSoz= new Regex("[ئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا]+([ ]?[-{1}][ ]?[ئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا]+)*",RegexOptions.Compiled);
		
		Regex         gUyghurcheSoz; //= new Regex("[ـئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا]+([-{1}][ـئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا]+)*",RegexOptions.Compiled);
		Regex         gLatincheSoz;  //= new Regex("[ABCDEFGHIJKLMNOPQRSTUWXYZÉÖÜabcdefghijklmnopqrstuwxyzéöü’']+([-{1}][abcdefghijklmnopqrstuwxyzéöü’']+)*",RegexOptions.Compiled);
		Regex         gSlawyancheSoz; //= new Regex("[АБВГДЕЖЗИЙКЛМНОПРСТУФХЧШҒҖҚҢҮҺӘӨабвгдежзийклмнопрстуфхчшғҗқңүһәөъ’']+([-{1}][абвгдежзийклмнопрстуфхчшғҗқңүһәөъ’']+)*",RegexOptions.Compiled);

		System.Windows.Controls.ContextMenu   gContextMenu = new System.Windows.Controls.ContextMenu();
		
		System.Windows.Controls.MenuItem   gMenuSozToghra;
		System.Windows.Controls.MenuItem   gMenuSozTekshurme;
		System.Windows.Controls.Separator  gMenuSplit, gMenuSplitToghrisi;
		
		int[]  gCodePages = {-3,-2,-1,65000,65001,1200,1201,932, 51932, 936, 950, 1250,1251,1252,1253,1254,1255,1256,1257};
		
		public    static string gImgexts = "";
		Hashtable gConfig= new Hashtable();
		string    gConfName = @"uyghuredit.cfg";
		string    gFontName = "UKIJ Tuz";
		float     gFontSize = 20.0f;
		int       gFontStyle  = 0;
		int       gFontWeight = 0;
		bool      gYeziqAuto = true;
		
		FindReplaceDialog gFindReplace = null;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			IntPtr appIns = Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]);
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			//this.Padding = new Padding(10,10,10,10);
			//string pattern = string.Format("[{0}]+([-{{1}}][{1}]+)*",Uyghur.UEYHerpler,Uyghur.UEYHerpler);
			string pattern = string.Format("[{0}]+([-]?[{1}]+)*",Uyghur.UEYHerpler,Uyghur.UEYHerpler);
			gUyghurcheSoz= new Regex(pattern,RegexOptions.Compiled);
			
			pattern = string.Format("[{0}]+([-]?[{1}]+)*",Uyghur.ULYHerpler,Uyghur.ULYHerpler);
			gLatincheSoz= new Regex(pattern,RegexOptions.Compiled);

			pattern = string.Format("[{0}]+([-]?[{1}]+)*",Uyghur.USYHerpler,Uyghur.USYHerpler);
			gSlawyancheSoz = new Regex(pattern,RegexOptions.Compiled);
			
			gImla = new Imla();
			mainTab.RemoveTab += DeleteTab;
			
			string fontpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "UKIJTuz.ttf".ToUpper());
			System.Diagnostics.Debug.WriteLine(fontpath);

			bool isexsit = File.Exists(fontpath);
			System.Diagnostics.Debug.WriteLine(isexsit);
			
			System.Diagnostics.Debug.WriteLine(IsFontInstalled("UKIJ Tuz"));
			
			
			var codecs = ImageCodecInfo.GetImageEncoders();
			foreach (var codec in codecs)
			{
				gImgexts += codec.FilenameExtension + ";";
			}
			
			gFindReplace = new FindReplaceDialog(gEditor);
			gFindReplace.Closing+=FindReplaceClosing;
			ElementHost.EnableModelessKeyboardInterop(gFindReplace);
			
			gMenuSozToghra    = new System.Windows.Controls.MenuItem();
			gMenuSozToghra.Name="TOGHRA";
			gMenuSozToghra.FontWeight = System.Windows.FontWeights.Bold;
			gMenuSozToghra.Click += menuSozImla;
			
			gMenuSozTekshurme = new System.Windows.Controls.MenuItem();
			gMenuSozTekshurme.Name="OTKUZUWET";
			gMenuSozTekshurme.FontWeight = System.Windows.FontWeights.Bold;
			gMenuSozTekshurme.Click += menuSozImla;
			
			gConfName = Path.Combine(Application.StartupPath, gConfName);
		}
		
		private bool IsFontInstalled(string fontName) {
			using (var testFont = new Font(fontName, 8)) {
				return 0 == string.Compare(
					fontName,
					testFont.Name,
					StringComparison.InvariantCultureIgnoreCase);
			}
		}

		void PreviewKey(object sender, System.Windows.Input.KeyEventArgs e)
		{
			int gModkey =(int)Control.ModifierKeys;
			gModkey = (gModkey>>16) &0x000f;
			if(gModkey == 2) //Ctrl Key
			{
				if(e.Key== System.Windows.Input.Key.K){ //Ctrl + K
					e.Handled = true;
					KunupkaClick(null,null);
				}
				else if(e.Key == System.Windows.Input.Key.End){
					e.Handled = true;
					MenuHojjetAxirClick(null,null);
				}
				else if(e.Key == System.Windows.Input.Key.Z){
					e.Handled = true;
					ToolYeniwalClick(null,null);
				}
				else if(e.Key == System.Windows.Input.Key.Y){
					e.Handled = true;
					ToolYPushaymanClick(null,null);
				}
				else if(e.Key == System.Windows.Input.Key.V){
					e.Handled = true;
					ToolChaplaClick(null,null);
				}
				else if(e.Key ==  System.Windows.Input.Key.F || e.Key ==  System.Windows.Input.Key.H){
					e.Handled = true;
					FindReplace();
				}
			}
			else if(gModkey == 0 && e.Key== System.Windows.Input.Key.F3){
				ToolIzdeDawamClick(null,null);
				e.Handled = true;
			}
		}

		void ToolIzdeDawamClick(object sender, EventArgs e)
		{
			if(string.IsNullOrEmpty(gFindReplace.txtFind.Text)){
				FindReplace();
			}
			else{
				gFindReplace.ContinueLastOperation();
			}
		}
		
		
		void FindReplace(){
			gImla.FindReplace=true;
			gEditor.TextArea.TextView.Redraw();
			gFindReplace.ShowMe();

			if (!gEditor.TextArea.Selection.IsMultiline)
			{
				gFindReplace.txtFind.FlowDirection = gEditor.FlowDirection;
				gFindReplace.txtFind.Text = gEditor.TextArea.Selection.GetText();
				gFindReplace.txtReplace.FlowDirection = gEditor.FlowDirection;
				gFindReplace.txtFind.SelectAll();
				gFindReplace.txtFind.Focus();
			}
		}
		
		void FindReplaceClosing(object sender, System.ComponentModel.CancelEventArgs e){
			e.Cancel = true;
			gFindReplace.HideMe();
			gImla.FindReplace=false;
			gEditor.TextArea.TextView.Redraw();
		}
		
		//Toghra yaki Otkuzuwet ni bir terep qilidu
		void menuSozImla(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Windows.Controls.MenuItem  menuNamzat= (System.Windows.Controls.MenuItem)sender;
			string soz = (string)menuNamzat.Tag;
			gImla.SpellCheker.Add(soz,1);
			gEditor.TextArea.TextView.Redraw();
			if(menuNamzat == gMenuSozToghra)
			{
				gImla.SpellCheker.SaveToIshletkuchi(soz);
			}
		}
		
		
		//Melum bir tab ni ochurmekchi bolghanda bu yer chaqirilidu
		void DeleteTab(int tabIndex){
			DialogResult dr = CloseTab(tabIndex);
			if(dr != DialogResult.Cancel){
				if(mainTab.TabPages.Count==0){
					gFileNum = 1;
					AddNew(String.Format("Namsiz_{0}.txt",gFileNum));
					gFileNum++;
				}
			}
			gEditor.Focus();
		}
		
		
		
		void AddNew(String fileName){
			TabPage     curPg   = null;
			TextEditor  curEdit = null;
			ElementHost curHost = null;

			int offset = 0;
			if(mainTab.TabPages.Count>0){
				curPg   = mainTab.TabPages[mainTab.TabPages.Count-1];
				curEdit = (TextEditor)((ElementHost)curPg.Controls[0]).Child;
				
				//Eng axirqi tab quruq bolsa, yeng hojjetni shu yerge qachilaydu
				if(curPg.Tag.Equals("") && curEdit.IsModified==false && File.Exists(fileName)){
					curEdit.Load(fileName);
					curPg.Tag = fileName;
					curPg.Text = Path.GetFileName(fileName);
					mainTab.SelectedTab = curPg;
					TabControl1SelectedIndexChanged(null,null);
					Text = fileName + " - UyghurEdit++";
					UpdateIzlar(fileName);
					if(!gIzOffset.TryGetValue(fileName,out offset)){
						offset = 0;
					}
					curEdit.Focus();
					if(offset>curEdit.Text.Length){
						offset = curEdit.Text.Length;
					}
					curEdit.CaretOffset = offset;
					curEdit.BringCaretToView();
					return;
				}
			}
			
			bool bar=false;
			foreach(TabPage pg in mainTab.TabPages){
				if(pg.Tag.Equals(fileName)){
					bar = true;
					curPg = pg;
					break;
				}
			}
			
			if(bar==false){
				curEdit  = new TextEditor();
				curHost = new ElementHost();
				curPg = new TabPage(Path.GetFileName(fileName));
				mainTab.TabPages.Add(curPg);
				curPg.Tag ="";
				if(File.Exists(fileName))
				{
					curEdit.Load(fileName);
					curPg.Tag = fileName;
				}
				
				curEdit.Padding = new System.Windows.Thickness(2,0,0,0);
				curEdit.ShowLineNumbers = true;
				
				curEdit.FontFamily = new System.Windows.Media.FontFamily(gFontName);
				curEdit.FontSize   = gFontSize;
				curEdit.FontStyle = gFontStyle == 0? System.Windows.FontStyles.Normal:System.Windows.FontStyles.Italic;
				curEdit.FontWeight = gFontWeight == 0? System.Windows.FontWeights.Normal:System.Windows.FontWeights.Bold;
				
				gContextMenu.FontFamily = curEdit.FontFamily;
				gContextMenu.FontSize = 20; //curEdit.FontSize;
				gContextMenu.FontStyle = curEdit.FontStyle;
				
				curEdit.WordWrap = true;
				curEdit.TextArea.Caret.PositionChanged += CaretChanged;
				curEdit.TextChanged += TextOzgerdi;
				
				curEdit.PreviewMouseWheel += PreviewMouseWheel; //Ctrolni besip  turup chaqanekning ghaltikini mangdursa, chongiyip kichikleydu
				curEdit.MouseRightButtonUp += PreMouseUp;   //chashqinekning ong teripi chekilse
				curEdit.PreviewKeyDown += PreviewKey;           //Kunupka almashturush degendek ishlarni qilidu
				//curEdit.PreviewMouseHoverStopped += MouseHoverStop;

				curEdit.TextArea.TextEntering += KeyboardTextInput; //Kunpkidin kirguzush meshghulati qilghanda bu yerge kelidu
				
				curEdit.AllowDrop = true;
				curEdit.DragEnter += MainFormDragEnter;
				curEdit.Drop += MainFormDragDrop;
				
				curEdit.TextArea.TextView.LineTransformers.Add(gImla);
				curEdit.TextArea.SelectionChanged +=TextSelctionChanged;
				curHost.Dock = DockStyle.Fill;
				curHost.Child = curEdit;
				curPg.Controls.Add(curHost);
			}
			else{
				curHost = (ElementHost)curPg.Controls[0];
				curEdit =(TextEditor)curHost.Child;
			}
			
			UpdateIzlar(fileName);
			
			mainTab.SelectedTab = curPg;
			Text = fileName + " - UyghurEdit++";
			TabControl1SelectedIndexChanged(null,null);
			if(!gIzOffset.TryGetValue(fileName,out offset)){
				offset = 0;
			}
			curEdit.Focus();
			if(offset>curEdit.Text.Length){
				offset = curEdit.Text.Length;
			}
			curEdit.CaretOffset = offset;
			curEdit.BringCaretToView();
			
			//System.Diagnostics.Debug.WriteLine(curEdit.PointToScreen(new System.Windows.Point(0, 0)));
		}
		
		void TextSelctionChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(gEditor.SelectedText))
			{
				gImla.Selection = "";
				gEditor.TextArea.TextView.Redraw();
				//gEditor.TextArea.TextView.InvalidateLayer(UyghurEditPP.Rendering.KnownLayer.Selection);
			}
			else{
				gImla.Selection = gEditor.SelectedText;
				gImla.SelectionOffset = gEditor.SelectionStart;
				gEditor.TextArea.TextView.Redraw();
				//gEditor.TextArea.TextView.InvalidateLayer(UyghurEditPP.Rendering.KnownLayer.Selection);
			}
		}
		
		//Tallanghan yaki nur belgisi turghan orundiki mezmunni CHong Yezilishqa ozgertidu
		void ChongYaz(object sender,EventArgs e)
		{
			if(gEditor.SelectionLength>0){
				gEditor.SelectedText = gEditor.SelectedText.ToUpper();
			}
			else{
				char nurHerp =gEditor.Document.GetCharAt(gEditor.CaretOffset);
				if(char.IsLower(nurHerp)){
					string txt=char.ToUpper(nurHerp)+"";
					gEditor.Document.Replace(gEditor.CaretOffset,1,txt);
				}
			}
		}
		
		//Tallanghan yaki nur belgisi turghan orundiki mezmunni kichik Yezilishqa ozgertidu
		void KichikYaz(object sender,EventArgs e)
		{
			if(gEditor.SelectionLength>0){
				gEditor.SelectedText = gEditor.SelectedText.ToLower();
			}
			else{
				char nurHerp =gEditor.Document.GetCharAt(gEditor.CaretOffset);
				if(char.IsUpper(nurHerp)){
					string txt=char.ToLower(nurHerp)+"";
					gEditor.Document.Replace(gEditor.CaretOffset,1,txt);
				}
			}
		}
		
		//Tallanghan rayondiki Her biz sozning bash heripini chong yezilishqa ozgertidu
		void MawzuYaz(object sender,EventArgs e)
		{
			if(gEditor.SelectionLength>0){
				gEditor.SelectedText = Regex.Replace(gEditor.SelectedText, @"(?<!\S)\p{Ll}", m => m.Value.ToUpper());
			}

		}

		void KeyboardTextInput(object sender,System.Windows.Input.TextCompositionEventArgs e)
		{
			string newtxt="";
			if(InputLanguage.CurrentInputLanguage.Culture.ThreeLetterISOLanguageName.Equals("uig")){
				
			}
			else if(gKunupka == KUNUPKA.Uyghur){
				newtxt = Uyghur.KeyToUEY(e.Text);
				e.Handled = true;
				if(Uyghur.IsUSozuq(newtxt[0]))
				{
					if((gEditor.CaretOffset==0) ||
					   (gEditor.CaretOffset>0 && (Uyghur.IsUSozuq(gEditor.Document.GetCharAt(gEditor.CaretOffset-1))||!Uyghur.IsUyghurcheHerp(gEditor.Document.GetCharAt(gEditor.CaretOffset-1))))
					  )
					{
						newtxt = Uyghur.UYG_UN_HM_6+newtxt;
					}
				}
				newtxt = Uyghur.Tirnaqlar(newtxt, gEditor.RightToLeft);
				if(gEditor.SelectionLength>0){
					gEditor.TextArea.Selection.ReplaceSelectionWithText(newtxt);
				}
				else{
					gEditor.Document.Insert(gEditor.CaretOffset,newtxt);
				}
				gEditor.TextArea.Caret.BringCaretToView();
			}
			else if(gEditor.RightToLeft){
				e.Handled = true;
				newtxt = Uyghur.Tirnaqlar(e.Text, gEditor.RightToLeft);
				if(gEditor.SelectionLength>0){
					gEditor.TextArea.Selection.ReplaceSelectionWithText(newtxt);
				}
				else{
					gEditor.Document.Insert(gEditor.CaretOffset,newtxt);
				}
				gEditor.TextArea.Caret.BringCaretToView();
			}
		}
		
		
		//Mouse besilghanda, besilgan orundiki sozni elip, uning imlasi toghrimu?
		//Xata bolsa namzat we bashqa munasiwetlik uchurni korsitidu
		void PreMouseUp(object sender, System.Windows.Input.MouseEventArgs e){
			stBarUchur.Text = "";
			var pp = e.GetPosition(gEditor);
			TextDocument curDoc = gEditor.Document;
			if(curDoc.Text.Length==0 || gImla.WordFinder == null){
				return;
			}
			
			var mousePosition = gEditor.GetPositionFromPoint(pp);
			var line = mousePosition.Value.Line;
			var column = mousePosition.Value.Column;
			var offset = curDoc.GetOffset(line, column);
			if (offset >= curDoc.TextLength || string.IsNullOrWhiteSpace(curDoc.GetText(offset, 1))){
				return;
			}
			
			int offsetStart = -1;
			char herp;
			while(offset>=0){
				herp = curDoc.GetCharAt(offset);
				if((herp!='-' && herp!='\'' && herp!='’' && !char.IsLetter(herp))|| offset == 0){
					offsetStart = offset;
					break;
				}
				offset--;
			}
			
			if (offsetStart == -1)
				return;
			
			//offsetStart++;
			Match usoz = gImla.WordFinder.Match(curDoc.Text,offsetStart);
			System.Windows.Controls.MenuItem  menuNamzat;
			string strNamzat;
			string toghrisi=null;
			if(usoz.Success && gImla.SpellCheker.IsListed(usoz.Value)==false){
				//gEditor.Select(usoz.Index,usoz.Length);
				gEditor.CaretOffset = usoz.Index;

				Point txtPos = new Point(usoz.Index,usoz.Length);
				gContextMenu.Items.Clear();
				gContextMenu.BeginInit();
				gContextMenu.FlowDirection = gEditor.FlowDirection;
				toghrisi = gImla.SpellCheker.Toghrisi(usoz.Value);
				if(toghrisi!=null){
					strNamzat = toghrisi;
					if(char.IsUpper(usoz.Value[0])){
						strNamzat=char.ToUpper(strNamzat[0])+strNamzat.Substring(1);
					}
					menuNamzat = new System.Windows.Controls.MenuItem{Header=strNamzat,Tag=txtPos};
					menuNamzat.HorizontalContentAlignment = gMenuSozToghra.HorizontalAlignment;
					menuNamzat.VerticalContentAlignment = gMenuSozToghra.VerticalAlignment;
					menuNamzat.FontWeight = System.Windows.FontWeights.Bold;
					menuNamzat.Click += namzat_Click;
					gContextMenu.Items.Add(menuNamzat);
					gContextMenu.Items.Add(gMenuSplitToghrisi);
				}
				
				var namzatlar = gImla.SpellCheker.Lookup(usoz.Value);
				System.Diagnostics.Debug.WriteLine("Symspell Namzat Sani = " + namzatlar.Count);
				foreach(var namzat in namzatlar){
					if(namzat.Equals(toghrisi))continue;
					strNamzat= namzat;
					//System.Diagnostics.Debug.WriteLine(strNamzat);
					if(char.IsUpper(usoz.Value[0])){
						strNamzat=char.ToUpper(strNamzat[0])+strNamzat.Substring(1);
					}
					menuNamzat = new System.Windows.Controls.MenuItem{Header=strNamzat,Tag=txtPos};
					menuNamzat.HorizontalContentAlignment = gMenuSozToghra.HorizontalAlignment;
					menuNamzat.VerticalContentAlignment = gMenuSozToghra.VerticalAlignment;
					menuNamzat.Click += namzat_Click;
					gContextMenu.Items.Add(menuNamzat);
					if(gContextMenu.Items.Count>=14){
						break;
					}
				}
				if(namzatlar.Count>0){
					gContextMenu.Items.Add(gMenuSplit);
				}
				gMenuSozToghra.Tag = usoz.Value;
				gMenuSozTekshurme.Tag = usoz.Value;
				gContextMenu.Items.Add(gMenuSozToghra);
				gContextMenu.Items.Add(gMenuSozTekshurme);
				gContextMenu.EndInit();
				
				var npp = gEditor.TextArea.TextView.GetVisualPosition(gEditor.TextArea.Caret.Position, Rendering.VisualYPosition.TextBottom);
				npp = npp - gEditor.TextArea.TextView.ScrollOffset;
				gContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Relative;
				npp = gEditor.PointToScreen(npp);
				gContextMenu.HorizontalOffset = npp.X;
				gContextMenu.VerticalOffset = npp.Y;
				//gContextMenu.SetValue(System.Windows.Controls.ContextMenuService.PlacementProperty, System.Windows.Controls.Primitives.PlacementMode.Top);
				gContextMenu.IsOpen = true;
				e.Handled = true;
			}
		}
		
		void namzat_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Windows.Controls.MenuItem  menuNamzat= (System.Windows.Controls.MenuItem)sender;
			string nsoz = menuNamzat.Header.ToString();
			Point txtPos = (Point)menuNamzat.Tag;
			string xatasoz = gEditor.Document.GetText(txtPos.X,txtPos.Y);
			gEditor.Document.Replace(txtPos.X,txtPos.Y,nsoz);
			gEditor.CaretOffset = txtPos.X + nsoz.Length;

			gImla.SpellCheker.SaveToXataToghra(xatasoz,nsoz);
			
			//Barliq Xatani izdep tepip almashturidu
			//string qelip = "\b"+xatasoz+"\b";
			int sani = 0;
			string qelip = "(?<!\\w)"+xatasoz+"(?!\\w)";
			Regex finder = new Regex(qelip,RegexOptions.Compiled|RegexOptions.IgnoreCase);
			string alltext = gEditor.Text.ToLower();
			int stpos = gEditor.CaretOffset;
			Match soz;
			int oldPos = gEditor.CaretOffset;
			while((soz = finder.Match(alltext,stpos)).Success)
			{
				gEditor.CaretOffset = soz.Index;
				gEditor.Document.Replace(soz.Index,xatasoz.Length,nsoz);
				alltext = gEditor.Text.ToLower();
				stpos = soz.Index+nsoz.Length;
				sani++;
			}
			
			if(sani>0){
				stBarUchur.Text = gLang.GetText("Oxshash xataliqlar tüzitildi") + "["+sani+"]";
				gEditor.CaretOffset = oldPos;
				gEditor.BringCaretToView();
			}
		}
		
		
		void UpdateIzlar(string fileName = null)
		{
			ToolStripMenuItem iz;
			
			if(gIzlar.Contains(fileName)){
				gIzlar.Remove(fileName);
			}
			if(File.Exists(fileName)){
				gIzlar.Insert(0,fileName);
			}
			if(gIzlar.Count>10){
				gIzlar.RemoveAt(gIzlar.Count-1);
			}
			
			menuIzlar.DropDownItems.Clear();
			foreach(string fname in gIzlar){
				iz = new ToolStripMenuItem(fname);
				iz.Click += OpenIz;
				menuIzlar.DropDownItems.Add(iz);
			}
		}
		
		void OpenIz(object sender,EventArgs e){
			ToolStripMenuItem iz=(ToolStripMenuItem)sender;
			if(File.Exists(iz.Text))
			{
				OpenaFile(iz.Text);
			}
		}
		
		//CRTL besilghan ehwalda Mouse Wheel ni herketlendurse,
		//Fontning chongluqini ozgertkili bolidu
		private void PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			bool ctrl = (Control.ModifierKeys == Keys.Control);
			if (ctrl)
			{
				double currentSize = gEditor.FontSize;
				if (e.Delta>0)
				{
					double newSize = Math.Min(64,currentSize + 1.0);
					gEditor.FontSize = newSize;
				}
				else
				{
					double newSize = Math.Max(12,currentSize - 1.0);
					gEditor.FontSize = newSize;
				}
				gFontSize = (int)gEditor.FontSize;
				gConfig["FONTSIZE"] = gFontSize;
				e.Handled = true;
			}
		}
		
		void MainFormSizeChanged(object sender, EventArgs e)
		{
			mainTab.Location = new Point(0,toolBar.Bottom);
			mainTab.Width = ClientSize.Width;
			mainTab.Height = ClientSize.Height-(toolBar.Height + stBar.Height+menuBar.Height);
			stBar.Location = new Point(0,mainTab.Bottom);
			if(this.WindowState == FormWindowState.Minimized){
				this.gFindReplace.HideMe();
			}
			gConfig["CHONGLUQI"] = new Rectangle(this.Location.X,this.Location.Y,this.Size.Width, this.Size.Height);
		}
		
		
		void LoadConfigurations()
		{
			String lang;
			string imyeziq;
			if(File.Exists(gConfName)){
				try{
					using(FileStream fs = new FileStream(gConfName, FileMode.Open, FileAccess.Read))
					{
						BinaryFormatter bf = new BinaryFormatter();
						gConfig = (Hashtable)bf.Deserialize(fs);
					}
				}
				catch(Exception ee){
					System.Diagnostics.Debug.WriteLine(ee.Message);
					gConfig = new Hashtable();
				}
			}
			
			if(gConfig.ContainsKey("LANG"))
			{
				lang = (string)gConfig["LANG"];
				lang = lang.ToLower();
			}
			else{
				lang=CultureInfo.CurrentCulture.ThreeLetterISOLanguageName.ToLower();
			}
			CheckLangMenu(lang);
			
			if(gConfig.ContainsKey("ORUNLAR")){
				gIzOffset =(Dictionary<string,int>)gConfig["ORUNLAR"];
			}

			if(gConfig.ContainsKey("IZLAR")){
				string[] tmpiz= (string[])gConfig["IZLAR"];
				foreach(string iz in tmpiz){
					if(File.Exists(iz)){
						gIzlar.Add(iz);
					}
					else if(gIzOffset.ContainsKey(iz)){
						gIzOffset.Remove(iz);
					}
				}
				UpdateIzlar();
			}
			
			
			if(gConfig.Contains("YEZIQAUTO")){
				gYeziqAuto=(bool)gConfig["YEZIQAUTO"];
			}
			else{
				gYeziqAuto=true;
			}
			
			
			if(gConfig.Contains("IMLAYEZIQ")){
				imyeziq=(string)gConfig["IMLAYEZIQ"];
			}
			else{
				imyeziq="UEY";
			}
			
			KUNUPKA kun;
			if(gConfig.Contains("KUNUPKA")){
				kun=(KUNUPKA)gConfig["KUNUPKA"];
			}
			else{
				kun=KUNUPKA.System;
			}
			
			
			if(gConfig.Contains("FONTNAME") && gConfig.Contains("FONTSIZE") && gConfig.Contains("FONTSTYLE") && gConfig.Contains("FONTWEIGHT")){
				gFontName     = (String)gConfig["FONTNAME"];
				gFontSize     = (float)gConfig["FONTSIZE"];
				gFontStyle    = (int)gConfig["FONTSTYLE"];
				gFontWeight   = (int)gConfig["FONTWEIGHT"];
			}
			else{
				gFontName   = "UKIJ Tuz";
				gFontSize   = 20;
				gFontStyle  = 0;
				gFontWeight = 0;
				
				gConfig["FONTNAME"]  = gFontName;
				gConfig["FONTSIZE"]  = gFontSize;
				gConfig["FONTSTYLE"] = gFontStyle;
				gConfig["FONTWEIGHT"] = gFontWeight;
				
			}
			SetKunupka(kun);
			
			if(!gConfig.Contains("CHONGLUQI")){
				Rectangle rc = new Rectangle(100,100,1200, 768);
				gConfig["CHONGLUQI"] = rc;
			}
		}
		
		void MainFormShown(object sender, EventArgs e)
		{
			
		}
		
		
		void MainFormLoad(object sender, EventArgs e)
		{
			int codepage;
			this.Font = new Font("UKIJ Tuz",12);
			this.menuBar.Font = this.Font;
			this.stBar.Font = this.Font;
			this.mainTab.Font = this.Font;
			this.stBar.Height = this.Font.Height+6;
			
			ToolStripMenuItem cpMenu;
			for(int i=0;i<gCodePages.Length;i++){
				codepage=gCodePages[i];
				if(codepage == -3){
					cpMenu = new ToolStripMenuItem("Boghda-Fangjeng");
					cpMenu.Click += menuCodePageClick;
					cpMenu.Tag = codepage;
					menuHKod.DropDownItems.Add(cpMenu);
					cpMenu.Enabled = true;
				}
				else if(codepage == -2){
					cpMenu = new ToolStripMenuItem("Weifang-WIN");
					cpMenu.Click += menuCodePageClick;
					cpMenu.Tag = codepage;
					menuHKod.DropDownItems.Add(cpMenu);
					cpMenu.Enabled = true;
				}
				else if(codepage == -1){
					cpMenu = new ToolStripMenuItem("Weifang-DOS");
					cpMenu.Click += menuCodePageClick;
					cpMenu.Tag = codepage;
					menuHKod.DropDownItems.Add(cpMenu);
					menuHKod.DropDownItems.Add(new ToolStripSeparator());
					//cpMenu.Enabled = false;
				}
				else{
					Encoding enc = Encoding.GetEncoding(codepage);
					cpMenu = new ToolStripMenuItem(enc.EncodingName);
					cpMenu.Click += menuCodePageClick;
					cpMenu.Tag = codepage;
					menuHKod.DropDownItems.Add(cpMenu);
					if(codepage == 1201){
						menuHKod.DropDownItems.Add(new ToolStripSeparator());
					}
				}
			}
			
			gMenuSplit         = new System.Windows.Controls.Separator();
			gMenuSplitToghrisi = new System.Windows.Controls.Separator();
			
			LoadConfigurations();
			Rectangle rc = (Rectangle)gConfig["CHONGLUQI"];
			this.Location = new Point(rc.X,rc.Y);
			this.Size = new Size(rc.Width,rc.Height);
			MenuYengiClick(null,null);
		}
		
		void CheckLangMenu(string lang){
			foreach(ToolStripMenuItem itm in menuTil.DropDownItems){
				itm.Checked = false;
				if(itm.Tag.Equals(lang)){
					itm.Checked = true;
				}
			}
			gConfig["LANG"] = lang;
			gLang.SetLangId(lang);
			if("uey".Equals(lang)){
				this.menuBar.RightToLeft = RightToLeft.Yes;
				//this.menuBar.Font = new Font("UKIJ Tuz",14.0f);
//				this.stBar.RightToLeft =  RightToLeft.Yes;
//				this.stBar.Font = this.menuBar.Font;
//				stBarUchur.Font = this.menuBar.Font;
//				this.stBar.Height = stBar.Font.Height+10;
			}
			else{
				this.menuBar.RightToLeft = RightToLeft.No;
//				this.stBar.RightToLeft =  RightToLeft.No;

				this.menuBar.Font = this.Font;
//				this.stBar.Font = this.menuBar.Font;
//				stBarUchur.Font = this.menuBar.Font;
			}
			UpdateMessage();
			gFindReplace.UpdateMessages();
		}
		
		
		void UpdateMessage(){
			toolBar.Font = menuBar.Font;

			menuHojjet.Text = gLang.GetText("Höjjet");
			menuYengi.Text = gLang.GetText("Yéngi höjjet");
			
			menuAch.Text = gLang.GetText("Ach");
			menuSaqla.Text =gLang.GetText("Saqla");
			
			menuBSaqla.Text = gLang.GetText("Bashqa Isimda Saqla");
			
			menuBas.Text = gLang.GetText("Bésip Chiqar");
			menuHKod.Text = gLang.GetText("Höjjetning Kodi");
			menuIzlar.Text = gLang.GetText("Izlar");
			menuAxirlashtur.Text = gLang.GetText("Axirlashtur");
			
			menuTehrir.Text = gLang.GetText("Tehrirlesh");
			menuFont.Text = gLang.GetText("Xet Nusxisi");
			menuQurNomur.Text = gLang.GetText("Qur Nomurini Körsetsun");
			menuYeniwal.Text = gLang.GetText("Yéniwal");
			menuYPushayman.Text = gLang.GetText("Pushayman Qil");
			menuOchur.Text = gLang.GetText("Öchür");
			menuKes.Text = gLang.GetText("Kes");
			menuKochur.Text = gLang.GetText("Köchür");
			menuChapla.Text = gLang.GetText("Chapla");
			menuHemme.Text = gLang.GetText("Hemmini Talla");
			menuChaplaUighursoft.Text = "«Uighursoft»" + gLang.GetText("ningkini Chapla");
			menuChaplaDuldul.Text = "«Duldul»" +gLang.GetText("ningkini Chapla");
			menuChaplaBashqilar.Text = gLang.GetText("Bashqilarningkini Chapla");
			menuHojjetBash.Text = gLang.GetText("Höjjetning Béshigha Yötkel");
			menuHojjetAxir.Text = gLang.GetText("Höjjetning Axirigha Yötkel");
			menuQuryotkel.Text = gLang.GetText("Körsitilgen Qurgha Yötkel");
			menuChong.Text = gLang.GetText("Chong Yézilishqa Özgert");
			menuKichik.Text = gLang.GetText("Kichik Yézilishqa Özgert");
			menuMawzu.Text = gLang.GetText("Bash Herpni Chong Yézilishqa Özgert");
			
			
			
			menuImla.Text = gLang.GetText("Imla");
			menuImlaUEY.Text = gLang.GetText("Uyghurchining Imlasini Közetsun");
			menuImlaULY.Text = gLang.GetText("Latinchining Imlasini Közetsun");
			menuImlaUSY.Text = gLang.GetText("Slawyanchining Imlasini Közetsun");
			
			menuBelge.Text = gLang.GetText("Tinish Belgiler we Boshluqni Tengshe");
			menuImlaAuto.Text = gLang.GetText("Aptomatik Tekshür");
			
			menuImlaAmbar.Text = gLang.GetText("Ishletküchi Ambirini Körset");
			
			menuQoral.Text  = gLang.GetText("Qorallar");
			menuTiz.Text  = gLang.GetText("Élipbe Tertipi Boyiche Tiz");
			menuTekrar.Text = gLang.GetText("Sözlerning Tekrarliqi");
			
			menuTil.Text = gLang.GetText("Til-Yéziq");
			menuUyghurA.Text = "ئۇيغۇرچە";
			menuUyghurL.Text = "Uyghurche";
			menuUyghurS.Text = "Уйғурчә";
			
			menuYardem.Text = gLang.GetText("Yardem");
			menuKunupka.Text = gLang.GetText("Kona Yéziq Kunupka Orunlashturulushi");
			menuULElipbe.Text = gLang.GetText("Uyghur Latin Yéziqi Élipbesi");
			menuHeqqide.Text = "UyghurEdit++" + gLang.GetText("Heqqide");
			
			toolYengi.ToolTipText = gLang.GetText("Yéngi höjjet yasaydu");
			toolAch.ToolTipText=gLang.GetText("Diskidiki höjjetni oqup tehrirleydu");
			toolSaqla.ToolTipText=gLang.GetText("Tehrirlewatqan höjjetni diskigha saqlaydu");
			toolBas.ToolTipText = gLang.GetText("Tehrirlewatqan höjjetni printérda bésip chiqiridu");
			toolKes.ToolTipText = gLang.GetText("Tallanghanni kesip éliwalidu");
			toolKochur.ToolTipText = gLang.GetText("Tallanghanni köchürüwalidu");

			toolOchur.ToolTipText = gLang.GetText("Tallanghanni öchürüwétidu");
			toolYeniwal.ToolTipText = gLang.GetText("Qilghan meshghulattin yéniwalidu");
			toolYPushayman.ToolTipText = gLang.GetText("Yéniwalghangha pushayman qilidu");
			toolDawam.ToolTipText = gLang.GetText("Nur belgisi turghan yerdin bashlap izdeydu yaki izdeshni dawam qilidu");
			toolQatla.ToolTipText = gLang.GetText("Ékran kenglikidin éship ketmigen tehrirlesh haliti");
			toolOngSol.ToolTipText = gLang.GetText("Ongdin yaki soldin bashlap yézishqa özgertidu");
			
			toolUEY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayonni Uyghurchigha aylanduridu");
			toolULY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayonni Latinchigha aylanduridu");
			toolUSY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayonni Slawyanchigha aylanduridu");
			
			
			menuYeziqAuto.Text = gLang.GetText("Közitidighan Yéziqni Aptomatik Tallisun");
			menuYeziqAuto.ToolTipText = gLang.GetText("Höjjet közniki almashqanda shu köznektiki yéziqqa mas kélidighan Imla Tekshürgüchni aktiplaydu");
			
			menuOCR.ToolTipText = gLang.GetText("Resimni yéziqqa aylanduridu");


			this.toolULY2UEY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayondiki Latinchini Uyghurchigha aylanduridu");
			this.toolUSY2UEY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayondiki Slawyanchini Uyghurchigha aylanduridu");
			this.toolUEY2ULY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayondiki Uyghurchini Latinchigha aylanduridu");
			this.toolUSY2ULY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayondiki Slawyanchini Latinchigha aylanduridu");
			this.toolUEY2USY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayondiki Uyghurchini Slawyanchigha aylanduridu");
			this.toolULY2USY.ToolTipText = gLang.GetText("Hazirqi höjjet yaki Tallanghan rayondiki Latinchchini Slawyanchigha aylanduridu");
		}
		
		
		void MenuTilClick(object sender, EventArgs e)
		{
			ToolStripMenuItem selItem = (ToolStripMenuItem)sender;
			CheckLangMenu((string)selItem.Tag);
		}
		
		void ToolOpenClick(object sender, EventArgs e)
		{
			OpenFileDialog opnFileDlg = new OpenFileDialog();
			string filter= "Text files|*.txt;*.uut|";
			filter += "Image files|" + gImgexts;
			filter += "|All files|*.*";
			opnFileDlg.Filter = filter;
			opnFileDlg.Multiselect = false;
			if(opnFileDlg.ShowDialog(this)== DialogResult.OK)
			{
				OpenaFile(opnFileDlg.FileName);
			}
		}
		
		public void OpenaFile(string filename){
			String  extName = Path.GetExtension(filename);
			if(gImgexts.IndexOf(extName,StringComparison.OrdinalIgnoreCase)!=-1)
			{
				UpdateIzlar(filename);
				OCRForm ocr = new OCRForm(gEditor, filename);
				ocr.Owner = this;
				ocr.ShowInTaskbar = false;
				ocr.Show();
			}
			else{
				AddNew(filename);
			}
		}
		
		void MenuYengiClick(object sender, EventArgs e)
		{
			AddNew(String.Format("Namsiz_{0}.txt",gFileNum));
			gFileNum++;
		}
		
		void MainFormPaint(object sender, PaintEventArgs e)
		{
			// stBarUchur.Text = "UyghurEdit++ V"+GetVersion() + "(2020/11/12) Aptor: Gheyret T.Kenji";
			// stBarUchur.Text = "UyghurEdit++ V "+GetVersion() + " Aptor: Gheyret T.Kenji";
			// stBarQur.Text = gLang.GetText("Jemiy ") + gEditor.LineCount.ToString() + gLang.GetText(" qur");
			toolQatla.Checked = gEditor.WordWrap;
			
			toolBas.Enabled = gEditor.Text.Length>0;
			toolSaqla.Enabled = gEditor.IsModified;
			if(gEditor.IsModified && mainTab.SelectedTab.Text.StartsWith("*")==false){
				mainTab.SelectedTab.Text = "*" + mainTab.SelectedTab.Text;
			}
			else if(gEditor.IsModified == false && mainTab.SelectedTab.Text.StartsWith("*")==true){
				mainTab.SelectedTab.Text = mainTab.SelectedTab.Text.Substring(1);
			}
			
			if(string.IsNullOrEmpty(mainTab.SelectedTab.Tag.ToString())){
				menuHKod.Enabled = false;
			}
			else{
				menuHKod.Enabled = true;
			}
			if(gEditor.CodePage<0){
				if(gEditor.CodePage == -3){
					stBarEncode.Text = "Boghda-Fangjeng";
				}
				else if(gEditor.CodePage == -2){
					stBarEncode.Text = "Weifang-WIN";
				}
				else if(gEditor.CodePage == -1){
					stBarEncode.Text = "Weifang-DOS";
				}
			}
			else{
				stBarEncode.Text = gEditor.Encoding==null? Encoding.UTF8.EncodingName:gEditor.Encoding.EncodingName;
			}
			
			menuBas.Enabled = toolBas.Enabled;
			menuSaqla.Enabled = toolSaqla.Enabled;
			
			menuYeniwal.Enabled = toolYeniwal.Enabled;
			menuYPushayman.Enabled = toolYPushayman.Enabled;

			toolKes.Enabled    = gEditor.SelectionLength>0;
			toolKochur.Enabled = gEditor.SelectionLength>0;
			toolOchur.Enabled = gEditor.SelectionLength>0;
			toolChapla.Enabled = Clipboard.GetDataObject()!=null;
			toolYeniwal.Enabled = gEditor.CanUndo;
			toolYPushayman.Enabled = gEditor.CanRedo;
			
			menuKes.Enabled    = toolKes.Enabled ;
			menuKochur.Enabled = toolKochur.Enabled;
			menuOchur.Enabled = toolOchur.Enabled;
			
			menuChapla.Enabled = toolChapla.Enabled;
			menuChaplaUighursoft.Enabled = toolChapla.Enabled;
			menuChaplaDuldul.Enabled = toolChapla.Enabled;
			menuChaplaBashqilar.Enabled = toolChapla.Enabled;
			
			menuQuryotkel.Enabled = gEditor.LineCount>5;
			menuQurNomur.Checked = gEditor.ShowLineNumbers;
		}
		
		public static String GetVersion()
		{
			Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			//return v.Major + "." + v.Minor + "." + v.Build;
			return v.Major + "." + v.Minor+ "." + v.Build;
		}
		
		void CaretChanged(object sender, EventArgs e){
			//System.Diagnostics.Debug.WriteLine(e.ToString());
			//System.Diagnostics.Debug.WriteLine(gEditor.SelectionStart + ": " + gEditor.CaretOffset);
			string herpcode="0000";
			try{
				UInt32 code = gEditor.TextArea.Document.GetCharAt(gEditor.CaretOffset);
				herpcode = code.ToString("X4");
//				Invalidate();
			}catch(Exception){
				
			}
			stBarLs.Text = gEditor.TextArea.Caret.Line+" : "+gEditor.TextArea.Caret.Column + " : U" + herpcode;
			
			//stBarLs.Text = "1000000"+":"+"1000" + " [" + herpcode+"]";
		}
		
		void TabControl1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(mainTab.TabPages.Count>0){
				if(mainTab.SelectedTab.Tag.ToString().Length ==0){
					Text = mainTab.SelectedTab.Text + " - UyghurEdit++";
				}
				else{
					Text = mainTab.SelectedTab.Tag.ToString() + " - UyghurEdit++";
				}
				
				ElementHost host = (ElementHost)mainTab.SelectedTab.Controls[0];
				gEditor = (TextEditor)host.Child;
				gEditor.Focus();
				CaretChanged(null,null);
				TextSelctionChanged(null,null);
				if(gFindReplace!=null){
					gFindReplace.Editor = gEditor;
				}
				
				gEditor.TextArea.TextView.Redraw();
				
				Uyghur.YEZIQ curYeziq = Uyghur.Detect(gEditor.Text);
				if(curYeziq == Uyghur.YEZIQ.UEY || curYeziq == Uyghur.YEZIQ.YOQ)
				{
					menuChong.Enabled = false;
					menuKichik.Enabled = false;
					menuMawzu.Enabled = false;
					gEditor.RightToLeft = true;
					
					if(gYeziqAuto){
						ImlaniAktipla("UEY");
					}
				}
				else if(curYeziq == Uyghur.YEZIQ.ULY || curYeziq == Uyghur.YEZIQ.USY){
					menuChong.Enabled = true;
					menuKichik.Enabled = true;
					menuMawzu.Enabled = true;
					gEditor.RightToLeft = false;
					
					if(gYeziqAuto)
					{
						if(curYeziq == Uyghur.YEZIQ.ULY){
							ImlaniAktipla("ULY");
						}
						else{
							ImlaniAktipla("USY");
						}
					}
				}
			}
			Invalidate();
		}
		
		void ToolYeniwalClick(object sender, EventArgs e)
		{
			gEditor.Undo();
			gEditor.BringCaretToView();
			
		}
		void ToolYPushaymanClick(object sender, EventArgs e)
		{
			gEditor.Redo();
			gEditor.BringCaretToView();
		}
		
		void TextOzgerdi(object sender, EventArgs e){
			Invalidate();
		}
		void ToolQatlaClick(object sender, EventArgs e)
		{
			gEditor.WordWrap = !toolQatla.Checked;
		}
		
		void ToolKesClick(object sender, EventArgs e)
		{
			gEditor.Cut();
		}
		void ToolKochurClick(object sender, EventArgs e)
		{
			gEditor.Copy();
		}
		void ToolChaplaClick(object sender, EventArgs e)
		{
			gEditor.Paste();
			gEditor.BringCaretToView();
		}
		void ToolOchurClick(object sender, EventArgs e)
		{
			gEditor.Delete();
		}
		
		void ToolOngSolClick(object sender, EventArgs e)
		{
			gEditor.RightToLeft = !gEditor.RightToLeft;
//			gRTL = gEditor.RightToLeft;
//			gConfig["RTL"] = gRTL;
		}
		
		string GetFromClipboad(){
			String clipText = null;
			try {
				IDataObject dataObject = Clipboard.GetDataObject();
				if(dataObject.GetDataPresent(DataFormats.UnicodeText)) {
					clipText = ((String)dataObject.GetData(DataFormats.UnicodeText)).Replace(Uyghur.Sozghuch,"");
				}
			} catch (Exception) {
			}
			return clipText;
		}
		
		void MenuSaqlaClick(object sender, EventArgs e)
		{
			if(File.Exists(mainTab.SelectedTab.Tag.ToString())){
				gEditor.Save(mainTab.SelectedTab.Tag.ToString());
				if(mainTab.SelectedTab.Text.StartsWith("*")){
					mainTab.SelectedTab.Text= mainTab.SelectedTab.Text.Substring(1);
				}
				//TabControl1SelectedIndexChanged(null,null);
			}
			else{
				MenuBSaqlaClick(null,null);
			}
		}
		
		void MenuBSaqlaClick(object sender, EventArgs e)
		{
			string newflname = SaveAs(gEditor, mainTab.SelectedTab.Tag.ToString());
			if(newflname!=null){
				mainTab.SelectedTab.Text = Path.GetFileName(newflname);
				mainTab.SelectedTab.Tag = newflname;
				Text = newflname + " - UyghurEdit++";
			}
		}
		
		string SaveAs(TextEditor curEdit, string fileName = null){
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = fileName;
			sfd.Filter = "Text file|*.txt|Text file|*.uut|All files(*.*)|*.*";
			DialogResult dr = sfd.ShowDialog();
			if(dr == DialogResult.OK){
				curEdit.Save(sfd.FileName);
				UpdateIzlar(sfd.FileName);
				TabControl1SelectedIndexChanged(null,null);
				return sfd.FileName;
			}
			else{
				return null;
			}
		}
		
		
		//Tool bar da Text box ni korsitidu
		void MenuQuryotkelClick(object sender, EventArgs e)
		{
			toolTBox.Visible = true;
			toolTBox.Focus();
		}
		
		//Korsitilgen Qur numurighan yotkilish
		void ToolTextKeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar=='\r'){
				e.Handled = true;
				try{
					int line = int.Parse(toolTBox.Text.Trim());
					toolTBox.Visible = false;
					gEditor.Focus();
					gEditor.TextArea.Caret.Line = line;
					gEditor.TextArea.Caret.Column = 0;
					gEditor.BringCaretToView();
				}
				finally{
					
				}
			}
		}
		
		void ButQurNomurClick(object sender, EventArgs e)
		{
			gEditor.ShowLineNumbers = !gEditor.ShowLineNumbers;
			Invalidate();
		}
		
		void MenuULElipbeClick(object sender, EventArgs e)
		{
			FormULElipbe frm = new FormULElipbe();
			frm.ShowInTaskbar = false;
			frm.ShowDialog();
			gEditor.Focus();
		}
		void MenuKunupkaClick(object sender, EventArgs e)
		{
			FormKunupka frm = new FormKunupka(this);
			frm.ShowInTaskbar = false;
			frm.Show(this);
			gEditor.Focus();
		}
		
		void MenuChaplaUighursoftClick(object sender, EventArgs e)
		{
			string cliptext = GetFromClipboad();
			if(cliptext!=null){
				cliptext = Uyghur.FromUighursoft(cliptext);
				System.Diagnostics.Debug.WriteLine(cliptext);
				gEditor.Document.Insert(gEditor.CaretOffset,cliptext);
			}
		}
		void MenuChaplaDuldulClick(object sender, EventArgs e)
		{
			string cliptext = GetFromClipboad();
			if(cliptext!=null){
				cliptext = Uyghur.FromDuldul(cliptext);
				System.Diagnostics.Debug.WriteLine(cliptext);
//				gEditor.SelectedText = cliptext;
				gEditor.Document.Insert(gEditor.CaretOffset,cliptext);
			}
			
		}
		void MenuChaplaBashqilarClick(object sender, EventArgs e)
		{
			string cliptext = GetFromClipboad();
			if(cliptext!=null){
				cliptext = Uyghur.FromBashqilar(cliptext);
				System.Diagnostics.Debug.WriteLine(cliptext);
				gEditor.Document.Insert(gEditor.CaretOffset,cliptext);
			}
		}
		
		void ToolULYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string newtext = Uyghur.ToULY(gEditor.SelectedText);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string newtext = Uyghur.ToULY(gEditor.Text);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = Uyghur.ULYJumleChongYaz(newtext);
					gEditor.RightToLeft = false;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
		}
		
		void ToolUSYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string newtext = Uyghur.ToUSY(gEditor.SelectedText);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string newtext = Uyghur.ToUSY(gEditor.Text);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = Uyghur.USYJumleChongYaz(newtext);
					gEditor.RightToLeft = false;
					TabControl1SelectedIndexChanged(null,null);
					
				}
			}
		}
		
		void ToolUEYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string txtuey = gEditor.SelectedText;
				string newtext = Uyghur.ToUEY(txtuey);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string txtuey = gEditor.Text;
				string newtext = Uyghur.ToUEY(txtuey);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = newtext;
					gEditor.RightToLeft = true;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
		}
		
		void ToolULY2UEYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string txtuey = gEditor.SelectedText;
				string newtext = Uyghur.ULY2UEY(txtuey);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string txtuey = gEditor.Text;
				string newtext = Uyghur.ULY2UEY(txtuey);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = newtext;
					gEditor.RightToLeft = true;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
		}
		void ToolUSY2UEYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string txtuey = gEditor.SelectedText;
				string newtext = Uyghur.USY2UEY(txtuey);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string txtuey = gEditor.Text;
				string newtext = Uyghur.USY2UEY(txtuey);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = newtext;
					gEditor.RightToLeft = true;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
		}
		
		void ToolUEY2ULYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string newtext = Uyghur.UEY2ULY(gEditor.SelectedText);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string newtext = Uyghur.UEY2ULY(gEditor.Text);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = Uyghur.ULYJumleChongYaz(newtext);
					gEditor.RightToLeft = false;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
			
		}

		void ToolUSY2ULYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string txtuey = gEditor.SelectedText;
				string newtext = Uyghur.USY2ULY(txtuey);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string txtuey = gEditor.Text;
				string newtext = Uyghur.USY2ULY(txtuey);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = newtext;
					gEditor.RightToLeft = false;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
		}
		
		void ToolUEY2USYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string newtext = Uyghur.UEY2USY(gEditor.SelectedText);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string newtext = Uyghur.UEY2USY(gEditor.Text);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = Uyghur.USYJumleChongYaz(newtext);
					gEditor.RightToLeft = false;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
		}

		void ToolULY2USYClick(object sender, EventArgs e)
		{
			if(gEditor.Text.Length==0) return;
			if(gEditor.SelectedText.Length>0){
				string txtuey = gEditor.SelectedText;
				string newtext = Uyghur.ULY2USY(txtuey);
				if(newtext!=null){
					gEditor.SelectedText = newtext;
				}
			}
			else{
				string txtuey = gEditor.Text;
				string newtext = Uyghur.ULY2USY(txtuey);
				if(newtext!=null){
					MenuYengiClick(null,null);
					gEditor.Text = newtext;
					gEditor.RightToLeft = false;
					TabControl1SelectedIndexChanged(null,null);
				}
			}
		}
		
		
		DialogResult CloseTab(int tabIndex){
			TabPage pg = mainTab.TabPages[tabIndex];
			ElementHost curHost = (ElementHost)pg.Controls[0];
			TextEditor  curEdit = (TextEditor)curHost.Child;
			string filenm = (String)pg.Tag;
			DialogResult dr = DialogResult.None;
			if(curEdit.IsModified)
			{
				dr = MessageBox.Show(this, gLang.GetText("Höjjetning mezmunida özgirish boldi. Saqlamsiz?"),"UyghurEdit++ v"+ GetVersion(), MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
				if(dr ==  DialogResult.Cancel){
					return dr;
				}
				if(dr == DialogResult.Yes){
					if(File.Exists(filenm)){
						curEdit.Save(pg.Tag.ToString());
					}
					else{
						SaveAs(curEdit);
					}
				}
			}
			gIzOffset[filenm] = curEdit.CaretOffset;
			mainTab.TabPages.RemoveAt(tabIndex);
			curEdit.Clear();
			curHost.Dispose();
			return dr;
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult dr;
			while(mainTab.TabPages.Count!=0){
				dr = CloseTab(0);
				if(dr == DialogResult.Cancel){
					e.Cancel = true;
					break;
				}
			}
			gConfig["IZLAR"]   = gIzlar.ToArray();
			gConfig["ORUNLAR"] = gIzOffset;
			try
			{
				System.Diagnostics.Debug.WriteLine(gConfig["CHONGLUQI"]);
				using(FileStream fs = new FileStream(gConfName, FileMode.Create)){
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(fs, gConfig);
				}
			}
			catch(SerializationException er)
			{
				System.Diagnostics.Debug.WriteLine("Failed to serialize. Reason: " + er.Message);
				throw;
			}
			gLang.Save(Path.Combine(Application.StartupPath, "langdata.txt"));
		}
		void MenuAxirlashturClick(object sender, EventArgs e)
		{
			this.Close();
		}
		void MenuHemmeClick(object sender, EventArgs e)
		{
			gEditor.SelectAll();
		}
		void MenuHojjetBashClick(object sender, EventArgs e)
		{
			gEditor.CaretOffset = 0;
			gEditor.ScrollToHome();
		}
		void MenuHojjetAxirClick(object sender, EventArgs e)
		{
			gEditor.CaretOffset = gEditor.Text.Length;
			gEditor.ScrollToEnd();
		}

		void MenuImlaClick(object sender, EventArgs e){
			menuYeziqAuto.Checked = gYeziqAuto;

			Uyghur.YEZIQ curYeziq = Uyghur.Detect(gEditor.Text);
			if(curYeziq == Uyghur.YEZIQ.UEY){
				menuBelge.Enabled = true;
			}
			else{
				menuBelge.Enabled = true;
			}
			
			if((menuImlaUEY.Checked && curYeziq == Uyghur.YEZIQ.UEY)||
			   (menuImlaULY.Checked && curYeziq == Uyghur.YEZIQ.ULY)||
			   (menuImlaUSY.Checked && curYeziq == Uyghur.YEZIQ.USY)
			  )
			{
				menuImlaAuto.Enabled = true;
			}
			else{
				menuImlaAuto.Enabled = false;
			}
			
			
		}
		
		void MenuImlaClickActive(object sender, EventArgs e){
			ToolStripMenuItem curMenu =(ToolStripMenuItem)sender;
			Cursor  = Cursors.WaitCursor;
			string yeziq = "YOQ";
			curMenu.Checked = !curMenu.Checked;
			if(curMenu.Checked)
			{
				if(curMenu == menuImlaUEY){
					yeziq="UEY";
				}
				else if(curMenu == menuImlaULY){
					yeziq="ULY";
				}
				else if(curMenu == menuImlaUSY){
					yeziq="USY";
				}
			}
			ImlaniAktipla(yeziq);
			Cursor  = Cursors.Default;
			gEditor.TextArea.TextView.Redraw();
		}
		
		void ImlaniAktipla(string yeziq)
		{
			Stream imlastrem;
			menuImlaUEY.Checked=false;
			menuImlaULY.Checked=false;
			menuImlaUSY.Checked=false;
			
			if(yeziq.Equals("YOQ")){
				gImla.WordFinder = null;
			}
			else{
				if(yeziq.Equals("UEY")){
					menuImlaUEY.Checked=true;
				}
				else if(yeziq.Equals("ULY")){
					menuImlaULY.Checked = true;
				}
				else if(yeziq.Equals("USY")){
					menuImlaUSY.Checked = true;
				}
				
				
//				if(File.Exists("uyghur_imla.txt")){
				//imlastrem = File.OpenRead("uyghur_imla.txt");
				imlastrem=System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("UyghurEditPP.uyghur_imla.txt");
				if(yeziq.Equals("UEY") && gUyghurcheSoz != gImla.WordFinder){
					gMenuSozTekshurme.Header= Uyghur.ULY2UEY("Bu sözni ötküzüwet");
					gMenuSozToghra.Header   = Uyghur.ULY2UEY("Bu söz toghra");
					gMenuSozTekshurme.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
					gMenuSozTekshurme.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
					gMenuSozToghra.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
					gMenuSozToghra.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

					gImla.WordFinder = gUyghurcheSoz;
					gImla.SpellCheker = new KenjiSpell();
					//gImla.SpellCheker = new SymSpell();
					gImla.SpellCheker.Load(imlastrem,Uyghur.YEZIQ.UEY);
				}
				else if(yeziq.Equals("ULY") && gLatincheSoz != gImla.WordFinder){
					gMenuSozTekshurme.Header= "Bu sözni ötküzüwet";
					gMenuSozToghra.Header   = "Bu söz toghra";
					gImla.WordFinder = gLatincheSoz;
					gImla.SpellCheker = new KenjiSpell();
					gImla.SpellCheker.Load(imlastrem,Uyghur.YEZIQ.ULY);
				}
				else if( yeziq.Equals("USY") && gSlawyancheSoz != gImla.WordFinder){
					gMenuSozTekshurme.Header= Uyghur.ULY2USY("Bu sözni ötküzüwet");
					gMenuSozToghra.Header   = Uyghur.ULY2USY("Bu söz toghra");
					gImla.WordFinder = gSlawyancheSoz;
					gImla.SpellCheker = new KenjiSpell();
					gImla.SpellCheker.LoadDictionary(imlastrem,Uyghur.YEZIQ.USY);//Imla mbirini slawyanchigha ozgertip ishlitidu
				}
				imlastrem.Close();
				System.Diagnostics.Debug.WriteLine("Sozluk Sani = " + gImla.SpellCheker.WordCount);
//				}
			}
			gConfig["IMLAYEZIQ"]=yeziq;
		}
		
		
		void MenuMouseEntered(object sender, EventArgs e)
		{
			ToolStripMenuItem curMenu =(ToolStripMenuItem)sender;
			string tooltip = null;
			if(curMenu == menuIzlar){
				tooltip=gLang.GetText("Yéqinda tehrirlen’gen höjjetlerning isimliri");
			}
			else if(curMenu == menuBSaqla){
				tooltip=gLang.GetText("Tehrirlewatqan höjjetni diskigha bashqa isim bilen saqlaydu");
			}
			else if(curMenu == menuHKod){
				tooltip = gLang.GetText("Tehrirlewatqan höjjettiki mezmunlar normal körünmise, bu yerni sinap béqing");
			}
			else if(curMenu == menuImlaAuto){
				tooltip = gLang.GetText("Xata-toghra ambirini ishlitip imlasi xata sözlerni aptomatik tüzitidu");
			}
			else if(curMenu == menuBelge){
				tooltip = gLang.GetText("Tinish belgilerning aldi-keynidiki kem qalghan yaki artuqche qoshulup qalghan boshluqlarni toghrilaydu.");
			}
			else if(curMenu == menuImlaUEY || curMenu == menuImlaUEY ||curMenu == menuImlaUEY){
				if(curMenu.Checked){
					tooltip = gLang.GetText("Yene bir chékilse Imla tekshürmeydu.");
				}
				else{
					tooltip = gLang.GetText("Imla tekshürüsh üchün chéking.");
				}
			}
			else if(curMenu == menuYeziqAuto){
				tooltip = curMenu.ToolTipText;
			}
			
			if(tooltip!=null){
				stBarUchur.Text = tooltip;
			}
		}
		
		void MenuBasClick(object sender, EventArgs e)
		{
//			gEditor.Document.print();
		}
		
		void KunupkaClick(object sender, EventArgs e)
		{
			KUNUPKA kun;
			if(gKunupka == KUNUPKA.System){
				kun = KUNUPKA.Uyghur;
			}
			else{
				kun = KUNUPKA.System;
			}
			SetKunupka(kun);
		}
		
		void SetKunupka(KUNUPKA kun){
			gKunupka = kun;
			if(gKunupka == KUNUPKA.System){
				stKunupka.Text = "System";
			}
			else{
				stKunupka.Text = "ئۇيغۇرچە";
			}
			gConfig["KUNUPKA"] = gKunupka;
		}
		
		//Hojjet baldiqi Qosh chekilse(Eng axirqi hojjetke yeqin yerder)
		//Yengi hojjet yasilidu
		void MainFormMouseDoubleClick(object sender, MouseEventArgs e)
		{
			int last = mainTab.TabPages.Count-1;
			Rectangle rc = mainTab.GetTabRect(last);
			rc.X = rc.X+rc.Width;
			Point np = new Point(e.Location.X, e.Location.Y - (menuBar.Height + rc.Height));
			if(rc.Contains(np))
			{
				MenuYengiClick(null,null);
			}
		}
		
		void MenuBelgeClick(object sender, EventArgs e)
		{
			Regex  regkopbosh= new Regex("[ ]{2,}",RegexOptions.Compiled);
			Regex  regaxir= new Regex("[ \t]*([!\\)›»\\]؟،؛?,;])",RegexOptions.Compiled); //Axirlashqan tinish belgiler
			Regex  regbash= new Regex("([‹«\\(\\[])[ \t]*",RegexOptions.Compiled); //Axirlashqan tinish belgiler

			Regex  siziq = new Regex("[ ]*[-–][ ]*",RegexOptions.Compiled);

			string txt = siziq.Replace(gEditor.Text, "-");
			txt = regbash.Replace(txt, new MatchEvaluator(bash));
			txt = regaxir.Replace(txt, new MatchEvaluator(axir));
			txt = regkopbosh.Replace(txt," ");
			MenuYengiClick(null,null);
			gEditor.Text = txt;
			if(Uyghur.Detect(txt) == Uyghur.YEZIQ.UEY){
				gEditor.RightToLeft = true;
			}
			else{
				gEditor.RightToLeft = false;
			}
			TabControl1SelectedIndexChanged(null,null);
		}
		
		string bash(Match mm){
			return " "+mm.Value.Trim();
		}

		string axir(Match mm){
			return mm.Value.Trim()+" ";
		}
		
		
		void MenuImlaAutoClick(object sender, EventArgs e)
		{
			string alltext = gEditor.Text;
			if (alltext.Length == 0)
				return;
			
			int sani = 0;
			int xatasani = 0;
			int tuz = 0;
			string toghrisi;
			int stpos = 0;
			Match soz;
			while((soz = gImla.WordFinder.Match(alltext,stpos)).Success)
			{
				sani++;
				if(gImla.SpellCheker.IsListed(soz.Value)==false)
				{
					xatasani++;
					toghrisi = gImla.SpellCheker.Toghrisi(soz.Value);
					if(toghrisi!=null){
						if(char.IsUpper(soz.Value[0])){
							toghrisi=char.ToUpper(toghrisi[0])+toghrisi.Substring(1);
						}
						gEditor.CaretOffset = soz.Index;
						gEditor.Document.Replace(soz.Index,soz.Value.Length,toghrisi);
						alltext = gEditor.Text.ToLower();
						stpos = soz.Index+toghrisi.Length;
						tuz++;
						continue;
					}
					if(gImla.WordFinder == gLatincheSoz)
					{
						toghrisi = soz.Value.Replace('o','ö').Replace('u','ü').Replace('e','é');
						if(gImla.SpellCheker.IsListed(toghrisi)){
							if(char.IsUpper(soz.Value[0])){
								toghrisi=char.ToUpper(toghrisi[0])+toghrisi.Substring(1);
							}
							gEditor.CaretOffset = soz.Index;
							gEditor.Document.Replace(soz.Index,soz.Value.Length,toghrisi);
							alltext = gEditor.Text.ToLower();
							stpos = soz.Index+toghrisi.Length;
							tuz++;
							continue;
						}
					}
				}
				stpos = soz.Index+soz.Value.Length;
			}
			
			gEditor.BringCaretToView();
			if(sani>0)
			{
				float toghriliq = (float)(sani-xatasani)/(float)sani;
				stBarUchur.Text = gLang.GetText("Tekshürülgen söz: ") + sani + "; " + gLang.GetText("Xata söz: ") + xatasani + "; " + gLang.GetText("Tüzitilgen söz: ") + tuz + "; "+ gLang.GetText("Toghriliqi: ")+toghriliq.ToString("0.0%");
			}
		}
		
		
//		string SanlarniOqu(string esli){
//			string oqughan=esli;
//			Regex reg = new Regex("[0-9]+[ ]*[-]?"); //(@"^\d$");
//			oqughan=reg.Replace(esli,match=>
//			                    {
//			                    	string san = match.ToString();
//			                    	return numstr2str(san);
//			                    });
//			return oqughan;
//		}
		


		private void menuCodePageClick(object sender, System.EventArgs e)
		{
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			if(menu==null) return;
			if(menu.Checked) return;
			
			int codePage = (int)menu.Tag;
			if(-3==codePage||
			   -2==codePage||
			   -1==codePage
			  )
			{
				FileStream inStrm = File.OpenRead(mainTab.SelectedTab.Tag.ToString());
				byte[] Buffer=new byte[inStrm.Length];
				inStrm.Read(Buffer,0,Buffer.Length);
				inStrm.Close();
				switch(codePage)
				{
					case -1:
						gEditor.Text=Uyghur.WeifangDosToUnicode(Buffer);
						gEditor.IsModified = false;
						break;
					case -2:
						gEditor.Text = Uyghur.WeifangDosToUnicode(Buffer);
						gEditor.IsModified = false;
						break;
					case -3:
						gEditor.Text=Uyghur.WeifangDosToUnicode(Buffer);
						gEditor.IsModified = false;
						break;
				}
				gEditor.CodePage = codePage;
			}
			else{
				gEditor.Encoding = System.Text.Encoding.GetEncoding(codePage);
				gEditor.Load(mainTab.SelectedTab.Tag.ToString());
			}
			
			TabControl1SelectedIndexChanged(null,null);
			
		}
		
		void MainFormDragEnter(object sender, System.Windows.DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			String  baseName = Path.GetFileName(file[0]).ToLower();
			
			e.Effects=  System.Windows.DragDropEffects.All;
		}
		
		void MainFormDragDrop(object sender, System.Windows.DragEventArgs e)
		{
			String[] files=(String[])e.Data.GetData(DataFormats.FileDrop);
			string 	file=files[0];
			OpenaFile(file);
		}
		
		void MenuHeqqideClick(object sender, EventArgs e)
		{
			FormHeqqide heqqide = new FormHeqqide();
			heqqide.ShowInTaskbar = false;
			heqqide.ShowDialog();
			gEditor.Focus();
		}
		
		void MenuTekrarClick(object sender, EventArgs e)
		{
			string ngramtext="";
			NGram ngram = new NGram(1);
			Uyghur.YEZIQ curYeziq = Uyghur.Detect(gEditor.Text);
			if(curYeziq == Uyghur.YEZIQ.UEY)
			{
				ngramtext = ngram.MakeNGram(gEditor.Text,this.gUyghurcheSoz);
				MenuYengiClick(null,null);
				gEditor.Text = ngramtext;
				gEditor.RightToLeft = true;
			}
			else if(curYeziq == Uyghur.YEZIQ.ULY){
				ngramtext = ngram.MakeNGram(gEditor.Text,this.gLatincheSoz);
				MenuYengiClick(null,null);
				gEditor.Text = ngramtext;
				gEditor.RightToLeft = false;
			}
			else if(curYeziq == Uyghur.YEZIQ.USY){
				ngramtext = ngram.MakeNGram(gEditor.Text,this.gSlawyancheSoz);
				MenuYengiClick(null,null);
				gEditor.Text = ngramtext;
				gEditor.RightToLeft = false;
			}
			TabControl1SelectedIndexChanged(null,null);
		}
		
		void MenuTizClick(object sender, EventArgs e)
		{
			List<string> abzaslar = new List<string>();
			foreach(DocumentLine qur in gEditor.Document.Lines){
				abzaslar.Add(gEditor.Document.GetText(qur.Offset,qur.Length));
			}
			USort usort= new USort();
			string sortedabzaslar = usort.Sort(abzaslar);
			MenuYengiClick(null,null);
			gEditor.WordWrap = false;
			gEditor.Text = sortedabzaslar;
			TabControl1SelectedIndexChanged(null,null);
		}
		
		void MenuOCRClick(object sender, EventArgs e)
		{
			OCRForm ocr = new OCRForm(gEditor);
			ocr.Owner = this;
			ocr.ShowInTaskbar = false;
			ocr.Show(this);
		}
		
		void MenuFontClick(object sender, EventArgs e)
		{
			FontDialog fontDlg = new FontDialog();
			Font tmpFont = new Font(gFontName,gFontSize);
			fontDlg.Font = tmpFont;
			fontDlg.ShowApply = true;
			fontDlg.ShowColor = false;
			fontDlg.ShowEffects = false;
			
			if(fontDlg.ShowDialog() != DialogResult.Cancel ){
				gFontName = fontDlg.Font.Name;
				gFontSize = fontDlg.Font.Size;
				gFontStyle  = fontDlg.Font.Style==FontStyle.Italic?1:0;
				gFontWeight = fontDlg.Font.Style==FontStyle.Bold? 1: 0;
				
				gConfig["FONTNAME"]  = gFontName;
				gConfig["FONTSIZE"]  = gFontSize;
				gConfig["FONTSTYLE"] = gFontStyle;
				gConfig["FONTWEIGHT"] = gFontWeight;
				
				gEditor.FontFamily = new System.Windows.Media.FontFamily(gFontName);
				gEditor.FontSize   = gFontSize;
				gEditor.FontStyle = gFontStyle == 0? System.Windows.FontStyles.Normal:System.Windows.FontStyles.Italic;
				gEditor.FontWeight = gFontWeight == 0? System.Windows.FontWeights.Normal:System.Windows.FontWeights.Bold;
				
				gContextMenu.FontFamily = gEditor.FontFamily;
				gContextMenu.FontSize = 20;
				gContextMenu.FontStyle = gEditor.FontStyle;
				
			}
		}
		
		void MenuYeziqAutoClick(object sender, EventArgs e)
		{
			gYeziqAuto = ! gYeziqAuto;
			menuYeziqAuto.Checked = gYeziqAuto;
			gConfig["YEZIQAUTO"] = gYeziqAuto;
			
			if(gYeziqAuto){
				TabControl1SelectedIndexChanged(null,null);
			}
		}
		
		void MenuQoralDropDownOpened(object sender, EventArgs e)
		{
			Uyghur.YEZIQ curYeziq = Uyghur.Detect(gEditor.Text);
			if(curYeziq == Uyghur.YEZIQ.UEY){
				menuTiz.Enabled = true;
			}
			else{
				menuTiz.Enabled = false;
			}
		}
		void MenuHKodDropDownOpened(object sender, EventArgs e)
		{
			if(gEditor.Encoding!=null){
				foreach(var itm in menuHKod.DropDownItems){
					var curitm = itm as ToolStripMenuItem;
					if(curitm!=null){
						curitm.Checked = false;
						if(gEditor.CodePage == (int)curitm.Tag){
							curitm.Checked = true;
						}
					}
				}
			}
		}
		void MenuImlaAmbarClick(object sender, EventArgs e)
		{
			if(File.Exists(gImla.SpellCheker.IshletkcuhiAmbarIsimi)){
				AddNew(gImla.SpellCheker.IshletkcuhiAmbarIsimi);
			}
			
			if(File.Exists(gImla.SpellCheker.XataToghraAmbarIsimi)){
				AddNew(gImla.SpellCheker.XataToghraAmbarIsimi);
			}
		}
		
		class NGram:IComparer<string>
		{
			int N = 1;
			public NGram(int cntGram = 1){
				N= cntGram;
			}
			
			public String MakeNGram(string strOrg, Regex sozFinder){
				int gSozSani = 0;
				TstTree           mBuf=new TstTree();
				StringBuilder     wBuf=new StringBuilder();
				int i,j;
				MatchCollection sozler;
				sozler = sozFinder.Matches(strOrg, 0);
				for(i=0;i<=sozler.Count-N;i++){
					wBuf.Length=0;
					for(j=i;j<N+i;j++){
						wBuf.Append(" "+sozler[j].Value);
					}
					if(mBuf.Add(wBuf.ToString().Trim())==1){
						gSozSani++;
					}
				}
				List<string> ret=new List<string>();
				mBuf.GetAll(1,ret);
				mBuf = null;
				ret.Sort(this);
				wBuf.Length = 0;
				foreach(string str in ret){
					wBuf.AppendLine(str);
				}
				return wBuf.ToString();
			}

			public String MakeNGram(TextEditor curEdit, Regex sozFinder){
				int gSozSani = 0;
				TstTree           mBuf=new TstTree();
				StringBuilder     wBuf=new StringBuilder();
				int i,j;
				MatchCollection sozler;
				foreach(DocumentLine qur in curEdit.Document.Lines){
					sozler = sozFinder.Matches(curEdit.Document.GetText(qur.Offset,qur.Length), 0);
					if(sozler.Count<N) continue ;
					for(i=0;i<=sozler.Count-N;i++){
						wBuf.Length=0;
						for(j=i;j<N+i;j++){
							wBuf.Append(" "+sozler[j].Value);
						}
						if(mBuf.Add(wBuf.ToString().Trim())==1){
							gSozSani++;
						}
					}
				}
				List<string> ret=new List<string>();
				mBuf.GetAll(1,ret);
				mBuf = null;
				ret.Sort(this);
				wBuf.Length = 0;
				foreach(string str in ret){
					wBuf.AppendLine(str);
				}
				return wBuf.ToString();
			}
			
			public int Compare(string aa,string bb)
			{
				int frq1=int.Parse(aa.Split('\t')[1]);
				int frq2=int.Parse(bb.Split('\t')[1]);
				return frq2-frq1;
			}
		}
		
		class USort:IComparer<string>
		{
			public USort(){
				
			}
			
			public string Sort(List<string> abzaslar){
				StringBuilder     wBuf=new StringBuilder();
				abzaslar.Sort(this);
				foreach(string str in abzaslar){
					wBuf.AppendLine(str);
				}
				return wBuf.ToString();
			}
			
			public int Compare_normal(string a,string b)
			{
				int ret=0;
				a = a.Replace(Uyghur.Sozghuch,"");
				b = b.Replace(Uyghur.Sozghuch,"");
				int lenA=a.Length;
				int lenB=b.Length;
				a +='\0';
				b +='\0';
				int posA=0,posB=0;
				int indexA=0,indexB=0;
				int index;
				bool siziqche=false;
				while((posA<=lenA) && (posB<=lenB))
				{
					if(a[posA]=='-'||a[posA]==' ')
					{
						posA++;
						siziqche=true;
						continue;
					}

					if(b[posB]=='-'||b[posB]==' ')
					{
						posB++;
						siziqche=true;
						continue;
					}

					indexA=2*Uyghur.GetUnicodeIndex(a[posA]);
					indexB=2*Uyghur.GetUnicodeIndex(b[posB]);
					if(indexA==2*Uyghur.UYGIN_HM)
					{
						indexA=67;
						if((posA+1)<=lenA){
							index=Uyghur.GetUnicodeIndex(a[posA+1]);
							if(index==Uyghur.UYGIN_A || index==Uyghur.UYGIN_E|| index==Uyghur.UYGIN_EE || index==Uyghur.UYGIN_I|| index==Uyghur.UYGIN_O|| index==Uyghur.UYGIN_OO|| index==Uyghur.UYGIN_U|| index==Uyghur.UYGIN_UU)
							{
								posA++;
								indexA=index*2-1;
							}
						}
					}
					if(indexB==2*Uyghur.UYGIN_HM)
					{
						indexB=67;
						if((posB+1)<=lenB){
							index=Uyghur.GetUnicodeIndex(b[posB+1]);
							if(index==Uyghur.UYGIN_A || index==Uyghur.UYGIN_E|| index==Uyghur.UYGIN_EE || index==Uyghur.UYGIN_I|| index==Uyghur.UYGIN_O|| index==Uyghur.UYGIN_OO|| index==Uyghur.UYGIN_U|| index==Uyghur.UYGIN_UU)
							{
								posB++;
								indexB=index*2-1;
							}
						}
					}

					if(indexA>=0 && indexB>=0)
					{
						ret=indexA-indexB;
					}
					else{
						ret=(int)(a[posA]-b[posB]);
					}
					if(ret!=0)
					{
						break;
					}
					posA++;
					posB++;
				}

				if(ret==0 && siziqche==true)
				{
					posA=0;
					posB=0;
					while((posA<=lenA) && (posB<=lenB))
					{
						indexA=2*Uyghur.GetUnicodeIndex(a[posA]);
						indexB=2*Uyghur.GetUnicodeIndex(b[posB]);
						if(indexA==2*Uyghur.UYGIN_HM)
						{
							indexA=65;
							if((posA+1)<=lenA){
								index=Uyghur.GetUnicodeIndex(a[posA+1]);
								if(index==Uyghur.UYGIN_A || index==Uyghur.UYGIN_E|| index==Uyghur.UYGIN_EE || index==Uyghur.UYGIN_I|| index==Uyghur.UYGIN_O|| index==Uyghur.UYGIN_OO|| index==Uyghur.UYGIN_U|| index==Uyghur.UYGIN_UU)
								{
									posA++;
									indexA=index*2-1;
								}
							}
						}
						if(indexB==2*Uyghur.UYGIN_HM)
						{
							indexB=65;
							if((posB+1)<=lenB){
								index=Uyghur.GetUnicodeIndex(b[posB+1]);
								if(index==Uyghur.UYGIN_A || index==Uyghur.UYGIN_E|| index==Uyghur.UYGIN_EE || index==Uyghur.UYGIN_I|| index==Uyghur.UYGIN_O|| index==Uyghur.UYGIN_OO|| index==Uyghur.UYGIN_U|| index==Uyghur.UYGIN_UU)
								{
									posB++;
									indexB=index*2-1;
								}
							}
						}

						if(indexA>=0 && indexB>=0)
						{
							ret=indexA-indexB;
						}
						else{
							ret=(int)(a[posA]-b[posB]);
						}
						if(ret!=0)
						{
							break;
						}
						posA++;
						posB++;
					}
				}
				return ret;
			}
			
			public int Compare(string a,string b)
			{
				int ret=0;
				a = a.Replace(Uyghur.Sozghuch,"");
				b = b.Replace(Uyghur.Sozghuch,"");
				int lenA=a.Length;
				int lenB=b.Length;
				a +='\0';
				b +='\0';
				int posA=0,posB=0;
				int indexA=0,indexB=0;
				ret=0;
				bool hemze=false;

				while((posA<=lenA) && (posB<=lenB))
				{
					indexA=Uyghur.GetUnicodeIndex(a[posA]);
					indexB=Uyghur.GetUnicodeIndex(b[posB]);
					if(indexA==Uyghur.UYGIN_HM){posA++;hemze=true;continue;}
					if(indexB==Uyghur.UYGIN_HM){posB++;hemze=true;continue;}
					if(indexA>=0 && indexB>=0)
					{
						ret=indexA-indexB;
					}
					else{
						ret=(int)(a[posA]-b[posB]);
					}
					if(ret!=0)
					{
						break;
					}
					posA++;
					posB++;
				}

				if(ret==0)
				{
					if(hemze)
						ret=lenA-lenB;
				}
				return ret;
			}


			public int Compare_neme(string a,string b)
			{
				int ret=0;
				int lenA=a.Length;
				int lenB=b.Length;
				int posA=0,posB=0;
				int indexA=0,indexB=0;
				int index;
				bool siziqche=false;
				while((posA<lenA) && (posB<lenB))
				{
					if(a[posA]=='-'||a[posA]==' ')
					{
						posA++;
						siziqche=true;
						continue;
					}

					if(b[posB]=='-'||b[posB]==' ')
					{
						posB++;
						siziqche=true;
						continue;
					}

					indexA=2*Uyghur.GetUnicodeIndex(a[posA]);
					indexB=2*Uyghur.GetUnicodeIndex(b[posB]);
					if(indexA==2*Uyghur.UYGIN_HM)
					{
						if((posA+1)<=lenA){
							index=2*Uyghur.GetUnicodeIndex(a[posA+1]);
							if(index>0)
							{
								posA++;
								indexA=index-1;
							}
						}
					}
					if(indexB==2*Uyghur.UYGIN_HM)
					{
						if((posB+1)<=lenB){
							index=2*Uyghur.GetUnicodeIndex(b[posB+1]);
							if(index>0)
							{
								posB++;
								indexB=index-1;
							}
						}
					}
					if(indexA>=0 && indexB>=0)
					{
						ret=indexA-indexB;
					}
					else{
						ret=(int)(a[posA]-b[posB]);
					}
					if(ret!=0)
					{
						break;
					}
					posA++;
					posB++;
				}
				if(ret==0 && siziqche==true)
				{
					posA=0;
					posB=0;
					while((posA<=lenA) && (posB<=lenB))
					{
						indexA=2*Uyghur.GetUnicodeIndex(a[posA]);
						indexB=2*Uyghur.GetUnicodeIndex(b[posB]);
						if(indexA==2*Uyghur.UYGIN_HM)
						{
							if((posA+1)<=lenA){
								index=2*Uyghur.GetUnicodeIndex(a[posA+1]);
								if(index>=0)
								{
									posA++;
									indexA=index-1;
								}
							}
						}
						if(indexB==2*Uyghur.UYGIN_HM)
						{
							if((posB+1)<=lenB){
								index=2*Uyghur.GetUnicodeIndex(b[posB+1]);
								if(index>=0)
								{
									posB++;
									indexB=index-1;
								}
							}
						}
						if(indexA>=0 && indexB>=0)
						{
							ret=indexA-indexB;
						}
						else{
							ret=(int)(a[posA]-b[posB]);
						}
						if(ret!=0)
						{
							break;
						}
						posA++;
						posB++;
					}
				}
				return ret;
			}
		}
	}

	enum KUNUPKA{System=0,Uyghur,UyghurLY};
}
