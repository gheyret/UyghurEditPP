﻿/*
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
            this.chkUyghurUKIJ = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkEng = new System.Windows.Forms.CheckBox();
            this.chkChi = new System.Windows.Forms.CheckBox();
            this.chkRus = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radAuto = new System.Windows.Forms.RadioButton();
            this.radSingle = new System.Windows.Forms.RadioButton();
            this.chkTur = new System.Windows.Forms.CheckBox();
            this.chkUyghur = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // butTonu
            // 
            this.butTonu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butTonu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butTonu.Location = new System.Drawing.Point(8, 543);
            this.butTonu.Margin = new System.Windows.Forms.Padding(0);
            this.butTonu.Name = "butTonu";
            this.butTonu.Size = new System.Drawing.Size(170, 30);
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
            this.ramka.Location = new System.Drawing.Point(188, 0);
            this.ramka.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ramka.Name = "ramka";
            this.ramka.Size = new System.Drawing.Size(615, 575);
            this.ramka.TabIndex = 43;
            // 
            // butAch
            // 
            this.butAch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butAch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butAch.Location = new System.Drawing.Point(8, 501);
            this.butAch.Margin = new System.Windows.Forms.Padding(0);
            this.butAch.Name = "butAch";
            this.butAch.Size = new System.Drawing.Size(170, 30);
            this.butAch.TabIndex = 44;
            this.butAch.Text = "Ach";
            this.butAch.UseVisualStyleBackColor = true;
            this.butAch.Click += new System.EventHandler(this.ButAchClick);
            // 
            // chkUyghurUKIJ
            // 
            this.chkUyghurUKIJ.Location = new System.Drawing.Point(25, 33);
            this.chkUyghurUKIJ.Margin = new System.Windows.Forms.Padding(0);
            this.chkUyghurUKIJ.Name = "chkUyghurUKIJ";
            this.chkUyghurUKIJ.Size = new System.Drawing.Size(155, 26);
            this.chkUyghurUKIJ.TabIndex = 45;
            this.chkUyghurUKIJ.Text = "Uyghurche(UKIJ)";
            this.chkUyghurUKIJ.UseVisualStyleBackColor = true;
            this.chkUyghurUKIJ.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 27);
            this.label1.TabIndex = 46;
            this.label1.Text = "Tonuydighan Tillar";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEng
            // 
            this.chkEng.Location = new System.Drawing.Point(25, 100);
            this.chkEng.Margin = new System.Windows.Forms.Padding(0);
            this.chkEng.Name = "chkEng";
            this.chkEng.Size = new System.Drawing.Size(155, 26);
            this.chkEng.TabIndex = 47;
            this.chkEng.Text = "Inglizche";
            this.chkEng.UseVisualStyleBackColor = true;
            this.chkEng.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // chkChi
            // 
            this.chkChi.Location = new System.Drawing.Point(25, 166);
            this.chkChi.Margin = new System.Windows.Forms.Padding(0);
            this.chkChi.Name = "chkChi";
            this.chkChi.Size = new System.Drawing.Size(155, 26);
            this.chkChi.TabIndex = 48;
            this.chkChi.Text = "Xenzuche";
            this.chkChi.UseVisualStyleBackColor = true;
            this.chkChi.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // chkRus
            // 
            this.chkRus.Location = new System.Drawing.Point(25, 199);
            this.chkRus.Margin = new System.Windows.Forms.Padding(0);
            this.chkRus.Name = "chkRus";
            this.chkRus.Size = new System.Drawing.Size(155, 26);
            this.chkRus.TabIndex = 49;
            this.chkRus.Text = "Slawyanche";
            this.chkRus.UseVisualStyleBackColor = true;
            this.chkRus.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 246);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 27);
            this.label2.TabIndex = 50;
            this.label2.Text = "Bet Qurulmisi:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radAuto
            // 
            this.radAuto.Checked = true;
            this.radAuto.Location = new System.Drawing.Point(25, 276);
            this.radAuto.Name = "radAuto";
            this.radAuto.Size = new System.Drawing.Size(153, 30);
            this.radAuto.TabIndex = 51;
            this.radAuto.TabStop = true;
            this.radAuto.Text = "Ozung Tap";
            this.radAuto.UseVisualStyleBackColor = true;
            // 
            // radSingle
            // 
            this.radSingle.Location = new System.Drawing.Point(25, 312);
            this.radSingle.Name = "radSingle";
            this.radSingle.Size = new System.Drawing.Size(153, 30);
            this.radSingle.TabIndex = 52;
            this.radSingle.Text = "Birla Bolek";
            this.radSingle.UseVisualStyleBackColor = true;
            // 
            // chkTur
            // 
            this.chkTur.Location = new System.Drawing.Point(23, 133);
            this.chkTur.Margin = new System.Windows.Forms.Padding(0);
            this.chkTur.Name = "chkTur";
            this.chkTur.Size = new System.Drawing.Size(155, 26);
            this.chkTur.TabIndex = 53;
            this.chkTur.Text = "Türkche";
            this.chkTur.UseVisualStyleBackColor = true;
            this.chkTur.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // chkUyghur
            // 
            this.chkUyghur.Location = new System.Drawing.Point(23, 65);
            this.chkUyghur.Margin = new System.Windows.Forms.Padding(0);
            this.chkUyghur.Name = "chkUyghur";
            this.chkUyghur.Size = new System.Drawing.Size(155, 26);
            this.chkUyghur.TabIndex = 54;
            this.chkUyghur.Text = "Uyghurche";
            this.chkUyghur.UseVisualStyleBackColor = true;
            // 
            // OCRForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(804, 578);
            this.Controls.Add(this.chkUyghur);
            this.Controls.Add(this.chkTur);
            this.Controls.Add(this.radSingle);
            this.Controls.Add(this.radAuto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkRus);
            this.Controls.Add(this.chkChi);
            this.Controls.Add(this.chkEng);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkUyghurUKIJ);
            this.Controls.Add(this.butAch);
            this.Controls.Add(this.ramka);
            this.Controls.Add(this.butTonu);
            this.Font = new System.Drawing.Font("UKIJ Tuz", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "OCRForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Simple Uyghur OCR using Tessract";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OCRFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.Shown += new System.EventHandler(this.OCRFormShown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OCRFormPaint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OCRFormKeyUp);
            this.ResumeLayout(false);

		}
		private UyghurEditPP.ResimRamka ramka;
		private System.Windows.Forms.Button butTonu;
		private System.Windows.Forms.Button butAch;
		private System.Windows.Forms.CheckBox chkUyghurUKIJ;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkEng;
		private System.Windows.Forms.CheckBox chkChi;
		private System.Windows.Forms.CheckBox chkRus;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton radAuto;
		private System.Windows.Forms.RadioButton radSingle;
		private System.Windows.Forms.CheckBox chkTur;
        private System.Windows.Forms.CheckBox chkUyghur;
    }
}