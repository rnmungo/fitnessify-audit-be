using AutoMapper;
using Audits.Domain.Entities;
using fitnessify_api_audit.DataTransferObjects.Request;
using fitnessify_api_audit.DataTransferObjects.Response;

namespace fitnessify_api_audit.Mappers
{
    public sealed class UIMapper : Profile
    {
        public UIMapper()
        {
            CreateMap<RequestPostAuditsDTO, Audit>()
                .ReverseMap();
            CreateMap<ResponseGetSearchAuditsDTO, Audit>()
                .ReverseMap();
            CreateMap<Paged<ResponseGetSearchAuditsDTO>, Paged<Audit>>()
                .ReverseMap();
        }
    }
}
