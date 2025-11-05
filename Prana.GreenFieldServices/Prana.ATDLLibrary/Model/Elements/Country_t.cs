using Prana.ATDLLibrary.Model.Enumerations;
using Prana.ATDLLibrary.Model.Reference;

namespace Prana.ATDLLibrary.Model.Elements
{
    public class Country_t
    {
        public IsoCountryCode CountryCode { get; set; }

        public Inclusion_t Inclusion { get; set; }
    }
}
