using AmyzApi.Helpers;
using AmyzFactory.Models;
using AmyzFeed.AmyzApi.Helpers;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
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
 
        public ProductController(IProductsBusiness _productsBusiness)
        {
            this.productsBusiness = _productsBusiness;
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



        // Testing 
        public IEnumerable<ProductDomainModel> GetProductsByCategoryID(int id)
        {
            
            List<ProductDomainModel> productsDm = this.productsBusiness.getProductsByCategoryID(id, GetRole());

            return productsDm;
        }

        // end testing
        public IEnumerable<ProductDomainModel> GetAllProducts(int pageNo, int displayLength)
        {
            List<ProductDomainModel> productsList = this.productsBusiness.getAllProducts(pageNo, displayLength, GetRole());

            return productsList;
        }

        public IEnumerable<ProductDomainModel> GetAllProducts()
        {
            List<ProductDomainModel> productsList = this.productsBusiness.getAllProducts(GetRole());

            return productsList;
        }


        [HttpGet]
        public IEnumerable<ProductDomainModel> SearchInProducts(string word, int pageNo, int displayLength)
        {
            List<ProductDomainModel> listDm = this.productsBusiness.SearchInAllProducts(word, pageNo, displayLength, GetRole());

            return listDm;
        }


        [HttpGet]
        public IEnumerable<ProductDomainModel> SearchInProducts(string word)
        {
            List<ProductDomainModel> listDm = this.productsBusiness.SearchInAllProducts(word, GetRole());

            return listDm;
        }




        public IEnumerable<PriceDomainModel> GetProductsPrices()
        {
            List<PriceDomainModel> productsDm = this.productsBusiness.getProductsPrices(GetRole());
            return productsDm;
        }

        public IEnumerable<ProductDomainModel> GetAllPrices(int pageNo, int displayLengt)
        {
            List<ProductDomainModel> listDm = this.productsBusiness.getAllPrices(pageNo, displayLengt, GetRole());

            return listDm;
        }

        [HttpPut]
        public IHttpActionResult UpdatePrices(List<ProductDomainModel> products)
        {
            ResultDomainModel result = this.productsBusiness.updatePrices(products);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.BadRequest, result);
        }

        public IHttpActionResult GetProductById(int id)
        {
            try
            {
                ProductDomainModel pDm = this.productsBusiness.getProductByID(id,GetRole());
 
                if (pDm == null)
                {
                    return Content(HttpStatusCode.NotFound, new ResultDomainModel(false, "المنتج عير متاح حاليا"));
                }

                return Ok(new ResultDomainModel(true, "category exist", id, Data: pDm));

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
                }
                else
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


            result = this.productsBusiness.uplaodImage(httpRequest, Constans.productsImageFolderPath, Constans.productsImageResponse);

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
        public IHttpActionResult Create(ProductDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You enter null data!!"));
            }

            if (string.IsNullOrEmpty(model.ImageUrl))
            {
                model.ImageUrl = Constans.defaultProductImage;
            }

            ResultDomainModel result = new ResultDomainModel();

            try
            {
 
                result = this.productsBusiness.createProduct(model);

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
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, e.Message));
            }

        }


        [HttpPut]
        public IHttpActionResult EditProduct(ProductDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You enter null data"));
            }
            try
            {
 
                ResultDomainModel result = this.productsBusiness.editProduct(model);

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
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, e.Message));
            }
        }
    }
}
