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
		private System.Windows.Forms.ToolStripSplitButton othersButton;
		private System.Windows.Forms.ToolStripMenuItem 压缩目录不包含ZIP文件ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 压缩目录不包含ZIP文件ToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 重命名压缩文件ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 鼠标下窗口句柄ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem cPUToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cLangFormatToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 压缩AndroidToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 压缩子目录ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 解压目录中文件ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 删除Aria2文件ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 清理HTMLSToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton ebookButton;
		private System.Windows.Forms.ToolStripMenuItem wkhtmlToPdfToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 清空CodeBlocks项目ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem gBKToUTF8ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uTF8ToGBKToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 按键F8ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 监视按键ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem vSC代码段格式化ToolStripMenuItem;
		
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
			this.鼠标下窗口句柄ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.按键F8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.监视按键ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cButton = new System.Windows.Forms.ToolStripSplitButton();
			this.编译CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.格式化C代码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vSC代码段热键F8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cLangFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.清空CodeBlocks项目ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.clearButton = new System.Windows.Forms.ToolStripButton();
			this.othersButton = new System.Windows.Forms.ToolStripSplitButton();
			this.压缩目录不包含ZIP文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.压缩目录不包含ZIP文件ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.重命名压缩文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.压缩AndroidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.压缩子目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.解压目录中文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cmdButton = new System.Windows.Forms.ToolStripSplitButton();
			this.aria2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cPUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ebookButton = new System.Windows.Forms.ToolStripSplitButton();
			this.删除Aria2文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.清理HTMLSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wkhtmlToPdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.gBKToUTF8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uTF8ToGBKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.globalEventProvider1 = new Gma.UserActivityMonitor.GlobalEventProvider();
			this.vSC代码段格式化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.kdButton,
			this.cButton,
			this.clearButton,
			this.othersButton,
			this.cmdButton,
			this.ebookButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(314, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// kdButton
			// 
			this.kdButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.kdButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.获取当前坐标值热键FToolStripMenuItem,
			this.取色器ToolStripMenuItem,
			this.记录鼠标事件热键F7ToolStripMenuItem,
			this.鼠标下窗口句柄ToolStripMenuItem,
			this.按键F8ToolStripMenuItem,
			this.监视按键ToolStripMenuItem});
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
			// 鼠标下窗口句柄ToolStripMenuItem
			// 
			this.鼠标下窗口句柄ToolStripMenuItem.Name = "鼠标下窗口句柄ToolStripMenuItem";
			this.鼠标下窗口句柄ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.鼠标下窗口句柄ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.鼠标下窗口句柄ToolStripMenuItem.Text = "鼠标下窗口句柄";
			this.鼠标下窗口句柄ToolStripMenuItem.Click += new System.EventHandler(this.鼠标下窗口句柄ToolStripMenuItemClick);
			// 
			// 按键F8ToolStripMenuItem
			// 
			this.按键F8ToolStripMenuItem.Name = "按键F8ToolStripMenuItem";
			this.按键F8ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.按键F8ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.按键F8ToolStripMenuItem.Text = "按键F8";
			this.按键F8ToolStripMenuItem.Click += new System.EventHandler(this.按键F8ToolStripMenuItemClick);
			// 
			// 监视按键ToolStripMenuItem
			// 
			this.监视按键ToolStripMenuItem.Name = "监视按键ToolStripMenuItem";
			this.监视按键ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.监视按键ToolStripMenuItem.Text = "监视按键";
			this.监视按键ToolStripMenuItem.Click += new System.EventHandler(this.监视按键ToolStripMenuItemClick);
			// 
			// cButton
			// 
			this.cButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.cButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.编译CToolStripMenuItem,
			this.格式化C代码ToolStripMenuItem,
			this.vSC代码段热键F8ToolStripMenuItem,
			this.cLangFormatToolStripMenuItem,
			this.清空CodeBlocks项目ToolStripMenuItem,
			this.toolStripSeparator3,
			this.vSC代码段格式化ToolStripMenuItem});
			this.cButton.Image = ((System.Drawing.Image)(resources.GetObject("cButton.Image")));
			this.cButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cButton.Name = "cButton";
			this.cButton.Size = new System.Drawing.Size(63, 22);
			this.cButton.Text = "C/C++";
			// 
			// 编译CToolStripMenuItem
			// 
			this.编译CToolStripMenuItem.Name = "编译CToolStripMenuItem";
			this.编译CToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.编译CToolStripMenuItem.Text = "编译C(热键F9)";
			this.编译CToolStripMenuItem.Click += new System.EventHandler(this.编译CToolStripMenuItemClick);
			// 
			// 格式化C代码ToolStripMenuItem
			// 
			this.格式化C代码ToolStripMenuItem.Name = "格式化C代码ToolStripMenuItem";
			this.格式化C代码ToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.格式化C代码ToolStripMenuItem.Text = "格式化C代码";
			this.格式化C代码ToolStripMenuItem.Click += new System.EventHandler(this.格式化C代码ToolStripMenuItemClick);
			// 
			// vSC代码段热键F8ToolStripMenuItem
			// 
			this.vSC代码段热键F8ToolStripMenuItem.Name = "vSC代码段热键F8ToolStripMenuItem";
			this.vSC代码段热键F8ToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.vSC代码段热键F8ToolStripMenuItem.Text = "VSC 代码段(C/C++ MSDN 热键F8)";
			this.vSC代码段热键F8ToolStripMenuItem.Click += new System.EventHandler(this.VSC代码段热键F8ToolStripMenuItemClick);
			// 
			// cLangFormatToolStripMenuItem
			// 
			this.cLangFormatToolStripMenuItem.Name = "cLangFormatToolStripMenuItem";
			this.cLangFormatToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.cLangFormatToolStripMenuItem.Text = "CLang Format";
			this.cLangFormatToolStripMenuItem.Click += new System.EventHandler(this.CLangFormatToolStripMenuItemClick);
			// 
			// 清空CodeBlocks项目ToolStripMenuItem
			// 
			this.清空CodeBlocks项目ToolStripMenuItem.Name = "清空CodeBlocks项目ToolStripMenuItem";
			this.清空CodeBlocks项目ToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.清空CodeBlocks项目ToolStripMenuItem.Text = "清空 Code::Blocks 项目";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(266, 6);
			// 
			// clearButton
			// 
			this.clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.clearButton.Image = ((System.Drawing.Image)(resources.GetObject("clearButton.Image")));
			this.clearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(36, 22);
			this.clearButton.Text = "重设";
			this.clearButton.Click += new System.EventHandler(this.ClearButtonClick);
			// 
			// othersButton
			// 
			this.othersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.othersButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.压缩目录不包含ZIP文件ToolStripMenuItem,
			this.压缩目录不包含ZIP文件ToolStripMenuItem1,
			this.重命名压缩文件ToolStripMenuItem,
			this.压缩AndroidToolStripMenuItem,
			this.压缩子目录ToolStripMenuItem,
			this.解压目录中文件ToolStripMenuItem});
			this.othersButton.Image = ((System.Drawing.Image)(resources.GetObject("othersButton.Image")));
			this.othersButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.othersButton.Name = "othersButton";
			this.othersButton.Size = new System.Drawing.Size(48, 22);
			this.othersButton.Text = "压缩";
			// 
			// 压缩目录不包含ZIP文件ToolStripMenuItem
			// 
			this.压缩目录不包含ZIP文件ToolStripMenuItem.Name = "压缩目录不包含ZIP文件ToolStripMenuItem";
			this.压缩目录不包含ZIP文件ToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.压缩目录不包含ZIP文件ToolStripMenuItem.Text = "压缩目录下文件(不包含ZIP文件)";
			this.压缩目录不包含ZIP文件ToolStripMenuItem.Click += new System.EventHandler(this.压缩目录不包含ZIP文件ToolStripMenuItemClick);
			// 
			// 压缩目录不包含ZIP文件ToolStripMenuItem1
			// 
			this.压缩目录不包含ZIP文件ToolStripMenuItem1.Name = "压缩目录不包含ZIP文件ToolStripMenuItem1";
			this.压缩目录不包含ZIP文件ToolStripMenuItem1.Size = new System.Drawing.Size(246, 22);
			this.压缩目录不包含ZIP文件ToolStripMenuItem1.Text = "压缩目录(不包含ZIP文件)";
			this.压缩目录不包含ZIP文件ToolStripMenuItem1.Click += new System.EventHandler(this.压缩目录不包含ZIP文件ToolStripMenuItem1Click);
			// 
			// 重命名压缩文件ToolStripMenuItem
			// 
			this.重命名压缩文件ToolStripMenuItem.Name = "重命名压缩文件ToolStripMenuItem";
			this.重命名压缩文件ToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.重命名压缩文件ToolStripMenuItem.Text = "重命名压缩文件";
			this.重命名压缩文件ToolStripMenuItem.Click += new System.EventHandler(this.重命名压缩文件ToolStripMenuItemClick);
			// 
			// 压缩AndroidToolStripMenuItem
			// 
			this.压缩AndroidToolStripMenuItem.Name = "压缩AndroidToolStripMenuItem";
			this.压缩AndroidToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.压缩AndroidToolStripMenuItem.Text = "压缩Android";
			this.压缩AndroidToolStripMenuItem.Click += new System.EventHandler(this.压缩AndroidToolStripMenuItemClick);
			// 
			// 压缩子目录ToolStripMenuItem
			// 
			this.压缩子目录ToolStripMenuItem.Name = "压缩子目录ToolStripMenuItem";
			this.压缩子目录ToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.压缩子目录ToolStripMenuItem.Text = "压缩子目录";
			this.压缩子目录ToolStripMenuItem.Click += new System.EventHandler(this.压缩子目录ToolStripMenuItemClick);
			// 
			// 解压目录中文件ToolStripMenuItem
			// 
			this.解压目录中文件ToolStripMenuItem.Name = "解压目录中文件ToolStripMenuItem";
			this.解压目录中文件ToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.解压目录中文件ToolStripMenuItem.Text = "解压目录中文件";
			this.解压目录中文件ToolStripMenuItem.Click += new System.EventHandler(this.解压目录中文件ToolStripMenuItemClick);
			// 
			// cmdButton
			// 
			this.cmdButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.cmdButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.aria2ToolStripMenuItem,
			this.toolStripMenuItem1,
			this.toolStripSeparator1,
			this.cPUToolStripMenuItem});
			this.cmdButton.Image = ((System.Drawing.Image)(resources.GetObject("cmdButton.Image")));
			this.cmdButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdButton.Name = "cmdButton";
			this.cmdButton.Size = new System.Drawing.Size(48, 22);
			this.cmdButton.Text = "命令";
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
			// 
			// cPUToolStripMenuItem
			// 
			this.cPUToolStripMenuItem.Name = "cPUToolStripMenuItem";
			this.cPUToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
			this.cPUToolStripMenuItem.Text = "CPU";
			this.cPUToolStripMenuItem.Click += new System.EventHandler(this.CPUToolStripMenuItemClick);
			// 
			// ebookButton
			// 
			this.ebookButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.ebookButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.删除Aria2文件ToolStripMenuItem,
			this.清理HTMLSToolStripMenuItem,
			this.wkhtmlToPdfToolStripMenuItem,
			this.toolStripSeparator2,
			this.gBKToUTF8ToolStripMenuItem,
			this.uTF8ToGBKToolStripMenuItem});
			this.ebookButton.Image = ((System.Drawing.Image)(resources.GetObject("ebookButton.Image")));
			this.ebookButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ebookButton.Name = "ebookButton";
			this.ebookButton.Size = new System.Drawing.Size(48, 21);
			this.ebookButton.Text = "书籍";
			// 
			// 删除Aria2文件ToolStripMenuItem
			// 
			this.删除Aria2文件ToolStripMenuItem.Name = "删除Aria2文件ToolStripMenuItem";
			this.删除Aria2文件ToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.删除Aria2文件ToolStripMenuItem.Text = "删除Aria2文件";
			this.删除Aria2文件ToolStripMenuItem.Click += new System.EventHandler(this.删除Aria2文件ToolStripMenuItemClick);
			// 
			// 清理HTMLSToolStripMenuItem
			// 
			this.清理HTMLSToolStripMenuItem.Name = "清理HTMLSToolStripMenuItem";
			this.清理HTMLSToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.清理HTMLSToolStripMenuItem.Text = "清理HTMLS";
			this.清理HTMLSToolStripMenuItem.Click += new System.EventHandler(this.清理HTMLSToolStripMenuItemClick);
			// 
			// wkhtmlToPdfToolStripMenuItem
			// 
			this.wkhtmlToPdfToolStripMenuItem.Name = "wkhtmlToPdfToolStripMenuItem";
			this.wkhtmlToPdfToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.wkhtmlToPdfToolStripMenuItem.Text = "Wkhtml To Pdf";
			this.wkhtmlToPdfToolStripMenuItem.Click += new System.EventHandler(this.WkhtmlToPdfToolStripMenuItemClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(159, 6);
			// 
			// gBKToUTF8ToolStripMenuItem
			// 
			this.gBKToUTF8ToolStripMenuItem.Name = "gBKToUTF8ToolStripMenuItem";
			this.gBKToUTF8ToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.gBKToUTF8ToolStripMenuItem.Text = "GBK To UTF8";
			this.gBKToUTF8ToolStripMenuItem.Click += new System.EventHandler(this.GBKToUTF8ToolStripMenuItemClick);
			// 
			// uTF8ToGBKToolStripMenuItem
			// 
			this.uTF8ToGBKToolStripMenuItem.Name = "uTF8ToGBKToolStripMenuItem";
			this.uTF8ToGBKToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.uTF8ToGBKToolStripMenuItem.Text = "UTF8 To GBK";
			// 
			// vSC代码段格式化ToolStripMenuItem
			// 
			this.vSC代码段格式化ToolStripMenuItem.Name = "vSC代码段格式化ToolStripMenuItem";
			this.vSC代码段格式化ToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.vSC代码段格式化ToolStripMenuItem.Text = "VSC 代码段格式化";
			this.vSC代码段格式化ToolStripMenuItem.Click += new System.EventHandler(this.VSC代码段格式化ToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(314, 38);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "KeyStroke";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
			this.DoubleClick += new System.EventHandler(this.MainFormDoubleClick);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
