namespace Prana.Interfaces
{
    public interface INewsServer
    {
        //by default is gives all symbols news and  number of HL/day are immaterial....thou esignal ask for them...
        void GetNewsHeadLines(int iNumDays, int iNumHeadLines);

        void GetNewsStory(int Locator1, int Locator2);

        event System.EventHandler NewsHeadLineData;

        event System.EventHandler NewsStoryData;

    }
}
