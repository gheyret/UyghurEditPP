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
		Random           grand = new System.Random();
		TesseractEngine  gOcr;
		TextEditor       gEditor;
		string           gImgFile = null;
		ToolTip          gTip;
		public OCRForm(TextEditor curedit, string imgFile = null)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			gEditor = curedit;
			gImgFile = imgFile;
			InitializeComponent();
			System.Reflection.Assembly asm =System.Reflection.Assembly.GetExecutingAssembly();
			gTip = new ToolTip();
		}
		
		
		
		async void ButtonRight(object sender, EventArgs e)
		{
			butTonu.Enabled = false;
			butAch.Enabled = false;
			Bitmap roibmp;
			Pix    roipix;
			Rectangle roi = ramka.getRoi();
			Cursor=Cursors.WaitCursor;
			ramka.Enabled = false;
			roibmp = ramka.Image.Clone(roi,ramka.Image.PixelFormat);
			roipix = PixConverter.ToPix(roibmp);
			roibmp.Dispose();
			roipix = roipix.Deskew();
			
			Task<string> ocr = Task.Run<string>(() =>{return DoOCR(roipix);});
			string txt = await ocr;
			roipix.Dispose();
			gEditor.AppendText(txt);
			butTonu.Enabled = true;
			butAch.Enabled = true;
			Cursor=Cursors.Default;
			ramka.Enabled = true;
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
			//string lang = "ukij+eng";
			string lang = "ukij";
			gOcr= new TesseractEngine(@".\tessdata",lang,EngineMode.LstmOnly);
			Text = MainForm.gLang.GetText("Uyghurche OCR(Resimdiki Yéziqni Tonush) Programmisi")+ "Tessract[v " +  gOcr.Version + "]" + " neshrini ishletken";
			butAch.Text = MainForm.gLang.GetText("Ach");
			gTip.SetToolTip(butAch,MainForm.gLang.GetText("Bu yerni chékip resimni éching yaki resimni tutup bu köznekke tashlang."));
			gTip.SetToolTip(ramka,MainForm.gLang.GetText("Resim körün’gende, Chashqinek bilen tonutidighan da’irini tallang."));
			butTonu.Text = MainForm.gLang.GetText("Tonu");
			
			if(gImgFile!=null){
				Bitmap bimg = new Bitmap(gImgFile);
				ramka.Image=bimg;
			}
			
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
		}
	}
}