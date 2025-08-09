using AutoMapper;
using serkan_test1.Models;

namespace serkan_test1.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MusteriDto, Customer>();
        }
    }
}


