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
        private readonly IProductsBusiness productsBusiness;
        private readonly IMapper mapper;

        public ProductController(IProductsBusiness _productsBusiness)
        {
            this.productsBusiness = _productsBusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }



        public IEnumerable<ProductsViewModel> GetProductsByCategoryID(int id)
        {
            List<ProductDomainModel> productsDm = this.productsBusiness.getProductsByCategoryID(id);

            var result = mapper.Map<List<ProductsViewModel>>(productsDm);

            return result;
        }

        public IEnumerable<ProductsViewModel> GetAllProducts(int pageNo, int displayLength)
        {
            List<ProductDomainModel> productsList = this.productsBusiness.getAllProducts(pageNo, displayLength);

            return this.mapper.Map<List<ProductsViewModel>>(productsList);
        }

        public IEnumerable<ProductsViewModel> GetAllProducts()
        {
            List<ProductDomainModel> productsList = this.productsBusiness.getAllProducts();

            return this.mapper.Map<List<ProductsViewModel>>(productsList);
        }
        public IEnumerable<ProductsViewModel> GetAllMaterials(int pageNo, int displayLength)
        {
            List<ProductDomainModel> productsDm = this.productsBusiness.getAllMaterials(pageNo, displayLength);

            var result = mapper.Map<List<ProductsViewModel>>(productsDm);

            return result;
        }
        public IEnumerable<ProductsViewModel> GetAllMaterials()
        {
            List<ProductDomainModel> productsDm = this.productsBusiness.getAllMaterials();

            var result = mapper.Map<List<ProductsViewModel>>(productsDm);

            return result;
        }


        public IEnumerable<ProductsViewModel> SearchInProducts(string word, int pageNo, int displayLength)
        {
            List<ProductDomainModel> listDm = this.productsBusiness.SearchInAllProducts(word, pageNo, displayLength);

            return this.mapper.Map<List<ProductsViewModel>>(listDm);
        }

        public IEnumerable<ProductsViewModel> SearchInMaterials(string word, int pageNo, int displayLength)
        {
            List<ProductDomainModel> listDm = this.productsBusiness.SearchInAllMaterials(word, pageNo, displayLength);

            return this.mapper.Map<List<ProductsViewModel>>(listDm);
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
            int count = this.productsBusiness.getSearchedProductCount(searchWord);
            return Ok(count);
        }


        public IHttpActionResult GetAllProductsCount()
        {
            int count = this.productsBusiness.getAllProductsCount();
            return Ok(count);
        }

        public IHttpActionResult GetSearchedMaterialsCount(string searchWord)
        {
            int count = this.productsBusiness.getSearchedMaterialsCount(searchWord);
            return Ok(count);
        }


        public IHttpActionResult GetAllMaterialsCount()
        {
            int count = this.productsBusiness.getAllMaterialsCount();
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
        public IHttpActionResult Create(ProductsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You enter null data!!"));
            }

            try
            {
                ProductDomainModel dm = this.mapper.Map<ProductDomainModel>(model);
                ResultDomainModel result = this.productsBusiness.createProduct(dm, null, null);

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
