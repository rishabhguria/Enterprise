using Prana.BusinessObjects.AppConstants;
using System;
using System.Xml.Serialization;
namespace Prana.BusinessObjects
{
    public class PostTradeEnums
    {
        public enum CloseTradeMethodology
        {
            [XmlEnumAttribute("0")]
            Manual = 0,
            [XmlEnumAttribute("1")]
            Automatic = 1
        }

        public enum CloseTradeAlogrithm
        {
            [XmlEnumAttribute("0")]
            NONE,
            [XmlEnumAttribute("1")]
            LIFO,
            [XmlEnumAttribute("2")]
            FIFO,
            [XmlEnumAttribute("3")]
            HIFO,
            [XmlEnumAttribute("4")]
            MFIFO,
            [XmlEnumAttribute("5")]
            PRESET,
            [XmlEnumAttribute("6")]
            ETM,
            [XmlEnumAttribute("7")]
            LOWCOST,
            [XmlEnumAttribute("8")]
            ACA,
            [XmlEnumAttribute("9")]
            HIHO,
            [XmlEnumAttribute("10")]
            BTAX,
            [XmlEnumAttribute("11")]
            TAXADV,
            [XmlEnumAttribute("12")]
            MANUAL,
            [XmlEnumAttribute("13")]
            HCST,
            [XmlEnumAttribute("14")]
            Multiple
        }

        public enum Status
        {
            CorporateAction,
            Closed,
            [EnumDescription("Exercise/Assign")]
            Exercise,
            IsExercised,
            CostBasisAdjustment,
            [EnumDescription("Exercise/Assign - Manually Modified")]
            ExerciseAssignManually,
            None
        }
        [Serializable]
        public enum DateType
        {
            AuecLocalDate,
            ProcessDate,
            OriginalPurchaseDate,

        }

        [Serializable]
        public enum SecondarySortCriteria
        {
            None,
            AvgPxASC,
            AvgPxDESC,
            SamePxAvgPxASC,
            SamePxAvgPxDESC,
            OrderSequenceASC,
            OrderSequenceDESC
        }
        [Serializable]
        public enum CashSettleType
        {
            Cost,
            ZeroPrice,
            ClosingDateSpotPx, // For FxForwards (PNL settle)
            DeliverFXAtCost,
            DeliverFXAtCostandPNLAtClosingDateSpotPx// For FxForwards (actual currency exchange)
        }

        public enum ClosingField
        {
            [XmlEnumAttribute("0")]
            Default = 0,
            [XmlEnumAttribute("1")]
            AvgPrice = 1,
            [XmlEnumAttribute("2")]
            UnitCost = 2,
            [XmlEnumAttribute("3")]
            AvgPriceBase = 3,
            [XmlEnumAttribute("4")]
            UnitCostBase = 4
        }

    }
}
