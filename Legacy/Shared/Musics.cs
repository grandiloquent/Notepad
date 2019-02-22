 
using System;
using System.Diagnostics;
using Common;
namespace Shared
{
 
	public static class Musics
	{
		 
		public static void Download163Music(string value){
			"mp3".GetDesktopPath().CreateDirectoryIfNotExists();
			Process.Start(new ProcessStartInfo(){
			              FileName="aria2c",
			              WorkingDirectory="mp3".GetDesktopPath(),
			              Arguments="http://music.163.com/song/media/outer/url?id="+value.SubstringAfterLast('=')+".mp3"
			              });
		}
	}
}
