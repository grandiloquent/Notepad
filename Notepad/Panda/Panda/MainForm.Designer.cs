/*
 * Created by SharpDevelop.
 * User: psycho
 * Date: 2018/9/30
 * Time: 21:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Panda
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripSplitButton qqxxzButton;
		private System.Windows.Forms.ToolStripMenuItem 其他ToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ToolStripSplitButton qqzhxButton;
		private System.Windows.Forms.ToolStripMenuItem 自动挂机ToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox vkButton;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		
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
			this.qqxxzButton = new System.Windows.Forms.ToolStripSplitButton();
			this.其他ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.qqzhxButton = new System.Windows.Forms.ToolStripSplitButton();
			this.自动挂机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vkButton = new System.Windows.Forms.ToolStripTextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.qqxxzButton,
			this.qqzhxButton,
			this.vkButton,
			this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(373, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// qqxxzButton
			// 
			this.qqxxzButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.qqxxzButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.其他ToolStripMenuItem});
			this.qqxxzButton.Image = ((System.Drawing.Image)(resources.GetObject("qqxxzButton.Image")));
			this.qqxxzButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.qqxxzButton.Name = "qqxxzButton";
			this.qqxxzButton.Size = new System.Drawing.Size(32, 22);
			this.qqxxzButton.Text = "toolStripSplitButton1";
			this.qqxxzButton.ButtonClick += new System.EventHandler(this.QqxxzButtonButtonClick);
			// 
			// 其他ToolStripMenuItem
			// 
			this.其他ToolStripMenuItem.Name = "其他ToolStripMenuItem";
			this.其他ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.其他ToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.其他ToolStripMenuItem.Text = "其他";
			this.其他ToolStripMenuItem.Click += new System.EventHandler(this.其他ToolStripMenuItemClick);
			// 
			// qqzhxButton
			// 
			this.qqzhxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.qqzhxButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.自动挂机ToolStripMenuItem});
			this.qqzhxButton.Image = ((System.Drawing.Image)(resources.GetObject("qqzhxButton.Image")));
			this.qqzhxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.qqzhxButton.Name = "qqzhxButton";
			this.qqzhxButton.Size = new System.Drawing.Size(48, 22);
			this.qqzhxButton.Text = "幻想";
			this.qqzhxButton.ButtonClick += new System.EventHandler(this.QqzhxButtonButtonClick);
			// 
			// 自动挂机ToolStripMenuItem
			// 
			this.自动挂机ToolStripMenuItem.Name = "自动挂机ToolStripMenuItem";
			this.自动挂机ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.自动挂机ToolStripMenuItem.Text = "自动挂机";
			this.自动挂机ToolStripMenuItem.Click += new System.EventHandler(this.自动挂机ToolStripMenuItemClick);
			// 
			// vkButton
			// 
			this.vkButton.AcceptsTab = true;
			this.vkButton.Name = "vkButton";
			this.vkButton.Size = new System.Drawing.Size(100, 25);
			this.vkButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.VkButtonKeyPress);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 25);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(373, 133);
			this.textBox1.TabIndex = 1;
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.Click += new System.EventHandler(this.ToolStripButton1Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(373, 158);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "Panda";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
