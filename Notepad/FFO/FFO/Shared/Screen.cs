

namespace  Shared
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Linq;
	public static class Screens
	{
		public static Color GetColorAt(Point location, Bitmap screenPixel)
		{
			if (screenPixel == null) {
				screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
			}
			using (Graphics gdest = Graphics.FromImage(screenPixel)) {
				using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
					IntPtr hSrcDC = gsrc.GetHdc();
					IntPtr hDC = gdest.GetHdc();
					int retval = NativeMethods.BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
					gdest.ReleaseHdc();
					gsrc.ReleaseHdc();
				}
			}

			return screenPixel.GetPixel(0, 0);
		}
		 
	}
}
