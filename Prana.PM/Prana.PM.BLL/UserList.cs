using Csla;
using System;


namespace Prana.PM.BLL
{
    [Serializable()]
    public class UserList : BusinessListBase<UserList, User>
    {
        /// <summary>
        /// Gets the index by user ID.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public int GetIndexByUserID(int userID)
        {
            int index = int.MinValue;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].CompanyUserID == userID)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}

