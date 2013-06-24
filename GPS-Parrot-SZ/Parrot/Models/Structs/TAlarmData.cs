using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Parrot.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TAlarmData
    {
        public bool Valid;
        public string RecvTime;
        public string MsgStr;
        public string MsgDescrible;
        public double Longitude;
        public double Latitude;
        public double Speed;
        public double Head;
        public string GpsTime;
        public string GpsState;
        public int UserStatus;
    }
}
