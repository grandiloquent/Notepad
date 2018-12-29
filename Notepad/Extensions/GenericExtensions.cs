
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
	
	public static class GenericExtensions
	{
		public static void ForEach<T>(IEnumerable<T> collections, Action<T> action)
		{
			var enumerable = collections as T[] ?? collections.ToArray();
			if (enumerable.Any()) {
				var size = enumerable.Count();

				for (var i = 0; i < size; i++) {

					action(enumerable.ElementAt(i));
				}

			}
		}
	}
}
