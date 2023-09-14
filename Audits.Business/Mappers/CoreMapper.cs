using AutoMapper;
using Audits.Domain.Entities;
using Audits.Domain.Models;

namespace Audits.Business.Mappers
{
    public sealed class CoreMapper : Profile
    {
        public CoreMapper()
        {
            CreateMap<Audit, AuditModel>()
                .ReverseMap();
        }
    }
}
