namespace fitnessify_api_audit.DataTransferObjects.Response
{
    public class ResponseGetSearchAuditsDTO
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string Action { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string IP { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
