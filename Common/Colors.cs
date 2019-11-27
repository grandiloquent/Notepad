using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Common
{
	public static class Colors
	{
		public static int Argb(
			int alpha,
			int red,
			int green,
			int blue)
		{
			return (alpha << 24) | (red << 16) | (green << 8) | blue;
		}
		
		public static int[] parseColor(string hex)
		{
			if (hex.StartsWith("#"))
				hex = hex.Substring(1);

			if (hex.Length != 6)
				throw new Exception("Color not valid");

			return new int[] {
				int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
				int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
				int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)
			};
		}
	}
}
