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
	public static class Tasks
	{
		
		public static Task ForEachAsync<TIn>(
			IEnumerable<TIn> inputEnumerable,
			Func<TIn, Task> asyncProcessor,
			int maxDegreeOfParallelism = 4)
		{
			int maxAsyncThreadCount = maxDegreeOfParallelism;
			// disable once SuggestUseVarKeywordEvident
			SemaphoreSlim throttler = new SemaphoreSlim(maxAsyncThreadCount, maxAsyncThreadCount);

			IEnumerable<Task> tasks = inputEnumerable.Select(async input => {
				await throttler.WaitAsync().ConfigureAwait(false);
				try {
					await asyncProcessor(input).ConfigureAwait(false);
				} finally {
					throttler.Release();
				}
			});

			return Task.WhenAll(tasks);
		}
		
	}
}