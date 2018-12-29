

using System;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using Shared;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Management;
using Common;

namespace KeyStroke
{
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		#region
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public static implicit operator Point(POINT point)
			{
				return new Point(point.X, point.Y);
			}
		}
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pnt);

		public static Point GetCursorPosition()
		{
			POINT lpPoint;
			GetCursorPos(out lpPoint);
			//bool success = User32.GetCursorPos(out lpPoint);
			// if (!success)

			return lpPoint;
		}
		#endregion
		
		string _cFile = null;
		string _cProcessName = null;
		IntPtr _hWnd = IntPtr.Zero;
		bool _bSurveillance = false;
		#region
		
		public static void CPlusPlusSnippetsVSC()
		{
			WinForms.OnClipboardString((str) => {
				var s = Regex.Replace(str, "[\r\n]+", "").Trim();// changed
				s = Regex.Replace(s, "\\s{2,}", " ");// changed
				var ls = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray();
				// var matches = Regex.Matches(ls.First(), "[a-zA-Z]+").Cast<Match>().Select(i => i.Value.First().ToString()).ToArray();
				
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix", s.SubstringBefore('(').SubstringAfter(" ").SubstringAfter("*").SubstringAfter("WINAPI ").Trim());
				//obj.Add("prefix", string.Join("", matches).ToLower());
				obj.Add("body", ls.Select(i => Regex.Replace(i.EscapeString(), "[a-z _A-Z]+[ \\*]+(?=[a-zA-Z0-9]+[, )])", "").SubstringAfter(" ").TrimStart('*')));// changed
				
				var r = new Dictionary<string,dynamic>();
				r.Add(ls.First(), obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return	sr.Substring(1, sr.Length - 2) + ",";
				
			});
		}
		
		
		
		public static void CFormat()
		{
			WinForms.OnClipboardString((str) => {
				var ls = Codes.FormatMethodList(Clipboard.GetText());
				var d = ls.Select(i => i.SubstringBefore(")") + ");").Where(i => i.IsReadable()).Select(i => i.Trim()).OrderBy(i => i.Split("(".ToArray(), 2).First().Split(' ').Last());
				var bodys = ls.OrderBy(i => Regex.Split(i.Split("(".ToArray(), 2).First(), "[: ]+").Last());
				return	string.Join("\n", d) + "\n\n\n" + string.Join("\n", string.Join("\n", bodys).Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries));
			});
		}
		
		#endregion
		int _key1 = 0;
		int _key2 = 0;
		int _key3 = 0;
		int _key7 = 0;
		int _key8 = 0;
		int _key10 = 0;
		int _runType = 0;
		string _recordMouse = null;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		protected override void WndProc(ref Message m)
		{
			//Debug.Write("Msg " + m.Msg + " WParam " + m.WParam + " LParam " + m.LParam + "\n");
			if (m.Msg == 0x0312) {
				var k = ((int)m.LParam >> 16) & 0xFFFF;
				if (k == 0x75) {
					var point = GetCursorPosition();
					_recordMouse += string.Format("{{{0},{1}}},\n", point.X, point.Y);
				} else if (k == 0x76) {
					if (_recordMouse == null) {
					
						this.globalEventProvider1.MouseDown += (o, e) => {
					
							if (e.Button == MouseButtons.Right) {
								_recordMouse += String.Format("{{{0},{1}}},\n", e.X, e.Y);
							}
						};
					} else {
					
						Clipboard.SetText(_recordMouse);
						_recordMouse = null;
					}
				} else if (k == 0x77) {
					//Codes.FormatVSCTypeDef();
					CPlusPlusSnippetsVSC();
				} else if (k == 0x79) {
//					if (_cFile != null)
//						Codes.FormatWithClangFormat(_cFile);
//					else {
//						WinForms.OnClipboardFile((f) => {
//							_cFile = f;
//						Codes.FormatWithClangFormat(f);
//						});
//					}
					var files =	Process.GetProcesses().Where(i => i.ProcessName == "cb_console_runner" || i.ProcessName == _cProcessName);
					foreach (var element in files) {
						element.Kill();
					}
					
					
				} 
			} else if (_bSurveillance && (m.Msg == 0x100 || m.Msg == 0x101 || m.Msg == 0x104 || m.Msg == 0x105)) {
				
				MessageBox.Show("Msg 0x" + m.Msg.ToString("X") + " WParam 0x" + m.WParam.ToString("X") + " LParam 0x" + m.LParam.ToString("X") + "\n");
				//Debug.WriteLine("Msg 0x" + m.Msg.ToString("X") + " WParam 0x" + m.WParam.ToString("X") + " LParam 0x" + m.LParam.ToString("X") + "\n");
				//Debug.WriteLine("PostMessage(hWnd,0x" + m.Msg.ToString("X") + " ,0x" + m.WParam.ToString("X") + " ,0x" + m.LParam.ToString("X") + ");\nSleep(100);\n");
				
			}
			//Debug.WriteLine("Msg 0x" + m.Msg.ToString("X") + " WParam 0x" + m.WParam.ToString("X") + " LParam 0x" + m.LParam.ToString("X") + "\n");
			
			base.WndProc(ref m);
		}
		void 获取当前坐标值热键FToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key1 == 0) {
				_key1 = 1 << 2;
				RegisterHotKey(Handle, _key1, 0, 0x75);
			} else {
				Clipboard.SetText(_recordMouse);
				_recordMouse = null;
			}
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (_key1 != 0) {
				UnregisterHotKey(Handle, _key1);
			}
			if (_key2 != 0) {
				UnregisterHotKey(Handle, _key2);
			}
			if (_key3 != 0) {
				UnregisterHotKey(Handle, _key3);
			}
			if (_key8 != 0) {
				UnregisterHotKey(Handle, _key8);
			}
		}
		void 编译CToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key2 == 0) {
				_key2 = 2 << 2;
				_runType = 1;
				RegisterHotKey(Handle, _key2, 0, 0x78);
			}
			
			if (_key7 == 0) {
				_key7 = 7;
				RegisterHotKey(Handle, _key7, 0, 0x79);
			}
		}
	
		void ClearButtonClick(object sender, EventArgs e)
		{
			_cFile = null;
		}
		void 格式化C代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			CFormat();
		}
		void 取色器ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		void Aria2ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = "aria2c".GetDesktopPath();
			dir.CreateDirectoryIfNotExists();
			Process.Start(new ProcessStartInfo() {
				FileName = "aria2c",
				WorkingDirectory = dir,
				Arguments = Clipboard.GetText()
			});
		}
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			MessageBox.Show("123");
		}
		void 记录鼠标事件热键F7ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key3 == 0) {
				_key3 = 3;
				RegisterHotKey(Handle, _key3, 0, 0x76);
			}
		}
		void VSC代码段热键F8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key8 == 0)
				_key8 = 8;
			RegisterHotKey(this.Handle, _key8, 0, (int)Keys.F8);
		}
		void 压缩目录不包含ZIP文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
				using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {
					var files = dir.GetFiles("zip", true);
					foreach (var element in files) {
						zip.AddFile(element, "");
					}
					var count = 0;
					var targetFileName = dir + ".zip";
					while (targetFileName.FileExists()) {
						targetFileName = dir + " V" + (++count).ToString().PadLeft(2, '0') + ".zip";
					}
					zip.Save(targetFileName);
				}
			});
		}
		void 压缩目录不包含ZIP文件ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
				using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {
					var files = dir.GetFiles("zip", true);
					
					zip.AddDirectory(dir, "");
					var count = 0;
					var targetFileName = dir + ".zip";
					while (targetFileName.FileExists()) {
						targetFileName = dir + " V" + (++count).ToString().PadLeft(2, '0') + ".zip";
					}
					zip.Save(targetFileName);
				}
			});
		}
		void 重命名压缩文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
				var files = Directory.GetFiles(dir, "*.zip");
				var c = files.Select(i => i.ConvertToInt()).Max();
				foreach (var element in files) {
					var targetFileName = Path.Combine(dir, Regex.Replace(Path.GetFileNameWithoutExtension(element), "[0-9]+$", "") + (++c).ToString().PadLeft(2, '0') + ".zip");
					File.Move(element, targetFileName);
				}
			                                  	
			});
		}
		void 鼠标下窗口句柄ToolStripMenuItemClick(object sender, EventArgs e)
		{
			IntPtr hWnd = WindowFromPoint(Control.MousePosition);
			_hWnd = hWnd;
			Clipboard.SetText("0X" + hWnd.ToString("X").PadLeft(8, '0'));
		}
		void MainFormDoubleClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
		}
		void CPUToolStripMenuItemClick(object sender, EventArgs e)
		{
	 
			var str = Clipboard.GetText();
			var lines = str.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim().TrimEnd(';').Split(' ').Last());
			
			var sb = new StringBuilder();
			foreach (var element in lines) {
				sb.AppendLine(string.Format("try{{sb.AppendLine(\"【{0}】：\"+obj[\"{0}\"].ToString());}}catch{{sb.AppendLine(\"{0}\");}}", element));
			}
			Clipboard.SetText(sb.ToString());
