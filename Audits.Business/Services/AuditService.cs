using System.Collections.Immutable;
using AutoMapper;
using Audits.Domain.Entities;
using Audits.Domain.Models;
using Audits.Infrastructure.BBDD.Contracts;
using Audits.Business.Contracts;

namespace Audits.Business.Services
{
    public class AuditService : IAuditService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<AuditModel, Guid> _auditRepository;

        public AuditService(IMapper mapper, IRepository<AuditModel, Guid> auditRepository)
        {
            _mapper = mapper;
            _auditRepository = auditRepository;
        }

        public Paged<Audit> GetAudits(int size, int currentPage, DateTime from, DateTime to, string applicationId, string action)
        {
            IQueryable<AuditModel> query = _auditRepository
                .GetAll()
                .Where(audit => audit.CreatedAt >= from && audit.CreatedAt <= to);
            
            if (!string.IsNullOrEmpty(applicationId))
            {
                query = query.Where(audit => audit.ApplicationId == applicationId);
            }

            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(audit => audit.Action == action);
            }

            IOrderedQueryable<AuditModel> orderedQuery = query.OrderByDescending(audit => audit.CreatedAt);
            IEnumerable<AuditModel> pagedElements = orderedQuery.Skip((currentPage - 1) * size).Take(size).ToImmutableArray();
            int totalElements = orderedQuery.Count();
            List<Audit> audits = _mapper.Map<List<Audit>>(pagedElements);
            return new Paged<Audit>()
            {
                Results = audits,
                CurrentPage = currentPage,
                SizeLimit = size,
                Total = totalElements,
            };
        }

        public async Task SaveAudit(Audit audit)
        {
            AuditModel auditModel = _mapper.Map<AuditModel>(audit);
            await _auditRepository.CreateAsync(auditModel);
        }
    }
}
