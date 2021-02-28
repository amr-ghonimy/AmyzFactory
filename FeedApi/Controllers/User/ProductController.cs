using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AmyzFeed.FeedApi.Helpers;
using AutoMapper;
using FeedApi.Helpers;
using FeedApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class ProductController : ApiController
    {
        // GET: Products
        private readonly IProductsBusiness productsBusiness;
        private readonly IMapper mapper;

        public ProductController(IProductsBusiness _productsBusiness)
        {
            this.productsBusiness = _productsBusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }



        private string GetRole()
        {
            string role = "";

            HttpRequestMessage request = this.ActionContext.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                role = "Users";
                return role;
            }

            if (!string.IsNullOrEmpty(authorization.Parameter))
            {
               string token = authorization.Parameter;

                role = TokenManager.GetRoleByToken(token);

                if (role == null)
                {
                    role = "Users";
                }
            }

            return role;
        }

        public IEnumerable<ProductsViewModel> GetProductsByCategoryID(int id)
        {
           


            List<ProductDomainModel> productsDm = this.productsBusiness.getProductsByCategoryID(id, GetRole());

            var result = mapper.Map<List<ProductsViewModel>>(productsDm);

            return result;
        }

        public IEnumerable<ProductsViewModel> GetAllProducts(int pageNo, int displayLength)
        {
            List<ProductDomainModel> productsList = this.productsBusiness.getAllProducts(pageNo, displayLength, GetRole());

            return this.mapper.Map<List<ProductsViewModel>>(productsList);
        }

        public IEnumerable<ProductsViewModel> GetAllProducts()
        {
            List<ProductDomainModel> productsList = this.productsBusiness.getAllProducts(GetRole());

            return this.mapper.Map<List<ProductsViewModel>>(productsList);
        }
        

        public IEnumerable<ProductsViewModel> SearchInProducts(string word, int pageNo, int displayLength)
        {
            List<ProductDomainModel> listDm = this.productsBusiness.SearchInAllProducts(word, pageNo, displayLength, GetRole());

            return this.mapper.Map<List<ProductsViewModel>>(listDm);
        }


        [HttpGet]
        public IEnumerable<ProductsViewModel> SearchInProducts(string word)
        {
            List<ProductDomainModel> listDm = this.productsBusiness.SearchInAllProducts(word, GetRole());

            return this.mapper.Map<List<ProductsViewModel>>(listDm);
        }



 
        public IEnumerable<PricesViewModel> GetProductsPrices()
        {

            List<PriceDomainModel> productsDm = this.productsBusiness.getProductsPrices(GetRole());

            var productsList = this.mapper.Map<List<PricesViewModel>>(productsDm);

            return productsList;
        }


        

        public IHttpActionResult GetProductById(int id)
        {
            try
            {
                ProductDomainModel pDm = this.productsBusiness.getProductByID(id);
                var result = this.mapper.Map<ProductsViewModel>(pDm);

                if (result == null)
                {
                    return Content(HttpStatusCode.NotFound, new ResultDomainModel(false, "المنتج عير متاح حاليا"));
                }

                return Ok(new ResultDomainModel(true, "category exist",id, Data: result));

            }
            catch (System.Exception ex)
            {
                return Content(HttpStatusCode.NotFound, new ResultDomainModel(false, ex.Message.ToString()));
            }

        }

        public IHttpActionResult GetSearchedProductCount(string searchWord)
        {
            int count = this.productsBusiness.getSearchedProductCount(searchWord, GetRole());
            return Ok(count);
        }


        public IHttpActionResult GetAllProductsCount()
        {
            int count = this.productsBusiness.getAllProductsCount(GetRole());
            return Ok(count);
        }

     
 



        [HttpDelete]
        public IHttpActionResult DeleteProduct(int id)
        {
            if (id == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "Item with Id = " + id + " Not Found", id));
            }
            try
            {
                ResultDomainModel result = this.productsBusiness.deleteProduct(id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                } else
                {
                    return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "Item with Id = " + id + " Not Found", id));
                }
            }
            catch (System.Exception e)
            {

                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, e.Message, id));

            }

        }


        [HttpPost]
        public IHttpActionResult UploadImage()
        {
            ResultDomainModel result = new ResultDomainModel();

            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count < 1)
            {
                result.IsSuccess = false;
                result.Message = "حدث مشكلة فى رفع الصورة";
                return Content(HttpStatusCode.BadRequest, result);
            }


            result = this.productsBusiness.uplaodImage(httpRequest, Constans.productsImageFolderPath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }


        [HttpPost]
        public IHttpActionResult Create(ProductsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You enter null data!!"));
            }

            ResultDomainModel result = new ResultDomainModel();

            try
            {
                ProductDomainModel dm = this.mapper.Map<ProductDomainModel>(model);

                 result = this.productsBusiness.createProduct(dm);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
            catch (System.Exception e)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false,e.Message));
            }

        }


        [HttpPut]
        public IHttpActionResult EditProduct(ProductsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You enter null data"));
            }
            try
            {
                ProductDomainModel dm = this.mapper.Map<ProductDomainModel>(model);

                ResultDomainModel result=this.productsBusiness.editProduct(dm);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }

            }
            catch (System.Exception e)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false,e.Message));
            }
        }
    }
}
