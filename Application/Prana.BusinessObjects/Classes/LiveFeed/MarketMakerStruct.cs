namespace Prana.BusinessObjects.LiveFeed
{
    /// <summary>
    /// Structure which keeps level2 individual data for bid/ask
    /// </summary>
    /// 
    public struct MarketMakerRecord
    {
        public string MMID;
        public double lPrice;
        //		public char cBase;
        public long lSize;
        public string tTime; //DateTime
        public char updateFlag;
        //		char cOpen;
        //		char cPrimary;
        //		int  iColor;
        //		int  iSortValue;
        //		BOOL bBB;//true for bulletin board
        //		BOOL bOMDF;// true for OMDF
        //		BOOL bUQDF;// tru for UQDF;
    };

}
