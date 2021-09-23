using Application.Contractors.Response;
using AutoMapper;
using Domian;

namespace Application.Mappings
{
   public class ContractorMappingProfile : Profile
   {
      public ContractorMappingProfile()
      {
          CreateMap<Contractor, ContractorDto>();
      }
   }
}