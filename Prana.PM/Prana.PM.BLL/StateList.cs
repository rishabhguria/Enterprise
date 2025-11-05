using System;
using System.Collections.Generic;
using System.Text;
//
using Prana.BusinessObjects;
using Prana.PM.DAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.PM.BLL
{
    public class StateList : SortableSearchableList<State>
    {
        public static SortableSearchableList<State> Retrieve
        {
            get 
                {
                    SortableSearchableList<State> stateList = new SortableSearchableList<State>();

                    try
                    {
                        stateList = RetrieveStateList();
                    }
                    catch (Exception ex)
                    {

                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                    return stateList;
                }
        }

        /// <summary>
        /// Retrieves the state list.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<State> RetrieveStateList()
        {
            SortableSearchableList<State> stateList = new SortableSearchableList<State>();
            try
            {
                stateList = DataSourceManager.GetStateList();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            stateList.Insert(0, new State(0, Prana.Global.ApplicationConstants.C_COMBO_SELECT, 0));
            return stateList;
        }

       



    }
}
