using System;

namespace OnlineEditor.Extensions
{
	/// <summary>
	/// Invocation helper.
	/// </summary>
	internal static class Invoke
	{
		public static void Safe(this Action call, Action<Exception> err = null)
		{
			try
			{
				call();
			}
			catch (Exception ex)
			{
				if (err != null)
				{
					err(ex);
				}
			}
		}

		public static T Safe<T>(this Func<T> call, Action<Exception> err = null)
		{
			try
			{
				return call();
			}
			catch (Exception ex)
			{
				if (err != null)
				{
					err(ex);
				}
			}

			return default(T);
		}
	}
}
