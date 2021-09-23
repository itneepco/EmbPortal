using Application.Uoms.Response;
using AutoMapper;
using Domian;

namespace Application.Mappings
{
    public class UomMappingProfile : Profile
    {
        public UomMappingProfile()
        {
            CreateMap<Uom, UomDto>()
                .ForMember(m => m.Dimension, opt => opt.MapFrom(p => p.Dimension.ToString()));
        }
    }
}