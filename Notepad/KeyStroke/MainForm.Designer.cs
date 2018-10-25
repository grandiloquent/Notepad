/*
 * Created by SharpDevelop.
 * User: psycho
 * Date: 2018/10/2
 * Time: 2:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace KeyStroke
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripSplitButton kdButton;
		private System.Windows.Forms.ToolStripMenuItem 获取当前坐标值热键FToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton cButton;
		private System.Windows.Forms.ToolStripMenuItem 编译CToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton clearButton;
		private System.Windows.Forms.ToolStripMenuItem 格式化C代码ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 取色器ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton cmdButton;
		private System.Windows.Forms.ToolStripMenuItem aria2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 记录鼠标事件热键F7ToolStripMenuItem;
		private Gma.UserActivityMonitor.GlobalEventProvider globalEventProvider1;
		private System.Windows.Forms.ToolStripMenuItem vSC代码段热键F8ToolStripMenuItem;
		
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
			this.kdButton = new System.Windows.Forms.ToolStripSplitButton();
			this.获取当前坐标值热键FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.取色器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.记录鼠标事件热键F7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cButton = new System.Windows.Forms.ToolStripSplitButton();
			this.编译CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.格式化C代码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearButton = new System.Windows.Forms.ToolStripButton();
			this.cmdButton = new System.Windows.Forms.ToolStripSplitButton();
			this.aria2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.globalEventProvider1 = new Gma.UserActivityMonitor.GlobalEventProvider();
			this.vSC代码段热键F8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.kdButton,
			this.cButton,
			this.clearButton,
			this.cmdButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(406, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// kdButton
			// 
			this.kdButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.kdButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.获取当前坐标值热键FToolStripMenuItem,
			this.取色器ToolStripMenuItem,
			this.记录鼠标事件热键F7ToolStripMenuItem});
			this.kdButton.Image = ((System.Drawing.Image)(resources.GetObject("kdButton.Image")));
			this.kdButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.kdButton.Name = "kdButton";
			this.kdButton.Size = new System.Drawing.Size(48, 22);
			this.kdButton.Text = "键盘";
			// 
			// 获取当前坐标值热键FToolStripMenuItem
			// 
			this.获取当前坐标值热键FToolStripMenuItem.Name = "获取当前坐标值热键FToolStripMenuItem";
			this.获取当前坐标值热键FToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.获取当前坐标值热键FToolStripMenuItem.Text = "获取当前坐标值(热键F6)";
			this.获取当前坐标值热键FToolStripMenuItem.Click += new System.EventHandler(this.获取当前坐标值热键FToolStripMenuItemClick);
			// 
			// 取色器ToolStripMenuItem
			// 
			this.取色器ToolStripMenuItem.Name = "取色器ToolStripMenuItem";
			this.取色器ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.取色器ToolStripMenuItem.Text = "取色器";
			this.取色器ToolStripMenuItem.Click += new System.EventHandler(this.取色器ToolStripMenuItemClick);
			// 
			// 记录鼠标事件热键F7ToolStripMenuItem
			// 
			this.记录鼠标事件热键F7ToolStripMenuItem.Name = "记录鼠标事件热键F7ToolStripMenuItem";
			this.记录鼠标事件热键F7ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.记录鼠标事件热键F7ToolStripMenuItem.Text = "记录鼠标事件(热键F7)";
			this.记录鼠标事件热键F7ToolStripMenuItem.Click += new System.EventHandler(this.记录鼠标事件热键F7ToolStripMenuItemClick);
			// 
			// cButton
			// 
			this.cButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.cButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.编译CToolStripMenuItem,
			this.格式化C代码ToolStripMenuItem,
			this.vSC代码段热键F8ToolStripMenuItem});
			this.cButton.Image = ((System.Drawing.Image)(resources.GetObject("cButton.Image")));
			this.cButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cButton.Name = "cButton";
			this.cButton.Size = new System.Drawing.Size(63, 22);
			this.cButton.Text = "C/C++";
			// 
			// 编译CToolStripMenuItem
			// 
			this.编译CToolStripMenuItem.Name = "编译CToolStripMenuItem";
			this.编译CToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.编译CToolStripMenuItem.Text = "编译C(热键F9)";
			this.编译CToolStripMenuItem.Click += new System.EventHandler(this.编译CToolStripMenuItemClick);
			// 
			// 格式化C代码ToolStripMenuItem
			// 
			this.格式化C代码ToolStripMenuItem.Name = "格式化C代码ToolStripMenuItem";
			this.格式化C代码ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.格式化C代码ToolStripMenuItem.Text = "格式化C代码";
			this.格式化C代码ToolStripMenuItem.Click += new System.EventHandler(this.格式化C代码ToolStripMenuItemClick);
			// 
			// clearButton
			// 
			this.clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.clearButton.Image = ((System.Drawing.Image)(resources.GetObject("clearButton.Image")));
			this.clearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(23, 22);
			this.clearButton.Text = "toolStripButton1";
			this.clearButton.Click += new System.EventHandler(this.ClearButtonClick);
			// 
			// cmdButton
			// 
			this.cmdButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.aria2ToolStripMenuItem,
			this.toolStripMenuItem1});
			this.cmdButton.Image = ((System.Drawing.Image)(resources.GetObject("cmdButton.Image")));
			this.cmdButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdButton.Name = "cmdButton";
			this.cmdButton.Size = new System.Drawing.Size(32, 22);
			this.cmdButton.Text = "toolStripSplitButton2";
			// 
			// aria2ToolStripMenuItem
			// 
			this.aria2ToolStripMenuItem.Name = "aria2ToolStripMenuItem";
			this.aria2ToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
			this.aria2ToolStripMenuItem.Text = "Aria2";
			this.aria2ToolStripMenuItem.Click += new System.EventHandler(this.Aria2ToolStripMenuItemClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
			this.toolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
			this.toolStripMenuItem1.Text = "toolStripMenuItem1";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1Click);
			// 
			// vSC代码段热键F8ToolStripMenuItem
			// 
			this.vSC代码段热键F8ToolStripMenuItem.Name = "vSC代码段热键F8ToolStripMenuItem";
			this.vSC代码段热键F8ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.vSC代码段热键F8ToolStripMenuItem.Text = "VSC 代码段(热键F8)";
			this.vSC代码段热键F8ToolStripMenuItem.Click += new System.EventHandler(this.VSC代码段热键F8ToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(406, 262);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "KeyStroke";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
