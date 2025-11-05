using Prana.BusinessObjects.Classes.ThirdParty;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ThirdPartyBatchExtensions : ThirdPartyCommon
    {


        private ThirdPartyFtp _ftp;
        private ThirdPartyGnuPG _gnuPG;
        private ThirdPartyUserDefinedFormat format;
        //private ThirdPartyDataFomatter formatter;
        private string _ThirdPartyName;
        private string _ThirdPartyTypeName;
        private string _FtpName;
        private string _GnuPGName;
        private string _EmailDataName;
        private string _EmailLogName;


        /// <summary>
        /// Gets or sets the name of the FTP.
        /// </summary>
        /// <value>The name of the FTP.</value>
        /// <remarks></remarks>
        public string FtpName
        {
            get { return _FtpName; }
            set { _FtpName = value; }
        }


        /// <summary>
        /// Gets or sets the name of the gnu PG.
        /// </summary>
        /// <value>The name of the gnu PG.</value>
        /// <remarks></remarks>
        public string GnuPGName
        {
            get { return _GnuPGName; }
            set { _GnuPGName = value; }
        }


        /// <summary>
        /// Gets or sets the email data file.
        /// </summary>
        /// <value>The email data file.</value>
        /// <remarks></remarks>
        public string EmailDataName
        {
            get { return _EmailDataName; }
            set { _EmailDataName = value; }
        }


        /// <summary>
        /// Gets or sets the email log file.
        /// </summary>
        /// <value>The email log file.</value>
        /// <remarks></remarks>
        public string EmailLogName
        {
            get { return _EmailLogName; }
            set { _EmailLogName = value; }
        }


        private string _FileFormatName;

        /// <summary>
        /// All Data
        /// </summary>
        private string serializedDataSource;
        /// <summary>
        /// Filtered Data
        /// </summary>
        private string serializedDataSet;


        /// <summary>
        /// Gets or sets the name of the third party type.
        /// </summary>
        /// <value>The name of the third party type.</value>
        /// <remarks></remarks>
        [Browsable(true)]
        public string ThirdPartyTypeName
        {
            get { return _ThirdPartyTypeName; }
            set { _ThirdPartyTypeName = value; }
        }
        /// <summary>
        /// Gets or sets the name of the third party.
        /// </summary>
        /// <value>The name of the third party.</value>
        /// <remarks></remarks>
        [Browsable(true)]
        public string ThirdPartyName
        {
            get { return _ThirdPartyName; }
            set { _ThirdPartyName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the file format.
        /// </summary>
        /// <value>The name of the file format.</value>
        /// <remarks></remarks>
        [Browsable(true)]
        public string FileFormatName
        {
            get { return _FileFormatName; }
            set { _FileFormatName = value; }
        }
        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        /// <remarks></remarks>

        /// <summary>
        /// Gets or sets the gnu PG.
        /// </summary>
        /// <value>The gnu PG.</value>
        /// <remarks></remarks>        
        [Browsable(false)]
        public ThirdPartyGnuPG GnuPG
        {
            get { return _gnuPG; }
            set { _gnuPG = value; }
        }

        public ThirdPartyEmail EmailData;

        public ThirdPartyEmail EmailLog;

        /// <summary>
        /// Gets or sets the FTP.
        /// </summary>
        /// <value>The FTP.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        public ThirdPartyFtp Ftp
        {
            get { return _ftp; }
            set { _ftp = value; }
        }

        [Browsable(false)]
        public string SerializedDataSource
        {
            get { return serializedDataSource; }
            set { serializedDataSource = value; }
        }
        /// <summary>
        /// Gets or sets the data set.
        /// </summary>
        /// <value>The data set.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        public string SerializedDataSet
        {
            get { return serializedDataSet; }
            set { serializedDataSet = value; }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        public ThirdPartyUserDefinedFormat Format
        {
            get { return format; }
            set { format = value; }
        }

		/// <summary>
		/// Gets or sets the order details organized by a dictionary of string keys.
		/// </summary>
        [Browsable(false)]
        public SerializableDictionary<string, List<ThirdPartyOrderDetail>> OrderDetail
        {
            get; set;
        }

        ///// <summary>
        ///// Gets or sets the formatter.
        ///// </summary>
        ///// <value>The formatter.</value>
        ///// <remarks></remarks>
        //[Browsable(false)]
        //public ThirdPartyDataFomatter Formatter
        //{
        //    get { return formatter; }
        //    set { formatter = value; }
        //}
    }
}
