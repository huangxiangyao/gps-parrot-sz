using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Parrot.Models.Longhan
{
    public class ImageTemp
    {
        // Fields
        private byte _CCD_ID;
        private DateTime _FilmDateTime;
        private string _ID;
        private byte _ImageID;
        private ArrayList _ImageTempList;
        private byte _PackAll;

        // Methods
        public void Inti(string _id, byte _imageID, byte _packAll, byte _ccd_ID)
        {
            this._ID = _id;
            this._ImageID = _imageID;
            this._PackAll = _packAll;
            this._CCD_ID = _ccd_ID;
            this._FilmDateTime = DateTime.Now;
            this._ImageTempList = new ArrayList();
            for (byte i = 0; i < _packAll; i = (byte)(i + 1))
            {
                this._ImageTempList.Add("");
            }
        }

        // Properties
        public byte CCD_ID
        {
            get
            {
                return this._CCD_ID;
            }
            set
            {
                this._CCD_ID = value;
            }
        }

        public DateTime FilmDateTime
        {
            get
            {
                return this._FilmDateTime;
            }
            set
            {
                this._FilmDateTime = value;
            }
        }

        public string ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
            }
        }

        public byte ImageID
        {
            get
            {
                return this._ImageID;
            }
            set
            {
                this._ImageID = value;
            }
        }

        public ArrayList ImageTempList
        {
            get
            {
                return this._ImageTempList;
            }
            set
            {
                this._ImageTempList = value;
            }
        }

        public byte PackAll
        {
            get
            {
                return this._PackAll;
            }
            set
            {
                this._PackAll = value;
            }
        }
    }
}