using System;
using Timer = System.Timers.Timer;
using System.Timers;
using Parrot.Models;
using System.Collections.Generic;

namespace Parrot
{
    public class DatabaseProcessor
    {
        // Fields
        public event MdtListUpdatedEventHandler MdtListUpdated;
        public event LoggingEventHandler Logging;

        private Timer TimerUpdate = new Timer();
        private string OperatorUsername;
        private string OperatorPassword;

        // Methods
        public DatabaseProcessor(string operatorUsername, string operatorPassword)
        {
            this.OperatorUsername = operatorUsername;
            this.OperatorPassword = operatorPassword;

            this.TimerUpdate.Elapsed += new ElapsedEventHandler(TimerUpdate_Elapsed);
            this.TimerUpdate.Interval = 240000;
            this.TimerUpdate.Enabled = false;
        }

        public void Start()
        {
            this.TimerUpdate.Start();
            TimerUpdate_Elapsed(this, null);
            FireLoggingEvent(Level.Info, "启动定时任务：每4分钟刷新一次GPS终端列表。");
        }

        void TimerUpdate_Elapsed(object sender, ElapsedEventArgs e)
        {
            FireLoggingEvent(Level.Info, "执行定时任务：刷新GPS终端列表。");
            this.GetAllMobileInfo();
        }
        
        public void GetAllMobileInfo()
        {
            try
            {
                string filterValues = "";
                int filterModel = 0; //0: by ConsumerIds, 1: by Ids, others: fetch all
                UserInfo operatorUserInfo = ParrotModelWrapper.GetUserInfo(OperatorUsername, OperatorPassword);
                if (operatorUserInfo == null) return;

                switch (operatorUserInfo.User_WatchRangeType.Trim())
                {
                    case "0": filterModel = 0; break;
                    case "1": filterModel = 1; break;
                    default: filterModel = 2; break;
                }

                filterValues = operatorUserInfo.User_RangerList.Trim();
                filterValues = filterValues.TrimEnd(new char[] { ',' });
                if (filterValues == "") return;

                List<MobileInfoList> mobiles = null;
                switch (filterModel)
                {
                    case 0:
                        mobiles = ParrotModelWrapper.GetMdtsByConsumerIds(filterValues);
                        break;
                    case 1:
                        mobiles = ParrotModelWrapper.GetMdtsByIds(filterValues);
                        break;
                    default:
                        mobiles = ParrotModelWrapper.GetAllMdts();
                        break;
                }
                if (mobiles == null) return;

                int nNewMdts = 0;
                foreach (MobileInfoList mobile in mobiles)
                {
                    lock (SmppAgent.MobileInfo_Hash)
                    {
                        MdtWrapper info = (MdtWrapper)SmppAgent.MobileInfo_Hash[mobile.Mobile_ID];
                        if (info == null)
                        {
                            info = new MdtWrapper();
                            info.Init(mobile.Mobile_ID, mobile.Mobile_Sim, mobile.id);
                            SmppAgent.MobileInfo_Hash.Add(mobile.Mobile_ID, info);
                            nNewMdts++;
                        }
                        info.UpdateByCarListTable();
                        info.UpdateByMobileInfoList();

                        if (MdtListUpdated != null)
                        {
                            MdtListUpdated(this, info);
                        }
                    }
                }
                if (nNewMdts > 0)
                {
                    FireLoggingEvent(Level.Info, string.Format("在GPS终端列表中新增{0}个。", nNewMdts));
                }
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, "数据库操作失败，无法读取GPS终端资料！请检查数据库配置。");
                FireLoggingEvent(Level.Advanced, ex);
            }
        }

        private void FireLoggingEvent(Level level, object message)
        {
            if (Logging != null)
                Logging(this, level, message);
        }
    }
}
