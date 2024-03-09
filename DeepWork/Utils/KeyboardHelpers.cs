using System.Runtime.InteropServices;
using System.Text;
using Windows.System;

namespace DeepWork.Utils;
public static class KeyboardHelpers
{
	// Todo: Hide this p/invoke function
	[DllImport("user32.dll")]
	public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
		byte[] keyboardState,
		[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
		int bufferSize, uint flags);

	public static string GetCharsFromKeys(VirtualKey key, bool shift, bool altGr)
	{
		StringBuilder buf = new(256);
		byte[] keyboardState = new byte[256];
		if (shift)
			keyboardState[(int)VirtualKey.Shift] = 0xff;
		if (altGr)
		{
			keyboardState[(int)VirtualKey.Control] = 0xff;
			keyboardState[(int)VirtualKey.Menu] = 0xff;
		}
		_ = ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);
		return buf.ToString();
	}
}