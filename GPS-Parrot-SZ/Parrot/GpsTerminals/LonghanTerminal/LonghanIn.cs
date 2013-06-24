using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Parrot.Models;

namespace Parrot.Models.Longhan
{
    public class LonghanIn
    {
        // Fields
        private static Hashtable ImgHashList;
        private LH101G_GPRS_PE_In LH101G_In = new LH101G_GPRS_PE_In();
        private LH108_GPRS_PE_In LH108_In = new LH108_GPRS_PE_In();
        private LH12_GPRS_PE_In LH12_In = new LH12_GPRS_PE_In();
        private LH14_GPRS_PE_In LH14_In = new LH14_GPRS_PE_In();
        private LH16_GPRS_PE_In LH16_In = new LH16_GPRS_PE_In();
        private LH208_GPRS_PE_In LH208_In = new LH208_GPRS_PE_In();
        private LHA6_GPRS_PE_In LHA6GS_In = new LHA6_GPRS_PE_In();
        private int LogID;
        private byte[] ReturnCmdByte;
        
        public event PlainMessageReceivedEventHandler PlainMessageReceived;

        public event PlainGpsDataReceivedEventHandler PlainGpsDataReceived;

        public event GpsDataReturnEventHandler GpsDataReturn;

        public event ImageReceivedEventHandler ImageReceived;


        // Methods
        public LonghanIn()
        {
            this.LH12_In.PlainGpsDataReceived += new PlainGpsDataReceivedEventHandler(this.FirePlainGpsDataReceivedEvent);
            this.LH12_In.PlainMessageReceived += new PlainMessageReceivedEventHandler(this.FirePlainMessageReceivedEvent);
            this.LH12_In.GpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            this.LH14_In.PlainGpsDataReceived += new PlainGpsDataReceivedEventHandler(this.FirePlainGpsDataReceivedEvent);
            //this.LH14_In.Alarm += new GpsAlarmReceivedEventHandler(this.Fire_RecivAlarmEvenEvent);
            this.LH14_In.PlainMessageReceived += new PlainMessageReceivedEventHandler(this.FirePlainMessageReceivedEvent);
            this.LH14_In.GpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            this.LH14_In.ImageReceived += new ImageReceivedEventHandler(this.FireImageReceivedEvent);
            this.LH16_In.RecivGpsData += new PlainGpsDataReceivedEventHandler(this.FirePlainGpsDataReceivedEvent);
            //this.LH16_In.RecivAlarmEven += new GpsAlarmReceivedEventHandler(this.Fire_RecivAlarmEvenEvent);
            this.LH16_In.RecivEven += new PlainMessageReceivedEventHandler(this.FirePlainMessageReceivedEvent);
            this.LH16_In.RecivGpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            this.LH16_In.RecivImgEven += new ImageReceivedEventHandler(this.FireImageReceivedEvent);
            //this.LH16_In.RecivTaxiOprationInfo += new TaxiOprationInfoReceivedEventHandler(this.RecivTaxiOprationInfo);
            this.LHA6GS_In.RecivGpsData += new PlainGpsDataReceivedEventHandler(this.FirePlainGpsDataReceivedEvent);
            //this.LHA6GS_In.RecivAlarmEven += new GpsAlarmReceivedEventHandler(this.Fire_RecivAlarmEvenEvent);
            this.LHA6GS_In.RecivEven += new PlainMessageReceivedEventHandler(this.FirePlainMessageReceivedEvent);
            this.LHA6GS_In.RecivGpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            this.LHA6GS_In.RecivImgEven += new ImageReceivedEventHandler(this.FireImageReceivedEvent);
            //this.LHA6GS_In.RecivTaxiOprationInfo += new TaxiOprationInfoReceivedEventHandler(this.RecivTaxiOprationInfo);
            this.LH108_In.RecivGpsData += new PlainGpsDataReceivedEventHandler(this.FirePlainGpsDataReceivedEvent);
            this.LH108_In.RecivEven += new PlainMessageReceivedEventHandler(this.FirePlainMessageReceivedEvent);
            this.LH108_In.RecivGpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            this.LH208_In.RecivGpsData += new PlainGpsDataReceivedEventHandler(this.FirePlainGpsDataReceivedEvent);
            //this.LH208_In.RecivAlarmEven += new GpsAlarmReceivedEventHandler(this.Fire_RecivAlarmEvenEvent);
            this.LH208_In.RecivEven += new PlainMessageReceivedEventHandler(this.FirePlainMessageReceivedEvent);
            this.LH208_In.RecivGpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            this.LH208_In.RecivImgEven += new ImageReceivedEventHandler(this.FireImageReceivedEvent);
            //this.LH208_In.RecivTaxiOprationInfo += new TaxiOprationInfoReceivedEventHandler(this.RecivTaxiOprationInfo);
            this.LH101G_In.RecivGpsData += new PlainGpsDataReceivedEventHandler(this.FirePlainGpsDataReceivedEvent);
            this.LH101G_In.RecivEven += new PlainMessageReceivedEventHandler(this.FirePlainMessageReceivedEvent);
            this.LH101G_In.RecivGpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            ImgHashList = new Hashtable();
            this.ReturnCmdByte = new byte[10];
            this.ReturnCmdByte[0] = 0x29;
            this.ReturnCmdByte[1] = 0x29;
            this.ReturnCmdByte[2] = 0x21;
            this.ReturnCmdByte[3] = 0;
            this.ReturnCmdByte[4] = 5;
            this.ReturnCmdByte[7] = 0;
            this.ReturnCmdByte[9] = 13;
        }


