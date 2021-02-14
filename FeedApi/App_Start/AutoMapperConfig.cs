using AmyzFactory.Models;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using AutoMapper;
using FeedApi.Model;
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

               

                  cfg.CreateMap<QuestionairViewModel, QuestionaireDomainModel > ()
                            .ForMember(dst => dst.UserName, src => src.MapFrom(e => e.FirstNAme +" "+ e.LastNAme))
                            .ForMember(dst => dst.Email, src => src.MapFrom(e => e.Email))
                            .ForMember(dst => dst.Question, src => src.MapFrom(e => e.Question))
                            .ReverseMap();


              
                  cfg.CreateMap<DepartmentDomainModel, CategoryViewModel>()
                                  .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                                  .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
                                  .ForMember(dst => dst.SubCategoriesList, src => src.MapFrom(e => e.SubCategoriesList))
                                  .ForMember(dst => dst.creation_date, src => src.MapFrom(e => e.creation_date))
                                  .ForMember(dst => dst.updated_date, src => src.MapFrom(e => e.updated_date))
                                  .ForMember(dst => dst.visibility, src => src.MapFrom(e => e.visibility))
                                  .ReverseMap();
                  
                  cfg.CreateMap<ResultViewModel, ResultDomainModel>()
                                 .ForMember(dst => dst.IsSuccess, src => src.MapFrom(e => e.IsSuccess))
                                 .ForMember(dst => dst.Message, src => src.MapFrom(e => e.Message))
                                 .ReverseMap();

                  cfg.CreateMap<ProductDomainModel, ProductViewModel>()
                                .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                                .ForMember(dst => dst.CategoryName, src => src.MapFrom(e => e.CategoryName))
                                .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
                                .ForMember(dst => dst.isAvailable, src => src.MapFrom(e => e.isAvailable))
                                .ForMember(dst => dst.isVisible, src => src.MapFrom(e => e.isVisible))
                                .ForMember(dst => dst.Quantity, src => src.MapFrom(e => e.Quantity))
                                .ForMember(dst => dst.Price, src => src.MapFrom(e => e.Price))
                                .ForMember(dst => dst.Defenition, src => src.MapFrom(e => e.Definition))
                                .ForMember(dst => dst.Descriprion, src => src.MapFrom(e => e.Description))
                                .ForMember(dst => dst.CategoryId, src => src.MapFrom(e => e.CategoryId))
                                .ForMember(dst => dst.ImageFile, src => src.MapFrom(e => e.ImageFile))
                                .ReverseMap();

                  cfg.CreateMap<OrderDomainModel, OrderViewModel>()
                              .ForMember(dst => dst.OrderID, src => src.MapFrom(e => e.OrderID))
                              .ForMember(dst => dst.UserID, src => src.MapFrom(e => e.UserID))
                              .ForMember(dst => dst.OrderDate, src =>src.MapFrom(e => e.OrderDate.ToString()))
                              .ForMember(dst => dst.OrderNumber, src => src.MapFrom(e => e.OrderNumber))
                              .ForMember(dst => dst.OrderTotalPrice, src => src.MapFrom(e => e.OrderTotalPrice))
                              .ForMember(dst => dst.OrderDetailsList, src => src.MapFrom(e => e.OrderDetailsList))
                               .ForMember(dst => dst.ItemsCount, src => src.MapFrom(e => e.ItemsCount))
                              .ForMember(dst => dst.Notes, src => src.MapFrom(e => e.Notes))
                              .ForMember(dst => dst.Addreess, src => src.MapFrom(e => e.Addreess))

                              .ReverseMap();

                  cfg.CreateMap<OrderDetailsDomainModel, OrderDetailsViewModel>()
                             .ForMember(dst => dst.OrderID, src => src.MapFrom(e => e.OrderID))
                             .ForMember(dst => dst.ItemID, src => src.MapFrom(e => e.ItemID))
                             .ForMember(dst => dst.ItemName, src => src.MapFrom(e => e.ItemName))
                             .ForMember(dst => dst.ItemImg, src => src.MapFrom(e => e.ItemImg))
                             .ForMember(dst => dst.Price, src => src.MapFrom(e => e.Price))
                             .ForMember(dst => dst.Quantity, src => src.MapFrom(e => e.Quantity))
                             .ForMember(dst => dst.Total, src => src.MapFrom(e => e.Total))
                             .ReverseMap();



                  // New Api
                  cfg.CreateMap<CategoryDomainModel, CategoriesViewModel>()
                            .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                            .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
                            .ForMember(dst => dst.DepartmentID, src => src.MapFrom(e => e.DepartmentID))
                            .ReverseMap();

                  cfg.CreateMap<DepartmentDomainModel, DepartmentsViewModel>()
                           .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                           .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
                           .ForMember(dst => dst.ImageUrl, src => src.MapFrom(e => e.ImageUrl))
                           .ForMember(dst => dst.Categories, src => src.MapFrom(e => e.SubCategoriesList))
                           .ReverseMap();

                  cfg.CreateMap<ProductDomainModel, ProductsViewModel>()
                          .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                          .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
                          .ForMember(dst => dst.Definition, src => src.MapFrom(e => e.Definition))
                          .ForMember(dst => dst.Description, src => src.MapFrom(e => e.Description))
                          .ForMember(dst => dst.ImageUrl, src => src.MapFrom(e => e.ImageURL))
                           .ForMember(dst => dst.Price, src => src.MapFrom(e => e.Price))
                           .ForMember(dst => dst.CategoryName, src => src.MapFrom(e => e.CategoryName))
                           .ForMember(dst => dst.isVisible, src => src.MapFrom(e => e.isVisible))
                            .ForMember(dst => dst.Quantity, src => src.MapFrom(e => e.Quantity))
                            .ForMember(dst => dst.CategoryID, src => src.MapFrom(e => e.CategoryId))
                             .ReverseMap();

                  cfg.CreateMap<ImageDomainModel, ImagesViewModel>()
                          .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                          .ForMember(dst => dst.ImageUrl, src => src.MapFrom(e => e.ImageUrl))
                          .ForMember(dst => dst.Title, src => src.MapFrom(e => e.Title))
                          .ReverseMap();


                  cfg.CreateMap<TextsDomainModel, TextsViewModel>()
                          .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                          .ForMember(dst => dst.ImageUrl, src => src.MapFrom(e => e.ImageUrl))
                          .ForMember(dst => dst.Title, src => src.MapFrom(e => e.Title))
                         .ForMember(dst => dst.SubTitle, src => src.MapFrom(e => e.SubTitle))
                           .ForMember(dst => dst.Description, src => src.MapFrom(e => e.Description))
                          .ReverseMap();

                  cfg.CreateMap<ContactDomainModel, ContactViewModel>()
                       .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                       .ForMember(dst => dst.Value, src => src.MapFrom(e => e.Value))
                       .ForMember(dst => dst.Title, src => src.MapFrom(e => e.Title))
                       .ReverseMap();

                  cfg.CreateMap<PriceDomainModel, PricesViewModel>()
                     .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                     .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
                     .ForMember(dst => dst.CategoryName, src => src.MapFrom(e => e.CategoryName))
                     .ForMember(dst => dst.CategoryID, src => src.MapFrom(e => e.CategoryID))
                     .ForMember(dst => dst.Price, src => src.MapFrom(e => e.Price))
                     .ReverseMap();

                  cfg.CreateMap<TechnicalTextDomainModel, TechnicalTextViewModel>()
                        .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                        .ForMember(dst => dst.Title, src => src.MapFrom(e => e.Title))
                        .ForMember(dst => dst.Description, src => src.MapFrom(e => e.Description))
                        .ForMember(dst => dst.TechID, src => src.MapFrom(e => e.TechID))
                        .ReverseMap();

                  cfg.CreateMap<TechnicalSupportDomainModel, TechnicalSupportViewModel>()
                      .ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                       .ForMember(dst => dst.Name, src => src.MapFrom(e => e.Name))
                       .ForMember(dst => dst.TechTextsList, src => src.MapFrom(e => e.TechTextsList))
                      .ReverseMap();


              });

            

            Mapper = config.CreateMapper();
        }

    }
}