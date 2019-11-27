
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
	public static class Cryptos
	{
		// https://github.com/aspnet/AspNetCore/blob/3c09d644cccdb21801f7a79e1188a1a1212de5d9/src/Shared/WebEncoders/WebEncoders.cs
		
		public static int Base64UrlEncode(byte[] input, int offset, char[] output, int outputOffset, int count)
		{
			if (input == null) {
				throw new ArgumentNullException("");
			}
			if (output == null) {
				throw new ArgumentNullException("");
			}

			ValidateParameters(input.Length, "", offset, count);
			if (outputOffset < 0) {
				throw new ArgumentOutOfRangeException("");
			}

			var arraySizeRequired = GetArraySizeRequiredToEncode(count);
			if (output.Length - outputOffset < arraySizeRequired) {
				throw new ArgumentException(
//                    string.Format(
//                        CultureInfo.CurrentCulture,
//                        EncoderResources.WebEncoders_InvalidCountOffsetOrLength,
//                        nameof(count),
//                        nameof(outputOffset),
//                        nameof(output)),
//                    nameof(count)
				);
			}

			// Special-case empty input.
			if (count == 0) {
				return 0;
			}

			// Use base64url encoding with no padding characters. See RFC 4648, Sec. 5.

			// Start with default Base64 encoding.
			var numBase64Chars = Convert.ToBase64CharArray(input, offset, count, output, outputOffset);

			// Fix up '+' -> '-' and '/' -> '_'. Drop padding characters.
			for (var i = outputOffset; i - outputOffset < numBase64Chars; i++) {
				var ch = output[i];
				if (ch == '+') {
					output[i] = '-';
				} else if (ch == '/') {
					output[i] = '_';
				} else if (ch == '=') {
					// We've reached a padding character; truncate the remainder.
					return i - outputOffset;
				}
			}

			return numBase64Chars;
		}
//		public static string GenerateFileVersion(string path)
//		{
//	 
//			return path.GetFileNameWithoutExtension().TrimStart('.')
//			                    +"_v_"+GetHashForFile(path)
//			                    + path.GetExtension();
//		}
		 
		public static SHA256 CreateSHA256()
		{
			try {
				return SHA256.Create();
			}
            // SHA256.Create is documented to throw this exception on FIPS compliant machines.
            // See: https://msdn.microsoft.com/en-us/library/z08hz7ad%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
            catch (System.Reflection.TargetInvocationException) {
				// Fallback to a FIPS compliant SHA256 algorithm.
				return new SHA256CryptoServiceProvider();
			}
		}
		private static void ValidateParameters(int bufferLength, string inputName, int offset, int count)
		{
//			if (offset < 0) {
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			}
//			if (count < 0) {
//				throw new ArgumentOutOfRangeException(nameof(count));
//			}
//			if (bufferLength - offset < count) {
//				throw new ArgumentException(
//					string.Format(
//						CultureInfo.CurrentCulture,
//						EncoderResources.WebEncoders_InvalidCountOffsetOrLength,
//						nameof(count),
//						nameof(offset),
//						inputName),
//					nameof(count)
//				);
//			}
		}
		public static int GetArraySizeRequiredToEncode(int count)
		{
			var numWholeOrPartialInputBlocks = checked(count + 2) / 3;
			return checked(numWholeOrPartialInputBlocks * 4);
		}
		public static string Base64UrlEncode(byte[] input, int offset, int count)
		{
			if (input == null) {
				throw new ArgumentNullException("");
			}

			ValidateParameters(input.Length, "", offset, count);

			// Special-case empty input
			if (count == 0) {
				return string.Empty;
			}

			var buffer = new char[GetArraySizeRequiredToEncode(count)];
			var numBase64Chars = Base64UrlEncode(input, offset, buffer, outputOffset: 0, count: count);

			return new String(buffer, startIndex: 0, length: numBase64Chars);
		}
		public static string GetHashForString(string str)
		{
			using (var sha256 = CryptographyAlgorithms.CreateSHA256()) {
					
				var hash = sha256.ComputeHash(new UTF8Encoding(false).GetBytes(str));
					return Cryptos.Base64UrlEncode(hash);
			}
		}
		private static string GetHashForFile(string fileInfo)
		{
			using (var sha256 = CryptographyAlgorithms.CreateSHA256()) {
				using (var readStream = File.OpenRead(fileInfo)) {
					var hash = sha256.ComputeHash(readStream);
					return Cryptos.Base64UrlEncode(hash);
				}
			}
		}
		public static string Base64UrlEncode(byte[] input)
		{
			if (input == null) {
				throw new ArgumentNullException("");
			}

			return Base64UrlEncode(input, offset: 0, count: input.Length);
		}
		public static byte[] GenerateSalt()
		{
			const int saltLength = 32;

			using (var randomNumberGenerator = new RNGCryptoServiceProvider()) {
				var randomNumber = new byte[saltLength];
				randomNumberGenerator.GetBytes(randomNumber);

				return randomNumber;
			}
		}
		public static byte[] GenerateRandomNumber(int length)
		{
			using (var randomNumberGenerator = new RNGCryptoServiceProvider()) {
				var randomNumber = new byte[length];
				randomNumberGenerator.GetBytes(randomNumber);

				return randomNumber;
			}
		}
		private static byte[] Combine(byte[] first, byte[] second)
		{
			var ret = new byte[first.Length + second.Length];

			Buffer.BlockCopy(first, 0, ret, 0, first.Length);
			Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

			return ret;
		}

		public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
		{
			using (var sha256 = SHA256.Create()) {
				return sha256.ComputeHash(Combine(toBeHashed, salt));
			}
		}
	}
}