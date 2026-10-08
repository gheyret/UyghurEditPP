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
		int              gEngineNomur = 0; // which engine request is the latest
		bool             gEngineYasiliwatidu = false; // an engine is being created
		string           gKutuwatqanTil = null;      // languages requested meanwhile
		
		public OCRForm(TextEditor curedit)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			gEditor = curedit;
			InitializeComponent();
			System.Reflection.Assembly asm =System.Reflection.Assembly.GetExecutingAssembly();
			gTip = new ToolTip();
			Til = "";
			UpdateButtons();
		}
		
		public string ImageFile{
			set{
				gImgFile= value;
				if(gImgFile!=null){
					Bitmap bimg = new Bitmap(gImgFile);
					ramka.Image=bimg;
					UpdateButtons();
				}
			}
		}
		
		string Til{
			get;
			set;
		}
		
		// Enables the controls for the current state. This used to be done in the Paint
		// handler, which can run before OCRFormShown has set Til (a NullReferenceException
		// that left the window drawn with red crosses); it is now called whenever the
		// state changes.
		void UpdateButtons()
		{
			butAch.Enabled = !gRunning;
			chkUyghurUKIJ.Enabled = !gRunning;
			chkUyghur.Enabled = !gRunning;
			chkEng.Enabled = !gRunning;
			chkRus.Enabled = !gRunning;
			chkChi.Enabled = !gRunning;
			chkTur.Enabled = !gRunning;
			radAuto.Enabled = !gRunning;
			radSingle.Enabled = !gRunning;
			butTonu.Enabled = !gRunning && gOcr!=null && !string.IsNullOrEmpty(Til) && ramka.Image != null;
		}
		
		async void ButtonRight(object sender, EventArgs e)
		{
			try{
				gRunning = true;
				// The background task uses this engine, not the field, so the field can change safely.
				TesseractEngine engine = gOcr;
				if(radAuto.Checked){
					engine.DefaultPageSegMode = PageSegMode.Auto;
				}
				else{
					engine.DefaultPageSegMode = PageSegMode.SingleBlock;
				}
				UpdateButtons();
				Bitmap roibmp;
				Pix    roipix;
				Rectangle roi = ramka.getRoi();
				Cursor=Cursors.WaitCursor;
				ramka.Enabled = false;
				roibmp = ramka.Image.Clone(roi,ramka.Image.PixelFormat);
//				roibmp.SetResolution(400,400);
				roipix = PixConverter.ToPix(roibmp).Deskew().Scale(4.3f,4.3f);
				roibmp.Dispose();
				
				Task<string> ocr = Task.Run<string>(() =>{return DoOCR(engine, roipix);});
				string txt = await ocr;
				roipix.Dispose();
				gEditor.AppendText(txt);
			}
			catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee.Message);
				MessageBox.Show(ee.Message, "UyghurEdit++", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally{
				// Also after an error, so the picture and the cursor do not stay disabled/busy.
				ramka.Enabled = true;
				Cursor=Cursors.Default;
			}
			gRunning = false;
			UpdateButtons();
		}
		
		
		string DoOCR(TesseractEngine engine, Pix pix){
			engine.DefaultPageSegMode = PageSegMode.SingleBlock;
			Page pg = engine.Process(pix);
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
			
			chkUyghurUKIJ.Text = MainForm.gLang.GetText("Uyghurche")+"UKIJ";
			chkUyghur.Text = MainForm.gLang.GetText("Uyghurche");
            chkEng.Text = MainForm.gLang.GetText("In’glizche");
			chkTur.Text = MainForm.gLang.GetText("Türkche");
			chkChi.Text = MainForm.gLang.GetText("Xenzuche");
			chkRus.Text = MainForm.gLang.GetText("Silawiyanche");
			radAuto.Text = MainForm.gLang.GetText("Özüng Tap");
			radSingle.Text = MainForm.gLang.GetText("Birla Bölek");
			
			chkUyghurUKIJ.Checked = true;
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
			
			UpdateButtons();
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
			UpdateButtons();
		}
		
		public Image Resim{
			set{
				ramka.Image=new Bitmap(value);
				UpdateButtons();
			}
		}
		
		void CheckedChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			char[] tr = {'+'};
			string lang = "";
			if(chkUyghurUKIJ.Checked){
//				lang += "ukij+uig";
				lang += "ukij";
			}

            if (chkUyghur.Checked)
            {
                lang += "uig";
            }

            if (chkEng.Checked){
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
			// Recognition is not running here: the language boxes are disabled while it runs.
			if(gOcr!=null){
				gOcr.Dispose();
				gOcr = null;
			}
			Til = lang;
			UpdateButtons();
			gEngineNomur++;           // an engine still being created is out of date now
			gKutuwatqanTil = null;

			if(lang.Length >=3){
				CreateEngine(lang);
			}
			else{
				this.Cursor = Cursors.Default;
			}
		}

		// Loading the language data takes a while (more for several languages), so the
		// engine is created on a thread-pool thread, one at a time: while one is being
		// created, only the latest request waits, and it starts when that one is done.
		// An engine that is out of date when it is ready (the languages changed again, or
		// the window was closed) is thrown away.
		// tessdata is looked up next to the program, not in the current directory,
		// so OCR also works when UyghurEdit++ is started from another folder.
		void CreateEngine(string lang)
		{
			if(gEngineYasiliwatidu){
				gKutuwatqanTil = lang;
				return;
			}
			StartEngine(lang, gEngineNomur);
		}

		async void StartEngine(string lang, int nomur)
		{
			gEngineYasiliwatidu = true;
			string tessdata = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
			TesseractEngine engine = null;
			Exception xata = null;
			try{
				engine = await Task.Run(() => new TesseractEngine(tessdata,lang,EngineMode.LstmOnly));
			}
			catch(Exception ee){
				xata = ee;
			}
			gEngineYasiliwatidu = false;
			if(nomur != gEngineNomur || IsDisposed){
				if(engine!=null){
					engine.Dispose();
				}
				if(xata!=null){
					ErrorLog.Write(xata); // not shown: nobody waits for this engine any more
				}
				if(!IsDisposed && gKutuwatqanTil!=null){
					string til = gKutuwatqanTil;
					gKutuwatqanTil = null;
					StartEngine(til, gEngineNomur);
				}
				return;
			}
			this.Cursor = Cursors.Default;
			if(engine!=null){
				gOcr = engine;
				Text = MainForm.gLang.GetText("Uyghurche OCR(Resimdiki Yéziqni Tonush) Programmisi")+ "Tessract[v " +  gOcr.Version + "]" + " neshrini ishletken";
			}
			else{
				Til = "";
				ShowEngineError(xata, tessdata);
			}
			UpdateButtons();
		}

		void ShowEngineError(Exception ee, string tessdata)
		{
			ErrorLog.Write(ee);
			string msg;
			if(IsMissingLibrary(ee)){
				msg = MainForm.gLang.GetText("OCR could not start because a Visual C++ runtime library is missing. Please install the Microsoft Visual C++ Redistributable (x64):")
					+ Environment.NewLine + "https://aka.ms/vs/17/release/vc_redist.x64.exe";
			}
			else{
				msg = MainForm.gLang.GetText("OCR could not start. Please check that this folder contains the language data (.traineddata) files:")
					+ Environment.NewLine + tessdata;
			}
			MessageBox.Show(this, msg + Environment.NewLine + Environment.NewLine + ee.Message, "UyghurEdit++", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		static bool IsMissingLibrary(Exception ee)
		{
			for(Exception ex = ee; ex!=null; ex = ex.InnerException){
				if(ex is DllNotFoundException || ex is BadImageFormatException){
					return true;
				}
			}
			return false;
		}
		void OCRFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(gRunning){
				e.Cancel = true;
			}
			else{
				gEngineNomur++; // an engine still being created is thrown away when it is ready
				gKutuwatqanTil = null;
				if(gOcr!=null){
					gOcr.Dispose();
					gOcr = null;
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
					img.Dispose();
				}
			}
		}
	}
}