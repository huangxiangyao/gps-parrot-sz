using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Parrot.Models;
using Parrot.Models.Db44;

namespace Parrot.Models
{
    public class ParrotModelWrapper
    {
        public static List<MobileInfoList> GetAllMdts()
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.MobileInfoLists.ToList();
            }
        }
        public static List<MobileInfoList> GetMdtsByIds(string ids)
        {
            List<int> arr2 = new List<int>();
            foreach (string s in ids.Split(','))
            {
                try
                {
                    arr2.Add(int.Parse(s));
                }
                catch { }
            }

            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.MobileInfoLists.Where(q => arr2.Contains(q.id)).ToList();
            }
        }
        public static List<MobileInfoList> GetMdtsByConsumerIds(string consumerIds)
        {
            string[] arr = consumerIds.Split(',');

            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.MobileInfoLists.Where(q => arr.Contains(q.Mobile_Consumer_ID)).ToList();
            }
        }
        public static List<UserInfo> GetAllUsers()
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.UserInfoes.ToList();
            }
        }
        public static string GetMobileSnByPlateNumber(string plateNumber)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                CarList mdt = ctx.CarLists.FirstOrDefault(q => q.Mobile_VehicleRegistration == plateNumber);
                if (mdt != null)
                {
                    return mdt.Mobile_SN;
                }
            }
            return string.Empty;
        }
        public static CarList GetMdtByPlateNumber(string plateNumber)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.CarLists.FirstOrDefault(q => q.Mobile_VehicleRegistration==plateNumber);
            }
        }
        /// <summary>
        /// 从CarList表中取出指定记录。
        /// </summary>
        /// <param name="mobileID"></param>
        /// <returns></returns>
        public static CarList GetCarListByMobileID(string mobileID)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.CarLists.FirstOrDefault(q => q.Mobile_ID == mobileID);
            }
        }
        /// <summary>
        /// 从MobileInfoList表中取出指定记录。
        /// </summary>
        /// <param name="mobileID"></param>
        /// <returns></returns>
        public static MobileInfoList GetMobileInfoListByMobileID(string mobileID)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.MobileInfoLists.FirstOrDefault(q => q.Mobile_ID == mobileID);
            }
        }
        public static UserInfo GetUserInfo(string username, string password)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                return ctx.UserInfoes.FirstOrDefault(q => q.User_Name == username && q.User_Pwd == password);
            }
        }
        public static bool SignOn(string username, string password, out byte userType)
        {
            userType = 9;
            
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                UserInfo user = ctx.UserInfoes.FirstOrDefault(q => q.User_Name == username && q.User_Pwd == password);
                if (user != null)
                {
                    if (user.User_Type.HasValue)
                    {
                        userType = (byte)user.User_Type.Value;
                    }
                    return true;
                }
            }
            return false;
        }
        public static void SignOut(string logoffTime, string username)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                UsrLogRec u = ctx.UsrLogRecs.FirstOrDefault(q => q.LogOffTime == logoffTime && q.UsrName == username);
                if (u != null)
                    u.LogOffTime = DateTime.Now.ToString();
            }
        }
        public static int AddUsrLogRec(UsrLogRec model)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                ctx.UsrLogRecs.AddObject(model);
                return ctx.SaveChanges();
            }
        }
        public static int UpdateUsrLogRec(UsrLogRec model)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                ctx.UsrLogRecs.Attach(model);
                ctx.ObjectStateManager.ChangeObjectState(model, EntityState.Modified);
                return ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 根据车牌号码获知车牌颜色。
        /// </summary>
        /// <param name="plateNumber"></param>
        /// <returns></returns>
        internal static string GetPlateColorByPlateNumber(string plateNumber)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                CarList mobile = ctx.CarLists.FirstOrDefault(q => q.Mobile_VehicleRegistration == plateNumber);
                if (mobile != null)
                {
                    try
                    {
                        return VehiclePlateColorHelper.Default.Items[(byte)(byte.Parse(mobile.DB44_VehicleRegistrationColor) + 1)];
                    }
                    catch
                    {
                        return string.Format("({0})", mobile.DB44_VehicleRegistrationColor);
                    }
                }
                return "";
            }
        }

        public static byte GetPlateColorValueByPlateNumber(string plateNumber)
        {
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            using (ParrotEntities ctx = new ParrotEntities(cs))
            {
                CarList mobile = ctx.CarLists.FirstOrDefault(q => q.Mobile_VehicleRegistration == plateNumber);
                if (mobile != null)
                {
                    try
                    {
                        return (byte)(byte.Parse(mobile.DB44_VehicleRegistrationColor) + 1);
                    }
                    catch
                    {
                    }
                }
                return 0;
            }
        }
    }
}
