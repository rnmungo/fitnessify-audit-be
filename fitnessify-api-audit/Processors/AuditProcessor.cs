using AutoMapper;
using Audits.Business.Contracts;
using Audits.Domain.Entities;
using Audits.Infrastructure.Queues.Contracts;
using fitnessify_api_audit.DataTransferObjects.Request;

namespace fitnessify_api_audit.Processors
{
    public class AuditProcessor : IProcessMessageAsync<RequestPostAuditsDTO>
    {
        private readonly ILogger<AuditProcessor> _logger;
        private readonly IMapper _mapper;
        private readonly IAuditService _auditService;

        public AuditProcessor(ILogger<AuditProcessor> logger, IMapper mapper, IAuditService auditService)
        {
            _logger = logger;
            _mapper = mapper;
            _auditService = auditService;
        }

        public async Task ProcessMessage(RequestPostAuditsDTO message)
        {
            try
            {
                Audit audit = _mapper.Map<Audit>(message);
                await _auditService.SaveAudit(audit);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while processing message");
            }
        }
    }
}
