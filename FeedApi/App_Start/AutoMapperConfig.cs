using AmyzFactory.Models;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using AutoMapper;
using FeedApi.Models;
namespace FeedApi
{
    public static class AutoMapperConfig
    {

        public static IMapper Mapper { get; private set; }

        public static void init()
        {
            var config = new MapperConfiguration(cfg =>
              {
                  /*
                  
                  cfg.CreateMap<QuestionairViewModel, QuestionaireDomainModel > ()
                            .ForMember(dst => dst.UserName, src => src.MapFrom(e => e.FullName))
                            .ForMember(dst => dst.Email, src => src.MapFrom(e => e.Email))
                            .ForMember(dst => dst.Question, src => src.MapFrom(e => e.Question))
                            .ReverseMap();
             
                   **/

              });

            

            Mapper = config.CreateMapper();
        }

    }
}