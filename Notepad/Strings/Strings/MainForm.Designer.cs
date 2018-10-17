/*
 * Created by SharpDevelop.
 * User: psycho
 * Date: 2018/10/16
 * Time: 9:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Strings
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton toGbkButton;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ToolStripButton tobyteButton;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem gBKBYTEToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 计算表达式ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hEXINTToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripSplitButton formattimeButton;
		private System.Windows.Forms.ToolStripMenuItem 微秒到分钟ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 粘贴ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem iNTHEXToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toGbkButton = new System.Windows.Forms.ToolStripButton();
			this.tobyteButton = new System.Windows.Forms.ToolStripButton();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.gBKBYTEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hEXINTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iNTHEXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.计算表达式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.formattimeButton = new System.Windows.Forms.ToolStripSplitButton();
			this.微秒到分钟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 162);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(656, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripLabel1,
			this.toGbkButton,
			this.tobyteButton,
			this.toolStripSeparator3,
			this.toolStripSplitButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(656, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(68, 22);
			this.toolStripLabel1.Text = "（剪切板）";
			// 
			// toGbkButton
			// 
			this.toGbkButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toGbkButton.Image = ((System.Drawing.Image)(resources.GetObject("toGbkButton.Image")));
			this.toGbkButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toGbkButton.Name = "toGbkButton";
			this.toGbkButton.Size = new System.Drawing.Size(86, 22);
			this.toGbkButton.Text = "BYTE ➤ GBK";
			this.toGbkButton.Click += new System.EventHandler(this.ToGbkButtonClick);
			// 
			// tobyteButton
			// 
			this.tobyteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tobyteButton.Image = ((System.Drawing.Image)(resources.GetObject("tobyteButton.Image")));
			this.tobyteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tobyteButton.Name = "tobyteButton";
			this.tobyteButton.Size = new System.Drawing.Size(90, 22);
			this.tobyteButton.Text = "GBK  ➤ BYTE";
			this.tobyteButton.Click += new System.EventHandler(this.TobyteButtonClick);
			// 
			// textBox1
			// 
			this.textBox1.ContextMenuStrip = this.contextMenuStrip1;
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 50);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(656, 112);
			this.textBox1.TabIndex = 2;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.gBKBYTEToolStripMenuItem,
			this.hEXINTToolStripMenuItem,
			this.iNTHEXToolStripMenuItem,
			this.toolStripSeparator2,
			this.计算表达式ToolStripMenuItem,
			this.toolStripSeparator1,
			this.全选ToolStripMenuItem,
			this.粘贴ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(151, 148);
			// 
			// gBKBYTEToolStripMenuItem
			// 
			this.gBKBYTEToolStripMenuItem.Name = "gBKBYTEToolStripMenuItem";
			this.gBKBYTEToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.gBKBYTEToolStripMenuItem.Text = "GBK ➤ BYTE";
			this.gBKBYTEToolStripMenuItem.Click += new System.EventHandler(this.GBKBYTEToolStripMenuItemClick);
			// 
			// hEXINTToolStripMenuItem
			// 
			this.hEXINTToolStripMenuItem.Name = "hEXINTToolStripMenuItem";
			this.hEXINTToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.hEXINTToolStripMenuItem.Text = "HEX ➤ INT";
			this.hEXINTToolStripMenuItem.Click += new System.EventHandler(this.HEXINTToolStripMenuItemClick);
			// 
			// iNTHEXToolStripMenuItem
			// 
			this.iNTHEXToolStripMenuItem.Name = "iNTHEXToolStripMenuItem";
			this.iNTHEXToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.iNTHEXToolStripMenuItem.Text = "INT ➤ HEX";
			this.iNTHEXToolStripMenuItem.Click += new System.EventHandler(this.INTHEXToolStripMenuItemClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(147, 6);
			// 
			// 计算表达式ToolStripMenuItem
			// 
			this.计算表达式ToolStripMenuItem.Name = "计算表达式ToolStripMenuItem";
			this.计算表达式ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.计算表达式ToolStripMenuItem.Text = "计算表达式";
			this.计算表达式ToolStripMenuItem.Click += new System.EventHandler(this.计算表达式ToolStripMenuItemClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(147, 6);
			// 
			// 全选ToolStripMenuItem
			// 
			this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
			this.全选ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.全选ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.全选ToolStripMenuItem.Text = "全选";
			this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItemClick);
			// 
			// 粘贴ToolStripMenuItem
			// 
			this.粘贴ToolStripMenuItem.Name = "粘贴ToolStripMenuItem";
			this.粘贴ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.粘贴ToolStripMenuItem.Text = "粘贴";
			this.粘贴ToolStripMenuItem.Click += new System.EventHandler(this.粘贴ToolStripMenuItemClick);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.formattimeButton});
			this.toolStrip2.Location = new System.Drawing.Point(0, 25);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(656, 25);
			this.toolStrip2.TabIndex = 3;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// formattimeButton
			// 
			this.formattimeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.formattimeButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.微秒到分钟ToolStripMenuItem});
			this.formattimeButton.Image = ((System.Drawing.Image)(resources.GetObject("formattimeButton.Image")));
			this.formattimeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.formattimeButton.Name = "formattimeButton";
			this.formattimeButton.Size = new System.Drawing.Size(84, 22);
			this.formattimeButton.Text = "格式化时间";
			// 
			// 微秒到分钟ToolStripMenuItem
			// 
			this.微秒到分钟ToolStripMenuItem.Name = "微秒到分钟ToolStripMenuItem";
			this.微秒到分钟ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.微秒到分钟ToolStripMenuItem.Text = "格式化微秒";
			this.微秒到分钟ToolStripMenuItem.Click += new System.EventHandler(this.微秒到分钟ToolStripMenuItemClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
			this.toolStripSplitButton1.Text = "toolStripSplitButton1";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(656, 184);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "Strings";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
