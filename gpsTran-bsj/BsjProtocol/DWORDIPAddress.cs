using System;
using System.Runtime.InteropServices;
namespace BSJProtocol
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DWORDIPAddress
	{
		[FieldOffset(3)]
		public byte Byte1;
		[FieldOffset(2)]
		public byte Byte2;
		[FieldOffset(1)]
		public byte Byte3;
		[FieldOffset(0)]
		public byte Byte4;
		[FieldOffset(0)]
		public int Address;
	}
}
