using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;
namespace Prana.Fix.FixDictionary
{
    class MessageContentList
    {
        List<FixFields> _dictReqdFixFields = new List<FixFields>();
        List<FixFields> _dictOptFixFields = new List<FixFields>();

        public void AddReqFields(FixFields fixField)
        {
            _dictReqdFixFields.Add(fixField);
        }
        public void AddOptFields(FixFields fixField)
        {
            _dictOptFixFields.Add(fixField);
        }
        public string ValidateMessage(PranaMessage pranaMessage)
        {

            string error = string.Empty;
            try
            {
                string validation1 = ValidateForRequiredFields(pranaMessage, _dictReqdFixFields);
                List<FixFields> extraReqdFixFields = new List<FixFields>();

                foreach (FixFields fixField in _dictOptFixFields)
                {
                    pranaMessage.FIXMessage.OptionalFixFields.Add(fixField.Tag);
                    if (fixField.ValidationRule != null)
                    {
                        if (fixField.ValidationRule.CheckIfRequiredField(pranaMessage))
                        {
                            extraReqdFixFields.Add(fixField);
                        }

                    }
                }
                string validation2 = ValidateForRequiredFields(pranaMessage, extraReqdFixFields);

                if (validation1 != string.Empty)
                    error = validation1;
                if (validation2 != string.Empty)
                {
                    if (error == string.Empty)
                        error = validation2;
                    else
                        error = validation1 + "and" + validation2;
                }


                foreach (FixFields fixField in _dictReqdFixFields)
                {
                    pranaMessage.FIXMessage.RequiredFixFields.Add(fixField.Tag);
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
            return error;


        }
        private string ValidateForRequiredFields(PranaMessage pranaMessage, List<FixFields> reqdFixFields)
        {
            string message = string.Empty;

            try
            {
                foreach (FixFields fixField in reqdFixFields)
                {
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(fixField.Tag))
                    {
                        string validationString = fixField.BasicValidation();
                        if (validationString != string.Empty)
                        {
                            message = validationString;
                            break;
                        }
                    }
                    else
                    {
                        // https://jira.nirvanasolutions.com:8443/browse/PRANA-34748
                        if (fixField.Tag == Prana.BusinessObjects.FIXConstants.TagLastShares)
                        {
                            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey((Prana.BusinessObjects.FIXConstants.TagExecTransType))
                                && pranaMessage.FIXMessage.ExternalInformation[Prana.BusinessObjects.FIXConstants.TagExecTransType].Value == "3")
                            {
                                continue;
                            }
                        }
                        else
                        {
                            message = "Missing " + fixField.FieldName;
                            break;
                        }
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

            return message;
        }
        public List<FixFields> RequiredFields
        {
            get
            {
                return _dictReqdFixFields;
            }
        }

        /// <summary>
        /// This property returns all optional fields
        /// </summary>
        public List<FixFields> OptionalFields
        {
            get
            {
                return _dictOptFixFields;
            }
        }
    }
}
