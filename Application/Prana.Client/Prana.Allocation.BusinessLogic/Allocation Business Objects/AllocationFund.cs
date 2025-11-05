using System;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Prana.Allocation.BLL

{
	/// <summary>
	/// Summary description for Allocation.
	/// </summary>
	public class AllocationFund
	{
		//string _fundAccountGroupID;

		int _fundID;
		string _fundName;
        string _groupID;
        double _orderQty;
        double _percentage;
		public AllocationFund()
		{
			

		}


        public AllocationFund(int fundAccountID, string fundname, double orderQty)
		{
			_fundID=fundAccountID;
			_fundName=fundname;
			_orderQty=orderQty;

		}
        public AllocationFund(int fundAccountID, string fundname, double orderQty,float percentage)
        {
            _fundID = fundAccountID;
            _fundName = fundname;
            _orderQty = orderQty;
            _percentage = percentage;

        }
		public AllocationFund(int fundAccountID,string fundname)
		{
			_fundID=fundAccountID;
			_fundName=fundname;
			

		}

//		

		public string FundName
		{
			set{_fundName=value;}
			get{return _fundName;}
		}
		public int FundID
		{
			set{_fundID=value;}
			get{return _fundID;}
		}

        public double Percentage
		{
			set{_percentage=value;}
			get{return _percentage;}
		}
        public double AllocatedQty
		{
			set{
                //if ((_parent != null))
                //{
                //    if (Parent.IsPreAllocated == true)
                //    {
                //        _orderQty = Parent.AllocatedQty;
                //    }
                //    else
                //    {
                //        _orderQty = value;
                //    }
                //}
                //else
                {
                    _orderQty = value;
                }
            }
			get{return _orderQty;}
		}
     
        public string GroupID
        {
            set { _groupID = value; }
            get { return _groupID; }
        }

        private double _commission = 0;
        public double Commission
        {
            set
            {
                _commission = value;
                if (_parent != null)
                {

                    _parent.RecalculateGroupCommissionFromTaxlotComm();
                }

                if (_parentBasketGroup != null)
                {

                    _parentBasketGroup.RecalculateGroupCommAndFee_FromTaxlotCommforBasket();
                }
            }
            get { return _commission; }
        }

        private double _fees = 0;
        public double Fees
        {
            set 
            { 
                _fees = value;
                if (_parent != null)
                {
                    _parent.RecalculateGroupFeesFromTaxlotComm();
                }
                if (_parentBasketGroup != null)
                {
                  
                    _parentBasketGroup.RecalculateGroupCommAndFee_FromTaxlotCommforBasket();
                }
            }
            get { return _fees; }
        }
       
        private AllocationGroup _parent;
        [XmlIgnore]
        [Browsable(false)]
        public AllocationGroup Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
      
        private AllocationOrder _parentBasketGroup;
        [XmlIgnore]
        [Browsable(false)]
        public AllocationOrder ParentBasketGroup
        {
            get { return _parentBasketGroup; }
            set { _parentBasketGroup = value; }
        }
        //private AllocationOrder _parentAllocationOrder;
        //[XmlIgnore]
        //[Browsable(false)]
        //public AllocationOrder ParentAllocationOrder
        //{
        //    get { return _parentAllocationOrder; }
        //    set { _parentAllocationOrder = value; }
        //}
       
	}
}
