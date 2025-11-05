
using System;
using Prana.Interfaces;

using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Reflection;
using Prana.ClientCommon;
using Prana.BusinessObjects;
namespace Prana.BasketTrading
{
   
    [XmlRoot("BasketPreferences")]
    public class BasketPreferences : IPreferenceData
    {

        #region Private  Variables
        
        private PreferencesUniversalSettingsCollection _prefsTrading = null;
        private RowProperties _rowProperties;
       // private int _templateID = int.MinValue;
        #endregion

        public BasketPreferences()
        {
            _rowProperties = new RowProperties();
        }
        public BasketPreferences(string data)
        {
            _rowProperties = new RowProperties();
            string[] dataArray = data.Split('@');
            _rowProperties.ClrCancelBack = int.Parse(dataArray[0]);
            _rowProperties.ClrCancelFore = int.Parse(dataArray[1]);
            _rowProperties.ClrDFDBack = int.Parse(dataArray[2]);
            _rowProperties.ClrDFDFore = int.Parse(dataArray[3]);
            _rowProperties.ClrErrorRowsBack = int.Parse(dataArray[4]);
            _rowProperties.ClrErrorRowsFore = int.Parse(dataArray[5]);
            _rowProperties.ClrFilledBack  = int.Parse(dataArray[6]);
            _rowProperties.ClrFilledFore  = int.Parse(dataArray[7]);
            _rowProperties.ClrNewBack = int.Parse(dataArray[8]);
            _rowProperties.ClrNewFore  = int.Parse(dataArray[9]);
            _rowProperties.ClrNotTradedBack = int.Parse(dataArray[10]);
            _rowProperties.ClrNotTradedFore = int.Parse(dataArray[11]);
            _rowProperties.ClrPendingBack  = int.Parse(dataArray[12]);
            _rowProperties.ClrPendingCXLBack = int.Parse(dataArray[13]);
            _rowProperties.ClrPendingCXLFore  = int.Parse(dataArray[14]);
            _rowProperties.ClrPendingFore  = int.Parse(dataArray[15]);
            _rowProperties.ClrPFBack = int.Parse(dataArray[16]);
            _rowProperties.ClrPFFore  = int.Parse(dataArray[17]);
            _rowProperties.ClrRejectedBack  = int.Parse(dataArray[18]);
            _rowProperties.ClrRejectedFore  = int.Parse(dataArray[19]);
            _rowProperties.ClrSelectedRowsBack  = int.Parse(dataArray[20]);
            _rowProperties.ClrSelectedRowsFore  = int.Parse(dataArray[21]);
            //_templateID = int.Parse(dataArray[22]);

        }
        #region Properties
       
        public RowProperties RowProperties
        {
            get { return _rowProperties; }
            set { _rowProperties = value; }

        }
        
         [XmlIgnore]
        public PreferencesUniversalSettingsCollection PrefsTrading
        {
            get { return _prefsTrading; }
            set { _prefsTrading = value; }
        }
        //public int TemplateID
        //{
        //    set { _templateID = value; }
        //    get { return _templateID; }
        //}
        #endregion

        public string Serialize()
        {

            string serializedString = _rowProperties.ClrCancelBack.ToString() + "@" + _rowProperties.ClrCancelFore.ToString() + "@" +
                                      _rowProperties.ClrDFDBack.ToString() + "@" + _rowProperties.ClrDFDFore.ToString() + "@" +
                                      _rowProperties.ClrErrorRowsBack.ToString() + "@" + _rowProperties.ClrErrorRowsFore.ToString() +"@"+
                                      _rowProperties.ClrFilledBack.ToString() + "@" + _rowProperties.ClrFilledFore.ToString() + "@" +
                                      _rowProperties.ClrNewBack.ToString() + "@" + _rowProperties.ClrNewFore.ToString() + "@" +
                                      _rowProperties.ClrNotTradedBack.ToString() + "@" + _rowProperties.ClrNotTradedFore.ToString() + "@" +
                                      _rowProperties.ClrPendingBack.ToString() + "@" + _rowProperties.ClrPendingCXLBack.ToString() + "@" +
                                      _rowProperties.ClrPendingCXLFore.ToString() + "@" + _rowProperties.ClrPendingFore.ToString() + "@" +
                                      _rowProperties.ClrPFBack.ToString() + "@" + _rowProperties.ClrPFFore.ToString() + "@" +
                                      _rowProperties.ClrRejectedBack.ToString() + "@" + _rowProperties.ClrRejectedFore.ToString() + "@" +
                                      _rowProperties.ClrSelectedRowsBack.ToString() + "@" + _rowProperties.ClrSelectedRowsFore.ToString();
            return serializedString;

        }
         
    }
    
    public class RowProperties
    {

        /// <remarks/>
        [XmlAttribute("ClrFilledBack")]
        public int ClrFilledBack = Color.LawnGreen.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrFilledFore")]
        public int ClrFilledFore = Color.Black.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrPFBack")]
        public int ClrPFBack = Color.LightSkyBlue.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrPFFore")]
        public int ClrPFFore = Color.Black.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrNewBack")]
        public int ClrNewBack = Color.Ivory.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrNewFore")]
        public int ClrNewFore = Color.Black.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrRejectedBack")]
        public int ClrRejectedBack = Color.Tomato.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrRejectedFore")]
        public int ClrRejectedFore = Color.Black.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrDFDBack")]
        public int ClrDFDBack = Color.LightBlue.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrDFDFore")]
        public int ClrDFDFore = Color.Black.ToArgb();
        //----------
        /// <remarks/>
        [XmlAttribute("ClrCancelBack")]
        public int ClrCancelBack = Color.LightPink.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrCancelFore")]
        public int ClrCancelFore = Color.Black.ToArgb();
        /// <remarks/>
        [XmlAttribute("ClrPendingBack")]
        public int ClrPendingBack = Color.LightSalmon.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrPendingFore")]
        public int ClrPendingFore = Color.Black.ToArgb();
        /// <remarks/>
        [XmlAttribute("ClrPendingCXLBack")]
        public int ClrPendingCXLBack = Color.LightYellow.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrPendingCXLFore")]
        public int ClrPendingCXLFore = Color.Black.ToArgb();
        //----------------

        /// <remarks/>
        [XmlAttribute("ClrSelectedRowsBack")]
        public int ClrSelectedRowsBack = Color.FromArgb(192, 192, 255).ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrSelectedRowsFore")]
        public int ClrSelectedRowsFore = Color.Black.ToArgb(); 
        
        /// <remarks/>
        [XmlAttribute("ClrErrorRowsBack")]
        public int ClrErrorRowsBack = Color.FromArgb(255, 128, 128).ToArgb(); 
        
        /// <remarks/>
        [XmlAttribute("ClrErrorRowsFore")]
        public int ClrErrorRowsFore = Color.Black.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrNotTradedBack")]
        public int ClrNotTradedBack = Color.Ivory.ToArgb();

        /// <remarks/>
        [XmlAttribute("ClrNotTradedFore")]
        public int ClrNotTradedFore = Color.Black.ToArgb();



    }

}