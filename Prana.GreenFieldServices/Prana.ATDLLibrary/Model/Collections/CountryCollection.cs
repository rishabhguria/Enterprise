using Prana.ATDLLibrary.Diagnostics;
using Prana.ATDLLibrary.Model.Elements;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.ATDLLibrary.Model.Reference;
using Prana.ATDLLibrary.Resources;
using System;
using System.Collections.Generic;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection for storing instances of Country_t.
    /// </summary>
    public class CountryCollection : HashSet<Country_t>
    {
        private readonly Region _region;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryCollection"/> class.
        /// </summary>
        /// <param name="region">The region.</param>
        public CountryCollection(Region_t region)
        {
            _region = region.Name;
        }

        /// <summary>
        /// Adds the specified item, validating that it belongs in the region specified when this collection was created.
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Add(Country_t item)
        {
            bool countryOkay = false;

            switch (_region)
            {
                case Region.AsiaPacificJapan:
                    countryOkay = Regions.AsiaPacificJapanCountries.Contains(item.CountryCode);
                    break;

                case Region.EuropeMiddleEastAfrica:
                    countryOkay = Regions.EuropeMiddleEastAfricaCountries.Contains(item.CountryCode);
                    break;

                case Region.TheAmericas:
                    countryOkay = Regions.TheAmericasCountries.Contains(item.CountryCode);
                    break;
            }

            if (!countryOkay)
                throw ThrowHelper.New<ArgumentException>(this, ErrorMessages.InvalidAttemptToAddCountryToRegion,
                    Enum.GetName(typeof(IsoCountryCode), item.CountryCode), Enum.GetName(typeof(Region), _region));

            base.Add(item);
        }
    }
}
