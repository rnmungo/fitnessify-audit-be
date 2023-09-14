namespace Audits.Domain.Contracts
{
    public interface ISoftDeleteEntity
    {
        public DateTime? DeletedAt { get; set; }
    }
}