//			using (ManagementObjectSearcher win321394Controller = new ManagementObjectSearcher("select * from Win32_1394Controller"),
//win321394ControllerDevice = new ManagementObjectSearcher("select * from Win32_1394ControllerDevice"),
//win32AssociatedProcessorMemory = new ManagementObjectSearcher("select * from Win32_AssociatedProcessorMemory"),
//win32AutochkSetting = new ManagementObjectSearcher("select * from Win32_AutochkSetting"),
//win32BaseBoard = new ManagementObjectSearcher("select * from Win32_BaseBoard"),
//win32Battery = new ManagementObjectSearcher("select * from Win32_Battery"),
//win32BIOS = new ManagementObjectSearcher("select * from Win32_BIOS"),
//win32Bus = new ManagementObjectSearcher("select * from Win32_Bus"),
//win32CacheMemory = new ManagementObjectSearcher("select * from Win32_CacheMemory"),
//win32CDROMDrive = new ManagementObjectSearcher("select * from Win32_CDROMDrive"),
//win32CIMLogicalDeviceCIMDataFile = new ManagementObjectSearcher("select * from Win32_CIMLogicalDeviceCIMDataFile"),
//win32ComputerSystemProcessor = new ManagementObjectSearcher("select * from Win32_ComputerSystemProcessor"),
//win32CurrentProbe = new ManagementObjectSearcher("select * from Win32_CurrentProbe"),
//win32DesktopMonitor = new ManagementObjectSearcher("select * from Win32_DesktopMonitor"),
//win32DeviceBus = new ManagementObjectSearcher("select * from Win32_DeviceBus"),
//win32DeviceChangeEvent = new ManagementObjectSearcher("select * from Win32_DeviceChangeEvent"),
//win32DeviceMemoryAddress = new ManagementObjectSearcher("select * from Win32_DeviceMemoryAddress"),
//win32DeviceSettings = new ManagementObjectSearcher("select * from Win32_DeviceSettings"),
//win32DiskDrive = new ManagementObjectSearcher("select * from Win32_DiskDrive"),
//win32DiskDriveToDiskPartition = new ManagementObjectSearcher("select * from Win32_DiskDriveToDiskPartition"),
//win32DiskPartition = new ManagementObjectSearcher("select * from Win32_DiskPartition"),
//win32DisplayControllerConfiguration = new ManagementObjectSearcher("select * from Win32_DisplayControllerConfiguration"),
//win32DMAChannel = new ManagementObjectSearcher("select * from Win32_DMAChannel"),
//win32DriverForDevice = new ManagementObjectSearcher("select * from Win32_DriverForDevice"),
//win32Fan = new ManagementObjectSearcher("select * from Win32_Fan"),
//win32FloppyController = new ManagementObjectSearcher("select * from Win32_FloppyController"),
//win32FloppyDrive = new ManagementObjectSearcher("select * from Win32_FloppyDrive"),
//win32HeatPipe = new ManagementObjectSearcher("select * from Win32_HeatPipe"),
//win32IDEController = new ManagementObjectSearcher("select * from Win32_IDEController"),
//win32IDEControllerDevice = new ManagementObjectSearcher("select * from Win32_IDEControllerDevice"),
//win32InfraredDevice = new ManagementObjectSearcher("select * from Win32_InfraredDevice"),
//win32IRQResource = new ManagementObjectSearcher("select * from Win32_IRQResource"),
//win32Keyboard = new ManagementObjectSearcher("select * from Win32_Keyboard"),
//win32LogicalDisk = new ManagementObjectSearcher("select * from Win32_LogicalDisk"),
//win32LogicalDiskRootDirectory = new ManagementObjectSearcher("select * from Win32_LogicalDiskRootDirectory"),
//win32LogicalDiskToPartition = new ManagementObjectSearcher("select * from Win32_LogicalDiskToPartition"),
//win32LogicalProgramGroup = new ManagementObjectSearcher("select * from Win32_LogicalProgramGroup"),
//win32LogicalProgramGroupDirectory = new ManagementObjectSearcher("select * from Win32_LogicalProgramGroupDirectory"),
//win32LogicalProgramGroupItem = new ManagementObjectSearcher("select * from Win32_LogicalProgramGroupItem"),
//win32LogicalProgramGroupItemDataFile = new ManagementObjectSearcher("select * from Win32_LogicalProgramGroupItemDataFile"),
//win32MappedLogicalDisk = new ManagementObjectSearcher("select * from Win32_MappedLogicalDisk"),
//win32MemoryArray = new ManagementObjectSearcher("select * from Win32_MemoryArray"),
//win32MemoryArrayLocation = new ManagementObjectSearcher("select * from Win32_MemoryArrayLocation"),
//win32MemoryDevice = new ManagementObjectSearcher("select * from Win32_MemoryDevice"),
//win32MemoryDeviceArray = new ManagementObjectSearcher("select * from Win32_MemoryDeviceArray"),
//win32MemoryDeviceLocation = new ManagementObjectSearcher("select * from Win32_MemoryDeviceLocation"),
//win32MotherboardDevice = new ManagementObjectSearcher("select * from Win32_MotherboardDevice"),
//win32NetworkAdapter = new ManagementObjectSearcher("select * from Win32_NetworkAdapter"),
//win32NetworkAdapterConfiguration = new ManagementObjectSearcher("select * from Win32_NetworkAdapterConfiguration"),
//win32NetworkAdapterSetting = new ManagementObjectSearcher("select * from Win32_NetworkAdapterSetting"),
//win32NetworkClient = new ManagementObjectSearcher("select * from Win32_NetworkClient"),
//win32NetworkConnection = new ManagementObjectSearcher("select * from Win32_NetworkConnection"),
//win32NetworkLoginProfile = new ManagementObjectSearcher("select * from Win32_NetworkLoginProfile"),
//win32NetworkProtocol = new ManagementObjectSearcher("select * from Win32_NetworkProtocol"),
//win32OnBoardDevice = new ManagementObjectSearcher("select * from Win32_OnBoardDevice"),
//win32ParallelPort = new ManagementObjectSearcher("select * from Win32_ParallelPort"),
//win32PCMCIAController = new ManagementObjectSearcher("select * from Win32_PCMCIAController"),
//win32PhysicalMemory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory"),
//win32PhysicalMemoryArray = new ManagementObjectSearcher("select * from Win32_PhysicalMemoryArray"),
//win32PhysicalMemoryLocation = new ManagementObjectSearcher("select * from Win32_PhysicalMemoryLocation"),
//win32PnPAllocatedResource = new ManagementObjectSearcher("select * from Win32_PnPAllocatedResource"),
//win32PnPDevice = new ManagementObjectSearcher("select * from Win32_PnPDevice"),
//win32PnPDeviceProperty = new ManagementObjectSearcher("select * from Win32_PnPDeviceProperty"),
//win32PnPDevicePropertyBinary = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyBinary"),
//win32PnPDevicePropertyBoolean = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyBoolean"),
//win32PnPDevicePropertyBooleanArray = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyBooleanArray"),
//win32PnPDevicePropertyDateTime = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyDateTime"),
//win32PnPDevicePropertyDateTimeArray = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyDateTimeArray"),
//win32PnPDevicePropertyReal32 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyReal32"),
//win32PnPDevicePropertyReal32Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyReal32Array"),
//win32PnPDevicePropertyReal64 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyReal64"),
//win32PnPDevicePropertyReal64Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyReal64Array"),
//win32PnPDevicePropertySecurityDescriptor = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySecurityDescriptor"),
//win32PnPDevicePropertySecurityDescriptorArray = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySecurityDescriptorArray"),
//win32PnPDevicePropertySint16 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint16"),
//win32PnPDevicePropertySint16Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint16Array"),
//win32PnPDevicePropertySint32 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint32"),
//win32PnPDevicePropertySint32Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint32Array"),
//win32PnPDevicePropertySint64 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint64"),
//win32PnPDevicePropertySint64Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint64Array"),
//win32PnPDevicePropertySint8 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint8"),
//win32PnPDevicePropertySint8Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertySint8Array"),
//win32PnPDevicePropertyString = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyString"),
//win32PnPDevicePropertyStringArray = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyStringArray"),
//win32PnPDevicePropertyUint16 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyUint16"),
//win32PnPDevicePropertyUint16Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyUint16Array"),
//win32PnPDevicePropertyUint32 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyUint32"),
//win32PnPDevicePropertyUint32Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyUint32Array"),
//win32PnPDevicePropertyUint64 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyUint64"),
//win32PnPDevicePropertyUint64Array = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyUint64Array"),
//win32PnPDevicePropertyUint8 = new ManagementObjectSearcher("select * from Win32_PnPDevicePropertyUint8"),
//win32PnPEntity = new ManagementObjectSearcher("select * from Win32_PnPEntity"),
//win32PointingDevice = new ManagementObjectSearcher("select * from Win32_PointingDevice"),
//win32PortableBattery = new ManagementObjectSearcher("select * from Win32_PortableBattery"),
//win32PortConnector = new ManagementObjectSearcher("select * from Win32_PortConnector"),
//win32PortResource = new ManagementObjectSearcher("select * from Win32_PortResource"),
//win32POTSModem = new ManagementObjectSearcher("select * from Win32_POTSModem"),
//win32POTSModemToSerialPort = new ManagementObjectSearcher("select * from Win32_POTSModemToSerialPort"),
//win32Printer = new ManagementObjectSearcher("select * from Win32_Printer"),
//win32PrinterConfiguration = new ManagementObjectSearcher("select * from Win32_PrinterConfiguration"),
//win32PrinterController = new ManagementObjectSearcher("select * from Win32_PrinterController"),
//win32PrinterDriver = new ManagementObjectSearcher("select * from Win32_PrinterDriver"),
//win32PrinterDriverDll = new ManagementObjectSearcher("select * from Win32_PrinterDriverDll"),
//win32PrinterSetting = new ManagementObjectSearcher("select * from Win32_PrinterSetting"),
//win32PrinterShare = new ManagementObjectSearcher("select * from Win32_PrinterShare"),
//win32PrintJob = new ManagementObjectSearcher("select * from Win32_PrintJob"),
//win32Processor = new ManagementObjectSearcher("select * from Win32_Processor"),
//win32Refrigeration = new ManagementObjectSearcher("select * from Win32_Refrigeration"),
//win32SCSIController = new ManagementObjectSearcher("select * from Win32_SCSIController"),
//win32SCSIControllerDevice = new ManagementObjectSearcher("select * from Win32_SCSIControllerDevice"),
//win32SerialPort = new ManagementObjectSearcher("select * from Win32_SerialPort"),
//win32SerialPortConfiguration = new ManagementObjectSearcher("select * from Win32_SerialPortConfiguration"),
//win32SerialPortSetting = new ManagementObjectSearcher("select * from Win32_SerialPortSetting"),
//win32SMBIOSMemory = new ManagementObjectSearcher("select * from Win32_SMBIOSMemory"),
//win32SoundDevice = new ManagementObjectSearcher("select * from Win32_SoundDevice"),
//win32TapeDrive = new ManagementObjectSearcher("select * from Win32_TapeDrive"),
//win32TCPIPPrinterPort = new ManagementObjectSearcher("select * from Win32_TCPIPPrinterPort"),
//win32TemperatureProbe = new ManagementObjectSearcher("select * from Win32_TemperatureProbe"),
//win32USBController = new ManagementObjectSearcher("select * from Win32_USBController"),
//win32USBControllerDevice = new ManagementObjectSearcher("select * from Win32_USBControllerDevice"),
//win32VideoController = new ManagementObjectSearcher("select * from Win32_VideoController"),
//win32VideoSettings = new ManagementObjectSearcher("select * from Win32_VideoSettings"),
//win32VoltageProbe = new ManagementObjectSearcher("select * from Win32_VoltageProbe")) {
//var sb = new StringBuilder();
//try{foreach (ManagementObject obj in win321394Controller.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win321394ControllerDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32AssociatedProcessorMemory.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32AutochkSetting.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32BaseBoard.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32Battery.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32BIOS.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32Bus.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32CacheMemory.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32CDROMDrive.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32CIMLogicalDeviceCIMDataFile.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32ComputerSystemProcessor.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32CurrentProbe.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DesktopMonitor.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DeviceBus.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DeviceChangeEvent.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DeviceMemoryAddress.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DeviceSettings.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DiskDrive.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DiskDriveToDiskPartition.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DiskPartition.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DisplayControllerConfiguration.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DMAChannel.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32DriverForDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32Fan.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32FloppyController.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32FloppyDrive.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32HeatPipe.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32IDEController.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32IDEControllerDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32InfraredDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32IRQResource.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32Keyboard.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32LogicalDisk.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32LogicalDiskRootDirectory.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32LogicalDiskToPartition.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32LogicalProgramGroup.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32LogicalProgramGroupDirectory.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32LogicalProgramGroupItem.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32LogicalProgramGroupItemDataFile.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32MappedLogicalDisk.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32MemoryArray.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32MemoryArrayLocation.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32MemoryDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32MemoryDeviceArray.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32MemoryDeviceLocation.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32MotherboardDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32NetworkAdapter.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32NetworkAdapterConfiguration.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32NetworkAdapterSetting.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32NetworkClient.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32NetworkConnection.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32NetworkLoginProfile.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32NetworkProtocol.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32OnBoardDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32ParallelPort.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PCMCIAController.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PhysicalMemory.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PhysicalMemoryArray.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PhysicalMemoryLocation.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPAllocatedResource.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDeviceProperty.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyBinary.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyBoolean.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyBooleanArray.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyDateTime.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyDateTimeArray.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyReal32.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyReal32Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyReal64.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyReal64Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySecurityDescriptor.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySecurityDescriptorArray.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint16.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint16Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint32.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint32Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint64.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint64Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint8.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertySint8Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyString.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyStringArray.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyUint16.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyUint16Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyUint32.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyUint32Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyUint64.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyUint64Array.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPDevicePropertyUint8.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PnPEntity.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PointingDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PortableBattery.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PortConnector.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PortResource.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32POTSModem.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32POTSModemToSerialPort.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32Printer.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PrinterConfiguration.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PrinterController.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PrinterDriver.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PrinterDriverDll.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PrinterSetting.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PrinterShare.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32PrintJob.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32Processor.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32Refrigeration.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32SCSIController.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32SCSIControllerDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32SerialPort.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32SerialPortConfiguration.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32SerialPortSetting.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32SMBIOSMemory.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32SoundDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32TapeDrive.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32TCPIPPrinterPort.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32TemperatureProbe.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32USBController.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32USBControllerDevice.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32VideoController.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32VideoSettings.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//try{foreach (ManagementObject obj in win32VoltageProbe.Get()) {sb.AppendLine(obj.GetText(TextFormat.Mof));}}catch{}
//Clipboard.SetText(sb.ToString().Replace(";", ";" + Environment.NewLine));
//}

