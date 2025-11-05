using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class AccountWiseSecurityDataGridModel
    {

        #region singleton

        private static object syncRoot = new Object();
        private static volatile AccountWiseSecurityDataGridModel instance;
        public static AccountWiseSecurityDataGridModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AccountWiseSecurityDataGridModel();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private AccountWiseSecurityDataGridModel()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool AddModel(SecurityDataGridModel model, Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict)
        {
            if (!AccountWiseDict.ContainsKey(model.AccountOrGroupId))
            {
                Dictionary<string, SecurityDataGridModel> dict = new Dictionary<string, SecurityDataGridModel>();
                dict.Add(model.Symbol, model);
                AccountWiseDict.Add(model.AccountOrGroupId, dict);
            }
            else
            {
                if (!AccountWiseDict[model.AccountOrGroupId].ContainsKey(model.Symbol))
                {
                    AccountWiseDict[model.AccountOrGroupId].Add(model.Symbol, model);
                }
                else
                {
                    if (model.IncreaseDecreaseOrSet.Equals(RebalancerEnums.RASIncreaseDecreaseOrSet.Set.ToString()))
                    {
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].IncreaseDecreaseOrSet = model.IncreaseDecreaseOrSet;
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].Target = model.Target;
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].Price = model.Price;
                    }
                    else if (model.IncreaseDecreaseOrSet.Equals(AccountWiseDict[model.AccountOrGroupId][model.Symbol].IncreaseDecreaseOrSet))
                    {
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].Price = RebalancerFormulas.GetWeightedPrice(AccountWiseDict[model.AccountOrGroupId][model.Symbol].TargetPercentage,
                                                                                                              AccountWiseDict[model.AccountOrGroupId][model.Symbol].Price, model.TargetPercentage, model.Price);
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].Target += model.Target;
                    }
                    else
                    {
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].Price = RebalancerFormulas.GetWeightedPrice(AccountWiseDict[model.AccountOrGroupId][model.Symbol].TargetPercentage,
                                                                                                               AccountWiseDict[model.AccountOrGroupId][model.Symbol].Price, model.TargetPercentage, model.Price);
                        decimal tPercentage = AccountWiseDict[model.AccountOrGroupId][model.Symbol].TargetPercentage - model.TargetPercentage;
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].IncreaseDecreaseOrSet = (tPercentage >= 0) ? AccountWiseDict[model.AccountOrGroupId][model.Symbol].IncreaseDecreaseOrSet : model.IncreaseDecreaseOrSet;
                        AccountWiseDict[model.AccountOrGroupId][model.Symbol].Target = (tPercentage >= 0) ? tPercentage : tPercentage * (-1);
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal ObservableCollection<SecurityDataGridModel> GetList(Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict)
        {
            ObservableCollection<SecurityDataGridModel> lst = new ObservableCollection<SecurityDataGridModel>();

            foreach (KeyValuePair<int, Dictionary<string, SecurityDataGridModel>> accountId in AccountWiseDict)
            {
                foreach (var symbol in accountId.Value)
                {
                    if (symbol.Value.Remove.Equals(string.Empty))
                    {
                        lst.Add(symbol.Value);
                    }
                }
            }
            return lst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal string IsModelValid(SecurityDataGridModel model, Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict)
        {
            string error = string.Empty;
            decimal num1 = 0;
            decimal num2 = 0;
            decimal targetPercentage = model.TargetPercentage;
            if (AccountWiseDict.ContainsKey(model.AccountOrGroupId) && model.AccountOrGroupId != 0)
            {
                if (AccountWiseDict[model.AccountOrGroupId].ContainsKey(model.Symbol))
                {
                    if ((AccountWiseDict[model.AccountOrGroupId][model.Symbol].AccountOrGroupId == 0 && model.AccountOrGroupId != 0)
                        || (AccountWiseDict[model.AccountOrGroupId][model.Symbol].AccountOrGroupId != 0 && model.AccountOrGroupId == 0))
                    {
                        return "Account can not be changed from Pro-Rata and vice-versa";
                    }
                }
                num1 = AccountWiseDict[model.AccountOrGroupId].Sum(p => (p.Value.TargetPercentage * p.Value.MultiplierFactor)) + targetPercentage * model.MultiplierFactor;
            }
            if (model.AccountOrGroupId == 0)
            {
                foreach (var abc in AccountWiseDict)
                {
                    if (abc.Key != 0 && abc.Value.ContainsKey(model.Symbol))
                    {
                        return "Account can not be changed from Pro-Rata and vice-versa";
                    }
                    decimal ss = abc.Value.Sum(p => p.Value.MultiplierFactor * p.Value.TargetPercentage);
                    if (Math.Abs(ss + targetPercentage * model.MultiplierFactor) > 100)
                    {
                        return "Account Sum should be less than 100%";
                    }
                }
            }
            if (AccountWiseDict.ContainsKey(0))
            {
                if (AccountWiseDict[0].ContainsKey(model.Symbol))
                {
                    if ((AccountWiseDict[0][model.Symbol].AccountOrGroupId == 0 && model.AccountOrGroupId != 0)
                        || (AccountWiseDict[0][model.Symbol].AccountOrGroupId != 0 && model.AccountOrGroupId == 0))
                    {
                        return "Account can not be changed from Pro-Rata and vice-versa";
                    }
                }

                num2 = AccountWiseDict[0].Sum(p => p.Value.TargetPercentage * p.Value.MultiplierFactor) + targetPercentage * model.MultiplierFactor;
            }
            if (Math.Abs(num1 + num2) > 100)
                return "Account Sum should be less than 100%";

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ClearData(Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict)
        {
            if (AccountWiseDict != null)
                AccountWiseDict.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        internal void RemoveData(SecurityDataGridModel model, Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict)
        {
            if (AccountWiseDict.ContainsKey(model.AccountOrGroupId) && AccountWiseDict[model.AccountOrGroupId].ContainsKey(model.Symbol))
            {
                AccountWiseDict[model.AccountOrGroupId].Remove(model.Symbol);
                if (AccountWiseDict[model.AccountOrGroupId].Count == 0)
                    AccountWiseDict.Remove(model.AccountOrGroupId);
            }

        }

        internal bool Validate<T>(string prop, T oldValue, T newValue, Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict, int accountId = -1, string symbol = null)
        {
            if (prop.Equals("Price"))
            {
                if ((double)Convert.ChangeType(newValue, typeof(double)) <= 0)
                {
                    ShowError("Price can not be less than Zero.");
                    return false;
                }
            }
            else if (prop.Equals("TargetPercentage"))
            {
                return ValidateTargetPercentage<T>(oldValue, newValue, accountId, symbol, AccountWiseDict);
            }
            else if (prop.Equals("IncreaseDecreaseOrSet"))
            {
                return ValidateTargetPercentage<T>(oldValue, newValue, accountId, symbol, AccountWiseDict);
            }

            return true;

        }

        private bool ValidateTargetPercentage<T>(T oldValue, T newValue, int accountId, string symbol, Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict)
        {
            decimal ov = (decimal)Convert.ChangeType(oldValue, typeof(decimal));
            decimal nv = (decimal)Convert.ChangeType(newValue, typeof(decimal));
            if (nv > 100)
            {
                ShowError("Target Percentage should be less than 100.");
                return false;
            }

            if (accountId > 0)
            {
                if (!AccountWiseDict.ContainsKey(accountId) || !AccountWiseDict[accountId].ContainsKey(symbol))
                {
                    return true;
                }
                decimal targetPercentage = AccountWiseDict[accountId][symbol].TargetPercentage;

                decimal sum = 0;
                if (AccountWiseDict.ContainsKey(accountId))
                    sum = AccountWiseDict[accountId].Sum(p => p.Value.TargetPercentage * p.Value.MultiplierFactor);
                decimal val = nv;
                if (AccountWiseDict.ContainsKey(0))
                {
                    sum += (AccountWiseDict[0].Sum(p => p.Value.TargetPercentage * p.Value.MultiplierFactor));
                }
                decimal res = 0;
                if ((AccountWiseDict.ContainsKey(accountId) && AccountWiseDict[accountId].ContainsKey(symbol)))
                    res = sum + (val - (targetPercentage * AccountWiseDict[accountId][symbol].MultiplierFactor));
                if (res > 100)
                {
                    ShowError("Account Sum should be less than 100%");
                    return false;
                }
            }
            else if (accountId == 0)
            {
                decimal targetPercentage = AccountWiseDict[0][symbol].TargetPercentage;
                if (nv > 100)
                {
                    ShowError("Target Percentage should be less than 100.");
                    return false;
                }
                decimal sum = 0;
                if (AccountWiseDict.ContainsKey(accountId))
                    sum = AccountWiseDict[accountId].Sum(p => p.Value.TargetPercentage * p.Value.MultiplierFactor);
                decimal val = nv;

                foreach (var kvp in AccountWiseDict)
                {
                    if (kvp.Key != 0)
                    {
                        sum += (kvp.Value.Sum(p => p.Value.TargetPercentage * p.Value.Multiplier));
                    }
                }
                decimal res = 0;
                if ((AccountWiseDict.ContainsKey(accountId) && AccountWiseDict[accountId].ContainsKey(symbol)))
                    res = sum + (val - (targetPercentage * AccountWiseDict[accountId][symbol].MultiplierFactor));
                if (res > 100)
                {
                    ShowError("Account Sum should be less than 100%");
                    return false;
                }
            }
            return true;
        }

        internal void ShowError(string msg)
        {
            if (!RebalancerCache.Instance.RebalancerHelperInstance.IsComingFromImport)
            {
                MessageBox.Show(msg, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
