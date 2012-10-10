using System;
using System.Collections.Generic;
using System.Timers;

namespace EditorClient.Services
{
	/// <summary>
	/// Scheduler, runs tasks.
	/// </summary>
	public static class Scheduler
	{
		private const int Default = -1;
		private static readonly Random _random = new Random();
		private static readonly Dictionary<int, Timer> _tasks = new Dictionary<int, Timer>();

		public static int RegisterTask(Action action, int timoutMs)
		{
			var key = _random.Next();
			while (_tasks.ContainsKey(key))
			{
				key = _random.Next();
			}

			var timer = new Timer(timoutMs);
			timer.Elapsed += (sender, args) => action();
			timer.Start();
			_tasks.Add(key, timer);
			return key;
		}

		public static int UnregisterTask(int key)
		{
			if (!_tasks.ContainsKey(key)) return Default;
			var timer = _tasks[key];
			timer.Stop();
			timer.Dispose();
			_tasks.Remove(key);
			return Default;
		}
	}
}
