using System.ComponentModel.DataAnnotations;

namespace fitnessify_api_audit.DataTransferObjects.Request
{
    public class RequestPostAuditsDTO
    {
        [Required(ErrorMessage = "El campo Action es requerido")]
        public string Action { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo ApplicationId es requerido")]
        public string ApplicationId { get; set; } = string.Empty;

        public string IP { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Method es requerido")]
        public string Method { get; set; } = string.Empty;

        public string RequestBody { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Resource es requerido")]
        public string Resource { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo UserAgent es requerido")]
        public string UserAgent { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo UserId es requerido")]
        public string UserId { get; set; } = string.Empty;
    }
}
