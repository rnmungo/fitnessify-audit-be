using Audits.Domain.Contracts;
using MongoDB.Bson.Serialization.Attributes;

namespace Audits.Domain
{
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>, ISoftDeleteEntity where TKey : struct
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public TKey Id { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        [BsonElement("DeletedAt")]
        public DateTime? DeletedAt { get; set; }
    }
}