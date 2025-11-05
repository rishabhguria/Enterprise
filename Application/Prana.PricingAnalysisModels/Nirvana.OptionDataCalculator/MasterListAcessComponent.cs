//using Prana.LiveFeedProvider;
using Prana.BusinessObjects.LiveFeed;
using System;
using System.Collections.Generic;
namespace Prana.OptionCalculator.CalculationComponent
{
    class MasterListAcessComponent
    {
        static private MasterListAcessComponent _masterListAcessComponent = null;
        private MasterListAcessComponent()
        {
            //eSignalOptionsManagerNew.GetInstance().MultipleSnapshotSymbolResponse += new EventHandler(MasterListAcessComponent_MultipleSnapshotSymbolResponse);

        }
        public static MasterListAcessComponent GetInstance
        {
            get
            {

                if (_masterListAcessComponent == null)
                    _masterListAcessComponent = new MasterListAcessComponent();
                return _masterListAcessComponent;
            }
        }

        public void GetSnapShotDataForSymbols(List<string> symbolList)
        {
            // eSignalOptionsManagerNew.GetInstance().RequestMultipleOptSymbolsSnapshot(symbolList);
        }

        void MasterListAcessComponent_MultipleSnapshotSymbolResponse(object sender, EventArgs e)
        {
            List<PricingModelData> optionData = sender as List<PricingModelData>;
        }



        //public OptionDataCache 

        //event for snapshot



    }
}
