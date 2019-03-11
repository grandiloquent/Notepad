using System.Security.Cryptography;
using System.Text;
namespace Common
{
	public static class Securitykeys
	{
		public static byte[] GenerateSalt()
		{
			using (var randomNumberGenerator = new RNGCryptoServiceProvider()) {
				var randomNumber = new byte[32];
				randomNumberGenerator.GetBytes(randomNumber);

				return randomNumber;
			}
		}
		
		public static string Md5(this string str)
		{

			var sb=new StringBuilder();
			var data = Encoding.GetEncoding("utf-8").GetBytes(str);
			MD5 md5 = new MD5CryptoServiceProvider();
			var bytes = md5.ComputeHash(data);
			for (int i = 0; i < bytes.Length; i++) {
				sb.Append(bytes[i].ToString("x2"));
			}
			return sb.ToString();
		}

	}
}