/*
 * Created by SharpDevelop.
 * User: psycho
 * Date: 2018/11/10
 * Time: 16:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FFO
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel handleLabel1;
		private System.Windows.Forms.ToolStripTextBox handleBox1;
		private System.Windows.Forms.ToolStripLabel thresholdLabel1;
		private System.Windows.Forms.ToolStripComboBox thresholdBox1;
		private System.Windows.Forms.ToolStripSplitButton startButton1;
		private System.Windows.Forms.ToolStripComboBox memoryBox1;
		private System.Windows.Forms.ToolStripMenuItem 扫描血量内存地址ToolStripMenuItem;
		private System.Windows.Forms.ToolStripLabel memoryLabel1;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox handleBox2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripComboBox thresholdBox2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel3;
		private System.Windows.Forms.ToolStripComboBox memoryBox2;
		private System.Windows.Forms.ToolStripSplitButton startButton2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 发送文本消息ToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox chatBox1;
		private System.Windows.Forms.ToolStrip toolStrip3;
		private System.Windows.Forms.ToolStripButton destroyClawsButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 勾魂利爪ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 注册药师技能热键ToolStripMenuItem;
		private System.Windows.Forms.ToolStripLabel messageLabel1;
		private System.Windows.Forms.ToolStripSplitButton keyButton1;
		private System.Windows.Forms.ToolStripMenuItem f8ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem f8Item2;
		private System.Windows.Forms.ToolStripMenuItem f2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 发送文本消息ToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem f3F5F4ToolStripMenuItem;
		
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
			this.handleLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.handleBox1 = new System.Windows.Forms.ToolStripTextBox();
			this.thresholdLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.thresholdBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.memoryLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.memoryBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.messageLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.chatBox1 = new System.Windows.Forms.ToolStripTextBox();
			this.startButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.扫描血量内存地址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.发送文本消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.勾魂利爪ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.注册药师技能热键ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.keyButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.f8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.handleBox2 = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.thresholdBox2 = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.memoryBox2 = new System.Windows.Forms.ToolStripComboBox();
			this.startButton2 = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.发送文本消息ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.f2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.f8Item2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip3 = new System.Windows.Forms.ToolStrip();
			this.destroyClawsButton = new System.Windows.Forms.ToolStripButton();
			this.f3F5F4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.handleLabel1,
			this.handleBox1,
			this.thresholdLabel1,
			this.thresholdBox1,
			this.memoryLabel1,
			this.memoryBox1,
			this.messageLabel1,
			this.chatBox1,
			this.startButton1,
			this.keyButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(767, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// handleLabel1
			// 
			this.handleLabel1.Name = "handleLabel1";
			this.handleLabel1.Size = new System.Drawing.Size(32, 22);
			this.handleLabel1.Text = "句柄";
			// 
			// handleBox1
			// 
			this.handleBox1.Name = "handleBox1";
			this.handleBox1.Size = new System.Drawing.Size(100, 25);
			// 
			// thresholdLabel1
			// 
			this.thresholdLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.thresholdLabel1.Name = "thresholdLabel1";
			this.thresholdLabel1.Size = new System.Drawing.Size(32, 22);
			this.thresholdLabel1.Text = "阙值";
			// 
			// thresholdBox1
			// 
			this.thresholdBox1.Name = "thresholdBox1";
			this.thresholdBox1.Size = new System.Drawing.Size(121, 25);
			// 
			// memoryLabel1
			// 
			this.memoryLabel1.Name = "memoryLabel1";
			this.memoryLabel1.Size = new System.Drawing.Size(56, 22);
			this.memoryLabel1.Text = "内存地址";
			// 
			// memoryBox1
			// 
			this.memoryBox1.Name = "memoryBox1";
			this.memoryBox1.Size = new System.Drawing.Size(121, 25);
			// 
			// messageLabel1
			// 
			this.messageLabel1.Name = "messageLabel1";
			this.messageLabel1.Size = new System.Drawing.Size(32, 22);
			this.messageLabel1.Text = "消失";
			// 
			// chatBox1
			// 
			this.chatBox1.Name = "chatBox1";
			this.chatBox1.Size = new System.Drawing.Size(100, 25);
			// 
			// startButton1
			// 
			this.startButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.startButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.扫描血量内存地址ToolStripMenuItem,
			this.发送文本消息ToolStripMenuItem,
			this.toolStripSeparator1,
			this.勾魂利爪ToolStripMenuItem,
			this.注册药师技能热键ToolStripMenuItem});
			this.startButton1.Image = ((System.Drawing.Image)(resources.GetObject("startButton1.Image")));
			this.startButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.startButton1.Name = "startButton1";
			this.startButton1.Size = new System.Drawing.Size(48, 22);
			this.startButton1.Text = "启动";
			this.startButton1.ButtonClick += new System.EventHandler(this.StartButton1ButtonClick);
			// 
			// 扫描血量内存地址ToolStripMenuItem
			// 
			this.扫描血量内存地址ToolStripMenuItem.Name = "扫描血量内存地址ToolStripMenuItem";
			this.扫描血量内存地址ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.扫描血量内存地址ToolStripMenuItem.Text = "扫描血量内存地址";
			this.扫描血量内存地址ToolStripMenuItem.Click += new System.EventHandler(this.扫描血量内存地址ToolStripMenuItemClick);
			// 
			// 发送文本消息ToolStripMenuItem
			// 
			this.发送文本消息ToolStripMenuItem.Name = "发送文本消息ToolStripMenuItem";
			this.发送文本消息ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.发送文本消息ToolStripMenuItem.Text = "发送文本消息";
			this.发送文本消息ToolStripMenuItem.Click += new System.EventHandler(this.发送文本消息ToolStripMenuItemClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
			// 
			// 勾魂利爪ToolStripMenuItem
			// 
			this.勾魂利爪ToolStripMenuItem.Name = "勾魂利爪ToolStripMenuItem";
			this.勾魂利爪ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.勾魂利爪ToolStripMenuItem.Text = "勾魂利爪";
			this.勾魂利爪ToolStripMenuItem.Click += new System.EventHandler(this.勾魂利爪ToolStripMenuItemClick);
			// 
			// 注册药师技能热键ToolStripMenuItem
			// 
			this.注册药师技能热键ToolStripMenuItem.Name = "注册药师技能热键ToolStripMenuItem";
			this.注册药师技能热键ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.注册药师技能热键ToolStripMenuItem.Text = "注册药师技能热键";
			this.注册药师技能热键ToolStripMenuItem.Click += new System.EventHandler(this.注册药师技能热键ToolStripMenuItemClick);
			// 
			// keyButton1
			// 
			this.keyButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.keyButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.f8ToolStripMenuItem});
			this.keyButton1.Image = ((System.Drawing.Image)(resources.GetObject("keyButton1.Image")));
			this.keyButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.keyButton1.Name = "keyButton1";
			this.keyButton1.Size = new System.Drawing.Size(48, 22);
			this.keyButton1.Text = "按键";
			// 
			// f8ToolStripMenuItem
			// 
			this.f8ToolStripMenuItem.Name = "f8ToolStripMenuItem";
			this.f8ToolStripMenuItem.Size = new System.Drawing.Size(89, 22);
			this.f8ToolStripMenuItem.Text = "F8";
			this.f8ToolStripMenuItem.Click += new System.EventHandler(this.F8ToolStripMenuItemClick);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripLabel1,
			this.handleBox2,
			this.toolStripLabel2,
			this.thresholdBox2,
			this.toolStripLabel3,
			this.memoryBox2,
			this.startButton2,
			this.toolStripSplitButton1});
			this.toolStrip2.Location = new System.Drawing.Point(0, 25);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(767, 25);
			this.toolStrip2.TabIndex = 1;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
			this.toolStripLabel1.Text = "句柄";
			// 
			// handleBox2
			// 
			this.handleBox2.Name = "handleBox2";
			this.handleBox2.Size = new System.Drawing.Size(100, 25);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(32, 22);
			this.toolStripLabel2.Text = "阙值";
			// 
			// thresholdBox2
			// 
			this.thresholdBox2.Name = "thresholdBox2";
			this.thresholdBox2.Size = new System.Drawing.Size(121, 25);
			// 
			// toolStripLabel3
			// 
			this.toolStripLabel3.Name = "toolStripLabel3";
			this.toolStripLabel3.Size = new System.Drawing.Size(56, 22);
			this.toolStripLabel3.Text = "内存地址";
			// 
			// memoryBox2
			// 
			this.memoryBox2.Name = "memoryBox2";
			this.memoryBox2.Size = new System.Drawing.Size(121, 25);
			// 
			// startButton2
			// 
			this.startButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.startButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripMenuItem1,
			this.发送文本消息ToolStripMenuItem1});
			this.startButton2.Image = ((System.Drawing.Image)(resources.GetObject("startButton2.Image")));
			this.startButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.startButton2.Name = "startButton2";
			this.startButton2.Size = new System.Drawing.Size(48, 22);
			this.startButton2.Text = "启动";
			this.startButton2.ButtonClick += new System.EventHandler(this.StartButton2ButtonClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuItem1.Text = "扫描血量内存地址";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1Click);
			// 
			// 发送文本消息ToolStripMenuItem1
			// 
			this.发送文本消息ToolStripMenuItem1.Name = "发送文本消息ToolStripMenuItem1";
			this.发送文本消息ToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.发送文本消息ToolStripMenuItem1.Text = "发送文本消息";
			this.发送文本消息ToolStripMenuItem1.Click += new System.EventHandler(this.发送文本消息ToolStripMenuItem1Click);
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.f2ToolStripMenuItem,
			this.f8Item2,
			this.f3F5F4ToolStripMenuItem});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(48, 22);
			this.toolStripSplitButton1.Text = "按键";
			// 
			// f2ToolStripMenuItem
			// 
			this.f2ToolStripMenuItem.Name = "f2ToolStripMenuItem";
			this.f2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.f2ToolStripMenuItem.Text = "F2";
			this.f2ToolStripMenuItem.Click += new System.EventHandler(this.F8Item2Click);
			// 
			// f8Item2
			// 
			this.f8Item2.Name = "f8Item2";
			this.f8Item2.Size = new System.Drawing.Size(152, 22);
			this.f8Item2.Text = "F8";
			this.f8Item2.Click += new System.EventHandler(this.F8Item2Click);
			// 
			// toolStrip3
			// 
			this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.destroyClawsButton});
			this.toolStrip3.Location = new System.Drawing.Point(0, 50);
			this.toolStrip3.Name = "toolStrip3";
			this.toolStrip3.Size = new System.Drawing.Size(767, 25);
			this.toolStrip3.TabIndex = 2;
			this.toolStrip3.Text = "toolStrip3";
			// 
			// destroyClawsButton
			// 
			this.destroyClawsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.destroyClawsButton.Image = ((System.Drawing.Image)(resources.GetObject("destroyClawsButton.Image")));
			this.destroyClawsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.destroyClawsButton.Name = "destroyClawsButton";
			this.destroyClawsButton.Size = new System.Drawing.Size(23, 22);
			this.destroyClawsButton.Text = "toolStripButton1";
			this.destroyClawsButton.Click += new System.EventHandler(this.DestroyClawsButtonClick);
			// 
			// f3F5F4ToolStripMenuItem
			// 
			this.f3F5F4ToolStripMenuItem.Name = "f3F5F4ToolStripMenuItem";
			this.f3F5F4ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.f3F5F4ToolStripMenuItem.Text = "F3+F5+F4";
			this.f3F5F4ToolStripMenuItem.Click += new System.EventHandler(this.F3F5F4ToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(767, 78);
			this.Controls.Add(this.toolStrip3);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "FFO";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
