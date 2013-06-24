using System;
using System.Text;
using Parrot.Models;

namespace Parrot
{
    /// <summary>
    /// 封装交通局通讯协议中的上行数据包。
    /// </summary>
    public static class UploadDataWrapper
    {
        /// <summary>
        /// 数据长度。
        /// </summary>
        public const int MaxDataLength = 2048;
        /// <summary>
        /// 数据包总长度。
        /// </summary>
        /// <seealso cref="MaxDataLength"/>
        public const int MaxPduSize = MaxDataLength + 16;

        /// <summary>
        /// 封装U05（上传“驾驶员上报数据包”给交通局）。
        /// </summary>
        /// <remarks>直接转发来自MDT（GPS终端）的数据。</remarks>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        public static byte[] U05(int clientId, string plateNumber, byte plateColor, byte isOnDriving, string driverLicenseNumber, string driverName, DateTime occurTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 封装U04（上传“事故疑点信息数据包”给交通局）。
        /// </summary>
        /// <remarks>直接转发来自MDT（GPS终端）的数据。</remarks>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        public static byte[] U04(int clientId, string plateNumber, byte plateColor, byte packetIndex, byte[] occurTimeBytes, byte[] trafficData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 封装U03（上传“OMC代码及车辆静态信息数据包”给交通局）。
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="mdt"></param>
        /// <returns></returns>
        public static byte[] U03(int clientId, CarList mdt)
        {
            byte[] body = new byte[MaxPduSize];
            Array.Clear(body, 0, body.Length);

            int bodyLength = 0;
            try
            {
                int n = int.Parse(mdt.DB44_EnterpriseCode);//字段1

                body[0] = (byte)((n & 0xff000000) >> 24);
                body[1] = (byte)((n & 0xff0000) >> 16);
                body[2] = (byte)((n & 0xff00) >> 8);
                body[3] = (byte)((n & 0xff));
            }
            catch { }
            finally
            {
                bodyLength += 4;
            }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = Encoding.Default.GetBytes(mdt.DB44_MDT_Type);//字段2
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {

                int n = int.Parse(mdt.DB44_CompanyCode);//字段3
                body[bodyLength] = (byte)((n & 0xff00) >> 8);
                body[bodyLength + 1] = (byte)((n & 0xff));

                //n = Int32.Parse(mdt.Mobile_SN);
                n = (int)MdtIdHelper.ParseMdtCode(mdt.Mobile_SN);
                body[bodyLength + 2] = (byte)((n & 0xff000000) >> 24);
                body[bodyLength + 3] = (byte)((n & 0xff0000) >> 16);
                body[bodyLength + 4] = (byte)((n & 0xff00) >> 8);
                body[bodyLength + 5] = (byte)((n & 0xff));
            }
            catch { }
            finally
            {
                bodyLength += 6;
            }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = Encoding.Default.GetBytes(mdt.Mobile_VehicleRegistration);
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {

                byte n = 0;
                switch (mdt.DB44_VehicleRegistrationColor)
                {
                    //case "蓝":
                    case "0": n = 1; break;
                    //case "黄":
                    case "1": n = 2; break;
                    //case "白":
                    case "2": n = 3; break;
                    //case "黑":
                    case "3": n = 4; break;
                    default: n = 0; break;
                }
                body[bodyLength] = n;
            }
            catch { }
            finally
            {
                bodyLength += 1;
            }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = Encoding.Default.GetBytes(mdt.DB44_VehicleRegistration_Type);
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = Encoding.Default.GetBytes(mdt.DB44_VehicleType);
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte n = (byte)(byte.Parse(mdt.DB44_VehicleUseType) + 1);
                body[bodyLength] = n;
            }
            catch { }
            finally
            {
                bodyLength += 1;
            }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = Encoding.Default.GetBytes(mdt.DB44_VehicleGroupCode);
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = Encoding.Default.GetBytes(mdt.DB44_VehicleGroupName);
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = Encoding.Default.GetBytes(mdt.DB44_VehicleGroupCYZGZ);
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            Array.Resize<byte>(ref body, bodyLength);
            return Wrap(clientId, "U03", body);
        }

