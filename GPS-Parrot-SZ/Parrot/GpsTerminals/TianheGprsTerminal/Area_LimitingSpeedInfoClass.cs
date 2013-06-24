using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Parrot.Models.TianheGprs
{
    public class Area_LimitingSpeedInfoClass
    {
        // Fields
        public byte AlarmType;
        public static Hashtable Area_LimitingSpeed_HashList;
        public int AreaID;
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
        }

        public void Inti(double X1, double Y1, double X2, double Y2, byte _AreaID, byte _AlarmType, byte _MaxSpeed)
        {
            this.MinLong = X1;
            this.MinLat = Y1;
            this.MaxLong = X2;
            this.MaxLat = Y2;
            this.AreaID = _AreaID;
            this.AlarmType = _AlarmType;
            this.MaxSpeed = _MaxSpeed;
        }
    }
}
