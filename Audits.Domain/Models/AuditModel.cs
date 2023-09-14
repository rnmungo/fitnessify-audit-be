using MongoDB.Bson.Serialization.Attributes;
using Audits.Domain.Models.Attributes;

namespace Audits.Domain.Models
{
    [BsonCollection("Audits")]
    public class AuditModel : BaseEntity<Guid>
    {
        [BsonElement("Action")]
        public string Action { get; set; } = string.Empty;

        [BsonElement("ApplicationId")]
        public string ApplicationId { get; set; } = string.Empty;

        [BsonElement("IP")]
        public string IP { get; set; } = string.Empty;

        [BsonElement("Method")]
        public string Method { get; set; } = string.Empty;

        [BsonElement("RequestBody")]
        public string RequestBody { get; set; } = string.Empty;

        [BsonElement("Resource")]
        public string Resource { get; set; } = string.Empty;

        [BsonElement("UserAgent")]
        public string UserAgent { get; set; } = string.Empty;

        [BsonElement("UserId")]
        public string UserId { get; set; } = string.Empty;
    }
}
