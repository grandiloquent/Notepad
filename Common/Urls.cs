using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common
{
	public static class Urls
	{
		public static int HexToInt(char h)
		{
			return (h >= '0' && h <= '9') ? h - '0' :
				(h >= 'a' && h <= 'f') ? h - 'a' + 10 :
				(h >= 'A' && h <= 'F') ? h - 'A' + 10 :
				-1;
		}
		public static bool IsUrlSafeChar(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
				return true;
			
			switch (ch) {
				case '-':
				case '_':
				case '.':
				case '!':
				case '*':
				case '(':
				case ')':
					return true;
			}
			
			return false;
		}
				internal static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
		{
			if (bytes == null && count == 0)
				return false;
			if (bytes == null) {
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length) {
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length) {
				throw new ArgumentOutOfRangeException("count");
			}
			
			return true;
		}
		
		
		
			public static byte[] UrlEncode(byte[] bytes, int offset, int count)
		{
			if (!ValidateUrlEncodingParameters(bytes, offset, count)) {
				return null;
			}
			
			int cSpaces = 0;
			int cUnsafe = 0;
			
			// count them first
			for (int i = 0; i < count; i++) {
				char ch = (char)bytes[offset + i];
				
				if (ch == ' ')
					cSpaces++;
				else if (!IsUrlSafeChar(ch))
					cUnsafe++;
			}
			
			// nothing to expand?
			if (cSpaces == 0 && cUnsafe == 0) {
				// DevDiv 912606: respect "offset" and "count"
				if (0 == offset && bytes.Length == count) {
					return bytes;
				} else {
					var subarray = new byte[count];
					Buffer.BlockCopy(bytes, offset, subarray, 0, count);
					return subarray;
				}
			}
			
			// expand not 'safe' characters into %XX, spaces to +s
			byte[] expandedBytes = new byte[count + cUnsafe * 2];
			int pos = 0;
			
			for (int i = 0; i < count; i++) {
				byte b = bytes[offset + i];
				char ch = (char)b;
				
				if (IsUrlSafeChar(ch)) {
					expandedBytes[pos++] = b;
				} else if (ch == ' ') {
					expandedBytes[pos++] = (byte)'+';
				} else {
					expandedBytes[pos++] = (byte)'%';
					expandedBytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
					expandedBytes[pos++] = (byte)IntToHex(b & 0x0f);
				}
			}
			
			return expandedBytes;
		}
		public static char IntToHex(int n)
		{
			//Debug.Assert(n < 0x10);
			
			if (n <= 9)
				return (char)(n + (int)'0');
			else
				return (char)(n - 10 + (int)'a');
		}
	
		
		public static  string UrlEncodeUnicode(string value, bool ignoreAscii)
		{
			if (value == null) {
				return null;
			}
			
			int l = value.Length;
			StringBuilder sb = new StringBuilder(l);
			
			for (int i = 0; i < l; i++) {
				char ch = value[i];
				
				if ((ch & 0xff80) == 0) {  // 7 bit?
					if (ignoreAscii || IsUrlSafeChar(ch)) {
						sb.Append(ch);
					} else if (ch == ' ') {
						sb.Append('+');
					} else {
						sb.Append('%');
						sb.Append(IntToHex((ch >> 4) & 0xf));
						sb.Append(IntToHex((ch) & 0xf));
					}
				} else { // arbitrary Unicode?
					sb.Append("%u");
					sb.Append(IntToHex((ch >> 12) & 0xf));
					sb.Append(IntToHex((ch >> 8) & 0xf));
					sb.Append(IntToHex((ch >> 4) & 0xf));
					sb.Append(IntToHex((ch) & 0xf));
				}
			}
			
			return sb.ToString();
		}
		public static string UrlDecode(this string value, Encoding encoding)
		{
			if (value == null) {
				return null;
			}
			
			int count = value.Length;
			UrlDecoder helper = new UrlDecoder(count, encoding);
			
			// go through the string's chars collapsing %XX and %uXXXX and
			// appending each char as char, with exception of %XX constructs
			// that are appended as bytes
			
			for (int pos = 0; pos < count; pos++) {
				char ch = value[pos];
				
				if (ch == '+') {
					ch = ' ';
				} else if (ch == '%' && pos < count - 2) {
					if (value[pos + 1] == 'u' && pos < count - 5) {
						int h1 = HexToInt(value[pos + 2]);
						int h2 = HexToInt(value[pos + 3]);
						int h3 = HexToInt(value[pos + 4]);
						int h4 = HexToInt(value[pos + 5]);
						
						if (h1 >= 0 && h2 >= 0 && h3 >= 0 && h4 >= 0) {   // valid 4 hex chars
							ch = (char)((h1 << 12) | (h2 << 8) | (h3 << 4) | h4);
							pos += 5;
							
							// only add as char
							helper.AddChar(ch);
							continue;
						}
					} else {
						int h1 = HexToInt(value[pos + 1]);
						int h2 = HexToInt(value[pos + 2]);
						
						if (h1 >= 0 && h2 >= 0) {     // valid 2 hex chars
							byte b = (byte)((h1 << 4) | h2);
							pos += 2;
							
							// don't add as char
							helper.AddByte(b);
							continue;
						}
					}
				}
				
				if ((ch & 0xFF80) == 0)
					helper.AddByte((byte)ch); // 7 bit have to go as bytes because of Unicode
				else
					helper.AddChar(ch);
			}
			
			return helper.GetString();
			//return Utf16StringValidator.ValidateString(helper.GetString());
		}
		
	}
}