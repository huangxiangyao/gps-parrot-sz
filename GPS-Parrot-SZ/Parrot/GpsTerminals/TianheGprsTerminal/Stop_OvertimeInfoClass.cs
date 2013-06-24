using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Parrot.Models.TianheGprs
{
    public class Stop_OvertimeInfoClass
    {
        // Fields
        public DateTime LastDatetime;
        public int OvertimeSet;
        private bool SendAlarm;
        public static Hashtable Stop_OvertimeInfo_HashList;
        public int StopedTimes;

        // Methods
        public Stop_OvertimeInfoClass()
        {
            if (Stop_OvertimeInfo_HashList == null)
            {
                Stop_OvertimeInfo_HashList = new Hashtable();
            }
        }

        public int CheckOvertime()
        {
            if (this.LastDatetime == DateTime.Parse("2001-01-01 00:00:00"))
            {
                this.LastDatetime = DateTime.Now;
                this.StopedTimes = 0;
                return 0;
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - this.LastDatetime);
            this.StopedTimes = span.Minutes;
            if ((this.StopedTimes > this.OvertimeSet) & !this.SendAlarm)
            {
                this.SendAlarm = true;
                return this.StopedTimes;
            }
            return 0;
        }

        public void Inti(int _OvertimeSet)
        {
            this.OvertimeSet = _OvertimeSet;
            this.LastDatetime = DateTime.Parse("2001-01-01 00:00:00");
            this.SendAlarm = false;
        }

        public int ReNew()
        {
            this.LastDatetime = DateTime.Parse("2001-01-01 00:00:00");
            if (this.SendAlarm)
            {
                this.SendAlarm = false;
                return this.StopedTimes;
            }
            return 0;
        }
    }
}