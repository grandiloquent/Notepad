namespace Helpers
{
	
	using System;
	using System.IO;
	using System.IO.Compression;
	using System.Text;
	
	public static class Files
	{
	public static	void CreateDirectoryRecursively(this string path)
{
    string[] pathParts = path.Split('\\');

    for (int i = 0; i < pathParts.Length; i++)
    {
    	if (i > 0)
            pathParts[i] = pathParts[i - 1]+"\\"+ pathParts[i];

        if (!Directory.Exists(pathParts[i]))
            Directory.CreateDirectory(pathParts[i]);
    }
}
		public static void ExtractToDirectory(this string sourceArchiveFileName
		                                      , string destinationDirectoryName
		                                      , Encoding entryNameEncoding
		                                      , Func<string,bool> predict
		                                     , bool  overwriteFiles = true)
		{
	 
			using(var fs=File.OpenRead(sourceArchiveFileName)){
			
			using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Read, true,entryNameEncoding)) {
				foreach (ZipArchiveEntry entry in archive.Entries) {
					if (predict(entry.FullName)) {
						var targetDirectory=Path.Combine(destinationDirectoryName,Path.GetDirectoryName(entry.FullName));
						if(!Directory.Exists(targetDirectory))
						{
							targetDirectory.CreateDirectoryRecursively();
						}
						entry.ExtractToFile(Path.Combine(targetDirectory,entry.Name), overwriteFiles);
					}
				 
				}
				}}
			
		}
	}
}