        /// <summary>
        /// 封装U02（上传“图片数据包”给交通局）。
        /// <remarks>直接转发来自MDT（GPS终端）的数据。</remarks>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="plateNumber"></param>
        /// <param name="plateColor"></param>
        /// <param name="captureTime"></param>
        /// <param name="cameraNumber"></param>
        /// <param name="ImageFormatName"></param>
        /// <param name="packetTotal"></param>
        /// <param name="cameraNumber"></param>
        /// <param name="imageData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        public static byte[] U02(int clientId, string plateNumber, byte plateColor, DateTime captureTime, byte cameraNumber, string fileExtensionName, int packetTotal, int packetIndex, byte[] imageData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 封装U01(上报“卫星定位数据包”给交通局)。
        /// </summary>
        /// <seealso cref="Db44GpsData"/>
        /// <param name="clientId">企业监控平台代码</param>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="plateColor">车牌颜色</param>
        /// <param name="gpsData">卫星定位数据</param>
        /// <exception cref="ArgumentException"/>
        public static byte[] U01(int clientId, string plateNumber, byte plateColor, Db44GpsData gpsData)
        {
            if (string.IsNullOrEmpty(plateNumber))
                throw new ArgumentException("车牌号不能为空。", "plateNumber");
            if (gpsData == null)
                throw new ArgumentNullException("gpsData", "卫星定位数据包不能为空。");

            byte[] body = new byte[MaxPduSize];
            Array.Clear(body, 0, body.Length);

            int bodyLength = 0;

            try
            {
                byte[] t = Encoding.Default.GetBytes(plateNumber);
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                body[bodyLength] = plateColor;
            }
            catch { }
            finally
            {
                bodyLength += 1;
            }

            body[bodyLength] = (byte)'|';
            bodyLength += 1;

            try
            {
                byte[] t = gpsData.Data;
                t.CopyTo(body, bodyLength);
                bodyLength += t.Length;
            }
            catch { }

            Array.Resize<byte>(ref body, bodyLength);
            return Wrap(clientId, "U01", body);
        }


        /// <summary>
        /// 封装T01（上传“链路检测申请”给交通局）。
        /// </summary>
        /// <param name="clientId">企业监控平台代码</param>
        public static byte[] T01(int clientId)
        {
            return Wrap(clientId, "T01", null);
        }

        /// <summary>
        /// 封装L01（上传“登录请求”给交通局）。
        /// </summary>
        /// <param name="clientId">企业监控平台代码</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="salt">随机序列</param>
        /// <exception cref="ArgumentException"/>
        public static byte[] L01(int clientId, string username, string password, string salt)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("用户名不能为空。", "username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("密码不能为空。", "password");
            if (string.IsNullOrEmpty(salt)) throw new ArgumentException("随机序列不能为空。", "salt");

            string saltedPassword = getMd5Hash(username + password + salt);
            string s = string.Format("{0}|{1}|{2}", username, saltedPassword, salt);
            byte[] body = Encoding.Default.GetBytes(s);

            return Wrap(clientId, "L01", body);
        }
        /// <summary>
        /// 按照交通局通讯协议的MD5杂凑算法计算。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        private static string getMd5Hash(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("输入字符串不能为空。", "value");

            byte[] buffer = System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 将数据按交通局的数据协议封装成上行数据包。
        /// <remarks>依赖配置文件中的“企业监控平台代码(<see cref="ClientId"/>)”。</remarks>
        /// </summary>
        /// <param name="clientId">企业监控平台代码</param>
        /// <param name="functionCode">功能关键字。如"U01"</param>
        /// <param name="bodyWithFunctionCode">数据体，如<see cref="Db44GpsData"/>；允许为空，如T01协议体为空。</param>
        /// <returns>数据包</returns>
        /// <exception cref="ArgumentException"/>
        private static byte[] Wrap(int clientId, string functionCode, byte[] body)
        {
            if (string.IsNullOrEmpty(functionCode)) throw new ArgumentException("功能关键字应为3字节长度的字符串。", "functionCode");
            if (functionCode.Length != 3) throw new ArgumentException("功能关键字应为3字节长度的字符串。", "functionCode");

            int length = 0;
            if (body != null)
            {
                length = body.Length;
            }
            byte[] array = new byte[16 + length];
            array[0] = (byte)'~';//0x7e
            Encoding.ASCII.GetBytes(functionCode).CopyTo(array, 1);
            array[4] = (byte)'&';//0x26;
            array[5] = (byte)((clientId & 0xff000000) >> 24);
            array[6] = (byte)((clientId & 0xff0000) >> 16);
            array[7] = (byte)((clientId & 0xff00) >> 8);
            array[8] = (byte)((clientId & 0xff));
            array[9] = (byte)'&';//0x26;
            array[10] = (byte)((length & 0xff000000) >> 24);
            array[11] = (byte)((length & 0xff0000) >> 16);
            array[12] = (byte)((length & 0xff00) >> 8);
            array[13] = (byte)((length & 0xff));
            array[14] = (byte)'&';//0x26;

            if (length > 0)
            {
                body.CopyTo(array, 15);
            }
            array[array.Length - 1] = (byte)'#';//0x23;
            return array;
        }

    }
}
