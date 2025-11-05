using Csla;
using Csla.Validation;

namespace Prana.PM.BLL
{
    public class StepAnalysis : BusinessBase<StepAnalysis>
    {
        public StepAnalysis()
        {
        }

        #region Constants

        const string CONST_IsChecked = "IsChecked";
        const string CONST_Step = "Step";
        const string CONST_LowValue = "LowValue";
        const string CONST_HighValue = "HighValue";

        const int CONST_STEP_MINIMUM_VALUE = 1;
        //const int CONST_STEP_MAXIMUM_VALUE = 15;
        //const int LOW_VALUE = 1;
        #endregion

        public StepAnalysis(bool isChecked, OptionsGreeksParameters optionGreeksParameter, int step, int lowValue, int highValue)
        {
            _isChecked = isChecked;
            _optionGreeksParameter = optionGreeksParameter;
            _step = step;
            _lowValue = lowValue;
            _highValue = highValue;
        }

        private bool _isChecked;
        /// <summary>
        /// Gets or sets the IsChecked.
        /// </summary>
        /// <value>The IsChecked value.</value>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                PropertyHasChanged(CONST_IsChecked);
            }
        }


        private OptionsGreeksParameters _optionGreeksParameter;

        /// <summary>
        /// Gets or sets the name of the Option Greeks Parameter.
        /// </summary>
        /// <value>The value of the Option Greeks Parameter.</value>
        public OptionsGreeksParameters OptionGreeksParameter
        {
            get
            {
                return _optionGreeksParameter;
            }
            set
            {
                _optionGreeksParameter = value;
            }
        }

        private int _step = int.MinValue;

        /// <summary>
        /// Gets or sets the name of the Step.
        /// </summary>
        /// <value>The value of the Step.</value>
        public int Step
        {
            get
            {
                return _step;
            }
            set
            {
                _step = value;
                PropertyHasChanged(CONST_Step);
            }
        }

        private int _lowValue = int.MinValue;

        /// <summary>
        /// Gets or sets the Low value.
        /// </summary>
        /// <value>The Low Value.</value>
        public int LowValue
        {
            get
            {
                return _lowValue;
            }
            set
            {
                _lowValue = value;
                PropertyHasChanged(CONST_LowValue);
                PropertyHasChanged(CONST_HighValue);
            }
        }

        private int _highValue = int.MinValue;

        /// <summary>
        /// Gets or sets the High value.
        /// </summary>
        /// <value>The High Value.</value>
        public int HighValue
        {
            get
            {
                return _highValue;
            }
            set
            {
                _highValue = value;
                PropertyHasChanged(CONST_HighValue);
                PropertyHasChanged(CONST_LowValue);
            }
        }

        /// <summary>
        /// Gets the id value.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _step;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        #region Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(StepAnalysisRules.IncludeParametersCheck, CONST_IsChecked);
            ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_Step, CONST_STEP_MINIMUM_VALUE));
            ValidationRules.AddRule(StepAnalysisRules.LowValue_HighValue_MatchCheck, CONST_LowValue);
            ValidationRules.AddRule(StepAnalysisRules.LowValue_HighValue_MatchCheck, CONST_HighValue);
            //ValidationRules.AddRule(StepAnalysisRules.SourceColumnRepeatNameCheck, CONST_SourceColumnName);
        }

        #endregion

        public class StepAnalysisRules : RuleArgs
        {
            public StepAnalysisRules(string validation)
                : base(validation)
            {
            }

            static bool passThrough = false;
            public static bool IncludeParametersCheck(object target, RuleArgs e)
            {
                StepAnalysis sourceTarget = target as StepAnalysis;

                StepAnalysisList stepAnalysisList = (StepAnalysisList)sourceTarget.Parent;
                //SelectColumnsItemList selectColumnsItemListClone = new SelectColumnsItemList(); ;

                int index = 1;

                bool breakRule = false;

                if (stepAnalysisList != null)
                {
                    //selectColumnsItemListClone = selectColumnsItemList.Clone();

                    //if(selectColumnsItemListClone.Contains(sourceTarget))
                    //{
                    //    int index = selectColumnsItemListClone.IndexOf(sourceTarget);
                    //    selectColumnsItemListClone.Remove(sourceTarget);
                    //}
                    //Have to use the follwoing link as the "selectColumnsItemListClone.Remove(sourceTarget)" was not working
                    //as the reason I think is that the indexes in the clone list gets reverse or changed from the orignal list.
                    foreach (StepAnalysis stepAnalysis in stepAnalysisList)
                    {
                        //stepAnalysis.MarkClean();
                        if (stepAnalysis.IsChecked == true)
                        {
                            index++;
                        }
                        if (index > 3)
                        {
                            breakRule = true;
                            break;
                        }
                    }
                    if (breakRule == true)
                    {
                        if (sourceTarget.IsChecked != false)
                        {
                            e.Description = "Please select only two parameters.";
                            breakRule = false;
                            return false;
                        }
                        else
                        {
                            e.Description = "";
                            breakRule = false;
                            return true;
                        }
                    }
                    else
                    {
                        if (passThrough == false)
                        {
                            foreach (StepAnalysis stepAnalysis in stepAnalysisList)
                            {
                                if (stepAnalysis.IsValid == false /*&& stepAnalysis.IsChecked == false*/)
                                {
                                    //stepAnalysisList.
                                    passThrough = true;
                                    target = stepAnalysis;
                                    e.Description = "";
                                    //return true;
                                    IncludeParametersCheck(target, e);
                                    //PropertyHasChanged(CONST_IsChecked);
                                    //ValidationRules.AddRule(StepAnalysisRules.IncludeParametersCheck, CONST_IsChecked);
                                }
                                //base.Description = "";
                            }
                        }
                        e.Description = "";
                        passThrough = false;
                        return true;
                    }
                }
                else
                {
                    //passThrough = false;
                    return true;
                }

            }

            public static bool LowValue_HighValue_MatchCheck(object target, RuleArgs e)
            {
                bool valid = false;
                StepAnalysis finalTarget = target as StepAnalysis;
                if (finalTarget != null)
                {
                    if (finalTarget.LowValue == finalTarget.HighValue)
                    {
                        e.Description = "Low Value and High Value can not be the same.";
                        return false;
                    }
                    else
                    {
                        valid = true;
                        //return true;
                    }

                    if (finalTarget.LowValue > finalTarget.HighValue)
                    {
                        e.Description = "Low Value can not exceed High Value.";
                        return false;
                    }
                    else
                    {
                        valid = true;
                        //return true;
                    }
                    if (valid == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
