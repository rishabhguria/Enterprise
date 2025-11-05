namespace Prana.ServiceGateway.Models.RequestDto
{
    public class BaseRequestDto
    {
        public string CorrelationId { get; set; }

        public int CompanyUserID { get; set; }
    }

    public class DeleteHotKeyRequestDto : BaseRequestDto
    {
        public string CompanyUserHotKeyName { get; set; }
    }

    public class UserHotKeyPreferencesRequestDto : BaseRequestDto
    {
        public string HotKeyPreferenceElements { get; set; }
        public bool EnableBookMarkIcon { get; set; } 
        public bool HotKeyOrderChanged { get; set; }
        public bool TTTogglePreferenceForWeb { get; set; }
    }


    public class ResponseDto
    {
        public ResponseDto(string correlationId, dynamic data, bool isSuccess)
        {
            CorrelationId = correlationId;
            Data = data;
            IsSuccess = isSuccess;
        }

        public ResponseDto()
        {
                
        }

        public string CorrelationId { get; set; }
        public int CompanyUserID { get; set; }
        public dynamic Data { get; set; }

        public bool IsSuccess { get; set; }

        public ResponseDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
