using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Parrot.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TGpsClientInfo
    {
        public string ID;
        public string CompterName;
        public string UserType;
        public string UserName;
    }
}
