using System;
using System.Collections.Generic;
using System.Text;
using Prana.Utilities.MiscUtilities;
namespace Prana.BasketTrading
{
    public class BasketTemplate
    {
        private string _templateID = string.Empty;
        private string _templateName = string.Empty;
        private List<string> _columns =new List<string>();
        //private int _assetID=int.MinValue;
       // private int _underlyingID=int.MinValue;
        private TemplateConvention _templateConvention;
        private TemplateExchange _templateExchange;
        private TemplateOrderSide _templateOrderSide;
        private bool _isDefaultTemplate = false;

        public void SetColumns(string columnInCommaList)
        {
            _columns= GeneralUtilities.GetListFromString(columnInCommaList,',');
            
        }
        public string  GetColumns()
        {
            return GeneralUtilities.GetStringFromList(_columns, ',');
        }
        public string TemplateID
        {
            set { _templateID = value; }
            get { return _templateID; }
        }
        public string TemplateName
        {
            set { _templateName = value; }
            get { return _templateName; }
        }
        public List< string> Columns
        {
            set { _columns = value; }
            get { return _columns; }
        }
        
        //public int AssetID
        //{
        //    set { _assetID = value; }
        //    get { return _assetID; }
        //}
        //public int UnderLyingID
        //{
        //    set { _underlyingID = value; }
        //    get { return _underlyingID; }
        //}
        public TemplateConvention TemplateConvention
        {
            set { _templateConvention = value; }
            get { return _templateConvention; }
        }
        public TemplateExchange TemplateExchange
        {
            set { _templateExchange = value; }
            get { return _templateExchange; }
        }
        public TemplateOrderSide TemplateOrderSide
        {
            set { _templateOrderSide = value; }
            get { return _templateOrderSide; }
        }
        public string  ConventionMappingID
        {
            set { _templateConvention.ConventionMappingID = value; }
            get { return _templateConvention.ConventionMappingID; }
        }
        public string TemplateExchangeID
        {
            set { _templateExchange.TemplateExchangeID = value; }
            get { return _templateExchange.TemplateExchangeID; }
        }
        public bool  IsDefaultTemplate
        {
            set { _isDefaultTemplate = value; }
            get { return _isDefaultTemplate; }
        }
        //public string  OrderSideMappingID
        //{
        //    set { _templateOrderSide.OrderSideMappingID = value; }
        //    get { return _templateOrderSide.OrderSideMappingID; }
        //}
    }
}
