using AutoMapper;
using Domain.Entities;
using Shared.Response;

namespace Application.Mappings
{
   public class UomMappingProfile : Profile
    {
        public UomMappingProfile()
        {
            CreateMap<Uom, UomResponse>()
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.Dimension.ToString()));
        }
    }
}