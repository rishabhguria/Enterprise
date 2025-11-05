using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class ConsolidationSummary
    {
        private double _pnlLongTotal;

        public double PNLLongTotal
        {
            get { return _pnlLongTotal; }
            set { _pnlLongTotal = value; }
        }

        private double _pnlShortTotal;

        public double PNLShortTotal
        {
            get { return _pnlShortTotal; }
            set { _pnlShortTotal = value; }
        }

        private double _pnlNetTotal;

        public double PNLNetTotal
        {
            get { return _pnlNetTotal; }
            set { _pnlNetTotal = value; }
        }

        private double _longExposureTotal;

        public double LongExposureTotal
        {
            get { return _longExposureTotal; }
            set { _longExposureTotal = value; }
        }

        private double _shortExposureTotal;

        public double ShortExposureTotal
        {
            get { return _shortExposureTotal; }
            set { _shortExposureTotal = value; }
        }

        private double _netExposureTotal;

        public double NetExposureTotal
        {
            get { return _netExposureTotal; }
            set { _netExposureTotal = value; }
        }

        private double _cashInflow;

        public double CashInflow
        {
            get { return _cashInflow; }
            set { _cashInflow = value; }
        }

        private double _cashOutflow;

        public double CashOutflow
        {
            get { return _cashOutflow; }
            set { _cashOutflow = value; }
        }

        private BindingList<ConsolidatedInfo> _consolidatedInfoList;

        public BindingList<ConsolidatedInfo> ConsolidatedInfoList
        {
            get { return _consolidatedInfoList; }
            set { _consolidatedInfoList = value; }
        }
	
    }
}
