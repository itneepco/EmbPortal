using AutoMapper;
using Domian;
using Shared.Response;

namespace Application.Mappings
{
   public class ContractorMappingProfile : Profile
   {
      public ContractorMappingProfile()
      {
          CreateMap<Contractor, ContractorResponse>();
      }
   }
}