using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AutoMapper;
using FeedApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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

        
        public List<ProductsViewModel> GetProductsPrices()
        {
 
            List<ProductDomainModel> productsDm = this.productsBusiness.getProductsPrices();

            var productsList = this.mapper.Map<List<ProductsViewModel>>(productsDm);

            return productsList;
        }

      
        public List<ProductsViewModel> GetMaterialsPrices()
        {
 
            List<ProductDomainModel> materialsDm = this.productsBusiness.getMaterialsPrices();

           var materialsList = this.mapper.Map<List<ProductsViewModel>>(materialsDm);

            return materialsList;
        }

        public HttpResponseMessage GetProductById(int id)
        {
            try
            {
                ProductDomainModel pDm = this.productsBusiness.getProductByID(id);
                var result = this.mapper.Map<ProductsViewModel>(pDm);

                if (result == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "المنتج عير متاح حاليا");
                }


                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (System.Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message.ToString());
            }

        }

       
    }
}
