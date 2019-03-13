using System;
using System.Collections.Generic;
namespace Common
{
	public static class Linqs
	{
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items) {
				action(item);
			}
		}
	}
}