//			using (ManagementObjectSearcher win32Proc = new ManagementObjectSearcher("select * from Win32_Processor"), 
//			       win32CompSys = new ManagementObjectSearcher("select * from Win32_ComputerSystem"),
//			       win32Memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory")  ) {
//				var sb = new StringBuilder();
//				foreach (ManagementObject obj in win32Proc.Get()) {
////					var clockSpeed = obj["CurrentClockSpeed"].ToString();
////					var procName = obj["Name"].ToString();
////					var manufacturer = obj["Manufacturer"].ToString();
////					var	version = obj["Version"].ToString();
//					sb.AppendLine(obj.GetText(TextFormat.Mof));
//					//MessageBox.Show(string.Format(" 当前时钟频率： {0}\r\n 名称: {1}\r\n 制造商: {2}\r\n {3}",clockSpeed,procName,manufacturer,version));
//				}
//				
//				foreach (ManagementObject obj in win32Memory.Get()) {
//					
//				 
//					sb.AppendLine(obj.GetText(TextFormat.Mof));
//					 
//				}
//				foreach (ManagementObject obj in win32CompSys.Get()) {
//					
//				 
//					sb.AppendLine(obj.GetText(TextFormat.Mof));
//					 
//				}
//				Clipboard.SetText(sb.ToString().Replace(";", ";" + Environment.NewLine));
//			}

