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
		private System.Windows.Forms.ToolStripSplitButton arrayTemplateButton;
		private System.Windows.Forms.ToolStripMenuItem 数组逻辑比较ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem memoryViewer到BYTE数组ToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip3;
		private System.Windows.Forms.ToolStripButton cformatButton;
		private System.Windows.Forms.ToolStripButton bytetoint32Button;
		private System.Windows.Forms.ToolStripButton intotbyteButton;
		private System.Windows.Forms.ToolStripMenuItem bYTEINTToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem iNTBYTEToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStrip toolStrip4;
		private System.Windows.Forms.ToolStripComboBox fileNameBox1;
		private System.Windows.Forms.ToolStripComboBox fileNameBox2;
		private System.Windows.Forms.ToolStripSplitButton compareFileButton;
		private System.Windows.Forms.ToolStripMenuItem 倒序字符串ToolStripMenuItem;
		
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
			this.bytetoint32Button = new System.Windows.Forms.ToolStripButton();
			this.intotbyteButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.arrayTemplateButton = new System.Windows.Forms.ToolStripSplitButton();
			this.数组逻辑比较ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.memoryViewer到BYTE数组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.gBKBYTEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hEXINTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iNTHEXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bYTEINTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iNTBYTEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.计算表达式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.formattimeButton = new System.Windows.Forms.ToolStripSplitButton();
			this.微秒到分钟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip3 = new System.Windows.Forms.ToolStrip();
			this.cformatButton = new System.Windows.Forms.ToolStripButton();
			this.toolStrip4 = new System.Windows.Forms.ToolStrip();
			this.fileNameBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.fileNameBox2 = new System.Windows.Forms.ToolStripComboBox();
			this.compareFileButton = new System.Windows.Forms.ToolStripSplitButton();
			this.倒序字符串ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			this.toolStrip4.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 234);
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
			this.bytetoint32Button,
			this.intotbyteButton,
			this.toolStripSeparator3,
			this.arrayTemplateButton,
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
			// bytetoint32Button
			// 
			this.bytetoint32Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.bytetoint32Button.Image = ((System.Drawing.Image)(resources.GetObject("bytetoint32Button.Image")));
			this.bytetoint32Button.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.bytetoint32Button.Name = "bytetoint32Button";
			this.bytetoint32Button.Size = new System.Drawing.Size(82, 22);
			this.bytetoint32Button.Text = "BYTE ➤ INT";
			this.bytetoint32Button.Click += new System.EventHandler(this.Bytetoint32ButtonClick);
			// 
			// intotbyteButton
			// 
			this.intotbyteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.intotbyteButton.Image = ((System.Drawing.Image)(resources.GetObject("intotbyteButton.Image")));
			this.intotbyteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.intotbyteButton.Name = "intotbyteButton";
			this.intotbyteButton.Size = new System.Drawing.Size(86, 22);
			this.intotbyteButton.Text = "INT ➤ BYTE ";
			this.intotbyteButton.Click += new System.EventHandler(this.IntotbyteButtonClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// arrayTemplateButton
			// 
			this.arrayTemplateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.arrayTemplateButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.数组逻辑比较ToolStripMenuItem,
			this.倒序字符串ToolStripMenuItem});
			this.arrayTemplateButton.Image = ((System.Drawing.Image)(resources.GetObject("arrayTemplateButton.Image")));
			this.arrayTemplateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.arrayTemplateButton.Name = "arrayTemplateButton";
			this.arrayTemplateButton.Size = new System.Drawing.Size(96, 22);
			this.arrayTemplateButton.Text = "数组（模板）";
			// 
			// 数组逻辑比较ToolStripMenuItem
			// 
			this.数组逻辑比较ToolStripMenuItem.Name = "数组逻辑比较ToolStripMenuItem";
			this.数组逻辑比较ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.数组逻辑比较ToolStripMenuItem.Text = "数组逻辑比较";
			this.数组逻辑比较ToolStripMenuItem.Click += new System.EventHandler(this.数组逻辑比较ToolStripMenuItemClick);
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.memoryViewer到BYTE数组ToolStripMenuItem});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
			this.toolStripSplitButton1.Text = "toolStripSplitButton1";
			// 
			// memoryViewer到BYTE数组ToolStripMenuItem
			// 
			this.memoryViewer到BYTE数组ToolStripMenuItem.Name = "memoryViewer到BYTE数组ToolStripMenuItem";
			this.memoryViewer到BYTE数组ToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
			this.memoryViewer到BYTE数组ToolStripMenuItem.Text = "Memory Viewer 到 BYTE 数组";
			this.memoryViewer到BYTE数组ToolStripMenuItem.Click += new System.EventHandler(this.MemoryViewer到BYTE数组ToolStripMenuItemClick);
			// 
			// textBox1
			// 
			this.textBox1.ContextMenuStrip = this.contextMenuStrip1;
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 100);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(656, 134);
			this.textBox1.TabIndex = 2;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.gBKBYTEToolStripMenuItem,
			this.hEXINTToolStripMenuItem,
			this.iNTHEXToolStripMenuItem,
			this.bYTEINTToolStripMenuItem,
			this.iNTBYTEToolStripMenuItem,
			this.toolStripSeparator2,
			this.计算表达式ToolStripMenuItem,
			this.toolStripSeparator1,
			this.全选ToolStripMenuItem,
			this.粘贴ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(151, 192);
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
			// bYTEINTToolStripMenuItem
			// 
			this.bYTEINTToolStripMenuItem.Name = "bYTEINTToolStripMenuItem";
			this.bYTEINTToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.bYTEINTToolStripMenuItem.Text = "BYTE ➤ INT";
			this.bYTEINTToolStripMenuItem.Click += new System.EventHandler(this.BYTEINTToolStripMenuItemClick);
			// 
			// iNTBYTEToolStripMenuItem
			// 
			this.iNTBYTEToolStripMenuItem.Name = "iNTBYTEToolStripMenuItem";
			this.iNTBYTEToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.iNTBYTEToolStripMenuItem.Text = "INT ➤ BYTE ";
			this.iNTBYTEToolStripMenuItem.Click += new System.EventHandler(this.INTBYTEToolStripMenuItemClick);
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
			this.toolStrip2.Location = new System.Drawing.Point(0, 50);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(656, 25);
			this.toolStrip2.TabIndex = 3;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// formattimeButton
			// 
			this.formattimeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.formattimeButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.微秒到分钟ToolStripMenuItem,
			this.toolStripMenuItem2});
			this.formattimeButton.Image = ((System.Drawing.Image)(resources.GetObject("formattimeButton.Image")));
			this.formattimeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.formattimeButton.Name = "formattimeButton";
			this.formattimeButton.Size = new System.Drawing.Size(84, 22);
			this.formattimeButton.Text = "格式化时间";
			// 
			// 微秒到分钟ToolStripMenuItem
			// 
			this.微秒到分钟ToolStripMenuItem.Name = "微秒到分钟ToolStripMenuItem";
			this.微秒到分钟ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.微秒到分钟ToolStripMenuItem.Text = "格式化微秒";
			this.微秒到分钟ToolStripMenuItem.Click += new System.EventHandler(this.微秒到分钟ToolStripMenuItemClick);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem2.Text = "123";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2Click);
			// 
			// toolStrip3
			// 
			this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.cformatButton});
			this.toolStrip3.Location = new System.Drawing.Point(0, 25);
			this.toolStrip3.Name = "toolStrip3";
			this.toolStrip3.Size = new System.Drawing.Size(656, 25);
			this.toolStrip3.TabIndex = 4;
			this.toolStrip3.Text = "toolStrip3";
			// 
			// cformatButton
			// 
			this.cformatButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.cformatButton.Image = ((System.Drawing.Image)(resources.GetObject("cformatButton.Image")));
			this.cformatButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cformatButton.Name = "cformatButton";
			this.cformatButton.Size = new System.Drawing.Size(80, 22);
			this.cformatButton.Text = "格式化C语言";
			this.cformatButton.Click += new System.EventHandler(this.CformatButtonClick);
			// 
			// toolStrip4
			// 
			this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.fileNameBox1,
			this.fileNameBox2,
			this.compareFileButton});
			this.toolStrip4.Location = new System.Drawing.Point(0, 75);
			this.toolStrip4.Name = "toolStrip4";
			this.toolStrip4.Size = new System.Drawing.Size(656, 25);
			this.toolStrip4.TabIndex = 5;
			this.toolStrip4.Text = "toolStrip4";
			// 
			// fileNameBox1
			// 
			this.fileNameBox1.Name = "fileNameBox1";
			this.fileNameBox1.Size = new System.Drawing.Size(121, 25);
			// 
			// fileNameBox2
			// 
			this.fileNameBox2.Name = "fileNameBox2";
			this.fileNameBox2.Size = new System.Drawing.Size(121, 25);
			// 
			// compareFileButton
			// 
			this.compareFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.compareFileButton.Image = ((System.Drawing.Image)(resources.GetObject("compareFileButton.Image")));
			this.compareFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.compareFileButton.Name = "compareFileButton";
			this.compareFileButton.Size = new System.Drawing.Size(96, 22);
			this.compareFileButton.Text = "比较文本文件";
			this.compareFileButton.ButtonClick += new System.EventHandler(this.CompareFileButtonButtonClick);
			// 
			// 倒序字符串ToolStripMenuItem
			// 
			this.倒序字符串ToolStripMenuItem.Name = "倒序字符串ToolStripMenuItem";
			this.倒序字符串ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.倒序字符串ToolStripMenuItem.Text = "倒序字符串";
			this.倒序字符串ToolStripMenuItem.Click += new System.EventHandler(this.倒序字符串ToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(656, 256);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.toolStrip4);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip3);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "Strings";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.toolStrip4.ResumeLayout(false);
			this.toolStrip4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
