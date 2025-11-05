using Prana.ATDLLibrary.Diagnostics.Exceptions;
using Prana.ATDLLibrary.Model.Elements;
using Prana.ATDLLibrary.Resources;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using ThrowHelper = Prana.ATDLLibrary.Diagnostics.ThrowHelper;

namespace Prana.ATDLLibrary.Model.Collections
{
    public class ListItemCollection : KeyedCollection<string, ListItem_t>
    {
        public new void Add(ListItem_t item)
        {
            try
            {
                base.Add(item);
            }
            catch (ArgumentException ex)
            {
                throw ThrowHelper.New<DuplicateKeyException>(this, ex, ErrorMessages.AttemptToAddDuplicateKey,
                    item.EnumId, "ListItems");
            }
        }

        public string[] EnumIds
        {
            get { return (from item in Items select item.EnumId).ToArray<string>(); }
        }

        public bool HasItems
        {
            get { return Count > 0; }
        }

        protected override string GetKeyForItem(ListItem_t item)
        {
            return item.EnumId;
        }
    }
}
