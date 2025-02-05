namespace fitnessify_api_audit.DataTransferObjects.Request
{
    public class RequestGetSearchAuditsDTO
    {
        public int CurrentPage { get; set; } = 1;
        public int SizeLimit { get; set; } = 10;
        public DateTime From { get; set; } = DateTime.Now;
        public DateTime To { get; set; } = DateTime.Now;
        public string Action { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
    }
}
