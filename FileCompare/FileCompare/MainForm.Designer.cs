/*
 * Created by SharpDevelop.
 * User: psycho
 * Date: 2018/12/18
 * Time: 5:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FileCompare
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripComboBox path1Box;
		private System.Windows.Forms.ToolStripComboBox path2Box;
		private System.Windows.Forms.ToolStripSplitButton compareButton;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.path1Box = new System.Windows.Forms.ToolStripComboBox();
			this.path2Box = new System.Windows.Forms.ToolStripComboBox();
			this.compareButton = new System.Windows.Forms.ToolStripSplitButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.path1Box,
			this.path2Box,
			this.compareButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(483, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// path1Box
			// 
			this.path1Box.Name = "path1Box";
			this.path1Box.Size = new System.Drawing.Size(200, 25);
			// 
			// path2Box
			// 
			this.path2Box.Name = "path2Box";
			this.path2Box.Size = new System.Drawing.Size(200, 25);
			// 
			// compareButton
			// 
			this.compareButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.compareButton.Image = ((System.Drawing.Image)(resources.GetObject("compareButton.Image")));
			this.compareButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.compareButton.Name = "compareButton";
			this.compareButton.Size = new System.Drawing.Size(48, 22);
			this.compareButton.Text = "比较";
			this.compareButton.ButtonClick += new System.EventHandler(this.CompareButtonButtonClick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.textBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBox2);
			this.splitContainer1.Size = new System.Drawing.Size(483, 192);
			this.splitContainer1.SplitterDistance = 227;
			this.splitContainer1.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.AllowDrop = true;
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.MaxLength = 3276700;
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(227, 192);
			this.textBox1.TabIndex = 0;
			this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextBox1DragDrop);
			this.textBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.TextBox1DragOver);
			// 
			// textBox2
			// 
			this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox2.Location = new System.Drawing.Point(0, 0);
			this.textBox2.MaxLength = 3276700;
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox2.Size = new System.Drawing.Size(252, 192);
			this.textBox2.TabIndex = 0;
			this.textBox2.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextBox2DragDrop);
			this.textBox2.DragOver += new System.Windows.Forms.DragEventHandler(this.TextBox2DragOver);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(483, 217);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "FileCompare";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
