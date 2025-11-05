using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class MappingItemList:SortableSearchableList<MappingItem>
    {
        public MappingItemList()
        { 
        }

        /// <summary>
        /// Retrieves this Mapping Items.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        /// <returns></returns>
        //public static SortableSearchableList<MappingItem> Retrieve(DataSourceNameID dataSourceID)
        //{
        //    return GetAllMappingItems(dataSourceID.ID);
        //}

        public static SortableSearchableList<MappingItem> RetrieveColumnMappings(DataSourceNameID dataSourceNameID)
        {
            SortableSearchableList<MappingItem> mappingItemList = new SortableSearchableList<MappingItem>();

            if (int.Equals(dataSourceNameID.ID, 1))
            {
                MappingItem mappingItem1 = new MappingItem();
                mappingItem1.SourceItemName = "Fund Name";
                mappingItem1.ApplicationItemName = "Account Name";
                mappingItem1.Lock = true;
                mappingItemList.Add(mappingItem1);

                mappingItem1 = new MappingItem();
                mappingItem1.SourceItemName = "Trading Acc";
                mappingItem1.ApplicationItemName = "None";
                mappingItem1.Lock = false;
                mappingItemList.Add(mappingItem1);
            }
            return mappingItemList;
        }

        /// <summary>
        /// Retrieves the fund mappings.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        /// <returns></returns>
        public static SortableSearchableList<MappingItem> RetrieveFundMappings(DataSourceNameID dataSourceNameID)
        {
            SortableSearchableList<MappingItem> mappingItemList = new SortableSearchableList<MappingItem>();

            if (int.Equals(dataSourceNameID.ID, 1))
            {
                mappingItemList.Add(new MappingItem("Fund 1", "Fund X"));
                mappingItemList.Add(new MappingItem("Fund 2", "Fund Y"));
                mappingItemList.Add(new MappingItem("Fund 3", "Fund Z"));

            }
            return mappingItemList;
        }
        /// <summary>
        /// Retrieves the asset mappings.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        /// <returns></returns>
        public static SortableSearchableList<MappingItem> RetrieveAssetMappings(DataSourceNameID dataSourceNameID)
        {
            SortableSearchableList<MappingItem> mappingItemList = new SortableSearchableList<MappingItem>();

            if (int.Equals(dataSourceNameID.ID, 1))
            {
                mappingItemList.Add(new MappingItem("Equ", "Equities"));
                mappingItemList.Add(new MappingItem("Fut", "Futures"));
                mappingItemList.Add(new MappingItem("Opt", "Options"));
                
            }
            return mappingItemList;
        }

        /// <summary>
        /// Retrieves the underlying mappings.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        /// <returns></returns>
        public static SortableSearchableList<MappingItem> RetrieveUnderlyingMappings(DataSourceNameID dataSourceNameID)
        {
            SortableSearchableList<MappingItem> mappingItemList = new SortableSearchableList<MappingItem>();

            if (int.Equals(dataSourceNameID.ID, 1))
            {
                mappingItemList.Add(new MappingItem("US Equitiy Options", "US Options"));
                mappingItemList.Add(new MappingItem("US Fut", "US Futures"));
                mappingItemList.Add(new MappingItem("UK Equitiy Opt", "UK Options"));

            }
            return mappingItemList;
        }

        /// <summary>
        /// Retrieves the exchange mappings.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        /// <returns></returns>
        public static SortableSearchableList<MappingItem> RetrieveExchangeMappings(DataSourceNameID dataSourceNameID)
        {
            SortableSearchableList<MappingItem> mappingItemList = new SortableSearchableList<MappingItem>();

            if (int.Equals(dataSourceNameID.ID, 1))
            {
                mappingItemList.Add(new MappingItem("tse", "TSE", "Toronto Stock Exchange"));
                mappingItemList.Add(new MappingItem("lse", "LSE", "London Stock Exchange"));
                mappingItemList.Add(new MappingItem("nyse", "NYSE", "New York Stock Exchange"));

            }
            return mappingItemList;
        }

        /// <summary>
        /// Retrieves the currency mappings.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        /// <returns></returns>
        public static SortableSearchableList<MappingItem> RetrieveCurrencyMappings(DataSourceNameID dataSourceNameID)
        {
            SortableSearchableList<MappingItem> mappingItemList = new SortableSearchableList<MappingItem>();

            if (int.Equals(dataSourceNameID.ID, 1))
            {
                mappingItemList.Add(new MappingItem("Us", "USD","US Dollor"));
                mappingItemList.Add(new MappingItem("Yn", "Yen", "Japanese Yen"));
                mappingItemList.Add(new MappingItem("Gb", "GBP", "Great Britain Pound"));
                
            }
            return mappingItemList;
        }
    }
}
