namespace Notepad
{
	using Renci.SshNet;
	using Renci.SshNet.Sftp;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	
	public static   class SshNets
	{
		public static Task DownloadAsync(this SftpClient client,
			string path, Stream output,
			TaskFactory factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task.Factory).FromAsync(
				client.BeginDownloadFile(path, output),
				client.EndDownloadFile,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
		public static Task DownloadAsync(this SftpClient client,
			string path, Stream output, Action<ulong> downloadCallback,
			TaskFactory factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task.Factory).FromAsync(
				client.BeginDownloadFile(path, output, null, null, downloadCallback),
				client.EndDownloadFile,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
		public static Task<string> ExecuteAsync(this SshCommand command,
			TaskFactory<string> factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task<string>.Factory).FromAsync(
				command.BeginExecute(),
				command.EndExecute,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
		public static Task<string> ExecuteAsync(this SshCommand command,
			string commandText,
			TaskFactory<string> factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task<string>.Factory).FromAsync(
				command.BeginExecute(commandText, null, null),
				command.EndExecute,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
		public static Task<IEnumerable<SftpFile>> ListDirectoryAsync(this SftpClient client,
			string path, Action<int> listCallback = null,
			TaskFactory<IEnumerable<SftpFile>> factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task<IEnumerable<SftpFile>>.Factory).FromAsync(
				client.BeginListDirectory(path, null, null, listCallback),
				client.EndListDirectory,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
		public static Task<IEnumerable<FileInfo>> SynchronizeDirectoriesAsync(this SftpClient client,
			string sourcePath, string destinationPath, string searchPattern,
			TaskFactory<IEnumerable<FileInfo>> factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task<IEnumerable<FileInfo>>.Factory).FromAsync(
				client.BeginSynchronizeDirectories(sourcePath, destinationPath, searchPattern, null, null),
				client.EndSynchronizeDirectories,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
		public static Task UploadAsync(this SftpClient client,
			Stream input, string path, Action<ulong> uploadCallback = null,
			TaskFactory factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task.Factory).FromAsync(
				client.BeginUploadFile(input, path, null, null, uploadCallback),
				client.EndUploadFile,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
		public static Task UploadAsync(this SftpClient client,
			Stream input, string path, bool canOverride, Action<ulong> uploadCallback = null,
			TaskFactory factory = null,
			TaskCreationOptions creationOptions = default(TaskCreationOptions),
			TaskScheduler scheduler = null)
		{
			return (factory = factory ?? Task.Factory).FromAsync(
				client.BeginUploadFile(input, path, canOverride, null, null, uploadCallback),
				client.EndUploadFile,
				creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
		}
	}
}