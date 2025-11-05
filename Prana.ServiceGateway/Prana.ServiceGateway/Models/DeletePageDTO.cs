namespace Prana.ServiceGateway.Models
{
    public class DeletePageDTO
    {
        public string pageId { get; set; }
        public List<String> viewIds { get; set; }
        public string title { get; set; }
    }
}
