namespace Audits.Domain.Contracts
{
    public interface IBaseEntity<TKey> where TKey : struct
    {
        TKey Id { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
