namespace Prana.ServiceGateway.Models.RequestDto
{
    public class SaveHotKeyPreferencesRequestDto : BaseRequestDto
    {
        public int CompanyUserHotKeyID { get; set; }
        public string HotKeyPreferenceNameValue { get; set; }
        public string CompanyUserHotKeyName { get; set; }
        public bool IsFavourites { get; set; }
        public int? HotKeySequence { get; set; }
        public string Module { get; set; }
        public string HotButtonType { get; set; }

        public List<HotKeySequenceRequestDto> HotKeySequenceOrder { get; set; }

        public override string ToString()
        {
            return
                   $"CompanyUserID: {CompanyUserID}, CompanyUserHotKeyName:{CompanyUserHotKeyName}, " +
                   $"IsFavourites: {IsFavourites}, HotKeySequence: {HotKeySequence},Module: {Module},HotButtonType: {HotButtonType},HotKeyPreferenceNameValue: {HotKeyPreferenceNameValue}";
        }
    }

    public class HotKeySequenceRequestDto : BaseRequestDto
    {
        public int CompanyUserHotKeyID { get; set; }
        public int HotKeySequence { get; set; }

    }
}
