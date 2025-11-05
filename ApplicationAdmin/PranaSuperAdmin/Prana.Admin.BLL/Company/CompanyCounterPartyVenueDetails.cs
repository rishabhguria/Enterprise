using System;
using System.Collections;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCounterPartyVenueDetails.
    /// </summary>
    public class CompanyCounterPartyVenueDetails : IList
    {
        ArrayList _companyCounterPartyVenueDetails = new ArrayList();
        public CompanyCounterPartyVenueDetails()
        {
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _companyCounterPartyVenueDetails.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add Users.this getter implementation
                if (index >= _companyCounterPartyVenueDetails.Count || index < 0)
                {
                    return new CompanyCounterPartyVenueDetails();
                }
                else
                {
                    return _companyCounterPartyVenueDetails[index];
                }
            }
            set
            {
                //Add Users.this setter implementation
                _companyCounterPartyVenueDetails[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add Users.RemoveAt implementation
            _companyCounterPartyVenueDetails.RemoveAt(index);
        }

        public void Insert(int index, Object companyCounterPartyVenueDetail)
        {
            //Add Users.Insert implementation
            _companyCounterPartyVenueDetails.Insert(index, (CompanyCounterPartyVenueDetail)companyCounterPartyVenueDetail);
        }

        public void Remove(Object companyCounterPartyVenueDetail)
        {
            //Add Users.Remove implementation
            _companyCounterPartyVenueDetails.Remove((CompanyCounterPartyVenueDetail)companyCounterPartyVenueDetail);
        }

        public bool Contains(object companyCounterPartyVenueDetail)
        {
            //Add Users.Contains implementation
            return _companyCounterPartyVenueDetails.Contains((CompanyCounterPartyVenueDetail)companyCounterPartyVenueDetail);
        }

        public void Clear()
        {
            //Add Users.Clear implementation
            _companyCounterPartyVenueDetails.Clear();
        }

        public int IndexOf(object companyCounterPartyVenueDetail)
        {
            //Add Users.IndexOf implementation
            //return _companyCounterPartyVenueDetails.IndexOf((CompanyCounterPartyVenueDetails)companyCounterPartyVenueDetail);
            //return _companyCounterPartyVenueDetails.IndexOf((CompanyCounterPartyVenueDetail)companyCounterPartyVenueDetail);

            CompanyCounterPartyVenueDetail tempCompanyCounterPartyVenueDetail = (CompanyCounterPartyVenueDetail)companyCounterPartyVenueDetail;
            int counter = 0;
            int result = int.MinValue;
            foreach (CompanyCounterPartyVenueDetail _companyCounterPartyVenueDetail in _companyCounterPartyVenueDetails)
            {
                if ( //_companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID == tempCompanyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID 
                     //					 _companyCounterPartyVenueDetail.ClearingFirmPrimeBroker == tempCompanyCounterPartyVenueDetail.ClearingFirmPrimeBroker
                     //					&& _companyCounterPartyVenueDetail.CMTAGiveUPName == tempCompanyCounterPartyVenueDetail.CMTAGiveUPName
                     _companyCounterPartyVenueDetail.CompanyCounterPartyVenueID == tempCompanyCounterPartyVenueDetail.CompanyCounterPartyVenueID


                    //					&& _companyCounterPartyVenueDetail.DeliverToCompanyID == tempCompanyCounterPartyVenueDetail.DeliverToCompanyID
                    //					&& _companyCounterPartyVenueDetail.AccountName == tempCompanyCounterPartyVenueDetail.AccountName
                    //					
                    //					&& _companyCounterPartyVenueDetail.OnBehalfOfSubID == tempCompanyCounterPartyVenueDetail.OnBehalfOfSubID
                    //					&& _companyCounterPartyVenueDetail.SenderCompanyID == tempCompanyCounterPartyVenueDetail.SenderCompanyID
                    //
                    //					&& _companyCounterPartyVenueDetail.TargetCompID == tempCompanyCounterPartyVenueDetail.TargetCompID
                    //					&& _companyCounterPartyVenueDetail.ClearingFirm == tempCompanyCounterPartyVenueDetail.ClearingFirm
                    //
                    //					&& _companyCounterPartyVenueDetail.IdentifierName == tempCompanyCounterPartyVenueDetail.IdentifierName
                    //					&& _companyCounterPartyVenueDetail.CompanyCounterPartyVenueName == tempCompanyCounterPartyVenueDetail.CompanyCounterPartyVenueName
                    //
                    //					&& _companyCounterPartyVenueDetail.StrategyName == tempCompanyCounterPartyVenueDetail.StrategyName
                    //					&& _companyCounterPartyVenueDetail.MPIDName == tempCompanyCounterPartyVenueDetail.MPIDName


                    )
                {
                    result = counter;
                    break;
                }
                else
                {
                    counter++;
                }
            }
            return result;
        }

        public int Add(object companyCounterPartyVenueDetail)
        {
            //Add Users.Add implementation
            return _companyCounterPartyVenueDetails.Add((CompanyCounterPartyVenueDetail)companyCounterPartyVenueDetail);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add Users.IsFixedSize getter implementation
                return _companyCounterPartyVenueDetails.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add Users.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _companyCounterPartyVenueDetails.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _companyCounterPartyVenueDetails.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _companyCounterPartyVenueDetails.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CompanyCounterPartyVenueDetailEnumerator(this));
        }

        #endregion

        #region CompanyCounterPartyVenueDetailEnumerator Class

        public class CompanyCounterPartyVenueDetailEnumerator : IEnumerator
        {
            CompanyCounterPartyVenueDetails _companyCounterPartyVenueDetails;
            int _location;

            public CompanyCounterPartyVenueDetailEnumerator(CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails)
            {
                _companyCounterPartyVenueDetails = companyCounterPartyVenueDetails;
                _location = -1;
            }

            #region IEnumerator Members
            public void Reset()
            {
                _location = -1;
            }
            public object Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _companyCounterPartyVenueDetails.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _companyCounterPartyVenueDetails[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _companyCounterPartyVenueDetails.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            #endregion
        }

        #endregion
    }
}
