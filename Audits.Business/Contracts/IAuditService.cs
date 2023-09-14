using Audits.Domain.Entities;

namespace Audits.Business.Contracts
{
    public interface IAuditService
    {
        Task SaveAudit(Audit audit);
        Paged<Audit> GetAudits(int size, int currentPage, DateTime from, DateTime to, string applicationId, string action);
    }
}
