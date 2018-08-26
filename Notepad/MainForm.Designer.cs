﻿/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2018/8/25
 * Time: 18:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Notepad
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStrip toolStrip3;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ComboBox comboBox;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem 逃逸路径ToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 剪切ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton h1Button;
		private System.Windows.Forms.ToolStripButton h2Button;
		private System.Windows.Forms.ToolStripButton formatButton;
		private System.Windows.Forms.ToolStripButton htmlsButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton iButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton titleButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSplitButton appButton;
		private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 粘贴ToolStripMenuItem;
		private System.Windows.Forms.ToolStripComboBox findBox;
		private System.Windows.Forms.ToolStripComboBox replaceBox;
		private System.Windows.Forms.ToolStripSplitButton findButton;
		private System.Windows.Forms.ToolStripButton newButton;
		private System.Windows.Forms.ToolStripButton chineseButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton englishButton;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
		private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 查找ToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton codeButton;
		private System.Windows.Forms.ToolStripMenuItem 排序ToolStripMenuItem;
		
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.appButton = new System.Windows.Forms.ToolStripSplitButton();
			this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newButton = new System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.findBox = new System.Windows.Forms.ToolStripComboBox();
			this.replaceBox = new System.Windows.Forms.ToolStripComboBox();
			this.findButton = new System.Windows.Forms.ToolStripSplitButton();
			this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip3 = new System.Windows.Forms.ToolStrip();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.逃逸路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.englishButton = new System.Windows.Forms.ToolStripButton();
			this.chineseButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.htmlsButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.formatButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.h1Button = new System.Windows.Forms.ToolStripButton();
			this.h2Button = new System.Windows.Forms.ToolStripButton();
			this.iButton = new System.Windows.Forms.ToolStripButton();
			this.titleButton = new System.Windows.Forms.ToolStripButton();
			this.codeButton = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listBox = new System.Windows.Forms.ListBox();
			this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.comboBox = new System.Windows.Forms.ComboBox();
			this.textBox = new System.Windows.Forms.TextBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.剪切ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.排序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.contextMenuStrip2.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.appButton,
			this.newButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1055, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// appButton
			// 
			this.appButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.appButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.保存ToolStripMenuItem});
			this.appButton.Image = ((System.Drawing.Image)(resources.GetObject("appButton.Image")));
			this.appButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.appButton.Name = "appButton";
			this.appButton.Size = new System.Drawing.Size(48, 22);
			this.appButton.Text = "程序";
			this.appButton.ButtonClick += new System.EventHandler(this.AppButtonButtonClick);
			// 
			// 保存ToolStripMenuItem
			// 
			this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
			this.保存ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.保存ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.保存ToolStripMenuItem.Text = "保存";
			this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItemClick);
			// 
			// newButton
			// 
			this.newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.newButton.Image = ((System.Drawing.Image)(resources.GetObject("newButton.Image")));
			this.newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newButton.Name = "newButton";
			this.newButton.Size = new System.Drawing.Size(36, 22);
			this.newButton.Text = "新建";
			this.newButton.Click += new System.EventHandler(this.NewButtonClick);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.findBox,
			this.replaceBox,
			this.findButton});
			this.toolStrip2.Location = new System.Drawing.Point(0, 25);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(1055, 25);
			this.toolStrip2.TabIndex = 1;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// findBox
			// 
			this.findBox.Name = "findBox";
			this.findBox.Size = new System.Drawing.Size(300, 25);
			// 
			// replaceBox
			// 
			this.replaceBox.Name = "replaceBox";
			this.replaceBox.Size = new System.Drawing.Size(300, 25);
			// 
			// findButton
			// 
			this.findButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.findButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.查找ToolStripMenuItem});
			this.findButton.Image = ((System.Drawing.Image)(resources.GetObject("findButton.Image")));
			this.findButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.findButton.Name = "findButton";
			this.findButton.Size = new System.Drawing.Size(48, 22);
			this.findButton.Text = "查找";
			// 
			// 查找ToolStripMenuItem
			// 
			this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
			this.查找ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.查找ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.查找ToolStripMenuItem.Text = "查找";
			this.查找ToolStripMenuItem.Click += new System.EventHandler(this.查找ToolStripMenuItemClick);
			// 
			// toolStrip3
			// 
			this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripSplitButton1,
			this.toolStripSeparator1,
			this.englishButton,
			this.chineseButton,
			this.toolStripSeparator5,
			this.htmlsButton,
			this.toolStripSeparator4,
			this.formatButton,
			this.toolStripSeparator2,
			this.h1Button,
			this.h2Button,
			this.iButton,
			this.titleButton,
			this.codeButton});
			this.toolStrip3.Location = new System.Drawing.Point(0, 50);
			this.toolStrip3.Name = "toolStrip3";
			this.toolStrip3.Size = new System.Drawing.Size(1055, 25);
			this.toolStrip3.TabIndex = 2;
			this.toolStrip3.Text = "toolStrip3";
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.逃逸路径ToolStripMenuItem});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(60, 22);
			this.toolStripSplitButton1.Text = "字符串";
			// 
			// 逃逸路径ToolStripMenuItem
			// 
			this.逃逸路径ToolStripMenuItem.Name = "逃逸路径ToolStripMenuItem";
			this.逃逸路径ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.逃逸路径ToolStripMenuItem.Text = "逃逸路径";
			this.逃逸路径ToolStripMenuItem.Click += new System.EventHandler(this.逃逸路径ToolStripMenuItemClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// englishButton
			// 
			this.englishButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.englishButton.Image = ((System.Drawing.Image)(resources.GetObject("englishButton.Image")));
			this.englishButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.englishButton.Name = "englishButton";
			this.englishButton.Size = new System.Drawing.Size(36, 22);
			this.englishButton.Text = "英文";
			this.englishButton.Click += new System.EventHandler(this.EnglishButtonClick);
			// 
			// chineseButton
			// 
			this.chineseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.chineseButton.Image = ((System.Drawing.Image)(resources.GetObject("chineseButton.Image")));
			this.chineseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.chineseButton.Name = "chineseButton";
			this.chineseButton.Size = new System.Drawing.Size(36, 22);
			this.chineseButton.Text = "中文";
			this.chineseButton.Click += new System.EventHandler(this.ChineseButtonClick);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// htmlsButton
			// 
			this.htmlsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.htmlsButton.Image = ((System.Drawing.Image)(resources.GetObject("htmlsButton.Image")));
			this.htmlsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.htmlsButton.Name = "htmlsButton";
			this.htmlsButton.Size = new System.Drawing.Size(53, 22);
			this.htmlsButton.Text = "HTMLS";
			this.htmlsButton.Click += new System.EventHandler(this.HtmlsButtonClick);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// formatButton
			// 
			this.formatButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.formatButton.Image = ((System.Drawing.Image)(resources.GetObject("formatButton.Image")));
			this.formatButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.formatButton.Name = "formatButton";
			this.formatButton.Size = new System.Drawing.Size(48, 22);
			this.formatButton.Text = "格式化";
			this.formatButton.Click += new System.EventHandler(this.FormatButtonClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// h1Button
			// 
			this.h1Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.h1Button.Image = ((System.Drawing.Image)(resources.GetObject("h1Button.Image")));
			this.h1Button.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.h1Button.Name = "h1Button";
			this.h1Button.Size = new System.Drawing.Size(28, 22);
			this.h1Button.Text = "H1";
			this.h1Button.Click += new System.EventHandler(this.H1ButtonClick);
			// 
			// h2Button
			// 
			this.h2Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.h2Button.Image = ((System.Drawing.Image)(resources.GetObject("h2Button.Image")));
			this.h2Button.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.h2Button.Name = "h2Button";
			this.h2Button.Size = new System.Drawing.Size(28, 22);
			this.h2Button.Text = "H2";
			this.h2Button.Click += new System.EventHandler(this.H2ButtonClick);
			// 
			// iButton
			// 
			this.iButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.iButton.Image = ((System.Drawing.Image)(resources.GetObject("iButton.Image")));
			this.iButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.iButton.Name = "iButton";
			this.iButton.Size = new System.Drawing.Size(23, 22);
			this.iButton.Text = "I";
			this.iButton.Click += new System.EventHandler(this.IButtonClick);
			// 
			// titleButton
			// 
			this.titleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.titleButton.Image = ((System.Drawing.Image)(resources.GetObject("titleButton.Image")));
			this.titleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.titleButton.Name = "titleButton";
			this.titleButton.Size = new System.Drawing.Size(36, 22);
			this.titleButton.Text = "标题";
			this.titleButton.Click += new System.EventHandler(this.TitleButtonClick);
			// 
			// codeButton
			// 
			this.codeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.codeButton.Image = ((System.Drawing.Image)(resources.GetObject("codeButton.Image")));
			this.codeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.codeButton.Name = "codeButton";
			this.codeButton.Size = new System.Drawing.Size(46, 22);
			this.codeButton.Text = "CODE";
			this.codeButton.Click += new System.EventHandler(this.CodeButtonClick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 75);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listBox);
			this.splitContainer1.Panel1.Controls.Add(this.comboBox);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBox);
			this.splitContainer1.Size = new System.Drawing.Size(1055, 548);
			this.splitContainer1.SplitterDistance = 289;
			this.splitContainer1.TabIndex = 3;
			// 
			// listBox
			// 
			this.listBox.ContextMenuStrip = this.contextMenuStrip2;
			this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.listBox.FormattingEnabled = true;
			this.listBox.ItemHeight = 17;
			this.listBox.Location = new System.Drawing.Point(0, 20);
			this.listBox.Name = "listBox";
			this.listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBox.Size = new System.Drawing.Size(289, 528);
			this.listBox.TabIndex = 1;
			this.listBox.DoubleClick += new System.EventHandler(this.ListBoxDoubleClick);
			// 
			// contextMenuStrip2
			// 
			this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.删除ToolStripMenuItem});
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			this.contextMenuStrip2.Size = new System.Drawing.Size(101, 26);
			// 
			// 删除ToolStripMenuItem
			// 
			this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
			this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.删除ToolStripMenuItem.Text = "删除";
			this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItemClick);
			// 
			// comboBox
			// 
			this.comboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox.FormattingEnabled = true;
			this.comboBox.Location = new System.Drawing.Point(0, 0);
			this.comboBox.Name = "comboBox";
			this.comboBox.Size = new System.Drawing.Size(289, 20);
			this.comboBox.TabIndex = 0;
			this.comboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
			// 
			// textBox
			// 
			this.textBox.ContextMenuStrip = this.contextMenuStrip1;
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.MaxLength = 32767000;
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox.Size = new System.Drawing.Size(762, 548);
			this.textBox.TabIndex = 0;
			this.textBox.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.复制ToolStripMenuItem,
			this.剪切ToolStripMenuItem,
			this.粘贴ToolStripMenuItem,
			this.toolStripSeparator3,
			this.全选ToolStripMenuItem,
			this.排序ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(153, 142);
			// 
			// 复制ToolStripMenuItem
			// 
			this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
			this.复制ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.复制ToolStripMenuItem.Text = "复制";
			this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItemClick);
			// 
			// 剪切ToolStripMenuItem
			// 
			this.剪切ToolStripMenuItem.Name = "剪切ToolStripMenuItem";
			this.剪切ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.剪切ToolStripMenuItem.Text = "剪切";
			this.剪切ToolStripMenuItem.Click += new System.EventHandler(this.剪切ToolStripMenuItemClick);
			// 
			// 粘贴ToolStripMenuItem
			// 
			this.粘贴ToolStripMenuItem.Name = "粘贴ToolStripMenuItem";
			this.粘贴ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.粘贴ToolStripMenuItem.Text = "粘贴";
			this.粘贴ToolStripMenuItem.Click += new System.EventHandler(this.粘贴ToolStripMenuItemClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
			// 
			// 全选ToolStripMenuItem
			// 
			this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
			this.全选ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.全选ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.全选ToolStripMenuItem.Text = "全选";
			this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItemClick);
			// 
			// 排序ToolStripMenuItem
			// 
			this.排序ToolStripMenuItem.Name = "排序ToolStripMenuItem";
			this.排序ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.排序ToolStripMenuItem.Text = "排序";
			this.排序ToolStripMenuItem.Click += new System.EventHandler(this.排序ToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1055, 623);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip3);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "Notepad";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.contextMenuStrip2.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