//			var hd = new HtmlAgilityPack.HtmlDocument();
//			hd.LoadHtml(Clipboard.GetText());
//			var nodes=hd.DocumentNode.SelectNodes("//li/a");
//			var ls=new List<string>();
//			var sb=new StringBuilder();
//			foreach (var element in nodes) {
//				var str=element.InnerText.Trim();
//				if(str.StartsWith("Win32_")&& !str.Contains(' '))
//				ls.Add(str);
//			}
//			ls=ls.Distinct().OrderBy(i=>i).ToList();
//			
//			var p1=ls.Select(i=>string.Format("win32{1} = new ManagementObjectSearcher(\"select * from {0}\")",i,i.Split('_').Last()));
//			sb.Append("using (ManagementObjectSearcher ");
//			sb.Append(string.Join(","+Environment.NewLine,p1));
//			sb.AppendLine(") {");
//			sb.AppendLine("var sb = new StringBuilder();");
//			var p2=ls.Select(i=>string.Format("try{{foreach (ManagementObject obj in win32{0}.Get()) {{sb.AppendLine(obj.GetText(TextFormat.Mof));}}}}catch{{}}",i.Split('_').Last()));
//			sb.AppendLine(string.Join(Environment.NewLine,p2));
//			sb.AppendLine("Clipboard.SetText(sb.ToString().Replace(\";\", \";\" + Environment.NewLine));");
//			sb.AppendLine("}");
//			
//			Clipboard.SetText(sb.ToString());
		}
		void CLangFormatToolStripMenuItemClick(object sender, EventArgs e)
		{

		}
		void 压缩AndroidToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory(WinForms.ZipAndroidProject);
		}
		void 压缩子目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
