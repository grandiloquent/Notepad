﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;

namespace ColorPicker
{

	public partial class MainForm : Form
	{
		private Screen screen;
		private readonly Timer getColor;
		private bool txtHexFullBlocked = false;
		private bool txtHexShortBlocked = false;
		// Sample
		private Bitmap sampleBitmap = null;
		private bool hasSampled = false;
		private const int sampleSize = 10;
		private readonly Color[,] previewColors;
		private Color _sampleColor = Color.Black;
		private Color sampleColor {
			get { return _sampleColor; }
			set {
				_sampleColor = value;
				this.BackColor = _sampleColor;
			}
		}
		// Sample preview
		private const int previewSize = 80;
		private const int previewX = 10;
		private const int previewY = 40;
		// Move form dependencies
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		private int _x = 0;
		private int _y = 0;
        
		private int _clipboardMode = 0;
        
		public MainForm()
		{
			InitializeComponent();
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			chkPin.Checked = Properties.Settings.Default.StayOnTop;
			this.TopMost = chkPin.Checked;

			// Setup get color timer
			getColor = new Timer { Interval = 10 };
			getColor.Tick += new EventHandler(GetColorTick);

			previewColors = new Color[previewSize, previewSize];
		}

		/// <summary>
		/// Populates textboxes with correct color values.
		/// </summary>
		private void TranslateColor()
		{
			if (!hasSampled)
				return;

			try {
				string htmlColor = ColorTranslator.ToHtml(sampleColor);
				htmlColor = htmlColor.StartsWith("#") ? htmlColor : htmlColor.ToLower();
				if (!txtHexFullBlocked)
					txtHexFull.Text = htmlColor;
				if (!txtHexShortBlocked)
					txtHexShort.Text = htmlColor.StartsWith("#") ? htmlColor.Substring(1) : htmlColor;

				if (Properties.Settings.Default.UseFloat) {
					string tmpR = (sampleColor.R / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
					string tmpG = (sampleColor.G / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
					string tmpB = (sampleColor.B / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
					txtRgbShort.Text = string.Format("{0}, {1}, {2}", tmpR, tmpG, tmpB);
				} else {
					txtRgbShort.Text = string.Format("{0}, {1}, {2}", sampleColor.R, sampleColor.G, sampleColor.B);
				}
			} finally {
			}
		}

		/// <summary>
		/// Returns the sample region.
		/// </summary>
		/// <param name="screen">Screen to sample from.</param>
		/// <param name="mouseX">Mouse x position.</param>
		/// <param name="mouseY">Mouse y position.</param>
		/// <returns></returns>
		private Bitmap GetSampleRegion(Screen screen, int mouseX, int mouseY)
		{
			var bmp = new Bitmap(sampleSize, sampleSize, PixelFormat.Format32bppArgb);
			Graphics gfxScreenshot = Graphics.FromImage(bmp);
			gfxScreenshot.CopyFromScreen(mouseX - sampleSize / 2, mouseY - sampleSize / 2, 0, 0, new Size(sampleSize, sampleSize));
			gfxScreenshot.Save();
			gfxScreenshot.Dispose();
			return bmp;
		}

		/// <summary>
		/// Gets the contrast color based on specified color.
		/// </summary>
		/// <param name="color">Color to compare to.</param>
		/// <returns>A white or dark gray color based on color.</returns>
		private Color GetContrastColor(Color color)
		{
			int yiq = ((color.R * 299) + (color.G * 587) + (color.B)) / 1000;
			if (yiq >= 131.5)
				return Color.FromArgb(255, 33, 33, 33);
			else
				return Color.White;
		}

		/// <summary>
		/// Opens the color chooser dialog.
		/// </summary>
		private void OpenColorChooserDialog()
		{
			var colorDialog = new ColorDialog {
				Color = sampleColor,
				FullOpen = true
			};
			if (colorDialog.ShowDialog() == DialogResult.OK) {
				sampleColor = colorDialog.Color;
				TranslateColor();
			}
		}

		protected override void OnPaint(PaintEventArgs paintEvnt)
		{
			Graphics gfx = paintEvnt.Graphics;
			Brush brush;
			Pen pen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
			int blockSize = previewSize / sampleSize;

			// Update UI for color contrast
			lbTitle.ForeColor = GetContrastColor(sampleColor);
			btnPickColor.FlatAppearance.BorderColor = GetContrastColor(sampleColor);

			// Draw magnified preview
			for (int i = 0; i < sampleSize; i++) {
				for (int j = 0; j < sampleSize; j++) {
					if (sampleBitmap != null) {
						previewColors[i, j] = sampleBitmap.GetPixel(i, j);
					}
					brush = new SolidBrush(previewColors[i, j]);
					gfx.FillRectangle(brush, new Rectangle(previewX + i * blockSize, previewY + j * blockSize, blockSize, blockSize));

				}
			}

			// Draw preview border
			if (hasSampled) {
				gfx.DrawRectangle(pen, previewX + blockSize * (sampleSize / 2), previewY + blockSize * (sampleSize / 2), blockSize, blockSize);
			}

			// Clean up
			if (sampleBitmap != null) {
				sampleBitmap.Dispose();
				sampleBitmap = null;
			}
		}

		void GetColorTick(object sender, EventArgs e)
		{
			try {
				// Get color sample from screen
				int mouseX = MousePosition.X;
				int mouseY = MousePosition.Y;
				_x = mouseX;
				_y = mouseY;
				screen = Screen.FromRectangle(new Rectangle(MousePosition.X, MousePosition.Y, 1, 1));
				sampleBitmap = GetSampleRegion(screen, mouseX, mouseY);
				Color newColor = sampleBitmap.GetPixel(sampleSize / 2, sampleSize / 2);
				// Set color sample and update form
				sampleColor = newColor;
				TranslateColor();
				if (this.BackColor == newColor)
					this.Invalidate(new Rectangle(previewX, previewY, previewSize, previewSize));
			} finally {
			}
		}

		private void chkPin_CheckedChanged(object sender, EventArgs e)
		{
			this.TopMost = chkPin.Checked;
			Properties.Settings.Default.StayOnTop = chkPin.Checked;
			Properties.Settings.Default.Save();
		}

		private void btnOpenColorChooser_Click(object sender, EventArgs e)
		{
			if (_clipboardMode == 2)
				_clipboardMode = 0;
			_clipboardMode++;
			//OpenColorChooserDialog();
		}

		private void btnPickColor_MouseDown(object sender, MouseEventArgs e)
		{
			getColor.Start();
			var cv = new CursorConverter();
			var cursor = (Cursor)cv.ConvertFrom(Properties.Resources.pipette);
			if (!hasSampled) {
				btnPickColor.BackgroundImage = null;
				hasSampled = true;
			}
			this.Cursor = cursor;
		}

		private void btnPickColor_MouseUp(object sender, MouseEventArgs e)
		{
			getColor.Stop();
			this.Cursor = Cursors.Default;

			// Clean up
			if (sampleBitmap != null) {
				sampleBitmap.Dispose();
				sampleBitmap = null;
			}
			System.GC.Collect();
			var str = txtHexFull.Text;
			str = "0x" + str.Substring(5, 2) + str.Substring(3, 2) + str.Substring(1, 2);
//            var cst=Clipboard.GetText();
//            if(string.IsNullOrEmpty(null)){
//            	cst+="&&";
//            }
			if (_clipboardMode == 0)
				Clipboard.SetText(string.Format("GetPixel(hdc,{0},{1})=={2}", _x, _y, str));
			else if (_clipboardMode == 1)
				Clipboard.SetText(string.Format("_Click({0},{1});\nSleep(1000);\n", _x, _y));
			else if (_clipboardMode == 2)
				Clipboard.SetText(string.Format("{{{0},{1},{2}}}", _x, _y, str));
	
//  Clipboard.SetText(string.Format("#define POS__X {0}\n #define POS__Y {1}\nGetPixel(hdc,{0},{1})=={2}\n {{{0},{1},{2}}}",_x,_y,str));
			//Clipboard.SetText(string.Format("{3}GetPixel(hdc,{0},{1})=={2}",_x,_y,str,cst));
			// Clipboard.SetText(string.Format("{0},{1}",_x,_y));
            
			//Clipboard.SetText(string.Format("{{{0},{1},{2}}},",_x,_y,str));
		}

		private void txtHexFull_TextChanged(object sender, EventArgs e)
		{
			if (txtHexFull.Text.Length == 7 && txtHexFull.Text.StartsWith("#")) {
				sampleColor = ColorTranslator.FromHtml(txtHexFull.Text);
				TranslateColor();
			} else if (txtHexFull.Text.Length == 4 && txtHexFull.Text.StartsWith("#")) {
				sampleColor = ColorTranslator.FromHtml(txtHexFull.Text);
				txtHexFullBlocked = true;
				TranslateColor();
			}
		}

		private void txtHexShort_TextChanged(object sender, EventArgs e)
		{
			try {
				if (txtHexShort.Text.Length == 6) {
					sampleColor = ColorTranslator.FromHtml("#" + txtHexShort.Text);
					TranslateColor();
				} else if (txtHexShort.Text.Length == 3) {
					sampleColor = ColorTranslator.FromHtml("#" + txtHexShort.Text);
					txtHexShortBlocked = true;
					TranslateColor();
				}
			} catch (FormatException fex) {
				sampleColor = Color.Black;
				TranslateColor();
			}
		}

		private void txtHexFull_Leave(object sender, EventArgs e)
		{
			txtHexFullBlocked = false;
		}

		private void txtHexShort_Leave(object sender, EventArgs e)
		{
			txtHexShortBlocked = false;
		}

		private void txtHexFull_KeyPress(object sender, KeyPressEventArgs e)
		{
			var keys = new int[] {
				'a',
				'b',
				'c',
				'd',
				'e',
				'f',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F',
				'0',
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'\b',
				'#'
			};
			if (!keys.Contains(e.KeyChar))
				e.Handled = true;
		}

		private void TextBoxSelectAll(object sender, EventArgs e)
		{
			((TextBox)sender).SelectAll();
		}

		private void TextBoxCopyToClipboard(object sender, EventArgs e)
		{
			Clipboard.SetText(((TextBox)sender).Text);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void MainForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Drag form to move
			if (e.Button == MouseButtons.Left) {
				ReleaseCapture();
               
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// Escape to close
			if (keyData == Keys.Escape) {
				this.Close();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void contextFloat_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.UseFloat = true;
			Properties.Settings.Default.Save();
			TranslateColor();
		}

		private void contextByte_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.UseFloat = false;
			Properties.Settings.Default.Save();
			TranslateColor();
		}
	}
}
