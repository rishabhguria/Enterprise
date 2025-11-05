using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;


namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class StateList : SortableSearchableList<State>
    {
        public static SortableSearchableList<State> Retrieve
        {
            get { return RetrieveStateList(); }
        }

        private static SortableSearchableList<State> RetrieveStateList()
        {
            SortableSearchableList<State> stateList = DataSourceManager.GetStateList();
            
            stateList.Insert(0, new State(0, "--Select--", 0));
            return stateList;
        }

       



    }
}
