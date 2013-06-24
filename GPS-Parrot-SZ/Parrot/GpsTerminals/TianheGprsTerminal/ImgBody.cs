using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Parrot.Models.TianheGprs
{
    public class ImgBody
    {
        // Fields
        private string _ImgNo;
        public bool _ReciveEnd;
        public string CarID;
        public ArrayList ImgPacklist;
        public DateTime LastReciveTime;
        public int PackLen;
        public bool ReciveEnd;
        public bool ReciveOver;
        public int VcdID;

        // Methods
        public void AddNewImgPack(int _PackID, string ImgPack)
        {
            this.ImgPacklist[_PackID] = ImgPack;
        }

        public void NewImg()
        {
            this.CarID = "";
            this._ImgNo = "";
            this.VcdID = 0;
            this.PackLen = 0;
            this.ImgPacklist = new ArrayList(0x40);
            this.ReciveEnd = false;
            this.ReciveOver = false;
            for (int i = 0; i < 0x40; i++)
            {
                this.ImgPacklist.Add("");
            }
            this.LastReciveTime = DateTime.Now;
            this._ReciveEnd = false;
        }

        // Properties
        public string ImgNo
        {
            get
            {
                return this._ImgNo;
            }
            set
            {
                this._ImgNo = value;
            }
        }
    }
}