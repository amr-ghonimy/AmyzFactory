using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AutoMapper;
using FeedApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class ProductController : ApiController
    {
        // GET: Products
        private readonly IProductsBusinessUsers productsBusiness;
        private readonly IMapper mapper;

        public ProductController(IProductsBusinessUsers _productsBusiness)
        {
            this.productsBusiness = _productsBusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }



        public IEnumerable<ProductsViewModel> GetProductsByCategoryID(int id)
        {
            List<ProductDomainModel> productsDm = this.productsBusiness.getProductsByCategory(id);

            var result = mapper.Map<List<ProductsViewModel>>(productsDm);

            return result;
        }

        public IEnumerable<ProductsViewModel> GetMaterials()
        {
            List<ProductDomainModel> productsDm = this.productsBusiness.getMaterials();

            var result = mapper.Map<List<ProductsViewModel>>(productsDm);

            return result;
        }

        
        public IEnumerable<PricesViewModel> GetProductsPrices()
        {
 
            List<PriceDomainModel> productsDm = this.productsBusiness.getProductsPrices();

            var productsList = this.mapper.Map<List<PricesViewModel>>(productsDm);

            return productsList;
        }

      
        public IEnumerable<PricesViewModel> GetMaterialsPrices()
        {
 
            List<PriceDomainModel> materialsDm = this.productsBusiness.getMaterialsPrices();

           var materialsList = this.mapper.Map<List<PricesViewModel>>(materialsDm);

            return materialsList;
        }

        public IHttpActionResult GetProductById(int id)
        {
            try
            {
                ProductDomainModel pDm = this.productsBusiness.getProductByID(id);
                var result = this.mapper.Map<ProductsViewModel>(pDm);

                if (result == null)
                {
                    return Content(HttpStatusCode.NotFound, "المنتج عير متاح حاليا");
                }


                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message.ToString());
            }

        }

       
    }
}
