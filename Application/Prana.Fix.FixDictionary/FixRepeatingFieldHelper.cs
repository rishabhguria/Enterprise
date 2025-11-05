using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Fix.FixDictionary
{
    public class FixRepeatingFieldHelper
    {
        /// <summary>
        /// Updates a repeating group within a FIX message.
        /// </summary>
        /// <param name="msgFields">The collection of message fields.</param>
        /// <param name="orderdedRepeatingFields">The ordered repeating fields.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="toTag">The target tag to update.</param>
        public static void UpdateGroup(RepeatingMessageFieldCollection msgFields, RepeatingFixField orderdedRepeatingFields, string value, string toTag)
        {
            try
            {
                int tagPosition = 0;
                List<MessageField> updatedMsgFields = msgFields.MessageFields;
                // Iterate through fields in the specified order
                foreach (RepeatingFixField field in orderdedRepeatingFields.RepeatingFixFields.Values.ToList())
                {
                    if (updatedMsgFields.Count == tagPosition)
                    {
                        // Insert the new field if position is reached
                        updatedMsgFields.Insert(tagPosition, new MessageField(toTag, value));
                        break;
                    }
                    if (updatedMsgFields[tagPosition].Tag == toTag)
                    {
                        // Update existing field value
                        updatedMsgFields[tagPosition].Value = value;
                        break;
                    }
                    if (updatedMsgFields[tagPosition].Tag == field.Tag)
                    {
                        // Move to the next position
                        tagPosition++;
                    }
                    else if (field.Tag == toTag)
                    {
                        // Insert the new field if it matches the desired tag
                        updatedMsgFields.Insert(tagPosition, new MessageField(toTag, value));
                        break;
                    }
                    else
                    {
                        tagPosition++;
                    }
                }
                msgFields.UpdateCollection(updatedMsgFields);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
