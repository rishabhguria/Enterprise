using System;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    public interface IDashboard
    {
        /// <summary>
        /// Load Data from file system and show on UI
        /// </summary>
        /// <param name="Path"></param>
        void LoadData(List<String> FilesList);

        /// <summary>
        /// Archive the files and related data based of selected ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool ArchiveData(int ID);

        /// <summary>
        /// Purge/ remove Selected Id's data 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool PurgeData(int ID);

    }
}
