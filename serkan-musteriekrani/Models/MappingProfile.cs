using AutoMapper;
using serkan_test1.Models;

namespace serkan_test1.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MusteriDto, Customer>();
        }
    }
}
