using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
namespace gpsTran
{
	internal class CFeiTianPacket
	{
		private MemoryStream m_Buffer = new MemoryStream();
		public void Append(byte[] pData, int nLength)
		{
			try
			{
				this.m_Buffer.Write(pData, 0, nLength);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		public void Append(byte[] pData)
		{
			try
			{
				this.m_Buffer.Write(pData, 0, pData.Length);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		public bool SplitPacket(out byte[] packet)
		{
			packet = null;
			bool result;
			try
			{
				int num = (int)this.m_Buffer.Position;
				if (num > 2)
				{
					byte[] buffer = this.m_Buffer.GetBuffer();
					int num2 = (int)buffer[0] << 8;
					num2 += (int)buffer[1];
					if (num2 > num - 2)
					{
						result = false;
					}
					else
					{
						packet = new byte[num2];
						Buffer.BlockCopy(buffer, 2, packet, 0, packet.Length);
						this.m_Buffer.Seek(0L, SeekOrigin.Begin);
						this.m_Buffer.Position = 0L;
						num2 += 2;
						this.m_Buffer.Write(buffer, num2, num - num2);
						result = true;
					}
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
				result = false;
			}
			return result;
		}
		public static byte[] CombinPacket(string strText)
		{
			byte[] result;
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(strText);
				int num = bytes.Length;
				byte[] array = new byte[num + 2];
				array[0] = (byte)((num & 65280) >> 8);
				array[1] = (byte)(num & 255);
				Buffer.BlockCopy(bytes, 0, array, 2, bytes.Length);
				result = array;
			}
			catch (Exception value)
			{
				Debug.Write(value);
				result = null;
			}
			return result;
		}
		public static bool TryParsePacket(byte[] buff, out string strContext)
		{
			strContext = null;
			bool result;
			try
			{
				strContext = Encoding.UTF8.GetString(buff);
				strContext = HttpUtility.UrlDecode(strContext);
				result = true;
			}
			catch (Exception value)
			{
				Debug.Write(value);
				result = false;
			}
			return result;
		}
		public static string FormatArray(byte[] aArray)
		{
			string[] array = new string[aArray.Length];
			for (int i = 0; i < aArray.Length; i++)
			{
				array[i] = aArray[i].ToString("X2");
			}
			return string.Join(" ", array);
		}
		public static string md5(string strText)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = Encoding.GetEncoding("gb2312").GetBytes(strText);
			array = mD.ComputeHash(array);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
		private void dbgPrint(Exception ex)
		{
		}
	}
}
