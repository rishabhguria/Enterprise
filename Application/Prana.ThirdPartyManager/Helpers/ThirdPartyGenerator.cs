using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;
using System.IO;


namespace Prana.ThirdPartyManager.Helper
{
    /// <summary>
    /// Third Party Generator
    /// </summary>
    /// <remarks></remarks>
    public class ThirdPartyGenerator
    {
        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="@event">The @event.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        /// <remarks></remarks>
        static private void RaiseEvent<T>(EventHandler<T> @event, object sender, T e) where T : EventArgs
        {
            EventHandler<T> handler = @event;
            handler(sender, e);

        }
        /// <summary>
        /// Raises the <see cref="E:Message"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Prana.ThirdPartyReport.Classes.MessageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        public virtual void OnMessage(MessageEventArgs e)
        {
            if (Message != null)
                RaiseEvent<MessageEventArgs>(Message, this, e);
        }

        /// <summary>
        /// Message Event Handler
        /// </summary>
        public EventHandler<MessageEventArgs> Message;


        /// <summary>
        /// Generates the XML.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataSet GenerateXML(ThirdPartyBatch batch, ThirdPartyFlatFileDetailCollection details, ThirdPartyFileFormat format)
        {
            string mappedfilePath = string.Empty;
            DataSet _dsXML = new DataSet();
            try
            {
                if (details == null) return _dsXML;

                if (format.PranaToThirdParty.Equals(""))
                {
                    OnMessage(new MessageEventArgs("No XSLT is available for the selected ThirdParty. Please contact the administrator!"));
                    return _dsXML;
                }

                string xsltPath = format.PranaToThirdParty;
                string xsltName = xsltPath.Substring(xsltPath.LastIndexOf("\\") + 1);

                string xsltStartUpPath;
                if (batch.TransmissionType == ((int)ApplicationConstants.TransmissionType.FIX).ToString())
                {
                    if (!format.GenerateCancelNewForAmend)
                        xsltStartUpPath = String.Format(@"{0}\{1}",
                        Directory.GetCurrentDirectory(), "FFGetThirdPartyFundsDetails_FIX.xslt");
                    else
                        xsltStartUpPath = String.Format(@"{0}\{1}",
                        Directory.GetCurrentDirectory(), "FFGetThirdPartyFundsDetails_FIX_CancelNew.xslt");
                }
                else
                    xsltStartUpPath = String.Format(@"{0}\{1}\{2}\{3}",
                    Directory.GetCurrentDirectory(), ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, xsltName);

                string str = Directory.GetCurrentDirectory() + @"\SerializedThirdPartyFlatFileXml.xml";
                string strFilePathNew = Directory.GetCurrentDirectory() + @"\ConvertedThirdPartyNew.xml";
                
                FileInfo fileInfo = new FileInfo(xsltStartUpPath);

                if (fileInfo.Exists)
                {
                    mappedfilePath = XMLHelper.GetTransformed(str, strFilePathNew, xsltStartUpPath, details);
                }
                else
                {
                    OnMessage(new MessageEventArgs("No XSLT available for this Third party, Please contact to admin"));
                    return _dsXML;
                }


                if (!mappedfilePath.Equals(""))
                {
                    _dsXML.ReadXml(mappedfilePath);

                    RepositionPrimaryColumn(_dsXML);
                }
                else
                {
                    OnMessage(new MessageEventArgs("No Data available for the selected Third Party CashAccounts."));

                    return _dsXML;
                }
                return _dsXML;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                OnMessage(new MessageEventArgs("Problem in loading data, Please contact to admin"));
                return _dsXML;
            }
        }

        private void RepositionPrimaryColumn(DataSet ds)
        {
            try
            {
                if (ds != null && (ds.Tables.Count == 2))
                {
                    if (ds.Tables[0].Columns.Contains("Group_Id"))
                    {
                        ds.Tables[0].Columns["Group_Id"].SetOrdinal(0);
                    }

                    if (ds.Tables[1].Columns.Contains("Group_Id"))
                    {
                        ds.Tables[1].Columns["Group_Id"].SetOrdinal(0);
                    }
                }
            }
            catch (Exception)
            {
                OnMessage(new MessageEventArgs("Problem in setting the position of column: Group_Id, Please contact to admin"));
            }
        }
    }
}
