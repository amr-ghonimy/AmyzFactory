using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Data;
using AmyzFeed.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Business
{
    public class ProductsBusinessUsers : IProductsBusinessUsers
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ProductRepository productRepository;
        private ResultDomainModel resultModel;


        public ProductsBusinessUsers(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.productRepository = new ProductRepository(this.unitOfWork);
        }




        private ResultDomainModel initResultModel(bool isSuccess, string message)
        {
            resultModel.IsSuccess = isSuccess;
            resultModel.Message = message;
            return resultModel;
        }




        public List<ProductDomainModel> getAllProducts()
        {
            List<ProductDomainModel> result = new List<ProductDomainModel>();
            List<Product> allProducts = this.productRepository.GetAll().ToList();

            if(allProducts!=null && allProducts.Count > 0)
            {
                return result;
            }

            foreach (var item in allProducts)
            {
                result.Add(new ProductDomainModel() {
                    CategoryId=item.CategoryID
                    
                });
            }


            return result;

        }
        

    
        public ProductDomainModel getProductByID(int id)
        {
            Product product = this.productRepository.SingleOrDefault(x => x.ID == id&&x.Visibility==true);

            if (product == null)
            {
                return null;
            }

            ProductDomainModel prdDm = new ProductDomainModel()
            {
                Id = product.ID,
                CategoryName = product.Category?.Name,
                Name = product.Name,
                CategoryId = product.CategoryID,
                Defenition = product.Definition,
                Descriprion = product.Description,
                ImageURL = product.Image,
                isVisible = product.Visibility,
                Price = Double.Parse(product.Price?.ToString()),
                Quantity = product.Quantity.Value
            };

            return prdDm;
        }

        public List<ProductDomainModel> getProductsByCategory(int categoryId)
        {
            return productRepository.GetAll(x => x.CategoryID == categoryId && x.Visibility == true &&x.IsDeleted==false).Select(x => new ProductDomainModel()
            {
                Id = x.ID,
                CategoryName = x.Category?.Name,
                Name = x.Name,
                Defenition = x.Definition,
                Descriprion = x.Description,
                CategoryId = x.CategoryID,
                ImageURL = x.Image,
                Price = float.Parse(x.Price?.ToString()),
                Quantity = x.Quantity.Value
            }).ToList();
        }

        public int getAllProductsCount()
        {
            return productRepository.GetAll(x => x.CategoryID != null && x.Visibility==true).Count();

        }

    

        public int getAllMaterialsCount()
        {
            return productRepository.GetAll(x => x.CategoryID == null&&x.Visibility==true).Count();
        }

    

        public List<ProductDomainModel> getMaterials()
        {
            List<Product> materials = this.productRepository.GetAll(x => x.CategoryID == null && x.Visibility == true).ToList();
            List<ProductDomainModel> materialsDm = new List<ProductDomainModel>();
            if (materials != null && materials.Count > 0)
            {
                foreach (var item in materials)
                {
                    materialsDm.Add(new ProductDomainModel()
                    {
                        Id = item.ID,
                        Name = item.Name,
                        Defenition = item.Definition,
                        Descriprion = item.Description,
                        ImageURL = item.Image,
                        Price = Double.Parse(item.Price.ToString()),
                        Quantity = item.Quantity.Value

                    });
                }
            }

            return materialsDm;
        }

        public List<ProductDomainModel> getProductsPrices()
        {
            return this.productRepository.GetAll(z => z.Visibility == true && z.Price>0 && z.CategoryID != null)
                                    .Select(x => new ProductDomainModel
                                    {
                                        Id = x.ID,
                                        Name = x.Name,
                                        Price = float.Parse(x.Price.ToString())
                                    }).ToList();
        }

        public List<ProductDomainModel> getMaterialsPrices()
        {
            return this.productRepository.GetAll(z => z.Visibility == true && z.Price > 0 && z.CategoryID == null)
                                          .Select(x => new ProductDomainModel
                                             {
                                                 Id = x.ID,
                                                 Name = x.Name,
                                                 Price = float.Parse(x.Price.ToString())
                                             }).ToList();
        }
    }
}
