using System;
using Prana.ATDLLibrary.Resources;
using ThrowHelper = Prana.ATDLLibrary.Diagnostics.ThrowHelper;

namespace Prana.ATDLLibrary.Model.Elements.Support
{
    public abstract class EditEvaluator<T>
    {
        private Edit_t<T> _edit;
        private EditRef_t<T> _editRef;
        public EditRef_t<T> EditRef
        {
            get { return _editRef; }

            set
            {
                if (_edit != null)
                    throw ThrowHelper.New<InvalidOperationException>(this, ErrorMessages.BothEditAndEditRefSetOnObject, this.GetType().Name);

                _editRef = value;
            }
        }

        public Edit_t<T> Edit
        {
            get { return _edit; }

            set
            {
                if (_editRef != null)
                    throw ThrowHelper.New<InvalidOperationException>(this, ErrorMessages.BothEditAndEditRefSetOnObject, this.GetType().Name);

                _edit = value;
            }
        }
    }
}

