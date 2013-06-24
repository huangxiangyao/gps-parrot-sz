using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Parrot.Models;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace Parrot.Models.DummyGprs
{
    public class DummyGprsIn
    {
        // Fields
        private string mdbname;
        private string mdbserver;
        private string password;
        private ArrayList SqlList;
        private int SqlListID;
        private string strSqlConnect;
        private string tempID;
        private MdtWrapper tempmobileInfo;
        public Timer Timer_Pool = new Timer();
        private string userid;

        // Events
        public event GpsDataReturnEventHandler RecivGpsDataReturn;

        // Methods
        public DummyGprsIn()
        {
            this.Timer_Pool.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);
        }

        private void _GPSDataIn(string _ID, int _Type, byte[] _Body, ref MdtWrapper mobileInfo)
        {
            string[] strArray = Encoding.Default.GetString(_Body, 0, _Body.Length).Split(new char[] { ',' });
            switch (strArray[2])
            {
                case "W1":
                    {
                        this.Timer_Pool.Enabled = false;
                        this.SqlListID = 0;
                        this.tempmobileInfo = mobileInfo;
                        this.tempID = _ID;
                        string sQL = Encoding.Default.GetString(Convert.FromBase64String(strArray[3]));
                        this.GetAllMobileInfo(sQL);
                        string s = "##1," + this.SqlList.Count.ToString() + ",\r\n";
                        s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
                        this.RecivGpsDataReturn(this.tempID, s,  this.tempmobileInfo, 0);
                        return;
                    }
                case "W2":
                    if (this.SqlList.Count > 0)
                    {
                        this.Timer_Pool.Interval = int.Parse(strArray[3]);
                        this.Timer_Pool.Enabled = true;
                    }
                    return;

                case "W3":
                    if (this.SqlList.Count > 0)
                    {
                        string str5 = this.SqlList[int.Parse(strArray[3])].ToString();
                        str5 = Convert.ToBase64String(Encoding.Default.GetBytes(str5));
                        string[] strArray2 = new string[] { "##2,", this.SqlList.Count.ToString(), ",", (int.Parse(strArray[3]) + 1).ToString(), ",", str5, ",\r\n" };
                        str5 = string.Concat(strArray2);
                        this.SqlListID++;
                        str5 = Convert.ToBase64String(Encoding.Default.GetBytes(str5));
                        this.RecivGpsDataReturn(this.tempID, str5,  this.tempmobileInfo, 0);
                    }
                    break;

                case "W4":
                    this.Timer_Pool.Enabled = false;
                    break;
            }
        }

        public void DataIn(string _ID, int _Type, ref byte[] body, int ByteLen, ref MdtWrapper mobileInfo)
        {
            if (body[0] == 0x2a)
            {
                this._GPSDataIn(_ID, _Type, body, ref mobileInfo);
            }
        }

        public void GetAllMobileInfo(string SQL)
        {
            this.SqlList = new ArrayList();
            using (SqlConnection connection = new SqlConnection(this.strSqlConnect))
            {
                SqlCommand command = new SqlCommand(SQL, connection);
                command.CommandType = CommandType.Text;
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Debug.Write("Begin GetAllMobileInfo\r\n");
                while (reader.Read())
                {
                    int fieldCount = reader.FieldCount;
                    string str = "";
                    for (int i = 0; i < fieldCount; i++)
                    {
                        string str2 = reader[i].ToString();
                        if (i == 0)
                        {
                            str = str2;
                        }
                        else
                        {
                            str = str + "^^" + str2;
                        }
                    }
                    this.SqlList.Add(str);
                }
                Debug.Write("End GetAllMobileInfo " + this.SqlList.Count.ToString() + "\r\n");
            }
        }

        public void Inti()
        {
            this.LoginTXT();
            this.strSqlConnect = "";
            this.strSqlConnect = "server=" + this.mdbserver + ";uid=" + this.userid + ";pwd=" + this.password + ";database=" + this.mdbname + ";max pool size=10;min pool size=5";
        }

        private void LoginTXT()
        {
            string path = @"login\\login.ini";
            try
            {
                string str2;
                StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
            Label_0014:
                str2 = reader.ReadLine();
                if (str2 == null)
                {
                    goto Label_00C6;
                }
                int index = str2.IndexOf("=");
                if (index > 0)
                {
                    string str3 = str2.Substring(index + 1).Trim();
                    switch (str2.Substring(0, index))
                    {
                        case "userid":
                            this.userid = str3;
                            goto Label_00C0;

                        case "password":
                            this.password = str3;
                            goto Label_00C0;

                        case "mdbname":
                            this.mdbname = str3;
                            break;

                        case "mdbserver":
                            this.mdbserver = str3;
                            break;
                    }
                }
            Label_00C0:
                if (str2 != null)
                {
                    goto Label_0014;
                }
            Label_00C6:
                reader.Close();
            }
            catch
            {
            }
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            string s = "";
            if (this.SqlListID >= this.SqlList.Count)
            {
                this.Timer_Pool.Interval = 500.0;
                s = "##3," + this.SqlList.Count.ToString() + ",\r\n";
                s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
            }
            else
            {
                s = this.SqlList[this.SqlListID].ToString();
                s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
                string[] strArray = new string[] { "##2,", this.SqlList.Count.ToString(), ",", (this.SqlListID + 1).ToString(), ",", s, ",\r\n" };
                s = string.Concat(strArray);
                this.SqlListID++;
                s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
            }
            this.RecivGpsDataReturn(this.tempID, s,  this.tempmobileInfo, 0);
        }
    }


}
