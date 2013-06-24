using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot;

namespace Parrot.Models.Db44
{
    public class Db44Wrapper
    {
        public static SimpleCycleCodeGenerator CycleCode = new SimpleCycleCodeGenerator();

        #region 必选协议（00-06）
        /// <summary>
        /// 查询当前位置。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D01(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D01";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }


        /// <summary>
        /// 定时监控设置。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="interval">定位数据采集时间间隔（秒）。</param>
        /// <param name="times">在每个时间间隔内发送的次数。</param>
        /// <param name="numberOfPackets">每次发送的定位数据包数。当该值为0xffff时，无包数限制；当该值为0时，关闭定时监控。</param>
        /// <returns></returns>
        public static string D02(uint mdtId, int mdtModel, byte interval, byte times, ushort numberOfPackets)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D02";

            byte[] body = new byte[1 + 1+2];
            int i = 0;
            body[i++] = interval;
            body[i++] = times;
            body[i++] = (byte)((numberOfPackets & 0xff00) >> 8);
            body[i++] = (byte)((numberOfPackets & 0xff));

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        /// <summary>
        /// 定时监控查看。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D03(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D03";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        /// <summary>
        /// 定距监控设置。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="distanceInterval">距离间隔(米)。有效值：50~9999</param>
        /// <param name="times">在每个时间间隔内发送的次数。</param>
        /// <param name="numberOfPackets">每次发送的定位数据包数。当该值为0xffff时，无包数限制；当该值为0时，关闭定时监控。</param>
        /// <returns></returns>
        public static string D04(uint mdtId, int mdtModel, ushort distanceInterval, byte times, byte numberOfPackets)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D04";

            byte[] body = new byte[2 + 1+2];
            int i = 0;
            body[i++] = (byte)((distanceInterval & 0xff00) >> 8);
            body[i++] = (byte)((distanceInterval & 0xff));
            body[i++] = times;
            body[i++] = (byte)((numberOfPackets & 0xff00) >> 8);
            body[i++] = (byte)((numberOfPackets & 0xff));

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        /// <summary>
        /// 定距监控查看。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D05(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D05";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        
        /// <summary>
        /// 速度监控设置。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="speedLimit">限定速度（0～255）km/h，特别地，0表示关闭速度监控。</param>
        /// <param name="duration">持续时间（1～255）秒。</param>
        /// <returns></returns>
        public static string D06(uint mdtId, int mdtModel, byte speedLimit, byte duration)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D06";

            byte[] body = new byte[1 + 1];
            int i = 0;
            body[i++] = speedLimit;
            body[i++] = duration;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        #endregion
        #region 可选协议（07-0e），仅部分支持。
        /// <summary>
        /// 区域设定（封闭区域）。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="areaList">封闭区域列表。</param>
        /// <returns></returns>
        public static string D07(uint mdtId, int mdtModel, List<GpsClosedArea> areaList)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D07";

            byte[] body = new byte[1];
            int i=0;
            byte areaListCount = (byte)areaList.Count;
            body[i++] = areaListCount;
            for (byte j = 0; j < areaListCount; j++)
            {
                byte areaListElementCount = (byte)areaList[j].Amount;

                Array.Resize<byte>(ref body, body.Length + (1 + 1 + 8 * areaListElementCount));

                body[i++] = areaList[j].Index;
                body[i++] = areaListElementCount;
                for (byte k = 0; k < areaListElementCount; k++)
                {
                    string s = areaList[j].GpsPositions[k].Longitude.ToString("00000.000");
                    body[i++] = byte.Parse(s.Substring(0, 2));
                    body[i++] = byte.Parse(s.Substring(2, 2));
                    body[i++] = byte.Parse(s.Substring(4, 2));
                    body[i++] = byte.Parse(s.Substring(6, 2));
                    s = areaList[j].GpsPositions[k].Latitude.ToString("00000.000");
                    body[i++] = byte.Parse(s.Substring(0, 2));
                    body[i++] = byte.Parse(s.Substring(2, 2));
                    body[i++] = byte.Parse(s.Substring(4, 2));
                    body[i++] = byte.Parse(s.Substring(6, 2));
                }
            }

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }

