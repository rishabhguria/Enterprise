namespace Prana.Interfaces
{
    public interface ICacheManager
    {
        event System.EventHandler UpdateLevel2Data;
        event System.EventHandler UpdateLevel1Data;
        /// <summary>
        /// Consumer module will get the data in form of EventArgs. They need to recast 
        /// the EventArg to LiveFeedEventArgs and access Level1SnapshotData property of EventArg
        /// </summary>
        event System.EventHandler Level1SnapshotResponseToConsumer;
        event System.EventHandler SnapShotLevel1Data;
        event System.EventHandler NewsHeadLineDataFromCache;
        event System.EventHandler NewsStoryDataFromCache;

        void AddLevel2Symbol(string Symbol, string Identifier);
        void RemoveLevel2Symbol(string Symbol, string Identifier);
        void RemoveAllLevel2Symbols();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="Exchange"></param>
        /// <param name="AssetType"></param>
        void AddLevel1Symbol(string Symbol); //, string Identifier);

        /// <summary>
        /// A comsumer module call this method to fetch the snapshot of current Level1 market data.
        /// Corresponding to this function call 'Level1SnapshotResponseToConsumer' event is generated, 
        /// to respond the data.
        /// </summary>
        /// <param name="symbol"></param>
        void GetLevel1SnapShot(string symbol); // ,Exchange,AssetType
        void RemoveLevel1Symbol(string Symbol); //, string Identifier);

        void RemoveAllLevel1Symbols();

        //by default is gives all symbols news and  number of HL/day are immaterial....thou esignal ask for them...

        void GetNewsHeadLines(int iNumDays, int iNumHeadLines);

        void GetNewsStory(int Locator1, int Locator2);
    }
}
