using Prana.Admin.BLL;
using Prana.AuditManager.Attributes;
using Prana.AuditManager.DAL;
using Prana.AuditManager.Definitions.Constants;
using Prana.AuditManager.Definitions.Data;
using Prana.AuditManager.Definitions.Enum;
using Prana.AuditManager.Definitions.Interface;
using Prana.AuditManager.UI;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prana.AuditManager.BLL
{
    /// <summary>
    /// Handles audit data
    /// </summary>
    public class AuditHandler
    {

        private static Object _singletonLockerObject = new object();
        private static AuditHandler _singletonObject;
        bool _hasAccessToAuditTrail = true;
        public int _userID { get; set; }

        /// <summary>
        /// Function to get instance of current thread
        /// </summary>
        /// <returns></returns>
        public static AuditHandler GetInstance()
        {
            try
            {
                lock (_singletonLockerObject)
                {
                    if (_singletonObject == null)
                        _singletonObject = new AuditHandler();

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _singletonObject;
        }

        private AuditHandler()
        {
            //Initialization goes here
        }

        private object lockerObjectAuditDataCategory = new object();
        Dictionary<Type, Dictionary<AuditAction, AuditAttribute>> _auditDataCategoryCache = new Dictionary<Type, Dictionary<AuditAction, AuditAttribute>>();
        Dictionary<IAuditSource, AuditControl> _controlCache = new Dictionary<IAuditSource, AuditControl>();


        /// <summary>
        /// Function to validate type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool ValidateTypeAndUpdateCache(Type type)
        {
            if (type.GetInterfaces().Contains(typeof(IAuditSource)))
            {
                AuditAttribute[] attrArray = (AuditAttribute[])type.GetCustomAttributes(typeof(AuditAttribute), true);

                if (attrArray == null || attrArray.Count() == 0)
                    return false;//No audit attribute is defined for given type

                if (type.IsSubclassOf(typeof(Form)))
                {

                }
                else if (type.IsSubclassOf(typeof(UserControl)))
                {

                }
                else
                {
                    foreach (AuditAttribute atrr in attrArray)
                    {
                        if (atrr.ShowAuditUI)
                            return false;
                    }
                }


                lock (lockerObjectAuditDataCategory)
                {
                    if (!_auditDataCategoryCache.ContainsKey(type))
                    {
                        _auditDataCategoryCache.Add(type, new Dictionary<AuditAction, AuditAttribute>());
                        foreach (AuditAttribute atrr in attrArray)
                        {
                            if (!_auditDataCategoryCache[type].ContainsKey(atrr.AuditAction))
                            {
                                _auditDataCategoryCache[type].Add(atrr.AuditAction, atrr);
                            }
                            else
                            {
                                throw new Exception("Same action is defined multiple times for audit in " + type.AssemblyQualifiedName);
                            }
                        }

                    }
                    else
                    {
                        //Needs to do validation if class definition is changed at runtime
                        //Currently not needed
                    }
                }


                return true;
            }
            else
            {
                //MessageBox.Show(type.FullName + " has not inherited IAuditGenerator. Please make sure your Form/UserControl implements IAuditGenerator.");
                return false;
            }
        }


        /// <summary>
        /// Function to validate instance
        /// </summary>
        /// <param name="auditGeneratorInstance"></param>
        /// <returns></returns>
        private bool ValidateInstance()//IAuditSource auditGeneratorInstance)
        {
            // TODO: Do validation of instance
            return true;
        }

        /// <summary>
        /// Function to unregister audit generator Instance
        /// </summary>
        /// <param name="auditGeneratorInstance"></param>
        //internal void UnRegister(IAuditSource auditGeneratorInstance)
        //{
        //    lock (lockerObjectAuditDataCategory)
        //    {
        //        if (_auditDataCategoryCache.ContainsKey(auditGeneratorInstance.GetType()))
        //        {
        //            _auditDataCategoryCache.Remove(auditGeneratorInstance.GetType());
        //            //auditGeneratorInstance.OnAudit -= auditGeneratorInstance_AuditData;
        //        }
        //        else
        //        {
        //            //Not registered so no need to unregister
        //        }
        //    }
        //}

        /// <summary>
        /// Function to register audit generator Instance
        /// </summary>
        /// <param name="auditGeneratorInstance"></param>
        internal void Register(IAuditSource auditGeneratorInstance)
        {
            if (ValidateTypeAndUpdateCache(auditGeneratorInstance.GetType()))
            {
                if (ValidateInstance())
                {
                    //auditGeneratorInstance.OnAudit += auditGeneratorInstance_AuditData;
                    // TODO:Add Audit UI to given control if required.
                    UpdateAndBindUIIfRequired(auditGeneratorInstance);
                }
                else
                {
                    //Show message instance is not valid
                }
            }
            else
            {
                //Show message type is not valid
            }

        }

        /// <summary>
        /// Function to Bind and Update UI
        /// </summary>
        /// <param name="auditGeneratorInstance"></param>
        private void UpdateAndBindUIIfRequired(IAuditSource auditGeneratorInstance)
        {
            //TODO: Add UI at runtime fill data to UI
            //check if UI is required
            try
            {
                if (auditGeneratorInstance.GetType().IsSubclassOf(typeof(Form)) || auditGeneratorInstance.GetType().IsSubclassOf(typeof(UserControl)))
                {
                    List<AuditAction> catList = GetListOfActionCategoryForType(auditGeneratorInstance.GetType(), true);
                    if (catList != null && catList.Count > 0)
                    {
                        List<AuditDataDefinition> dataToLoad = AuditDataManager.GetInstance().GetAuditDataFor(catList);
                        AuditControl ctrl = GetAuditControlFor(auditGeneratorInstance);
                        if (ctrl != null)
                        {
                            ctrl.LoadData(dataToLoad);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Function to get audit control instance
        /// </summary>
        /// <param name="auditGeneratorInstance"></param>
        /// <returns></returns>
        private AuditControl GetAuditControlFor(IAuditSource auditGeneratorInstance)
        {
            //TODO: return a control
            try
            {
                if (_hasAccessToAuditTrail)
                {
                    if (auditGeneratorInstance.GetType().IsSubclassOf(typeof(Form)) || auditGeneratorInstance.GetType().IsSubclassOf(typeof(UserControl)))
                    {
                        if (_controlCache.ContainsKey(auditGeneratorInstance))
                            return _controlCache[auditGeneratorInstance];
                        else
                        {
                            _controlCache.Add(auditGeneratorInstance, GenerateNewAuditControlAndBindTo(auditGeneratorInstance));
                            return _controlCache[auditGeneratorInstance];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return null;
        }

        /// <summary>
        /// Function to generate and bind new audit control
        /// </summary>
        /// <param name="auditGeneratorInstance"></param>
        /// <returns></returns>
        private AuditControl GenerateNewAuditControlAndBindTo(IAuditSource auditGeneratorInstance)
        {
            AuditControl auditTemp = new AuditControl();

            //Setting basic properties for auditTemp
            auditTemp.Dock = DockStyle.Bottom;

            Control ctrl = auditGeneratorInstance as Control;

            if (ctrl != null && ctrl.Controls != null)
            {
                ctrl.Controls.Add(auditTemp);
            }
            return auditTemp;
        }



        /// <summary>
        /// Function to get list of Action categories
        /// </summary>
        /// <param name="type"></param>
        /// <param name="forUIOnly"></param>
        /// <returns></returns>
        internal List<AuditAction> GetListOfActionCategoryForType(Type type, bool forUIOnly)
        {

            // TODO: Do a validation for Permisssions
            List<AuditAction> catTemp = new List<AuditAction>();

            try
            {
                //Modified By: Bharat Raturi, 17 apr 2014
                //check if the dictionary is not empty
                if (_auditDataCategoryCache.Keys.Count > 0 && _auditDataCategoryCache[type].Keys.Count > 0)
                {
                    foreach (AuditAction actionCategory in _auditDataCategoryCache[type].Keys)
                    {
                        AuditAttribute attribute = _auditDataCategoryCache[type][actionCategory];
                        if (forUIOnly)
                        {
                            if (attribute.ShowAuditUI)
                            {
                                if (type.IsSubclassOf(typeof(Form)) || type.IsSubclassOf(typeof(UserControl)))
                                {
                                    catTemp.Add(attribute.AuditAction);
                                }
                            }
                        }
                        else
                            catTemp.Add(attribute.AuditAction);

                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return catTemp;
        }

        /// <summary>
        /// Function to save audit data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="argObject"></param>
        /// <param name="action"></param>
        public void AuditDataForGivenInstance(IAuditSource sender, object argObject, AuditAction action)
        {

            AuditEventArgs args = GetAuditEventArgsForObject(argObject, action);

            try
            {
                if (args == null || args.AuditData == null)
                    return;
                Boolean isSenderRegistered = true;

                foreach (AuditDataDefinition auditData in args.AuditData)
                {
                    lock (lockerObjectAuditDataCategory)
                    {
                        if (_auditDataCategoryCache.ContainsKey(sender.GetType()))
                        {
                            if (!_auditDataCategoryCache[sender.GetType()].ContainsKey(auditData.Action))
                            {
                                isSenderRegistered = false;
                                break;
                            }
                        }
                        else
                        {
                            isSenderRegistered = false;
                            break;
                        }
                    }
                }
                //lock (lockerObjectAuditDataCategory)
                //{
                //    if (_auditDataCategoryCache.ContainsKey(sender.GetType()))
                //    {
                //        if (_auditDataCategoryCache[sender.GetType()].ContainsKey(args.AuditData.Action))
                //            isSenderRegistered = true;
                //        else
                //            isSenderRegistered = false;
                //    }
                //    else
                //    {
                //        isSenderRegistered = false;
                //        //sender not registered needs to log
                //    }
                //}

                if (isSenderRegistered)
                {
                    AuditDataManager.GetInstance().LogAuditData(args.AuditData);
                    UpdateAndBindUIIfRequired(sender as IAuditSource);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Function to get audit data
        /// </summary>
        /// <param name="args"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsForObject(Object args, AuditAction action)
        {
            if (args is AuditEventArgs)
            {
                return args as AuditEventArgs;
            }
            else if (args is User)
            {
                return GetAuditEventArgsFromUser(args as User, action);
            }
            else if (args is CounterPartyVenue)
            {
                return GetAuditEventArgsFromCounterPartyVenue(args as CounterPartyVenue, action);
            }
            else if (args is Company)
            {
                return GetAuditEventArgsFromCompany(args as Company, action);
            }
            else if (args is Prana.BusinessObjects.ThirdParty)
            {
                return GetAuditEventArgsFromThirdParty(args as Prana.BusinessObjects.ThirdParty, action);
            }
            else if (args is AUEC)
            {
                return GetAuditEventArgsFromAUEC(args as AUEC, action);
            }
            else if (args is Dictionary<String, List<String>>)
            {
                Dictionary<String, List<String>> temp = args as Dictionary<String, List<String>>;
                foreach (String key in temp.Keys)
                {
                    if (key == CustomAuditSourceConstants.AuditSourceTypeMasterFund)
                    {
                        return GetAuditEventArgsFromMasterFund(args as Dictionary<String, List<String>>, action);
                    }
                    if (key == CustomAuditSourceConstants.AuditSourceTypePricingRule)
                    {
                        return GetAuditEventArgsFromPricingRule(args as Dictionary<String, List<String>>, action);
                    }
                    if (key == CustomAuditSourceConstants.AuditSourceTypeBatch)
                    {
                        return GetAuditEventArgsFromBatch(args as Dictionary<String, List<String>>, action);
                    }
                    if (key == CustomAuditSourceConstants.AuditSourceTypeAccount)
                    {
                        return GetAuditEventArgsFromAccount(args as Dictionary<String, List<String>>, action);
                    }
                    if (key == CustomAuditSourceConstants.AuditSourceTypeAccountGroup)
                    {
                        return GetAuditEventArgsFromAccountGroup(args as Dictionary<String, List<String>>, action);
                    }
                    if (key == CustomAuditSourceConstants.AuditSourceTypeMasterStrategy)
                    {
                        return GetAuditEventArgsFromMasterStrategy(args as Dictionary<String, List<String>>, action);
                    }
                    if (key == CustomAuditSourceConstants.AuditSourceTypeStrategy)
                    {
                        return GetAuditEventArgsFromStrategy(args as Dictionary<String, List<String>>, action);
                    }
                }
                return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Function to get audit trail for selected user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromUser(User user, AuditAction action)
        {
            try
            {
                if (user.UserID != int.MinValue)
                {
                    AuditEventArgs arg = new AuditEventArgs();
                    arg.AuditData = new List<AuditDataDefinition>();

                    AuditDataDefinition def = new AuditDataDefinition();
                    def.Action = action;
                    def.ActualAuditTime = DateTime.Now;
                    def.AppliedAuditTime = DateTime.Now;
                    def.AuditDimensionValue = user.UserID;
                    def.CompanyAccountId = -1;
                    def.CompanyId = user.CompanyID;
                    def.UserId = _userID;
                    def.StatusId = 1;
                    def.ModuleId = -1;
                    def.IsActive = true;
                    def.Comment = action.Equals(AuditAction.UserUpdated) ? "User Updated" : action.Equals(AuditAction.UserCreated) ? "User Created" : action.Equals(AuditAction.UserDeleted) ? "User Deleted" : string.Empty;
                    arg.AuditData.Add(def);

                    return arg;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Function to get audit trail for selected counterParty venue
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromCounterPartyVenue(CounterPartyVenue counterPartyVenue, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            try
            {
                if (counterPartyVenue.CounterPartyVenueID != int.MinValue)
                {
                    arg.AuditData = new List<AuditDataDefinition>();

                    AuditDataDefinition def = new AuditDataDefinition();
                    def.Action = action;
                    def.ActualAuditTime = DateTime.Now;
                    def.AppliedAuditTime = DateTime.Now;
                    def.AuditDimensionValue = counterPartyVenue.CounterPartyVenueID;
                    def.CompanyAccountId = -1;
                    def.CompanyId = counterPartyVenue.CounterPartyVenueID;
                    def.UserId = _userID;
                    def.StatusId = 1;
                    def.ModuleId = -1;
                    def.IsActive = true;
                    def.Comment = action.Equals(AuditAction.CounterPartyVenueUpdated) ? "Broker Venue Updated" : action.Equals(AuditAction.CounterPartyVenueCreated) ? "Broker Venue Created" : action.Equals(AuditAction.CounterPartyVenueDeleted) ? "Broker Venue Deleted" : string.Empty;
                    arg.AuditData.Add(def);

                    return arg;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        /// Function to get audit trail for selected company
        /// </summary>
        /// <param name="company"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromCompany(Company company, AuditAction action)
        {
            try
            {
                if (company.CompanyID != int.MinValue)
                {
                    AuditEventArgs arg = new AuditEventArgs();
                    arg.AuditData = new List<AuditDataDefinition>();

                    AuditDataDefinition def = new AuditDataDefinition();
                    def.Action = action;
                    def.ActualAuditTime = DateTime.Now;
                    def.AppliedAuditTime = DateTime.Now;
                    def.AuditDimensionValue = company.CompanyID;
                    def.CompanyAccountId = -1;
                    def.CompanyId = company.CompanyID;
                    def.UserId = _userID;
                    def.StatusId = 1;
                    def.ModuleId = -1;
                    def.IsActive = true;
                    def.Comment = action.Equals(AuditAction.ClientUpdated) ? "Client Updated" : action.Equals(AuditAction.ClientCreated) ? "Client Created" : action.Equals(AuditAction.ClientDeleted) ? "Client Deleted" : string.Empty;
                    arg.AuditData.Add(def);

                    return arg;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Function to get audit trail for selected third party
        /// </summary>
        /// <param name="thirdParty"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromThirdParty(Prana.BusinessObjects.ThirdParty thirdParty, AuditAction action)
        {
            try
            {
                if (thirdParty.ThirdPartyID != int.MinValue)
                {
                    AuditEventArgs arg = new AuditEventArgs();
                    arg.AuditData = new List<AuditDataDefinition>();

                    AuditDataDefinition def = new AuditDataDefinition();
                    def.Action = action;
                    def.ActualAuditTime = DateTime.Now;
                    def.AppliedAuditTime = DateTime.Now;
                    def.AuditDimensionValue = thirdParty.ThirdPartyID;
                    def.CompanyAccountId = -1;
                    def.CompanyId = thirdParty.ThirdPartyID;
                    def.UserId = _userID;
                    def.StatusId = 1;
                    def.ModuleId = -1;
                    def.IsActive = true;
                    def.Comment = action.Equals(AuditAction.ThirdPartyUpdated) ? "ThirdParty Updated" : action.Equals(AuditAction.ThirdPartyCreated) ? "ThirdParty Created" : action.Equals(AuditAction.ThirdPartyDeleted) ? "ThirdParty Deleted" : string.Empty;
                    arg.AuditData.Add(def);

                    return arg;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Function to get audit trail for selected AUEC
        /// </summary>
        /// <param name="AUEC"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromAUEC(AUEC auec, AuditAction action)
        {
            try
            {
                if (auec.AUECID != int.MinValue)
                {
                    AuditEventArgs arg = new AuditEventArgs();
                    arg.AuditData = new List<AuditDataDefinition>();

                    AuditDataDefinition def = new AuditDataDefinition();
                    def.Action = action;
                    def.ActualAuditTime = DateTime.Now;
                    def.AppliedAuditTime = DateTime.Now;
                    def.AuditDimensionValue = auec.AUECID;
                    def.CompanyAccountId = -1;
                    def.CompanyId = auec.AUECID;
                    def.UserId = _userID;
                    def.StatusId = 1;
                    def.ModuleId = -1;
                    def.IsActive = true;
                    def.Comment = action.Equals(AuditAction.AUECUpdated) ? "AUEC Updated" : action.Equals(AuditAction.AUECCreated) ? "AUEC Created" : action.Equals(AuditAction.AUECDeleted) ? "AUEC Deleted" : string.Empty;
                    arg.AuditData.Add(def);

                    return arg;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Function to get audit trail for selected MasterFund
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromMasterFund(Dictionary<String, List<String>> masterfunddetails, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            List<string> masterFunds = masterfunddetails.SelectMany(x => x.Value).ToList();
            try
            {
                arg.AuditData = new List<AuditDataDefinition>();

                foreach (string item in masterFunds.Skip(1))
                {
                    if (Convert.ToInt32(item) != int.MinValue)
                    {
                        AuditDataDefinition def = new AuditDataDefinition();
                        def.Action = action;
                        def.ActualAuditTime = DateTime.Now;
                        def.AppliedAuditTime = DateTime.Now;
                        def.AuditDimensionValue = Convert.ToInt32(item);
                        def.CompanyAccountId = -1;
                        def.CompanyId = Convert.ToInt32(masterFunds[0]);
                        def.UserId = _userID;
                        def.StatusId = 1;
                        def.ModuleId = -1;
                        if (def.Action == AuditAction.MasterFundDeleted)
                        {
                            def.IsActive = false;
                        }
                        else
                        {
                            def.IsActive = true;
                        }
                        def.Comment = action.Equals(AuditAction.MasterFundUpdated) ? "Master Fund Updated" : action.Equals(AuditAction.MasterFundApproved) ? "Master Fund Approved" : action.Equals(AuditAction.MasterFundCreated) ? "Master Fund Created" : action.Equals(AuditAction.MasterFundDeleted) ? "Master Fund Deleted" : string.Empty;
                        arg.AuditData.Add(def);
                    }
                }
                return arg;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        /// Function to get audit trail for selected Pricing Rule
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromPricingRule(Dictionary<String, List<String>> pricingRuledetails, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            List<string> pricingRule = pricingRuledetails.SelectMany(x => x.Value).ToList();
            try
            {
                arg.AuditData = new List<AuditDataDefinition>();

                foreach (string item in pricingRule.Skip(1))
                {
                    if (Convert.ToInt32(item) != int.MinValue)
                    {
                        AuditDataDefinition def = new AuditDataDefinition();
                        def.Action = action;
                        def.ActualAuditTime = DateTime.Now;
                        def.AppliedAuditTime = DateTime.Now;
                        def.AuditDimensionValue = Convert.ToInt32(item);
                        def.CompanyAccountId = -1;
                        def.CompanyId = Convert.ToInt32(pricingRule[0]);
                        def.UserId = _userID;
                        def.StatusId = 1;
                        def.ModuleId = -1;
                        if (def.Action == AuditAction.PricingRuleDeleted)
                        {
                            def.IsActive = false;
                        }
                        else
                        {
                            def.IsActive = true;
                        }
                        def.Comment = action.Equals(AuditAction.PricingRuleUpdated) ? "Pricing Rule Updated" : action.Equals(AuditAction.PricingRuleApproved) ? "Pricing Rule Approved" : action.Equals(AuditAction.PricingRuleCreated) ? "Pricing Rule Created" : action.Equals(AuditAction.PricingRuleDeleted) ? "Pricing Rule Deleted" : string.Empty;
                        arg.AuditData.Add(def);
                    }
                }
                return arg;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        /// Function to get audit trail for selected Batch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromBatch(Dictionary<String, List<String>> batchDetails, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            List<string> batch = batchDetails.SelectMany(x => x.Value).ToList();
            try
            {
                arg.AuditData = new List<AuditDataDefinition>();

                foreach (string item in batch.Skip(1))
                {
                    if (Convert.ToInt32(item) != int.MinValue)
                    {
                        AuditDataDefinition def = new AuditDataDefinition();
                        def.Action = action;
                        def.ActualAuditTime = DateTime.Now;
                        def.AppliedAuditTime = DateTime.Now;
                        def.AuditDimensionValue = Convert.ToInt32(item);
                        def.CompanyAccountId = -1;

                        // Currently ThirdPartyID is being saved in CompanyID which has no impact in audit trail since 
                        // Batch ID is unique irrespective of ThirdParty.

                        def.CompanyId = Convert.ToInt32(batch[0]);
                        def.UserId = _userID;
                        def.StatusId = 1;
                        def.ModuleId = -1;
                        if (def.Action == AuditAction.BatchDeleted)
                        {
                            def.IsActive = false;
                        }
                        else
                        {
                            def.IsActive = true;
                        }
                        def.Comment = action.Equals(AuditAction.BatchUpdated) ? "Batch Updated" : action.Equals(AuditAction.BatchCreated) ? "Batch Created" : action.Equals(AuditAction.BatchDeleted) ? "Batch Deleted" : string.Empty;
                        arg.AuditData.Add(def);
                    }
                }
                return arg;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        /// Function to get audit trail for selected Account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromAccount(Dictionary<String, List<String>> accountDetails, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            List<string> account = accountDetails.SelectMany(x => x.Value).ToList();
            try
            {
                arg.AuditData = new List<AuditDataDefinition>();

                foreach (string item in account.Skip(1))
                {
                    if (Convert.ToInt32(item) != int.MinValue)
                    {
                        AuditDataDefinition def = new AuditDataDefinition();
                        def.Action = action;
                        def.ActualAuditTime = DateTime.Now;
                        def.AppliedAuditTime = DateTime.Now;
                        def.AuditDimensionValue = Convert.ToInt32(item);
                        def.CompanyAccountId = -1;
                        def.CompanyId = Convert.ToInt32(account[0]);
                        def.UserId = _userID;
                        def.StatusId = 1;
                        def.ModuleId = -1;
                        if (def.Action == AuditAction.AccountDeleted)
                        {
                            def.IsActive = false;
                        }
                        else
                        {
                            def.IsActive = true;
                        }
                        def.Comment = action.Equals(AuditAction.AccountUpdated) ? "Account Updated" : action.Equals(AuditAction.AccountApproved) ? "Account Approved" : action.Equals(AuditAction.AccountCreated) ? "Account Created" : action.Equals(AuditAction.AccountDeleted) ? "Account Deleted" : string.Empty;
                        arg.AuditData.Add(def);
                    }
                }
                return arg;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        /// Function to get audit trail for selected Account Group
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromAccountGroup(Dictionary<String, List<String>> accountGroupDetails, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            List<string> account = accountGroupDetails.SelectMany(x => x.Value).ToList();
            try
            {
                arg.AuditData = new List<AuditDataDefinition>();
                foreach (string item in account)
                {
                    if (Convert.ToInt32(item) != int.MinValue)
                    {
                        AuditDataDefinition def = new AuditDataDefinition();
                        def.Action = action;
                        def.ActualAuditTime = DateTime.Now;
                        def.AppliedAuditTime = DateTime.Now;
                        def.AuditDimensionValue = Convert.ToInt32(item);
                        def.CompanyAccountId = -1;
                        //def.CompanyId = Convert.ToInt32(account[0]);
                        def.CompanyId = 0;
                        def.UserId = _userID;
                        def.StatusId = 1;
                        def.ModuleId = -1;
                        def.IsActive = true;
                        def.Comment = action.Equals(AuditAction.AccountGroupUpdated) ? "Account Group Updated" : action.Equals(AuditAction.AccountGroupCreated) ? "Account Group Created" : action.Equals(AuditAction.AccountGroupDeleted) ? "Account Group Deleted" : string.Empty;
                        arg.AuditData.Add(def);
                    }
                }
                return arg;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        /// Function to get audit trail for selected Master Strategy
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromMasterStrategy(Dictionary<String, List<String>> masterstrategydetails, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            List<string> masterStrategy = masterstrategydetails.SelectMany(x => x.Value).ToList();
            try
            {
                arg.AuditData = new List<AuditDataDefinition>();

                foreach (string item in masterStrategy.Skip(1))
                {
                    if (Convert.ToInt32(item) != int.MinValue)
                    {
                        AuditDataDefinition def = new AuditDataDefinition();
                        def.Action = action;
                        def.ActualAuditTime = DateTime.Now;
                        def.AppliedAuditTime = DateTime.Now;
                        def.AuditDimensionValue = Convert.ToInt32(item);
                        def.CompanyAccountId = -1;
                        def.CompanyId = Convert.ToInt32(masterStrategy[0]);
                        def.UserId = _userID;
                        def.StatusId = 1;
                        def.ModuleId = -1;
                        if (def.Action == AuditAction.MasterStrategyDeleted)
                        {
                            def.IsActive = false;
                        }
                        else
                        {
                            def.IsActive = true;
                        }
                        def.Comment = action.Equals(AuditAction.MasterStrategyUpdated) ? "Master Strategy Updated" : action.Equals(AuditAction.MasterStrategyCreated) ? "Master Strategy Created" : action.Equals(AuditAction.MasterStrategyDeleted) ? "Master Strategy Deleted" : string.Empty;
                        arg.AuditData.Add(def);
                    }
                }
                return arg;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        ///Added By Faisal Shah to get Audit Details of Strategy
        /// Function to get audit trail for selected Strategy
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AuditEventArgs GetAuditEventArgsFromStrategy(Dictionary<String, List<String>> strategydetails, AuditAction action)
        {
            AuditEventArgs arg = new AuditEventArgs();
            List<string> Strategy = strategydetails.SelectMany(x => x.Value).ToList();
            try
            {
                arg.AuditData = new List<AuditDataDefinition>();

                foreach (string item in Strategy.Skip(1))
                {
                    if (Convert.ToInt32(item) != int.MinValue)
                    {
                        AuditDataDefinition def = new AuditDataDefinition();
                        def.Action = action;
                        def.ActualAuditTime = DateTime.Now;
                        def.AppliedAuditTime = DateTime.Now;
                        def.AuditDimensionValue = Convert.ToInt32(item);
                        def.CompanyAccountId = -1;
                        def.CompanyId = Convert.ToInt32(Strategy[0]);
                        def.UserId = _userID;
                        def.StatusId = 1;
                        def.ModuleId = -1;
                        if (def.Action == AuditAction.StrategyDeleted)
                        {
                            def.IsActive = false;
                        }
                        else
                        {
                            def.IsActive = true;
                        }
                        def.Comment = action.Equals(AuditAction.StrategyUpdated) ? "Strategy Updated" : action.Equals(AuditAction.StrategyCreated) ? "Strategy Created" : action.Equals(AuditAction.StrategyDeleted) ? "Strategy Deleted" : string.Empty;
                        arg.AuditData.Add(def);
                    }
                }
                return arg;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return arg;
        }

        /// <summary>
        /// Function to get default audit data
        /// </summary>
        /// <param name="auditGeneratorInstance"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        //internal AuditDataDefinition GetDefaultAuditData(IAuditSource auditGeneratorInstance, AuditAction action)
        //{
        //    AuditDataDefinition def = new AuditDataDefinition();
        //    def.Action = AuditAction.NotDefined;
        //    def.ActualAuditTime= DateTime.UtcNow;
        //    try
        //    {
        //        lock (lockerObjectAuditDataCategory)
        //        {
        //            if (_auditDataCategoryCache.ContainsKey(auditGeneratorInstance.GetType()))
        //            {
        //                if (_auditDataCategoryCache[auditGeneratorInstance.GetType()].Keys.Contains(action))
        //                    def.Action = action;
        //                else
        //                {
        //                    //TODO: log inappropriate acces by generator
        //                    return null;
        //                }
        //                //else

        //                //if (_auditDataCategoryCache[auditGeneratorInstance.GetType()].Keys.Count > 0)
        //                //    def.Action = _auditDataCategoryCache[auditGeneratorInstance.GetType()].Keys.First();
        //            }
        //            else
        //            {
        //                //sender not registered needs to log
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return def;
        //}


        /// <summary>
        /// Function to refresh Audit data
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="dataToAudit"></param>
        internal void RefreshAuditDataForGivenInstance(IAuditSource generator, int dataToAudit)
        {
            AuditControl ctrl = GetAuditControlFor(generator);
            if (ctrl != null)
                ctrl.ShowDataFor(dataToAudit);
        }

        /// <summary>
        /// to show or hide audit trail on permission
        /// </summary>
        /// <param name="auditAccessStatus"></param>
        public void SetUIonPermission(bool auditAccessStatus, int UserID)
        {
            _userID = UserID;
            _hasAccessToAuditTrail = auditAccessStatus;
        }
    }
}