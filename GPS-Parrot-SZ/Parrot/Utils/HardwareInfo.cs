using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;
using System.Net;

namespace Parrot
{
    /// <summary>
    /// 本机硬件资料。
    /// </summary>
    public class HardwareInfo
    {
        /// <summary>
        /// 获取CPU识别码。
        /// </summary>
        /// <returns></returns>
        public string GetCpuID()
        {
            try
            {
                ManagementObjectCollection instances = new ManagementClass("Win32_Processor").GetInstances();
                string str = null;
                foreach (ManagementObject obj2 in instances)
                {
                    str = obj2.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取硬盘序列号。
        /// </summary>
        /// <returns></returns>
        public string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string str = null;
                foreach (ManagementObject obj2 in searcher.Get())
                {
                    str = obj2["SerialNumber"].ToString().Trim();
                    break;
                }
                return str;
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }

        /// <summary>
        /// 获取本机的计算机名。
        /// </summary>
        /// <returns></returns>
        public string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// 获取网卡物理地址。
        /// </summary>
        /// <returns></returns>
        public string GetMacAddress()
        {
            string str = "";
            try
            {
                NCB ncb = new NCB();
                ncb.ncb_command = 0x37;
                int cb = Marshal.SizeOf(typeof(LANA_ENUM));
                ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                ncb.ncb_length = (ushort)cb;
                char ch = Win32API.Netbios(ref ncb);
                LANA_ENUM lana_enum = (LANA_ENUM)Marshal.PtrToStructure(ncb.ncb_buffer, typeof(LANA_ENUM));
                Marshal.FreeHGlobal(ncb.ncb_buffer);
                if (ch != '\0')
                {
                    return "";
                }
                for (int i = 0; i < lana_enum.length; i++)
                {
                    ASTAT astat;
                    ncb.ncb_command = 50;
                    ncb.ncb_lana_num = lana_enum.lana[i];
                    if (Win32API.Netbios(ref ncb) != '\0')
                    {
                        return "";
                    }
                    ncb.ncb_command = 0x33;
                    ncb.ncb_lana_num = lana_enum.lana[i];
                    ncb.ncb_callname[0] = 0x2a;
                    cb = Marshal.SizeOf(typeof(ADAPTER_STATUS)) + (Marshal.SizeOf(typeof(NAME_BUFFER)) * 30);
                    ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                    ncb.ncb_length = (ushort)cb;
                    ch = Win32API.Netbios(ref ncb);
                    astat.adapt = (ADAPTER_STATUS)Marshal.PtrToStructure(ncb.ncb_buffer, typeof(ADAPTER_STATUS));
                    Marshal.FreeHGlobal(ncb.ncb_buffer);
                    if (ch == '\0')
                    {
                        if (i > 0)
                        {
                            str = str + ":";
                        }
                        str = string.Format("{0,2:X}{1,2:X}{2,2:X}{3,2:X}{4,2:X}{5,2:X}", new object[] { astat.adapt.adapter_address[0], astat.adapt.adapter_address[1], astat.adapt.adapter_address[2], astat.adapt.adapter_address[3], astat.adapt.adapter_address[4], astat.adapt.adapter_address[5] });
                    }
                }
            }
            catch
            {
            }
            return str.Replace(' ', '0');
        }

        #region Nested Types
        [StructLayout(LayoutKind.Sequential)]
        public struct ADAPTER_STATUS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] adapter_address;
            public byte rev_major;
            public byte reserved0;
            public byte adapter_type;
            public byte rev_minor;
            public ushort duration;
            public ushort frmr_recv;
            public ushort frmr_xmit;
            public ushort iframe_recv_err;
            public ushort xmit_aborts;
            public uint xmit_success;
            public uint recv_success;
            public ushort iframe_xmit_err;
            public ushort recv_buff_unavail;
            public ushort t1_timeouts;
            public ushort ti_timeouts;
            public uint reserved1;
            public ushort free_ncbs;
            public ushort max_cfg_ncbs;
            public ushort max_ncbs;
            public ushort xmit_buf_unavail;
            public ushort max_dgram_size;
            public ushort pending_sess;
            public ushort max_cfg_sess;
            public ushort max_sess;
            public ushort max_sess_pkt_size;
            public ushort name_count;
        }

        public struct ASTAT
        {
            // Fields
            public HardwareInfo.ADAPTER_STATUS adapt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public HardwareInfo.NAME_BUFFER[] NameBuff;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LANA_ENUM
        {
            public byte length;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xfe)]
            public byte[] lana;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NAME_BUFFER
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] name;
            public byte name_num;
            public byte name_flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCB
        {
            public byte ncb_command;
            public byte ncb_retcode;
            public byte ncb_lsn;
            public byte ncb_num;
            public IntPtr ncb_buffer;
            public ushort ncb_length;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ncb_callname;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ncb_name;
            public byte ncb_rto;
            public byte ncb_sto;
            public IntPtr ncb_post;
            public byte ncb_lana_num;
            public byte ncb_cmd_cplt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] ncb_reserve;
            public IntPtr ncb_event;
        }

        public enum NCBCONST
        {
            MAX_LANA = 0xfe,
            NCBASTAT = 0x33,
            NCBENUM = 0x37,
            NCBNAMSZ = 0x10,
            NCBRESET = 50,
            NRC_GOODRET = 0,
            NUM_NAMEBUF = 30
        }

        public class Win32API
        {
            // Methods
            [DllImport("NETAPI32.DLL")]
            public static extern char Netbios(ref HardwareInfo.NCB ncb);
        }
        #endregion
    }
}