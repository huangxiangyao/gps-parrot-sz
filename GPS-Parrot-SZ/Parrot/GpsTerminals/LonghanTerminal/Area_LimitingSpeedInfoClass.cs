using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Parrot.Models.Longhan
{
    public class Area_LimitingSpeedInfoClass
    {
        // Fields
        public static Hashtable Area_Limiting_FJZZ_HashList;
        public static Hashtable Area_LimitingSpeed_HashList;
        public byte AreaAlarmType;
        public byte AreaID;
        public int LocaAreaID;
        public double MaxLat;
        public double MaxLong;
        public byte MaxSpeed;
        public double MinLat;
        public double MinLong;

        // Methods
        public Area_LimitingSpeedInfoClass()
        {
            if (Area_LimitingSpeed_HashList == null)
            {
                Area_LimitingSpeed_HashList = new Hashtable();
            }
            if (Area_Limiting_FJZZ_HashList == null)
            {
                Area_Limiting_FJZZ_HashList = new Hashtable();
            }
        }

        public void Inti(double X1, double Y1, double X2, double Y2, byte _AreaID, byte _AlarmType, byte _MaxSpeed)
        {
            this.MinLong = X1;
            this.MinLat = Y1;
            this.MaxLong = X2;
            this.MaxLat = Y2;
            this.AreaID = _AreaID;
            this.AreaAlarmType = _AlarmType;
            this.MaxSpeed = _MaxSpeed;
            this.LocaAreaID = -1;
        }
    }
}