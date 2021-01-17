/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2017/05/29
 * Time: 11:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace UyghurEditPP
{
	partial class OCRForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OCRForm));
			this.butTonu = new System.Windows.Forms.Button();
			this.ramka = new UyghurEditPP.ResimRamka();
			this.butAch = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// butTonu
			// 
			this.butTonu.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butTonu.Location = new System.Drawing.Point(379, 503);
			this.butTonu.Name = "butTonu";
			this.butTonu.Size = new System.Drawing.Size(143, 32);
			this.butTonu.TabIndex = 37;
			this.butTonu.Text = "Tonu";
			this.butTonu.UseVisualStyleBackColor = true;
			this.butTonu.Click += new System.EventHandler(this.ButtonRight);
			// 
			// ramka
			// 
			this.ramka.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.ramka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ramka.Image = null;
			this.ramka.Location = new System.Drawing.Point(0, 0);
			this.ramka.Name = "ramka";
			this.ramka.Size = new System.Drawing.Size(577, 497);
			this.ramka.TabIndex = 43;
			// 
			// butAch
			// 
			this.butAch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butAch.Location = new System.Drawing.Point(54, 503);
			this.butAch.Name = "butAch";
			this.butAch.Size = new System.Drawing.Size(143, 32);
			this.butAch.TabIndex = 44;
			this.butAch.Text = "Ach";
			this.butAch.UseVisualStyleBackColor = true;
			this.butAch.Click += new System.EventHandler(this.ButAchClick);
			// 
			// OCRForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(577, 542);
			this.Controls.Add(this.butAch);
			this.Controls.Add(this.ramka);
			this.Controls.Add(this.butTonu);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OCRForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Simple Uyghur OCR using Tessract";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
			this.ResumeLayout(false);

		}
		private UyghurEditPP.ResimRamka ramka;
		private System.Windows.Forms.Button butTonu;
		private System.Windows.Forms.Button butAch;
	}
}