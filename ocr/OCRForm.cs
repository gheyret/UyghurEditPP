/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2017/05/29
 * Time: 11:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.IO;
using Tesseract;
using System.Drawing;
using System.Threading.Tasks;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class OCRForm : Form
	{
		TesseractEngine  gOcr = null;
		TextEditor       gEditor;
		string           gImgFile = null;
		ToolTip          gTip;
		bool             gRunning = false;
		
		public OCRForm(TextEditor curedit)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			gEditor = curedit;
			InitializeComponent();
			System.Reflection.Assembly asm =System.Reflection.Assembly.GetExecutingAssembly();
			gTip = new ToolTip();
		}
		
		public string ImageFile{
			set{
				gImgFile= value;
				if(gImgFile!=null){
					Bitmap bimg = new Bitmap(gImgFile);
					ramka.Image=bimg;
					Invalidate();
				}
			}
		}
		
		string Til{
			get;
			set;
		}
		
		void OCRFormPaint(object sender, PaintEventArgs e)
		{
			if(Til.Length==0 || ramka.Image == null){
				butTonu.Enabled = false;
			}
			else{
				butAch.Enabled = !gRunning;
				butTonu.Enabled = !gRunning;
				chkUyghur.Enabled = !gRunning;
				chkEng.Enabled = !gRunning;
				chkRus.Enabled = !gRunning;
				chkChi.Enabled = !gRunning;
				chkTur.Enabled = !gRunning;
				radAuto.Enabled = !gRunning;
				radSingle.Enabled = !gRunning;
			}
		}
		
		async void ButtonRight(object sender, EventArgs e)
		{
			try{
				gRunning = true;
				if(radAuto.Checked){
					gOcr.DefaultPageSegMode = PageSegMode.Auto;
				}
				else{
					gOcr.DefaultPageSegMode = PageSegMode.SingleBlock;
				}
				Invalidate();
				Bitmap roibmp;
				Pix    roipix;
				Rectangle roi = ramka.getRoi();
				Cursor=Cursors.WaitCursor;
				ramka.Enabled = false;
				roibmp = ramka.Image.Clone(roi,ramka.Image.PixelFormat);
				roibmp.SetResolution(400,400);
				roipix = PixConverter.ToPix(roibmp).Deskew().Scale(4.3f,4.3f);
				roibmp.Dispose();
				
				Task<string> ocr = Task.Run<string>(() =>{return DoOCR(roipix);});
				string txt = await ocr;
				roipix.Dispose();
				ramka.Enabled = true;
				gEditor.AppendText(txt);
				Cursor=Cursors.Default;
			}
			catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee.Message);
				MessageBox.Show(ee.Message, "UyghurEdit++", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			gRunning = false;
			Invalidate();
		}
		
		
		string DoOCR(Pix pix){
			gOcr.DefaultPageSegMode = PageSegMode.SingleBlock;
			Page pg = gOcr.Process(pix);
			String buf = pg.GetText();
			pix.Dispose();
			pg.Dispose();
			return buf.Replace("ی","ي").Replace("ه","ە").Replace("\n",Environment.NewLine);
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void OCRFormShown(object sender, EventArgs e)
		{
			butAch.Text = MainForm.gLang.GetText("Ach");
			gTip.SetToolTip(butAch,MainForm.gLang.GetText("Bu yerni chékip resimni éching yaki resimni tutup bu köznekke tashlang."));
			gTip.SetToolTip(ramka,MainForm.gLang.GetText("Resim körün’gende, Chashqinek bilen tonutidighan da’irini tallang."));
			butTonu.Text = MainForm.gLang.GetText("Tonu");
			
			label1.Text = MainForm.gLang.GetText("Tonuydighan Tillar")+":";
			label2.Text = MainForm.gLang.GetText("Bet Qurulmisi")+":";
			
			chkUyghur.Text = MainForm.gLang.GetText("Uyghurche");
			chkEng.Text = MainForm.gLang.GetText("In’glizche");
			chkTur.Text = MainForm.gLang.GetText("Türkche");
			chkChi.Text = MainForm.gLang.GetText("Xenzuche");
			chkRus.Text = MainForm.gLang.GetText("Slawyanche");
			radAuto.Text = MainForm.gLang.GetText("Özüng Tap");
			radSingle.Text = MainForm.gLang.GetText("Birla Bölek");
			
			chkUyghur.Checked = true;
			radAuto.Checked = true;
			
			int startx = this.Owner.Location.X + (this.Owner.Width-this.Width)/2;
			int starty = this.Owner.Location.Y + (this.Owner.Height-this.Height)/2;
			this.Location = new Point(startx,starty);
		}
		
		
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			String  extName = Path.GetExtension(file[0]);
			if(MainForm.gImgexts.IndexOf(extName,StringComparison.OrdinalIgnoreCase)!=-1)
			{
				e.Effect= DragDropEffects.All;
			}
		}
		
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			gImgFile=file[0];
			Bitmap bimg = new Bitmap(gImgFile);
			ramka.Image=bimg;
			
			Invalidate();
		}
		void ButAchClick(object sender, EventArgs e)
		{
			OpenFileDialog opnFileDlg = new OpenFileDialog();
			String filter = "Image files|" + MainForm.gImgexts;
			opnFileDlg.Filter = filter;
			opnFileDlg.Multiselect = false;
			if(opnFileDlg.ShowDialog(this)== DialogResult.OK){
				Bitmap bimg = new Bitmap(opnFileDlg.FileName);
				ramka.Image=bimg;
			}
			Invalidate();
		}
		
		public Image Resim{
			set{
				ramka.Image=new Bitmap(value);
				Invalidate();
			}
		}
		
		void CheckedChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			char[] tr = {'+'};
			string lang = "";
			if(chkUyghur.Checked){
				lang += "ukij+uig";
			}

			if(chkEng.Checked){
				lang += "+eng";
			}
			
			if(chkTur.Checked){
				lang += "+tur";
			}

			if(chkChi.Checked){
				lang += "+chi_sim";
			}

			if(chkRus.Checked){
				lang += "+rus";
			}
			lang = lang.Trim(tr);
			System.Diagnostics.Debug.WriteLine(lang);
			if(gOcr!=null){
				gOcr.Dispose();
				gOcr = null;
			}
			Til = lang;
			
			if(lang.Length >=3){
				gOcr= new TesseractEngine(@".\tessdata",lang,EngineMode.LstmOnly);
				Text = MainForm.gLang.GetText("Uyghurche OCR(Resimdiki Yéziqni Tonush) Programmisi")+ "Tessract[v " +  gOcr.Version + "]" + " neshrini ishletken";
			}
			this.Cursor = Cursors.Default;
			Invalidate();
		}
		void OCRFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(gRunning){
				e.Cancel = true;
			}
			else{
				if(gOcr!=null){
					gOcr.Dispose();
				}
			}
		}
		void OCRFormKeyUp(object sender, KeyEventArgs e)
		{
			int gModkey =(int)Control.ModifierKeys;
			gModkey = (gModkey>>16) &0x000f;
			if(gModkey == 2 && e.KeyCode == Keys.V) {
				System.Diagnostics.Debug.WriteLine("OCR Key UP");
				IDataObject dataObject = Clipboard.GetDataObject();
				if(dataObject==null) return;
				if(dataObject.GetDataPresent(DataFormats.Bitmap)){
					Image img = (Image)dataObject.GetData(DataFormats.Bitmap);
					this.Resim = img;
				}
			}
		}
	}
}