using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.AllocationNew
{
    class AllocationCtrlObject
    {
        private float  _percentage;
        public float  Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }
        private double  _qty;
        public double  Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }
        private bool _isLong;
        public bool ISLong
        {
            get { return _isLong; }
            set { _isLong = value; }
        }
        public void SetValues(int id,float percentage,double  qty,bool islong)
        {
            _id = id;
            _percentage = percentage;
            _qty =qty;
            _isLong = islong;
        }
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _groupID;

        public int GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }
	
    }
}
