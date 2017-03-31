using System;
using System.Diagnostics;

namespace BotFramework
{
	internal static class Console
	{
		public static void WriteLine()
		{
			Debug.WriteLine("");
		}

		public static void WriteLine(bool value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(char value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(char[] buffer)
		{
			Debug.WriteLine(buffer);
		}

		public static void WriteLine(decimal value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(double value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(int value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(long value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(object value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(float value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(string value)
		{
			Debug.WriteLine(value);
		}

		[CLSCompliant(false)]
		public static void WriteLine(uint value)
		{
			Debug.WriteLine(value);
		}

		[CLSCompliant(false)]
		public static void WriteLine(ulong value)
		{
			Debug.WriteLine(value);
		}

		public static void WriteLine(string format, object arg0)
		{
			Debug.WriteLine(format, arg0);
		}

		public static void WriteLine(string format, params object[] arg)
		{
			if (arg == null)
				Debug.WriteLine(format);
			else
				Debug.WriteLine(format, arg);
		}


		public static void WriteLine(string format, object arg0, object arg1)
		{
			Debug.WriteLine(format, arg0, arg1);
		}

		public static void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			Debug.WriteLine(format, arg0, arg1, arg2);
		}

	}
}