using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface IExpnlService : IServiceStatus, IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task UpdateRefreshTimeInterval(int seconds);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RefreshData();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task StartDataDumper();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task StopDataDumper();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<bool> IsDataDumperEnabled();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<bool> IsDataDumperRunning();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<int> GetRefreshTimeInterval();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<TimeZoneAndTime> GetBaseTimeZoneAndBaseTimeZoneTime();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SaveBaseTimeZoneAndBaseTimeZoneTime(string timeZone, DateTime dateTime);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SaveClearanceTime(DataTable clearanceTable);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<int, DateTime>> GetDBClearanceTime();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task UpdateClearance(Dictionary<int, DateTime> dictionary);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<int, MarketTimes>> GetMarketStartEndTime();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<int, DateTime>> FetchClearanceTime();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<int, BusinessObjects.TimeZone>> GetAllAUECTimeZones();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<string> GetAUECText(int auecID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<BusinessObjects.TimeZone> GetAUECTimeZone(int auecID);
    }
}
