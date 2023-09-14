using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Audits.Business.Contracts;
using Audits.Domain.Entities;
using fitnessify_api_audit.DataTransferObjects.Request;
using fitnessify_api_audit.DataTransferObjects.Response;

namespace fitnessify_api_audit.Controllers
{
    [ApiController]
    [Route("api/audit")]
    public class AuditController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuditService _auditService;

        public AuditController(IMapper mapper, IAuditService auditService)
        {
            _mapper = mapper;
            _auditService = auditService;
        }

        [HttpPost]
        public IActionResult SaveAudit([FromBody] RequestPostAuditsDTO body)
        {
            Audit audit = _mapper.Map<Audit>(body);
            _auditService.SaveAudit(audit);
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] RequestGetSearchAuditsDTO query)
        {
            Paged<Audit> audits = _auditService.GetAudits(query.SizeLimit, query.CurrentPage, query.From, query.To, query.ApplicationId, query.Action);
            Paged<ResponseGetSearchAuditsDTO> response = _mapper.Map<Paged<ResponseGetSearchAuditsDTO>>(audits);
            return Ok(response);
        }
    }
}
