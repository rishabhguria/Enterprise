using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Enumerations;

namespace Prana.ATDLLibrary.Model.Elements
{
    public class Region_t
    {
        private CountryCollection _countries;

        public Region Name { get; set; }
        public Inclusion_t Inclusion { get; set; }

        public CountryCollection Countries 
        {
            get
            {
                // Lazy initialize as we can't use 'this' in constructor.
                if (_countries == null)
                    _countries = new CountryCollection(this);

                return _countries;
            }
        }
    }
}
