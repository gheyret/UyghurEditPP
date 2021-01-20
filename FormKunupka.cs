/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2020/11/27
 * Time: 13:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of FormKunupka.
	/// </summary>
	public partial class FormKunupka : Form
	{
		MainForm parForm;
		public FormKunupka(MainForm fp)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			parForm = fp;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void FormKunupkaLoad(object sender, EventArgs e)
		{
			int startx = parForm.Location.X + (parForm.Width-this.Width)/2;
			int starty = parForm.Location.Y + (parForm.Height-this.Height)/2;
			this.Location = new Point(startx,starty);
		}
	}
}
