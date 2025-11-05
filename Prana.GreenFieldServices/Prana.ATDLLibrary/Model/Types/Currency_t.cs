using Prana.ATDLLibrary.Model.Reference;
using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'string field representing a currency type using ISO 4217 Currency code (3 character) values (see Appendix 6-A).'
    /// </summary>
    public class Currency_t : AtdlValueType<IsoCurrencyCode>
    {
    }
}
