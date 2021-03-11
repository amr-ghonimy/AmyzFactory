using AmyzFeed.Business;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Repository.Infrastructure;
using AmyzFeed.Repository.Infrastructure.Contract;
using AutoMapper;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace AmyzApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IProductsBusiness, ProductsBusiness>();
            container.RegisterType<ICategoriesBusiness, CategoriesBusiness>();
            container.RegisterType<IImageBusiness, ImagesBusiness>();
            container.RegisterType<IOrdersBusiness, OrderBusiness>();
            container.RegisterType<IQuestionaireBusiness, QuestionaireBusiness>();
            container.RegisterType<IAuthBusiness, AuthBusiness>();
            container.RegisterType<IFeedsProgramBusiness, FeedsProgramBusiness>();




            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IMapper, Mapper>();
            container.RegisterType<IAddressesBusiness, AddressesBusiness>();



            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}