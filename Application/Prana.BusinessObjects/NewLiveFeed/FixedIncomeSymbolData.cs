// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Unknown
// Created          : Unknown
//
// Last Modified By : MJCarlucci
// Last Modified On : 07-02-2013
// ***********************************************************************
// <copyright file="FixedIncomeSymbolData.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.AppConstants;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace Prana.BusinessObjects
{


    /// <summary>
    /// Class FixedIncomeSymbolData
    /// </summary>
    [Serializable]
    [DataContract]
    public class FixedIncomeSymbolData : SymbolData
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedIncomeSymbolData"/> class.
        /// </summary>
        public FixedIncomeSymbolData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public FixedIncomeSymbolData(string[] data, ref int i)
            : base(data, ref i)
        {
            if (i < data.Length)
            {
                this.AccrualBasis = (AccrualBasis)Enum.Parse(typeof(AccrualBasis), data[i++].ToString(), true);
                this.AccrualBasisID = int.Parse(data[i++]);
                this.BondDescription = data[i++];
                this.BondType = (SecurityType)Enum.Parse(typeof(SecurityType), data[i++].ToString(), true);
                this.BondTypeID = int.Parse(data[i++]);
                this.Coupon = double.Parse(data[i++]);
                this.CouponFrequencyID = int.Parse(data[i++]);
                this.FirstCouponDate = DateTime.Parse(data[i++]);
                this.Frequency = (CouponFrequency)Enum.Parse(typeof(CouponFrequency), data[i++].ToString(), true);
                this.IssueDate = DateTime.Parse(data[i++]);
                this.IsZero = bool.Parse(data[i++]);
                this.MaturityDate = DateTime.Parse(data[i++]);
                this.Delta = double.Parse(data[i++]);
                this.DeltaSource = (DeltaSource)Enum.Parse(typeof(DeltaSource), data[i++].ToString(), true);
                this.SharesOutstanding = long.Parse(data[i++]);
            }
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(_splitter);
            sb.Append(this.AccrualBasis.ToString());
            sb.Append(_splitter);
            sb.Append(this.AccrualBasisID.ToString());
            sb.Append(_splitter);
            sb.Append(this.BondDescription.ToString());
            sb.Append(_splitter);
            sb.Append(this.BondType.ToString());
            sb.Append(_splitter);
            sb.Append(this.BondTypeID.ToString());
            sb.Append(_splitter);
            sb.Append(this.Coupon.ToString());
            sb.Append(_splitter);
            sb.Append(this.CouponFrequencyID.ToString());
            sb.Append(_splitter);
            sb.Append(this.FirstCouponDate.ToString());
            sb.Append(_splitter);
            sb.Append(this.Frequency.ToString());
            sb.Append(_splitter);
            sb.Append(this.IssueDate.ToString());
            sb.Append(_splitter);
            sb.Append(this.IsZero.ToString());
            sb.Append(_splitter);
            sb.Append(this.MaturityDate.ToString());
            sb.Append(_splitter);
            sb.Append(this.Delta.ToString());
            sb.Append(_splitter);
            sb.Append(this.DeltaSource.ToString());
            sb.Append(_splitter);
            sb.Append(this.SharesOutstanding.ToString());
            return sb.ToString();
        }

        private CollateralType _collateralType;
        public CollateralType CollateralType
        {
            get { return _collateralType; }
            set { _collateralType = value; }
        }

        private int _collateralTypeID;
        public int CollateralTypeID
        {
            get { return _collateralTypeID; }
            set { _collateralTypeID = value; }
        }

        private string _bondDescription = string.Empty;

        public string BondDescription
        {
            get { return _bondDescription; }
            set { _bondDescription = value; }
        }     
    }
}