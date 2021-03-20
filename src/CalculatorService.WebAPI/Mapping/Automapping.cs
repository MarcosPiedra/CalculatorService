using AutoMapper;
using CalculatorService.Domain.Models;
using CalculatorServices.WebAPI.DTOs;

namespace CalculatorServices.WebAPI.Mapping
{
    public class Automapping : Profile
    {
        public Automapping()
        {
            CreateMap<DivRequest, DivParams>();
            CreateMap<DivResult, DivResponse>();

            CreateMap<MultRequest, MultParams>();
            CreateMap<IntResult, MultResponse>()
                .ForMember(m => m.Product, o => o.MapFrom(r => r.Result));

            CreateMap<Operation, OperationResponse>();

            CreateMap<SqrtRequest, SqrtParams>();
            CreateMap<IntResult, SqrtResponse>()
                .ForMember(m => m.Square, o => o.MapFrom(r => r.Result)); 

            CreateMap<SubRequest, SubParams>();
            CreateMap<IntResult, SubResponse>()
                .ForMember(m => m.Difference, o => o.MapFrom(r => r.Result)); 

            CreateMap<SumRequest, SumParams>();
            CreateMap<IntResult, SumResponse>()
                .ForMember(m => m.Sum, o => o.MapFrom(r => r.Result));
        }
    }
}