//			WinForms.OnClipboardDirectory((dir) => {
//			                              	var directories = Directory.GetFiles(dir,"*",SearchOption.AllDirectories)
//			                              		.Where(i=>Regex.IsMatch(i,"\\.(?:opf|ncx)$")).ToArray();
//			                              	foreach (var element in directories) {
//			                              		var str=element.ReadAllText().Replace(".xhtml",".html");
//			                              		element.WriteAllText(str);
//			                              	}
//			                              });
			WinForms.ZipDirectories();
		}
		void 解压目录中文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText().Trim();
			if (!Directory.Exists(dir))
				return;
			var zipFiles = Directory.GetFiles(dir, "*.zip");
			foreach (var element in zipFiles) {
				using (var zip = new Ionic.Zip.ZipFile(element, Encoding.GetEncoding("gbk"))) {
					zip.ExtractAll(Path.Combine(element.GetDirectoryName(), element.GetFileNameWithoutExtension()));
				}
			}
		}
		void 删除Aria2文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.RemoveAria2File();
		}
		void 清理HTMLSToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.CleanHtmls();
		}
		void WkhtmlToPdfToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText().Trim();
			if (!Directory.Exists(dir))
				return;

			foreach (var item in Directory.GetDirectories(dir)) {
				WinForms.InvokeWkhtmltopdf(item);
			}
		}
		void CodeBlocksToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
				_cProcessName = v;
				return null;
			});
			if (_key10 == 0)
				_key10 = 10;
			RegisterHotKey(this.Handle, _key10, 0, (int)Keys.F10);
		}
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			_cFile = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
		}
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else
				e.Effect = DragDropEffects.None;
		}
		
		void GBKToUTF8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
				var files = Directory.GetFiles(dir, "*").Where(i => Regex.IsMatch(i, "\\.(?:c|h|txt)"));
				foreach (var element in files) {
			                              		
					element.GbkToUTF8();
				}
			});
		}
		void 按键F8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_hWnd != IntPtr.Zero) {
				Win32.SetForegroundWindow(_hWnd);
				System.Threading.Tasks.Task.Factory.StartNew(() => {
					while (true) {
						SendKeys.SendWait("{F8}");
						System.Threading.Thread.Sleep(1000);
					}
				});
				
			}
		}
		void 监视按键ToolStripMenuItemClick(object sender, EventArgs e)
		{
			_bSurveillance = !_bSurveillance;
		}
		void VSC代码段格式化ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((s) => {
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix", "f" + s.SubstringBefore('(').SubstringAfterLast(" ").Trim());
				//obj.Add("prefix", string.Join("", matches).ToLower());
				var ls = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray();
				obj.Add("body", ls.Select(i => i.EscapeString()));// changed
				
				var r = new Dictionary<string,dynamic>();
				r.Add(ls.First(), obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return sr.Substring(1, sr.Length - 2) + ",";
				;            
			});
			
			
				
		}
		void 格式化C代码ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			var v = Clipboard.GetText();
			var str = Codes.FormatCSharpCode(v);
			Clipboard.SetText(str);
		}
		void KotlinButtonButtonClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
			                           
				var fileName = v.SubstringBefore('{').SubstringBefore('(').SubstringBefore('<').Trim().SubstringAfterLast(' ') + ".kt";
			                           
				fileName.GetDesktopPath().WriteAllText(v);
				return null;
			});
		}
		void 清理JavaKt文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((v) => {
				var files = Directory
			                           		.GetFiles(v, "*", SearchOption.AllDirectories)
			                           		.Where(i => Regex.IsMatch(i, "\\.(?:java|kt)$"));
				var pattern = "package ";
				foreach (var element in files) {
					var content = element.ReadAllText().TrimStart();
					if (content.StartsWith(pattern)) {
						continue;
					} else {
						var pos =	content.IndexOf(pattern);
						if (pos > -1) {
							element.WriteAllText(content.Substring(pos));
						}
					}
				}
			});
		}
	 
		void 生成IDXMLToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Codes.GenerateAndroidId);
		}
		void 清除空行ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((v) => v.ClearEmptyLinesInDirectory());
		}
	 
		void 合并XML文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory(Codes.CombineAndroidResource);
	
		}
		void JavaStaticKotlinConstToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		void KotlinConstToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
				return Regex.Replace(v, "(?<!const) val", " const val");
			});
		}
		void 函数ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Codes.OrderKotlinFun);
		}
		 
		void PublicMethodsToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
				var ls = new List<string>();
				var ls1 = new List<string>();
				var hd = new HtmlAgilityPack.HtmlDocument();
				hd.LoadHtml(v);
				var nodes = hd.DocumentNode.SelectNodes("//tbody/tr[contains(@class, 'api')]").ToArray();
				foreach (var element in nodes) {
					try {
					 
						var children = element.ChildNodes.Where(i => i.NodeType != HtmlAgilityPack.HtmlNodeType.Text).ToArray();
						var rs = children[0].InnerText.Trim().SubstringAfterLast(" ");
						if (rs.Contains("final"))
							continue;
						var rs1 = children[1].ChildNodes[1].InnerText.Trim();
						var rs2 = "";

						try {
							rs2 = children[1].ChildNodes[3].InnerText.Trim();
						} catch {
						
						}
						var sb = new StringBuilder();
			                           		
						var ab = new string[2];
						try {
							if (rs1.IsReadable())
								ab =	Codes.ParseJavaParameters(rs1);
						} catch (Exception ex) {
						
						}
			                           		
						//sb.AppendLine("TODO(\"not implemented\")");
						if (rs == "void") {
							ls1.Add(string.Format("// override fun {0}({1})", rs1.SubstringBefore('('), ab[1]));
							sb.AppendLine(string.Format("override fun {0}({1}){{", rs1.SubstringBefore('('), ab[1]));
							sb.AppendLine("// " + Regex.Replace(rs2, "[\r\n]+", " "));
			                           			
							sb.AppendLine("TODO(\"not implemented\")\r\n// " + ab[0]);
			                           		
						} else {
							ls1.Add(string.Format("// override fun {0}({1}):{2}", rs1.SubstringBefore('('), ab[1], rs.Capitalize()));
							
							sb.AppendLine(string.Format("override fun {0}({1}):{2}{{", rs1.SubstringBefore('('), ab[1], rs.Capitalize()));
							sb.AppendLine("// " + Regex.Replace(rs2, "[\r\n]+", " "));
			                           			
							sb.AppendLine("TODO(\"not implemented\")\r\n// return " + ab[0]);
							
						}
						sb.AppendLine("}");
						ls.Add(sb.ToString());
					} catch (Exception ex) {
						
					}
			                    
			                           		
				}
				return string.Join(Environment.NewLine, ls1.OrderBy(i => i)) + "\r\n\r\n\rn" + string.Join(Environment.NewLine, ls);
			});
		}
		void KotlinParameterToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
				 
				var a = v.SubstringAfter("(").SubstringBeforeLast(")");
				var b = a.Split(new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
				var c = b.Select(i => Regex.Split(i, "\\s").Where(ix => ix.IsReadable()).Last()).ToArray();
 
			
				return  string.Join(",", c);
			});
		}
		
		void 排序资源文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Codes.FormatAndroidResource);
	
		}
		void 函数执行标记ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Codes.OrderKotlinFunLog);
		}
		void CMSDNAPIToKotlinFunToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			var value = Clipboard.GetText();
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(value);
	
			var nodes = hd.DocumentNode.SelectNodes("//span[contains(@class,'lang-csharp')]/a").Select(i => HtmlAgilityPack.HtmlEntity.DeEntitize(i.InnerText)).ToArray();
			var list = new List<String>();
			
			foreach (var element in nodes) {
				list.Add(string.Format("fun File.{0}{{\n}}", element.DeCapitalize()));
			}
			Clipboard.SetText(string.Join("\n", list));
		}
		void SVGToDrawableToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((v) => {
			                              
				var files = System.IO.Directory.GetFiles(v, "*.svg");
				foreach (var element in files) {
			                              		
					Codes.ConvertSVGToVector(element);
				}
			});
		}
		void Val区块ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Codes.OrderKotlinValFun);
	
		}
		void ToolStripMenuItem3Click(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
				v = v.Trim();
				if (v.StartsWith("\"")) {
					v = v.Trim('\"');
					return "\"" + string.Join("|", v.Split("|".ToArray(), StringSplitOptions.RemoveEmptyEntries).Distinct().OrderBy(i => i)) + "\"";
			                           		
				}
				return string.Join("|", v.Split("|".ToArray(), StringSplitOptions.RemoveEmptyEntries).Distinct().OrderBy(i => i));
			});
	
		}
		void LogToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Javas.GenerateVariableLog);
		}
	 
	
		void 生成Safari文件夹ToolStripMenuItemClick(object sender, EventArgs e)
		{
			 
			var dir = Clipboard.GetText();
			var files = Directory.GetFiles(dir, "*.html");
			foreach (var element in files) {
				var hd = new HtmlAgilityPack.HtmlDocument();
				hd.LoadHtml(element.ReadAllText());
				var title = Htmls.DeEntitize(hd.DocumentNode.SelectSingleNode("//title").InnerText.SubstringBefore('[').Trim().GetValidFileName());
					
				var targetDirectory = Path.Combine(dir, title);
				if (!Directory.Exists(targetDirectory))
					Directory.CreateDirectory(targetDirectory);
					
				var targetFile = Path.Combine(targetDirectory, "目录.html");
				var node = hd.DocumentNode.SelectSingleNode("//*[@class='detail-toc']");
				var sb = new List<string>();
				var str = Regex.Replace(node.InnerHtml, "(?<=\\<a href\\=\")[#\\:\\w\\d\\-\\./]+", new MatchEvaluator((m) => {
					sb.Add(m.Value.SubstringBeforeLast("#").TrimEnd('"'));
					return m.Value.SubstringBeforeLast(".").SubstringAfterLast('/') + ".html";
				}));
				Path.Combine(targetDirectory, "links.txt").WriteAllText(string.Join(Environment.NewLine, sb.Distinct()));
				targetFile.WriteAllText(
					"<!DOCTYPE html> <html lang=en> <head> <meta charset=utf-8> <meta content=\"IE=edge http-equiv=X-UA-Compatible> <meta content=\"width=device-width,initial-scale=1\" name=viewport><link href=\"style.css\" rel=\"stylesheet\"></head><body><ol>" +
					str +
					"</ol></body>");
			}
			 
		}
		void 下载Safari文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
				var directories = Directory.GetDirectories(dir);
				
				foreach (var element in directories) {
					var hd = new HtmlAgilityPack.HtmlDocument();
					hd.LoadHtml((Path.Combine(element, "目录.html")).ReadAllText());
					
					var targetFile = Path.Combine(element, "links.txt");
					var nodes = hd.DocumentNode.SelectNodes("//a");
					var list = new List<String>();
					foreach (var n in nodes) {
						var h = n.GetAttributeValue("href", "").SubstringBefore('#');
						if (!list.Contains(h))
							list.Add(h);
					}
					targetFile.WriteAllText(string.Join(Environment.NewLine, list));
					
					Process.Start(new ProcessStartInfo() {
						FileName = "aria2c.exe",
						WorkingDirectory = element,
						Arguments = "-i \"" + targetFile + "\" " + "--load-cookies=\"C:\\Users\\Administrator\\Desktop\\Safari\\cookie.txt\""
					});
				}
			});
		}
		void 清理Safari文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
				var files = Directory.GetFiles(dir, "*.html", SearchOption.AllDirectories);
				foreach (var element in files) {
					// <a href="06_Chapter01.xhtml#c
					
					var valuue = Regex.Replace(element.ReadAllText(), "(?<=\\<a href\\=\")[\\w\\d\\-\\.]+", new MatchEvaluator((m) => {
					                                                                                                        
						return m.Value.SubstringBeforeLast(".") + ".html";
					}));
					element.WriteAllText(valuue);
				}
				files = Directory.GetFiles(dir, "*.ncx", SearchOption.AllDirectories);
				foreach (var element in files) {
					// <a href="06_Chapter01.xhtml#c
					
					var value = Regex.Replace(element.ReadAllText(), "(?<=\\<content src\\=\")[\\:\\w\\d\\-\\./#]+\"", new MatchEvaluator((m) => {
					                                                                                                        
						return m.Value.SubstringAfterLast("/");
					}));
					element.WriteAllText(value);
				}
			});
		}
		void 下载页面链接ToolStripMenuItemClick(object sender, EventArgs e)
		{
			  
		 
			var node = string.Join(Environment.NewLine, Safari.ParseSearch(Clipboard.GetText()));
			//var node = hd.DocumentNode.SelectNodes("//a[contains(@class,'js-search-link t-title')]").ToArray().Select(i => "https://www.safaribooksonline.com" + i.GetAttributeValue("href", "")).Distinct().ToArray();
			("aria2c".GetDesktopPath() + "\\links.txt").WriteAllText(string.Join(Environment.NewLine, node));
			Process.Start(new ProcessStartInfo() {
				FileName = "aria2c",
				WorkingDirectory = "aria2c".GetDesktopPath(),
				Arguments = "-i \"" + ("aria2c".GetDesktopPath() + "\\links.txt") + "\""
			});
		}
	
		void 清除重复SafariToolStripMenuItemClick(object sender, EventArgs e)
		{
		 
			WinForms.OnClipboardDirectory((dir) => {
				var file = Path.Combine(dir, "books.txt");
				if (File.Exists(file)) {
					var names =	file.ReadAllLines().Select(i => i.SubstringBeforeLast('.')).ToList();
					var epubs = @"C:\Users\Administrator\Desktop\Safari\EPUBS".GetFiles(false, "\\.(?:epub)$").Select(i => i.SubstringAfterLast('\\').SubstringBeforeLast('.')).ToList();
					names.AddRange(epubs);
					names.Distinct();
					var directories = dir.GetDirectories();
					var targetDirectory = Path.Combine(dir, ".others");
					targetDirectory.CreateDirectoryIfNotExists();
					
					foreach (var element in directories) {
						if (names.Contains(element.GetFileName())) {
							Directory.Move(element, Path.Combine(targetDirectory, element.GetFileName()));
						}
					}
				}
			});
		}
		void XxxToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			Clipboard.SetText(Android.ConvertToGradle(Clipboard.GetText()));
			