        private void AutoSendCmdToMobile(string _ID, string ReCmd,  MdtWrapper mobileInfo, byte ProtocolType)
        {
            this.GpsDataReturn(_ID, ReCmd,  mobileInfo, ProtocolType);
        }

        public void DataIn(string _ID, int _Type,  byte[] body, int ByteLen, MdtWrapper mobileInfo)
        {
            if (_Type < 210)
            {
                for (int i = 0; i < ByteLen; i++)
                {
                    try
                    {
                        if ((body[i] == 0x29) & (body[i + 1] == 0x29))
                        {
                            int num2 = (body[i + 3] * 0x100) + body[i + 4];
                            byte[] buffer = Convert.FromBase64String(Convert.ToBase64String(body, i, 5 + num2));
                            this.GPSDataIn_(_ID, _Type, buffer, buffer.Length, ref mobileInfo);
                            i = (i + num2) + 4;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else if (_Type == 0xd3)
            {
                this.LH101G_In.GprsDataIn_(_ID, _Type, body, ref mobileInfo);
            }
        }

        private void GprsDataIn_(string _ID, int MobileType, byte[] body, ref MdtWrapper mobileInfo)
        {
            this.LogID++;
            if (this.LogID > 0xf4240)
            {
                this.LogID = 0;
            }
            switch (MobileType)
            {
                case 0xc9:
                    this.LH12_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xca:
                    this.LH14_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xcb:
                    this.LH16_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xcd:
                    this.LH108_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xce:
                    this.LHA6GS_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xcf:
                    this.LH108_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xd0:
                    this.LH16_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xd1:
                    this.LH208_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 0xd3:
                    this.LH101G_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;

                case 230:
                    this.LH12_In.GprsDataIn_(_ID, MobileType, body, ref mobileInfo);
                    break;
            }
        }

        private void GPSDataIn_(string _ID, int _Type, byte[] body, int ByteLen, ref MdtWrapper mobileInfo)
        {
            for (int i = 0; i < ByteLen; i++)
            {
                if ((body[i] == 0x29) & (body[i + 1] == 0x29))
                {
                    int num2 = (body[i + 3] * 0x100) + body[i + 4];
                    string s = Convert.ToBase64String(body, i, 5 + num2);
                    this.GprsDataIn_(_ID, _Type, Convert.FromBase64String(s), ref mobileInfo);
                    i = (i + num2) + 4;
                }
            }
        }
        
        private void FirePlainMessageReceivedEvent(string _ID, int OperationType, string Operation, string OperationDescribe,  MdtWrapper mobileInfo)
        {
            this.PlainMessageReceived(_ID, OperationType, Operation, OperationDescribe,  mobileInfo);
        }

        private void FirePlainGpsDataReceivedEvent(string _ID,  byte[] GpsPackByte,  MdtWrapper mobileInfo)
        {
            this.PlainGpsDataReceived(_ID,  GpsPackByte,    mobileInfo);
        }

        public void FireImageReceivedEvent(string _ID, int MointerID, long ImageID, int ImageLen, int ImageSeq, string ImageBody, string ImgStatu, DateTime FilmDateTime,  MdtWrapper mobileInfo)
        {
            this.ImageReceived(_ID, MointerID, ImageID, ImageLen, ImageSeq, ImageBody, ImgStatu, FilmDateTime,  mobileInfo);
        }
    }
}