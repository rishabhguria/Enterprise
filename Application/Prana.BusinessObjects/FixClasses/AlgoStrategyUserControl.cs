using Prana.Global.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Prana.BusinessObjects
{
    public abstract class AlgoStrategyUserControl : UserControl
    {
        public AlgoStrategyParameters _parameters;
        public Dictionary<string, AlgoStrategyUserControl> _algoStrategyCtrlDict = new Dictionary<string, AlgoStrategyUserControl>();
        public abstract string ValidateValues();
        public abstract AlgoStrategyParameters GetValue();
        public abstract void SetValues(AlgoStrategyParameters algoStrategyParameters);
        public abstract Dictionary<string, string> GetFixValue();
        public abstract void SetUserControls(string type);
        public abstract AlgoStrategyUserControl GetUserCtrl(string underlyingText);
        public abstract AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl alsoStart, AlgoStrategyParametersToUpdate parameterToUpdate);
        public abstract void SetFixValues(string tag, string value, OrderSingle order);
        public void SetAlgoStrategyParameters(AlgoStrategyParameters algoStrategyParameters)
        {
            _parameters = algoStrategyParameters;
        }
        public virtual string Validate(OrderSingle order, Dictionary<string, string> TagValueDictionary)
        {
            string error = ValidateValues();
            if (!string.IsNullOrEmpty(error))
                return error;
            AlgoStrategyParameters algoStrategyParamters = GetValue();
            if (algoStrategyParamters.IsRequired)
            {
                Dictionary<string, string> IDValuedict = GetFixValue();

                foreach (string parameterId in algoStrategyParamters.IDs)
                {
                    if (!(IDValuedict.ContainsKey(parameterId) && !string.IsNullOrEmpty(IDValuedict[parameterId])))
                        return algoStrategyParamters.IDtoNameMapping[parameterId] + " field is a mandatory field. Please provide a valid value for it.";
                }

            }
            return string.Empty;
        }
        public abstract void SetValues(OrderSingle order);
        public abstract void SetIfReplaceControl(bool isReplaceControl);
        public abstract void SetVisibilityIfReplaceControl(bool isReplaceControl);
        public virtual Boolean IsAvailableInRegions(string underlyingText)
        {
            AlgoStrategyParameters algoStrategyParamters = GetValue();
            if (algoStrategyParamters.AvailableInRegions == null)
            {
                return true;
            }
            return algoStrategyParamters.AvailableInRegions.Contains(underlyingText);
        }


        /// <summary>
        /// Check Is Enable In Regions
        /// </summary>
        /// <param name="underlyingText"></param>
        /// <returns></returns>
        public virtual Boolean IsEnableInRegionsParamDefined()
        {
            AlgoStrategyParameters algoStrategyParamters = GetValue();
            if (algoStrategyParamters.EnableInRegions != null)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Check Is Enable In Regions
        /// </summary>
        /// <param name="underlyingText"></param>
        /// <returns></returns>
        public virtual Boolean IsEnableInRegions(string underlyingText)
        {
            AlgoStrategyParameters algoStrategyParamters = GetValue();
            if (algoStrategyParamters.EnableInRegions == null)
            {
                return true;
            }
            return algoStrategyParamters.EnableInRegions.Contains(underlyingText);
        }


        /// <summary>
        /// Get the clone of the algoStrategy paramters and amend as per the underlying text
        /// </summary>
        /// <param name="underlyingText"></param>
        /// <returns>AlgoStrategyParameters</returns>
        public virtual AlgoStrategyParameters GetClonedAlgoStrategyParameters(string underlyingText)
        {

            AlgoStrategyParameters algoStrategyParamterOrig = GetValue();
            AlgoStrategyParameters algoStrategyParamterClone = DeepCopyHelper.Clone(algoStrategyParamterOrig);

            //Setting DefaultValues Region Wise
            if (algoStrategyParamterClone.RegionWiseDefaultValues != null)
            {
                if (algoStrategyParamterClone.RegionWiseDefaultValues.ContainsKey("All"))
                {
                    algoStrategyParamterClone.DefaultValue = algoStrategyParamterClone.RegionWiseDefaultValues["All"];
                }
                if (algoStrategyParamterClone.RegionWiseDefaultValues.ContainsKey(underlyingText))
                {
                    algoStrategyParamterClone.DefaultValue = algoStrategyParamterClone.RegionWiseDefaultValues[underlyingText];
                }
                //Checking to make sure that the default value provided is available for that region
                if (algoStrategyParamterClone.DefaultValue != null && !isControlValueAvailableInUnderlyingRegion(algoStrategyParamterClone.DefaultValue, underlyingText))
                {
                    algoStrategyParamterClone.DefaultValue = null;
                }
            }


            //Setting Is Required to true if the paramter is required in the respective Underlying 
            if (algoStrategyParamterClone.RequiredInRegions != null && (algoStrategyParamterClone.RequiredInRegions.Contains("All") || algoStrategyParamterClone.RequiredInRegions.Contains(underlyingText)))
                algoStrategyParamterClone.IsRequired = true;

            algoStrategyParamterClone.InnerControlNames = new List<string>();
            algoStrategyParamterClone.InnerControlValues = new List<string>();


            //Handling ControlValues Region Wise
            int i = 0;
            if (algoStrategyParamterOrig.InnerControlValues != null)
            {
                foreach (string innerControlValue in algoStrategyParamterOrig.InnerControlValues)
                {
                    if (isControlValueAvailableInUnderlyingRegion(innerControlValue, underlyingText))
                    {
                        algoStrategyParamterClone.InnerControlValues.Add(innerControlValue);
                        algoStrategyParamterClone.InnerControlNames.Add(algoStrategyParamterOrig.InnerControlNames[i]);
                    }
                    i++;
                }
            }

            return algoStrategyParamterClone;

        }



        private Boolean isControlValueAvailableInUnderlyingRegion(string controlValue, string underlyingText)
        {
            AlgoStrategyParameters algoStrategyParameters = GetValue();
            if (algoStrategyParameters.ValueWiseRegionAvailabilty == null || string.IsNullOrEmpty(underlyingText))
                return true;

            List<string> availableRegions = null;
            if (algoStrategyParameters.ValueWiseRegionAvailabilty.ContainsKey(controlValue))
                availableRegions = algoStrategyParameters.ValueWiseRegionAvailabilty[controlValue];


            if (availableRegions == null || availableRegions.Contains("All") || availableRegions.Contains(underlyingText))
                return true;

            return false;
        }
    }


}
