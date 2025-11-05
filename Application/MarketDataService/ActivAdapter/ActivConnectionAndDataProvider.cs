using ActivFinancial.ContentPlatform.ContentGatewayApi;
using ActivFinancial.ContentPlatform.ContentGatewayApi.Common;
using ActivFinancial.ContentPlatform.ContentGatewayApi.Consts;
using ActivFinancial.ContentPlatform.ContentGatewayApi.RequestParameters;
using ActivFinancial.ContentPlatform.ContributionGatewayApi.Reconnects;
using ActivFinancial.Middleware;
using ActivFinancial.Middleware.ActivBase;
using ActivFinancial.Middleware.Application;
using ActivFinancial.Middleware.FieldTypes;
using ActivFinancial.Middleware.Misc;
using ActivFinancial.Middleware.Service;
using ActivFinancial.Middleware.System;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ActivAdapter
{
    class ActivConnectionAndDataProvider : ContentGatewayClient
    {
        //Connect Parameters
        private string _serviceId, _serviceInstanceId, _userId, _password;

        // To deserialize opaque field lists.
        private FieldListValidator _fieldListValidator;

        private bool _displayResponses, _displayRecordUpdates;

        private long nextRequestId;
        private int currentUpdateCount;

        private Dictionary<string, string> _openSymbol, _fieldResult;
        public static Dictionary<string, Dictionary<string, string>> _symbolFieldResult = new Dictionary<string, Dictionary<string, string>>();

        private enum RequestMode
        {
            RequestModeAsync,
            RequestModeSync,
            RequestModeNonBlockingSync
        };

        private RequestMode _requestMode;

        private string _activLogFilePath = Path.Combine(Environment.CurrentDirectory, @"\ActivLogs.txt");

        private const int _connectRetryTimeout = 5;

        public ActivConnectionAndDataProvider(ActivApplication application, Dictionary<string, string> openSymbol, Dictionary<string, string> credentials)
            : base(application)
        {
            _requestMode = RequestMode.RequestModeAsync;
            this._openSymbol = openSymbol;

            this._serviceId = credentials["ServiceId"];
            this._userId = credentials["UserId"];
            this._password = credentials["Pwd"];
            this._serviceInstanceId = null;

            _displayResponses = true;
            _displayRecordUpdates = true;
            _fieldListValidator = new FieldListValidator(this);

            File.WriteAllText(_activLogFilePath, String.Empty);
            TextWriter tw = new StreamWriter(_activLogFilePath, true);
            System.DateTime now = System.DateTime.Now;
            tw.WriteLine("---------- Current Date and Time is---------- " + now);
            tw.Close();

            try
            {
                ConnectToCG();
            }
            catch (MiddlewareException ex)
            {
                File.AppendAllText(_activLogFilePath, "Exception" + ex.ToString() + Environment.NewLine);
            }

            try
            {
                Thread.Sleep(5000);
                GetLiveData();
            }
            catch (Exception e)
            {
                File.AppendAllText(_activLogFilePath, "Exception" + e.ToString() + Environment.NewLine);
            }
        }

        #region Connect Methods

        /// <summary>
        /// Establish connection to Content Gateway Server
        /// </summary>
        private void ConnectToCG()
        {
            StatusCode statusCode;

            // First stage to connect is to find a service to connect to
            // we need to resolve a service id (eg "Service.ContentGateway") to a list of service instances with that id.
            // each instance of a service has a list of access points associated with it.
            IList<ServiceInstance> serviceInstanceList = new List<ServiceInstance>();

            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add(FileConfiguration.FileLocation, Application.Settings.ServiceLocationIniFile);

            statusCode = ServiceApi.FindServices(ServiceApi.ConfigurationTypeFile, _serviceId, attributes, serviceInstanceList);

            if (StatusCode.StatusCodeSuccess != statusCode)
            {
                File.AppendAllText(_activLogFilePath, string.Format("\nFailed to find service <{0}>, {1}.", _serviceId, statusCode.ToString()) + Environment.NewLine);
            }

            ConnectParameters connectParameters = new ConnectParameters();

            // here we are just going to pick the first service that is returned, and its first access point url
            ServiceInstance serviceInstance = serviceInstanceList[0];

            if (_serviceInstanceId != null)
            {
                foreach (ServiceInstance si in serviceInstanceList)
                {
                    if (si.ServiceAccessPointList[0].Id == _serviceInstanceId)
                    {
                        serviceInstance = si;
                        break;
                    }
                }
            }

            string url = serviceInstance.ServiceAccessPointList[0].Url;

            File.AppendAllText(_activLogFilePath, string.Format("CG SERVER URL ----> {0}", url) + Environment.NewLine);

            connectParameters.ServiceId = serviceInstance.ServiceId;
            connectParameters.Url = url;
            connectParameters.UserId = _userId;
            connectParameters.Password = _password;

            statusCode = base.Connect(connectParameters, (RequestMode.RequestModeAsync == this._requestMode) ? 0 : DefaultTimeout);

            File.AppendAllText(_activLogFilePath, string.Format("Connect() returned --> {0}", statusCode.ToString()) + Environment.NewLine);
        }

        /// <summary>
        /// invoked when asynchronous connect to the data source is successful
        /// </summary>
        override public void OnConnect()
        {
            File.AppendAllText(_activLogFilePath, "Connection Status Received async connect" + Environment.NewLine);
        }

        /// <summary>
        /// invoked when asynchronous connect to the data source fails
        /// </summary>
        /// <param name="code"></param>
        override public void OnConnectFailed(StatusCode code)
        {
            File.AppendAllText(_activLogFilePath, string.Format("Received async connect failure, status {0}", code.ToString()) + Environment.NewLine);
        }

        /// <summary>
        /// invoked on unsolicited disconnect from the CG
        /// </summary>
        override public void OnBreak()
        {
            File.AppendAllText(_activLogFilePath, "Connection to gateway has broken" + Environment.NewLine);
        }

        #endregion

        private void GetLiveData()
        {
            GetEqual.RequestParameters requestParameters = new GetEqual.RequestParameters();
            if (!GetRequestParameters(requestParameters))
                return;

            GetRequest<SymbolIdListRequestParameters>(Equal, requestParameters);
        }

        private bool GetRequestParameters(SymbolIdListRequestParameters requestParameters)
        {
            if (!GetSymbolIdList(requestParameters.SymbolIdList))
                return false;

            return GetRequestParameters((RealtimeRequestParameters)(requestParameters));
        }

        /// <summary>
        /// Adds Symbol to SymbolIdList from Symbols received from T_PMDataDump
        /// </summary>
        /// <param name="symbolIdList"></param>
        /// <returns></returns>
        private bool GetSymbolIdList(IList<SymbolId> symbolIdList)
        {
            foreach (var symbol in _openSymbol)
            {
                SymbolId symbolId = new SymbolId();
                symbolId.TableNumber = GetTableNumber(symbolId.TableNumber, true);
                symbolId.Symbol = symbol.Value;
                symbolIdList.Add(symbolId);
            }
            return symbolIdList.Count != 0;
        }

        private int GetTableNumber(int tableNumber, bool shouldAllowUndefined)
        {
            try
            {
                if (shouldAllowUndefined)
                {
                    //tableNumber = Enter a custom table Number otherwise ACTIV API will choose the most suitable one automatically
                }
                else
                {
                    //tableNumber = Enter a custom table Number otherwise ACTIV API will choose the most suitable one automatically
                }
            }
            catch (MiddlewareException e)
            {
                if (e.StatusCode == StatusCode.StatusCodeUnchanged && shouldAllowUndefined)
                {
                    tableNumber = TableNumbers.TableNoUndefined;
                }
                else
                {
                    throw e;
                }
            }

            return tableNumber;
        }

        private bool GetRequestParameters(RealtimeRequestParameters requestParameters)
        {
            if (!GetSubscribeParameters(requestParameters.SubscribeParameters))
                return false;

            bool isFirstRequest = true;

            while (true)
            {
                RequestBlock requestBlock = new RequestBlock();

                if (isFirstRequest)
                    //Define Relationship ID
                    requestBlock.RelationshipId = RelationshipIds.RelationshipIdNone;

                isFirstRequest = false;

                if (requestBlock.FieldIdList.Count == 0)
                {
                    // No fields entered. May want to get all fields.
                    if (StatusCode.StatusCodeSuccess == StatusCode.StatusCodeSuccess)
                        requestBlock.Flags |= RequestBlock.FlagAllFields;
                }
                else
                {
                    // Whether or not to return invalid (not permissioned or not found) fields that have been specified.
                    if (StatusCode.StatusCodeFailure == StatusCode.StatusCodeFailure)
                        requestBlock.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                }

                // May not want to subscribe to the results of a particular relationship.
                if (SubscribeParameters.TypeEnum.TypeNone != requestParameters.SubscribeParameters.Type)
                {
                    if (StatusCode.StatusCodeSuccess != StatusCode.StatusCodeSuccess)
                        requestBlock.Flags |= RequestBlock.FlagNoSubscription;
                }

                // Navigation responses can optionally be filtered server-side by a pattern.
                //if (RelationshipIds.RelationshipIdNone != requestBlock.RelationshipId)
                //{
                //    try
                //    {
                //        requestBlock.NavigationFilterPattern = uiIo.GetString("Enter navigation filter pattern", requestBlock.NavigationFilterPattern, true, true);
                //    }
                //    catch (MiddlewareException e)
                //    {
                //        if (e.StatusCode == StatusCode.StatusCodeFailure)
                //            return false;
                //    }
                //}

                requestParameters.RequestBlockList.Add(requestBlock);
                break;
            }

            if (SubscribeParameters.TypeEnum.TypeFull == requestParameters.SubscribeParameters.Type)
            {
                if (!GetConflationParameters(requestParameters.ConflationParameters))
                    return false;
            }

            if (StatusCode.StatusCodeSuccess != StatusCode.StatusCodeSuccess)
                requestParameters.Flags |= RealtimeRequestParameters.FlagDoNotResolveAlias;

            if ((SubscribeParameters.TypeEnum.TypeNone != requestParameters.SubscribeParameters.Type)
                && (StatusCode.StatusCodeSuccess == StatusCode.StatusCodeSuccess))
            {
                requestParameters.Flags |= RealtimeRequestParameters.FlagUseRequestIdInUpdates;
            }

            try
            {
                requestParameters.PermissionLevel = GetPermissionLevel(true, requestParameters.PermissionLevel);
                requestParameters.UserContext = "";
            }
            catch (MiddlewareException)
            {
                return false;
            }

            return true;
        }

        private bool GetSubscribeParameters(SubscribeParameters subscribeParameters)
        {
            char subscriptionTypeChar;

            try
            {
                subscriptionTypeChar = 'F';
            }
            catch (MiddlewareException)
            {
                return false;
            }

            subscriptionTypeChar = Char.ToUpper(subscriptionTypeChar);

            switch (subscriptionTypeChar)
            {
                case 'N':
                    //subscribeParameters.Type = SubscribeParameters.TypeEnum.TypeNone;
                    break;

                case 'A':
                    //subscribeParameters.Type = SubscribeParameters.TypeEnum.TypeAddOrDelete;
                    break;

                case 'I':
                case 'E':
                    //if (subscriptionTypeChar == 'I')
                    //    subscribeParameters.Type = SubscribeParameters.TypeEnum.TypeEventTypeFilterIncludeList;
                    //else
                    //    subscribeParameters.Type = SubscribeParameters.TypeEnum.TypeEventTypeFilterExcludeList;

                    //while (true)
                    //{
                    //    byte eventType = 0;

                    //    try
                    //    {
                    //        eventType = (byte)uiIo.GetUnsignedIntegral("Enter event type to subscribe to", eventType, false, false);
                    //        subscribeParameters.EventTypeList.Add(eventType);
                    //    }
                    //    catch (MiddlewareException e)
                    //    {
                    //        if (StatusCode.StatusCodeUnchanged == e.StatusCode)
                    //        {
                    //            // Enter hit?
                    //            if (subscribeParameters.EventTypeList.Count == 0)
                    //                return false;

                    //            break;
                    //        }
                    //        else
                    //        {
                    //            return false;
                    //        }
                    //    }

                    //}
                    break;

                case 'F':
                    subscribeParameters.Type = SubscribeParameters.TypeEnum.TypeFull;
                    break;
            }

            return true;
        }

        private bool GetConflationParameters(ConflationParameters conflationParameters)
        {
            char conflationTypeChar = 'N';

            switch (Char.ToUpper(conflationTypeChar))
            {
                case 'N':
                    conflationParameters.Type = ConflationType.ConflationTypeNone;
                    break;

                case 'Q':
                    //conflationParameters.Type = ConflationType.ConflationTypeQuote;

                    //try
                    //{
                    //    conflationParameters.Interval = (int)uiIo.GetUnsignedIntegral("Enter required conflation interval", conflationParameters.Interval, false, false);
                    //}
                    //catch (MiddlewareException)
                    //{
                    //    return false;
                    //}
                    break;

                case 'T':
                    //conflationParameters.Type = ConflationType.ConflationTypeTrade;

                    //try
                    //{
                    //    conflationParameters.Interval = (int)uiIo.GetUnsignedIntegral("Enter required conflation interval", conflationParameters.Interval, false, false);
                    //}
                    //catch (MiddlewareException)
                    //{
                    //    return false;
                    //}
                    break;
            }

            //Enable or Disbale Conflations
            conflationParameters.ShouldEnableDynamicConflation = false;

            return true;
        }

        private byte GetPermissionLevel(bool shouldAllowDefaultPermissionLevel, byte permissionLevel)
        {
            char chr;

            switch (permissionLevel)
            {
                case PermissionLevels.PermissionLevelRealtime:
                    chr = 'R';
                    break;

                case PermissionLevels.PermissionLevelDelayed:
                    chr = 'D';
                    break;

                case PermissionLevels.PermissionLevelDefault:
                    chr = 'B';
                    break;

                default:
                    throw new MiddlewareException(StatusCode.StatusCodeFailure);
            }

            try
            {
                if (shouldAllowDefaultPermissionLevel)
                    chr = 'B'; // Define Char for Permission Level
            }
            catch (MiddlewareException e)
            {
                if ((StatusCode.StatusCodeSuccess != e.StatusCode) && (StatusCode.StatusCodeUnchanged != e.StatusCode))
                    throw new MiddlewareException(StatusCode.StatusCodeFailure);
            }

            switch (Char.ToUpper(chr))
            {
                case 'R':
                    permissionLevel = PermissionLevels.PermissionLevelRealtime;
                    break;

                case 'D':
                    permissionLevel = PermissionLevels.PermissionLevelDelayed;
                    break;

                case 'B':
                    permissionLevel = PermissionLevels.PermissionLevelDefault;
                    break;

                default:
                    throw new MiddlewareException(StatusCode.StatusCodeFailure);
            }

            return permissionLevel;
        }

        private void GetRequest<RequestParameters>(RealtimeRequestHelper<RequestParameters> requestHelper, RequestParameters requestParameters) where RequestParameters : IRealtimeRequestParameters
        {
            StatusCode statusCode;

            // All request modes forward the timeoutPeriod to the server. The server will cancel the request if it is exceeded.
            int timeoutPeriod;

            if (_requestMode == RequestMode.RequestModeSync)
            {
                timeoutPeriod = ContentGatewayClient.DefaultTimeout;
                try
                {
                    timeoutPeriod = 0; // Define Timeout period
                }
                catch (MiddlewareException e)
                {
                    File.AppendAllText(_activLogFilePath, string.Format("Timeout Exception {0}", e.ToString()) + Environment.NewLine);
                }

            }
            else
            {
                timeoutPeriod = ContentGatewayClient.WaitInfiniteTimeout;
                try
                {
                    timeoutPeriod = -1; 
                }
                catch (MiddlewareException e)
                {
                    if (e.StatusCode != StatusCode.StatusCodeUnchanged)
                        return;
                }
            }

            if (StatusCode.StatusCodeSuccess != StatusCode.StatusCodeSuccess)
                return;

            switch (this._requestMode)
            {
                case RequestMode.RequestModeAsync:
                    // Fully async call. Response will invoke an on*Response() callback at some point in the future.
                    statusCode = requestHelper.PostRequest(this, new RequestId(++this.nextRequestId), requestParameters, timeoutPeriod);
                    File.AppendAllText(_activLogFilePath, string.Format("Returned {0} using request id {1}.\n", statusCode.ToString(), this.nextRequestId) + Environment.NewLine);
                    break;

                case RequestMode.RequestModeSync:
                    //{
                    //    System.DateTime start = System.DateTime.Now;

                    //    // "traditional" sync call. Request is sent, then wait for response and process.
                    //    RealtimeResponseParameters responseParameters = new RealtimeResponseParameters();

                    //    statusCode = requestHelper.SendRequest(this, requestParameters, responseParameters, timeoutPeriod);

                    //    double requestTime = ((TimeSpan)(System.DateTime.Now - start)).TotalMilliseconds / 1000;
                    //    int numberOfResponseBlocks = responseParameters.ResponseBlockList.Count;

                    //    if (0.0 == requestTime)
                    //        File.AppendAllText(_activLogFilePath, string.Format("Returned {0}. {1} response blocks in 0 time.\n", statusCode.ToString(), numberOfResponseBlocks) + Environment.NewLine));
                    //    else
                    //        File.AppendAllText(_activLogFilePath, string.Format("Returned {0}. {1} response blocks in {2:0.0000} secs, {3} / sec\n", statusCode.ToString(), numberOfResponseBlocks, requestTime, (int)(numberOfResponseBlocks / requestTime)) + Environment.NewLine);

                    //    if ((StatusCode.StatusCodeSuccess == statusCode) && displayResponses)
                    //    {
                    //        StringBuilder output = new StringBuilder();
                    //        DisplayResponseBlockList(responseParameters.ResponseBlockList, output);
                    //        uiIo.PutString(output.ToString());
                    //    }
                    //}
                    break;

                case RequestMode.RequestModeNonBlockingSync:
                    // Non-blocking sync. Request is sent, and then response can be polled for or waited on
                    // using syncRequestId.getResponse(), from the same thread context or another thread context
                    // (compare with fully async where the response is received and processed when it is received, by a
                    // thread not under the api user's control).

                    // SyncRequestId CANNOT BE REUSED FOR MULTIPLE CALLS! 
                    // Always allocate a new SyncRequestId for each call.
                    //syncRequestId = new SyncRequestId(); 
                    //statusCode = requestHelper.SendRequest(this, syncRequestId, requestParameters);
                    //uiIo.PutString(string.Format("Returned {0}.\n", statusCode.ToString()));
                    break;
            }
        }

        /// <summary>
        /// Called on receiving an asynchronous Equal.PostRequest() response.
        /// The response can be deserialized using Equal.Deserialize() if it is valid.
        /// The validity of the response can be determined using ContentGatewayClient.IsValidResponse().
        /// If this is the final (or only) part of a response, ContentGatewayClient.IsCompleteResponse() will return true.
        /// </summary>
        /// <param name="response">HeapMessage containing the serialized response.</param>
        override public void OnGetEqualResponse(HeapMessage response)
        {
            DisplayAsyncRealtimeResponse("OnGetEqualResponse", response, Equal);
        }

        // Display async realtime request result.
        private void DisplayAsyncRealtimeResponse<T>(string function, HeapMessage response, RealtimeRequestHelper<T> helper) where T : IRealtimeRequestParameters
        {
            StringBuilder output = new StringBuilder();

            DisplayRealtimeResponse(function, response, helper, output);

            if (IsCompleteResponse(response))
            {
                output.Append(string.Format("{0}(): Request id [{1}] now complete", function, response.RequestId.ToString()));
            }

            if (_symbolFieldResult.Count == 0)
                File.AppendAllText(_activLogFilePath, "Symbols Data is empty" + Environment.NewLine);
            else
                ActivProvider.SetDic(_symbolFieldResult);

            if (output.Length != 0)
            {
                File.AppendAllText(_activLogFilePath, "****************************** Respone Blocks ******************************");
                File.AppendAllText(_activLogFilePath, output + Environment.NewLine);
            }

            System.Diagnostics.Process.Start(_activLogFilePath);

        }

        // Display async realtime request result.
        private void DisplayRealtimeResponse<T>(string function, HeapMessage response, RealtimeRequestHelper<T> helper, StringBuilder output) where T : IRealtimeRequestParameters
        {
            if (!_displayResponses)
                return;

            output.Append(string.Format("{0}(): Request id [{1}] - status {2}\n",
                function,
                response.RequestId.ToString(),
                response.StatusCode.ToString()));

            if (!IsValidResponse(response))
            {
                File.AppendAllText(_activLogFilePath,string.Format("Response Block Exception -> {0}", response.StatusCode.ToString()) + Environment.NewLine);
                return;
            }

            RealtimeResponseParameters responseParameters = new RealtimeResponseParameters();
            StatusCode statusCode = helper.Deserialize(this, response, responseParameters);

            if (StatusCode.StatusCodeSuccess == statusCode)
            {
                output.Append(string.Format("Subscription cookie ............... {0}\n", SubscriptionCookieToString(responseParameters.SubscriptionCookie)));
                DisplayResponseBlockList(responseParameters.ResponseBlockList, output);
            }
            else
            {
                output.Append(string.Format("Failed to deserialize response, {0}\n", statusCode.ToString()));
            }
        }

        private string SubscriptionCookieToString(long subscriptionCookie)
        {
            if (MiscConsts.SubscriptionCookieUndefined == subscriptionCookie)
                return "undefined";

            return subscriptionCookie.ToString();
        }

        // Display response block list.
        private void DisplayResponseBlockList(IList<ResponseBlock> responseBlockList, StringBuilder output)
        {
            for (int i = 0, size = responseBlockList.Count; i < size; ++i)
            {
                ResponseBlock responseBlock = responseBlockList[i];

                output.Append(string.Format("**** Response block {0}/{1} ****\n", (i + 1), size));
                output.Append(string.Format("Requested key ..................... {0}\n", responseBlock.RequestedKey.ToString()));
                output.Append(string.Format("Resolved key ...................... {0}\n", responseBlock.ResolvedKey.ToString()));
                output.Append(string.Format("Relationship id {0,3} ............... {1}\n", (int)responseBlock.RelationshipId, EnumDescription.GetDescription(responseBlock.StatusCode)));
                output.Append(string.Format("Flags ............................. {0}\n", responseBlock.FlagsToString(responseBlock.Flags)));

                if (responseBlock.IsResponseKeyDefined())
                {
                    // response key is not defined for all error cases. Only display if it is set
                    output.Append(string.Format("Response key ...................... {0}\n", responseBlock.ResponseKey.ToString()));
                }

                DisplayPermissionInfo(responseBlock.PermissionId, responseBlock.permissionLevel, responseBlock.PermissionLevelData, output);

                if (responseBlock.IsValidResponse())
                {
                    _fieldResult = new Dictionary<string, string>();

                    StatusCode statusCode = DisplayFieldData(responseBlock.FieldData, true, output);

                    _symbolFieldResult.Add(responseBlock.ResolvedKey.ToString(), _fieldResult);

                    if (StatusCode.StatusCodeSuccess != statusCode)
                    {
                        output.Append(string.Format("Failed to deserialize field data for {0}, {1}.\n",
                            responseBlock.ResolvedKey.ToString(),
                            statusCode.ToString()));
                    }
                }
            }
        }

        private void DisplayPermissionInfo(int permissionId, byte permissionLevel, PermissionLevelData permissionLevelData, StringBuilder output)
        {
            switch (permissionId)
            {
                case PermissionIds.PermissionIdUnknown:
                    output.Append("Permission id ..................... unknown\n");
                    break;

                case PermissionIds.PermissionIdList:
                    output.Append("Permission id ..................... list\n");
                    break;

                default:
                    output.Append(string.Format("Permission id ..................... {0}\n", (int)permissionId));
                    break;
            }

            if (PermissionLevels.PermissionLevelDefault != permissionLevel)
            {
                output.Append(string.Format("Permission level .................. {0}\n", PermissionLevels.PermissionLevelToString(permissionLevel)));

                switch (permissionLevel)
                {
                    case PermissionLevels.PermissionLevelRealtime:
                        // No data for realtime permission level
                        break;

                    case PermissionLevels.PermissionLevelDelayed:
                        // Delayed response block. display delay in minutes.
                        output.Append(string.Format("Delay period (mins) ............... {0}\n", permissionLevelData.DelayPeriod));
                        break;
                }
            }
        }

        // Display field data.
        private StatusCode DisplayFieldData(FieldData fieldData, bool shouldDisplay, StringBuilder output)
        {
            if (fieldData.IsEmpty())
                return StatusCode.StatusCodeSuccess;

            try
            {
                // Use a field list validator to recover field data from the opaque data block.
                // FieldListValidator caches Activ field type objects, so it is considerably more efficient to reuse
                // the same FieldListValidator when possible (if threading issues allow it).
                _fieldListValidator.Initialize(fieldData);
            }
            catch (MiddlewareException e)
            {
                return e.StatusCode;
            }
            if (shouldDisplay)
                DisplayFieldListValidator(output);

            return StatusCode.StatusCodeSuccess;
        }

        // Display all fields in a FieldListValidator.
        private void DisplayFieldListValidator(StringBuilder output)
        {
            foreach (FieldListValidator.Field field in _fieldListValidator)
            {
                StringBuilder buff = new StringBuilder();

                UniversalFieldHelper universalFieldHelper = MetaData.GetUniversalFieldHelper2(this, field.FieldId);
                buff.Append(universalFieldHelper == null ? "" : universalFieldHelper.Name);
                buff.Append(" [");
                buff.Append(field.FieldId);
                buff.Append("]");

                while (buff.Length < 35)
                    buff.Append(".");

                if (FieldStatuses.FieldStatusDefined == field.FieldStatus)
                {
                    bool hasLoggedField = false;

                    if (CrcBlob.FieldType == field.FieldType.Type)
                    {
                        // Do a little bit of extra work for CrcBlob fields (e.g. a news story body), as they may be compressed
                        try
                        {
                            CrcBlob crcBlob = null;
                            field.GetActivFieldType((int)CrcBlob.FieldType, out crcBlob);

                            int dataLength = 0;

                            if (crcBlob != null)
                            {
                                byte[] data = crcBlob.Get(ref dataLength);
                                if (data != null)
                                {
                                    output.Append(string.Format("{0} {1}{2}\n{3}\n",
                                        buff,
                                        FieldStatuses.FieldStatusToString(field.FieldStatus),
                                        (field.DoesUpdateLastValue() ? "" : " *"),
                                        DebugUtils.BufferToHexDumpString(data, dataLength, 16)));

                                    hasLoggedField = true;
                                }
                            }
                        }
                        catch (MiddlewareException)
                        {
                        }
                    }

                    // Normal case we just use the virtual IFieldType.ToString().
                    if (!hasLoggedField)
                    {
                        output.Append(string.Format("{0} {1}{2}\n", buff.ToString(), field.FieldType.ToString(), (field.DoesUpdateLastValue() ? "" : " *")));
                        AddFieldsToDictionary(universalFieldHelper.Name, field.FieldType.ToString());
                    }
                }
                else
                {
                    output.Append(string.Format("{0} {1}{2}\n", buff.ToString(), FieldStatuses.FieldStatusToString(field.FieldStatus), (field.DoesUpdateLastValue() ? "" : " *")));
                    AddFieldsToDictionary(universalFieldHelper.Name, FieldStatuses.FieldStatusToString(field.FieldStatus));
                }
            }
        }

        private void AddFieldsToDictionary(string fieldName, string fieldData)
        {
            if (fieldData == "undefined")
                fieldData = "0";
            if (!_fieldResult.ContainsKey(fieldName))
                _fieldResult.Add(fieldName, fieldData);
            else
                _fieldResult[fieldName] = fieldData;
        }

        private RecordUpdate recordUpdate = new RecordUpdate();
        private StringBuilder onRecordUpdateOutput = new StringBuilder();
        private object updateLock = new object();

        /// <summary>
        /// Called on receiving a record update.
        /// The message can be deserialized using RecordUpdateHelper.deserialize().
        /// </summary>
        /// <param name="update">HeapMessage containing the serialized update message.</param>
        override public void OnRecordUpdate(HeapMessage update)
        {
            ++currentUpdateCount;

            StatusCode statusCode = RecordUpdateHelper.Deserialize(this, update, recordUpdate);

            if (StatusCode.StatusCodeSuccess == statusCode)
            {
                onRecordUpdateOutput.Length = 0;

                DisplayRecordUpdate(update.RequestId, recordUpdate, onRecordUpdateOutput);

                if (_displayRecordUpdates)
                {
                    lock (updateLock)
                    {
                        Console.WriteLine(onRecordUpdateOutput.ToString());
                    }
                }
            }
            else
            {
                File.AppendAllText(_activLogFilePath, string.Format("Failed to deserialize update, {0}", statusCode.ToString()) + Environment.NewLine);
            }
        }

        // Display record update.
        private void DisplayRecordUpdate(RequestId requestId, RecordUpdate recordUpdate, StringBuilder output)
        {
            if (_displayRecordUpdates)
            {
                output.Append(string.Format("**** Update received for {0} ****\n", recordUpdate.SymbolId.ToString()));

                if ((recordUpdate.Flags & RecordUpdate.FlagRequestKey) != 0)
                    output.Append(string.Format("Request key ....................... {0}\n", recordUpdate.RequestKey.ToString()));

                if ((recordUpdate.Flags & RecordUpdate.FlagRelationshipId) != 0)
                    output.Append(string.Format("Relationship id ................... {0}\n", (int)recordUpdate.RelationshipId));

                if (requestId.Id.Length > 0)
                    output.Append(string.Format("Request id ........................ {0}\n", requestId.ToString()));

                output.Append(string.Format("Flags ............................. {0}\n", RecordUpdate.FlagsToString(recordUpdate.Flags)));
                output.Append(string.Format("Update id ......................... {0}\n", (int)recordUpdate.UpdateId));
                output.Append(string.Format("Event type ........................ {0}\n", recordUpdate.EventType));
                DisplayPermissionInfo(recordUpdate.PermissionId, recordUpdate.PermissionLevel, recordUpdate.PermissionLevelData, output);
            }

            StatusCode statusCode = DisplayFieldData(recordUpdate.FieldData, _displayRecordUpdates, output);

            if (StatusCode.StatusCodeSuccess != statusCode)
            {
                output.Append(string.Format("Failed to deserialize field data for {0}, {1}. Contents:\n{2}\n",
                    recordUpdate.SymbolId.ToString(),
                    statusCode.ToString(),
                    DebugUtils.BufferToHexDumpString(recordUpdate.FieldData.Buffer.Buffer, recordUpdate.FieldData.Buffer.DataLength, 16)));
            }
        }
    }
}