//			var files = Directory.GetFileSystemEntries(Clipboard.GetText());
//			
//			foreach (var element in files) {
//				
//				if (element != Htmls.DeEntitize(element)) {
//					if (Directory.Exists(element)) {
//						Directory.Move(element,Htmls.DeEntitize(element));
//					}else if(File.Exists(element)){
//						File.Move(element,Htmls.DeEntitize(element));
//					}
//				}
//			}
		}
		void StaticInt字段ToolStripMenuItemClick(object sender, EventArgs e)
		{
			 
			WinForms.OnClipboardString(Javas.FormatStaticIntField);
		}
		void 排序ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(v => v.SortLines());
	
		}
		void StaticString字段ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Javas.FormatStaticStringField);
	
		}
		void 方法LogToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Javas.GenerateLog);
	
		}
		void CsharpButtonButtonClick(object sender, EventArgs e)
		{
			Musics.Download163Music(Clipboard.GetText());
		}
		void PublicLgToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Javas.GeneratePublicLog);
	
		}
		void 从Android文档生成可重载方法ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Javas.GenerateMethods);
	
		}
		void 格式化static字符串字段短ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Javas.FormatStaticStringFieldShort);
	
		}
		void PublicToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
				var value = Javas.ExtractPublicMethodName(v).Where(i => i.Contains(" get")).OrderBy(i => i).Distinct().ToArray();
				var l1 = string.Join("\n", value.Select(i => "private " + i.ReplaceFirst("get", "m") + ";"));
				var l2 = string.Join("\n", value.Select(i => i.ReplaceFirst("get", "m").SubstringAfterLast(' ') + " = " + " " + i.SubstringAfterLast(' ') + "();\n"));
			                          
				return l1 + "\n\n\n" + l2;
			});
		}
		void BuilderToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => Javas.GenerateBuilder(v, "FileItem"));
		}
		void View从layout资源文件生成view代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString(Javas.GeneateViewFromXML);
		
	
		}
		void 合并HTMLSToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((v) => {
			                              
				foreach (var element in Directory.GetDirectories(v)) {
					Safari.CombineBook(element);
				}
			                              
			});
		}
		void MobiToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory(Utilities.MobiUtilities.PrettyName);
		}
		void EpubToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory(v => {
				var files = Directory.GetFiles(v, "*.epub");
				var targetDirectory=Path.Combine(v,".EPUB");
				targetDirectory.CreateDirectoryIfNotExists();
				foreach (var element in files) {
					 
					Utilities.EpubUtilities.PrettyName(element,targetDirectory);
					 
				}
			});
	
		}
	
	}
}
