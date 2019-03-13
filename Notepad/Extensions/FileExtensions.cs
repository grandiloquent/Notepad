using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;
using System.Runtime.Versioning;
using System.Threading;
using System.Collections;

namespace Utils
{
	abstract internal class Iterator<TSource> : IEnumerable<TSource>, IEnumerator<TSource>
	{
		int threadId;
		internal int state;
		internal TSource current;
 
		public Iterator()
		{
			threadId = Thread.CurrentThread.ManagedThreadId;
		}
 
		public TSource Current {
			get { return current; }
		}
 
		protected abstract Iterator<TSource> Clone();
 
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
 
		protected virtual void Dispose(bool disposing)
		{
			current = default(TSource);
			state = -1;
		}
 
		public IEnumerator<TSource> GetEnumerator()
		{
			if (threadId == Thread.CurrentThread.ManagedThreadId && state == 0) {
				state = 1;
				return this;
			}
 
			Iterator<TSource> duplicate = Clone();
			duplicate.state = 1;
			return duplicate;
		}
 
		public abstract bool MoveNext();
 
		object IEnumerator.Current {
			get { return Current; }
		}
 
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
 
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
 
	}
	internal class ReadLinesIterator : Iterator<string>
	{
		private readonly string _path;
		private readonly Encoding _encoding;
		private StreamReader _reader;
 
		[ResourceExposure(ResourceScope.Machine)]
		[ResourceConsumption(ResourceScope.Machine)]
		private ReadLinesIterator(string path, Encoding encoding, StreamReader reader)
		{
			Contract.Requires(path != null);
			Contract.Requires(path.Length > 0);
			Contract.Requires(encoding != null);
			Contract.Requires(reader != null);
 
			_path = path;
			_encoding = encoding;
			_reader = reader;
		}
 
		public override bool MoveNext()
		{
			if (this._reader != null) {
				this.current = _reader.ReadLine();
				if (this.current != null)
					return true;
 
				// To maintain 4.0 behavior we Dispose 
				// after reading to the end of the reader.
				Dispose();
			}
 
			return false;
		}
 
		protected override Iterator<string> Clone()
		{
			// NOTE: To maintain the same behavior with the previous yield-based
			// iterator in 4.0, we have all the IEnumerator<T> instances share the same 
			// underlying reader. If we have already been disposed, _reader will be null, 
			// which will cause CreateIterator to simply new up a new instance to start up
			// a new iteration. Dev10 Bugs 904764 has been filed to fix this in next side-
			// by-side release.
			return CreateIterator(_path, _encoding, _reader);
		}
 
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing) {
					if (_reader != null) {
						_reader.Dispose();
					}
				}
			} finally {
				_reader = null;
				base.Dispose(disposing);
			}
		}
 
		internal static ReadLinesIterator CreateIterator(string path, Encoding encoding)
		{
			return CreateIterator(path, encoding, (StreamReader)null);
		}
 
		private static ReadLinesIterator CreateIterator(string path, Encoding encoding, StreamReader reader)
		{
			return new ReadLinesIterator(path, encoding, reader ?? new StreamReader(path, encoding));
		}
	}
	public static class FileExtensions
	{
	
		
		
		
		public static IEnumerable<string> GetFiles(this string dir, string pattern, bool bExclude = false)
		{
			if (bExclude)
				return Directory.GetFiles(dir).Where(i => !Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			else
				return Directory.GetFiles(dir).Where(i => Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			
		}

	
		
		
		
	
		public static IEnumerable<String> ReadLines(this String path)
		{
             
 
			return ReadLinesIterator.CreateIterator(path, Encoding.UTF8);
		}
 
		public static string GetApplicationPath(this string path)
		{
			return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), path);
		}
		
		public static string GetFileSha1(this string path)
		{
			using (var fs = new FileStream(path, FileMode.Open))
			using (var bs = new BufferedStream(fs))
			using (var reader = new StreamReader(bs)) {
				using (var sha1 = new System.Security.Cryptography.SHA1Managed()) {
					var hash = sha1.ComputeHash(bs);
					var formatted = new StringBuilder(2 * hash.Length);
					foreach (var b in hash) {
						formatted.AppendFormat("{0:X2}", b);
					}
				}
				return reader.ReadToEnd();
			}
		}

		public static string GetUniqueFileName(this string v)
		{
			var i = 1;
			var regex = new Regex(" \\- [0-9]+");
			var t = Path.Combine(Path.GetDirectoryName(v),
				        regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
				        Path.GetExtension(v));

			while (File.Exists(t)) {
				i++;
				t = Path.Combine(Path.GetDirectoryName(v),
					regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
					Path.GetExtension(v));
			}
			return t;
		}
		
	
		public static string[] ReadAllLines(this String path)
		{


			string line;
			var lines = new List<string>();

			using (var sr = new StreamReader(path, new UTF8Encoding(false)))
				while ((line = sr.ReadLine()) != null)
					lines.Add(line);

			return lines.ToArray();
		}
		 
	}
}
