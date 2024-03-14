using System.Runtime.InteropServices;
using System.Text;

namespace DeepWork.Helpers;

internal static partial class KeyboardHelpers
{
	[DllImport("user32.dll", SetLastError = true)]
	public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
		byte[] keyboardState,
		[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
		int bufferSize, uint flags);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool GetKeyboardState([Out]byte[] receivingBuffer);
}