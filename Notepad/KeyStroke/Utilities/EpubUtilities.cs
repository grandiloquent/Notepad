 
using Ionic.Zip;
using Shared;
using System.IO;
using System.Linq;
using System.Xml.Linq;
namespace Utilities
{
	public static class EpubUtilities
	{
		public static void PrettyName(string fileName, string directory)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			XNamespace dc = "http://purl.org/dc/elements/1.1/";
			ZipFile val = new ZipFile(fileName);
			ZipEntry val2 = val.Entries.First(i => i.FileName.EndsWith(".opf"));
			MemoryStream memoryStream = new MemoryStream();
			val2.Extract((Stream)memoryStream);
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			XDocument xDocument = XDocument.Load((Stream)memoryStream);
			XElement[] source = xDocument.Descendants().ToArray();
			XElement xElement = (from i in source
			                     where i.Name == dc + "title"
			                     select i).First();
			XElement xElement2 = null;
			try {
				xElement2 = (from i in source
				             where i.Name == dc + "creator"
				             select i).First();
			} catch {
				 
				xElement2= (from i in source
				             where i.Name == dc + "description"
				             select i).First();
			}
			if (xElement != null) {
				string value = xElement.Value;
				value = value.Split("/;:".ToArray(), 2).First();
				value = value.GetValidFileName(' ').Trim();
				string arg = "Anonymous";
				if (xElement2 != null) {
					arg = xElement2.Value.Split('/').Last().Trim().GetValidFileName(' ');
				}
				//$"{value}-{arg}.epub"
				string text = directory.Combine(string.Format("{0} - {1}.epub", value, arg));
				val.Dispose();
				memoryStream.Dispose();
				streamReader.Dispose();
				if (fileName != text && !text.FileExists()) {
					File.Move(fileName, text);
				}
			}
		}
	}
}