        /// <summary>
        /// 查询封闭区域设定。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="areaNumberList">想要查询的区域编号列表。</param>
        /// <returns></returns>
        public static string D08(uint mdtId, int mdtModel, List<byte> areaNumberList)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D08";
            
            byte amount = (byte)areaNumberList.Count;
            byte[] body = new byte[1+amount];
            body[0] = amount;
            areaNumberList.CopyTo(body, 1);

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        
        /// <summary>
        /// 区域监控。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="areaNumber">区域编号。</param>
        /// <param name="speedLimit">限速（km/h）。</param>
        /// <param name="limitDuration">限速持续时间（秒）。</param>
        /// <param name="restrictInOut">禁止入/出。若为0则解除该禁令；1禁入；2禁出；3禁入禁出。</param>
        /// <returns></returns>
        public static string D09(uint mdtId, int mdtModel, byte areaNumber, byte speedLimit, byte limitDuration, byte restrictInOut, byte restrictDuration)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D09";

            byte[] body = new byte[5];
            int i = 0;
            body[i++] = areaNumber;
            body[i++] = speedLimit;
            body[i++] = limitDuration;
            body[i++] = restrictInOut;
            body[i++] = restrictDuration;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }


        /// <summary>
        /// 查看区域监控设定。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="areaNumberList">想要查询的区域编号列表。</param>
        /// <returns></returns>
        public static string D0a(uint mdtId, int mdtModel, List<byte> areaNumberList)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D0a";

            byte amount = (byte)areaNumberList.Count;
            byte[] body = new byte[1 + amount];
            body[0] = amount;
            areaNumberList.CopyTo(body, 1);

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }

        /// <summary>
        /// 道路设定。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D0b(uint mdtId, int mdtModel)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 查看道路设定。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D0c(uint mdtId, int mdtModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 道路监控。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D0d(uint mdtId, int mdtModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查看道路监控。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D0e(uint mdtId, int mdtModel)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 必选协议（0F-1D）
        /// <summary>
        /// 下达设备自检指令。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="cmd">
        /// 0x55: 下次启动时上报启动信息；
        /// 0x66: 下次启动时定位上报启动信息；
        /// 0x77：下次启动时不上报启动信息。
        /// </param>
        /// <returns></returns>
        public static string D0f(uint mdtId, int mdtModel, byte cmd)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D0f";

            byte[] body = new byte[1];
            int i = 0;
            body[i++] = cmd;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }

        /// <summary>
        /// 查询事故终点数据。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="stopIndex">停车序号（0～10）。当该值为0时，按最近时间查询。</param>
        /// <param name="stoppedTime">停车时间（无需年月日，时分秒即足够）。</param>
        /// <returns></returns>
        public static string D10(uint mdtId, int mdtModel, byte stopIndex, DateTime stoppedTime)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D10";

            byte[] body = new byte[1+3];
            int i = 0;
            body[i++] = stopIndex;
            body[i++] = NumberConverter.ToBcd((byte)stoppedTime.Hour);
            body[i++] = NumberConverter.ToBcd((byte)stoppedTime.Minute);
            body[i++] = NumberConverter.ToBcd((byte)stoppedTime.Second);

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        /// <summary>
        /// 查询历史轨迹。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="startTime">起始时间（无需年分秒，月日时即足够）。</param>
        /// <returns></returns>
        public static string D11(uint mdtId, int mdtModel, DateTime startTime, byte numberOfPosition)
        {

            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D11";

            byte[] body = new byte[3+1];
            int i = 0;
            body[i++] = NumberConverter.ToBcd((byte)startTime.Month);
            body[i++] = NumberConverter.ToBcd((byte)startTime.Day);
            body[i++] = NumberConverter.ToBcd((byte)startTime.Hour);
            body[i++] = numberOfPosition;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }

        /// <summary>
        /// 查询驾驶员身份。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D12(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D12";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        public static string D13(uint mdtId, int mdtModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置疲劳驾驶时限。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="maxDrivingDuration">最大允许连续驾驶的时间长度。单位：5分钟。</param>
        /// <param name="restDuration">休息时间。单位：分钟。</param>
        /// <returns></returns>
        public static string D14(uint mdtId, int mdtModel, byte maxDrivingDuration, byte restDuration)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D14";

            byte[] body = new byte[1 + 1];
            int i = 0;
            body[i++] = maxDrivingDuration;
            body[i++] = restDuration;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        /// <summary>
        /// 下发文本信息。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="text">GB2312编码的字符串。</param>
        /// <returns></returns>
        public static string D15(uint mdtId, int mdtModel, string text)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D15";
                        
            byte[] body = Encoding.Default.GetBytes(text);

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D16(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D16";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }


        /// <summary>
        /// 应答紧急报警。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D17(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D17";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        /// <summary>
        /// 解除紧急报警。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D18(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D18";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        /// <summary>
        /// 远程断油控制。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="off">true 断油; false 恢复油路。</param>
        /// <param name="expireTime">过期时间。在指定的时间之前执行指令，否则不执行。</param>
        /// <returns></returns>
        public static string D19(uint mdtId, int mdtModel, bool off, DateTime expireTime)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);
            
            string functionCode = "D19";

            byte[] body = new byte[1+6];
            int i=0;
            body[i++] = (byte)(off ? 1 : 0);
            body[i++] = NumberConverter.ToBcd((byte)(expireTime.Year - 2000));
            body[i++] = NumberConverter.ToBcd((byte)(expireTime.Month));
            body[i++] = NumberConverter.ToBcd((byte)(expireTime.Day));
            body[i++] = NumberConverter.ToBcd((byte)(expireTime.Hour));
            body[i++] = NumberConverter.ToBcd((byte)(expireTime.Minute));
            body[i++] = NumberConverter.ToBcd((byte)(expireTime.Second));

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }

        /// <summary>
        /// 查询MDT基本参数。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="mdtBasicParamName">MDT基本参数类型。如2，表示车牌号码。</param>
        /// <returns></returns>
        public static string D1a(uint mdtId, int mdtModel, byte mdtBasicParamName)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D1a";

            byte[] body = new byte[1];
            int i = 0;
            body[i++] = (byte)(mdtBasicParamName);

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }

        /// <summary>
        /// MDT基本参数设置（远程参数设置）。详见“MDT基本参数表”。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="mdtBasicParamName">MDT基本参数类型。如2，表示车牌号码。</param>
        /// <param name="mdtBasicParamValue">MDT基本参数。具体长度根据参数类型而定。不允许为null，否则将抛出异常(<see cref="ArgumentNullException"/>)。。</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public static string D1b(uint mdtId, int mdtModel, byte mdtBasicParamName, byte[] mdtBasicParamValue)
        {
            if (mdtBasicParamValue == null) throw new ArgumentNullException();

            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D1b";

            byte[] body = new byte[1+mdtBasicParamValue.Length];
            body[0] = mdtBasicParamName;
            mdtBasicParamValue.CopyTo(body, 1);

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        
        /// <summary>
        /// 复位。
        /// </summary>
        /// <remarks>终端收到指令后必须应答，之后，1分钟内重启。</remarks>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <param name="reset">整机复位。</param>
        /// <param name="restoreToFactorySettings">恢复出厂设置。</param>
        /// <returns></returns>
        public static string D1c(uint mdtId, int mdtModel, bool reset, bool restoreToFactorySettings)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D1c";

            byte[] body = new byte[1];
            body[0] = (byte)(reset ? 0 : 1);
            if (restoreToFactorySettings)
            {
                body[0] |= 0x02;
            }

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        
        /// <summary>
        /// 要求MDT报告密钥。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string D1d(uint mdtId, int mdtModel)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "D1d";

            byte[] body = null;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        #endregion
        #region 自定义协议

        /// <summary>
        /// 要求MDT重传监控图像。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="mdtModel"></param>
        /// <returns></returns>
        public static string Df3(uint mdtId, int mdtModel, byte picNumber, byte packetIndex)
        {
            byte sequenceNumber = CycleCode.NextTrace();
            Db44ClientAccount account = Db44ClientAccountRepository.Default.GetAccount(mdtModel);

            string functionCode = "Df3";

            byte[] body = new byte[1 + 1];
            body[0] = picNumber;
            body[1] = packetIndex;

            Db44Packet pdu = new Db44Packet(sequenceNumber, account.MdtManufacturerCode, mdtId, account.OmcId, account.Pin, functionCode, body);
            return Db44WrapperHelper.Pack(mdtModel, pdu);
        }
        #endregion
    }
}
