using AmyzFactory.Models;
using AmyzFeed.Business.helpers;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Data;
using AmyzFeed.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Configuration;

namespace AmyzFeed.Business
{
    public class ProductsBusiness:IProductsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ProductRepository productRepository;
        private ResultDomainModel resultModel;
        private string baseUrl;


       
        public ProductsBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.productRepository = new ProductRepository(this.unitOfWork);
             this.baseUrl = WebConfigurationManager.AppSettings["baseUrl"];
         }


        private ResultDomainModel initResultModel(bool isSuccess, string message, object data = null)
        {
            resultModel.IsSuccess = isSuccess;
            resultModel.Message = message;
            resultModel.Data = data;
            return resultModel;
        }


        public ResultDomainModel uplaodImage(HttpRequest image, string savedFilePath, string folderResponse)
        {
 
            var imageExtention = Path.GetExtension(image.Files[0].FileName);
            if (imageExtention == "")
            {
                return initResultModel(false, "Please select image with english litters only");

            }

            string uniqueKey = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssf"), Guid.NewGuid().ToString()) + imageExtention;


 
            foreach (string file in image.Files)
            {
                var postedFile = image.Files[file];
                string filePath = savedFilePath + uniqueKey;
                postedFile.SaveAs(filePath);
            }


            return initResultModel(true, "Image Uploaded!", folderResponse+uniqueKey);
            
        }

        public ResultDomainModel createProduct(ProductDomainModel product)
        {
                // mapping ProductDomainModel to Product

                var prd = new Product()
                {
                    Name = product.Name,
                    CreationDate = DateTime.Now,
                    Image=product.ImageUrl,
                    Definition = product.Definition,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    Price = float.Parse(product.Price.ToString()),
                    Visibility = product.isVisible,
                    CategoryID = product.CategoryID
                };


               

                return confirmUploadProduct(prd);
            

        }

        private ResultDomainModel confirmUploadProduct(Product product)
        {
            try
            {
                productRepository.Insert(product);

                return initResultModel(true, "Product Upload Successfully");

            }
            catch (Exception e)
            {
                return initResultModel(false, e.Message.ToString());
            }
        }



        public ResultDomainModel deleteProduct(int productID)
        {
            try
            {
                Product obj = this.productRepository.SingleOrDefault(x => x.ID == productID);

                obj.IsDeleted = true;
                this.productRepository.Update(obj);
                return initResultModel(true,"Product Deleted Successfully!");
            }
            catch (Exception)
            {
                return initResultModel(true, "Product Deleted Failed !");
            }
        }

        public ResultDomainModel editProduct(ProductDomainModel newProduct)
        {

            Product oldProduct = this.productRepository.SingleOrDefault(x => x.ID == newProduct.Id);

            if (oldProduct == null)
            {
                return initResultModel(false, "Product is not exists in database refresh page");
            }

            var ifNewNameExists = this.productRepository.Exists(x => x.Name.Trim() == newProduct.Name.Trim()&&x.ID!=newProduct.Id);

            if (ifNewNameExists)
            {
                return initResultModel(false, "Product is already exists in database enter another name");
            }



            try
            {
                oldProduct.Name = newProduct.Name;
                oldProduct.Definition= newProduct.Definition;
                oldProduct.Description = newProduct.Description;
                oldProduct.UpdatedDate = DateTime.Now;
                oldProduct.Visibility = newProduct.isVisible;
                oldProduct.Quantity = newProduct.Quantity;
                oldProduct.CategoryID= newProduct.CategoryID;
                oldProduct.Price = float.Parse(newProduct.Price.ToString());
                oldProduct.Image = newProduct.ImageUrl;

                this.productRepository.Update(oldProduct);
                return initResultModel(true, "Product updated successfully");
            }
            catch (Exception e)
            {
                return initResultModel(true, "failed to update product :" + e.Message.ToString());
            }
        }

        public List<ProductDomainModel> getAllProducts(int pageNo, int displayLength, string role)
        {
            return productRepository.GetAll(generateQuery(role), "Category")
                  .OrderBy(x => x.CreationDate)
                    .Skip((pageNo - 1) * displayLength)
                    .Take(displayLength)
            .Select(x => new ProductDomainModel()
            {
                Id = x.ID,
                CategoryName = x.Category?.Name,
                Name = x.Name,
                Definition = x.Definition,
                Description = x.Description,
                CategoryID = x.CategoryID,
                ImageUrl = baseUrl + x.Image,
                isVisible = x.Visibility,
                Price = float.Parse(x.Price?.ToString()),
                Quantity = x.Quantity.Value
            }).ToList();
        }


        private Expression<Func<Product, bool>> generateQuery(string role)
        {
            Expression<Func<Product, bool>> whereCondition = null;
            switch (role)
            {
                case "Admins":
                    whereCondition = x => x.IsDeleted == false && x.Category.IsDeleted == false;
                    break;
                case "Users":
                    whereCondition = x => x.IsDeleted == false && x.Category.IsDeleted == false&&x.Visibility== true && x.Category.Visibility==true;
                    break;
                default:
                    whereCondition = x => x.IsDeleted == false && x.Category.IsDeleted == false && x.Visibility == true && x.Category.Visibility == true;
                    break;
            }

            return whereCondition;
        }


        private Expression<Func<Product, bool>> generateQueryByCategoryID(string role,int id)
        {
            Expression<Func<Product, bool>> whereCondition = null;
            switch (role)
            {
                case "Admins":
                    whereCondition = x => x.IsDeleted == false && x.Category.IsDeleted == false&&x.CategoryID==id;
                    break;
                case "Users":
                    whereCondition = x => x.IsDeleted == false && x.Category.IsDeleted == false && x.CategoryID == id &&x.Visibility == true && x.Category.Visibility == true;
                    break;
                default:
                    whereCondition = x => x.IsDeleted == false && x.Category.IsDeleted == false && x.Visibility == true && x.Category.Visibility == true;
                    break;
            }

            return whereCondition;
        }

        private Expression<Func<Product, bool>> generateSearchQuery(string role,string word)
        {

            Expression<Func<Product, bool>> whereCondition = null;
            switch (role)
            {
                case "Admins":
                    whereCondition = p => p.IsDeleted == false && p.Category.IsDeleted == false && p.Name.Contains(word) || p.Category.Name.Contains(word);
                   
                    break;
                case "Users":
                    whereCondition = p => p.IsDeleted == false && p.Category.IsDeleted == false &&p.Category.Visibility==true&&p.Visibility==true&& p.Name.Contains(word) || p.Category.Name.Contains(word);
                    break;
                default:
                    whereCondition = x => x.IsDeleted == false && x.Category.IsDeleted == false && x.Visibility == true && x.Category.Visibility == true;
                    break;
            }

            return whereCondition;
        }

        public ProductDomainModel getProductByID(int id)
        {

           Product product= this.productRepository.SingleOrDefault(x => x.ID == id, "Category");

            ProductDomainModel prdDm = new ProductDomainModel()
            {
                Id = product.ID,
                CategoryName = product.Category?.Name,
                Name = product.Name,
                CategoryID = product.CategoryID,
                Definition = product.Definition,
                Description = product.Description,
                ImageUrl = baseUrl + product.Image,
                isVisible = product.Visibility,
                Price = Double.Parse( product.Price?.ToString()),
                Quantity=product.Quantity.Value
            };

            return prdDm;
        }

        public List<ProductDomainModel> SearchInAllProducts(string searchWord, int pageNo, int displayLength, string role)
        {
 
            return productRepository.GetAll(generateSearchQuery(role,searchWord))
                .OrderBy(x => x.ID)
                    .Skip((pageNo - 1) * displayLength)
                    .Take(displayLength)
                .Select(x => new ProductDomainModel()
            {
                Id = x.ID,
                CategoryName = x.Category?.Name,
                Name = x.Name,
                Definition = x.Definition,
                Description = x.Description,
                CategoryID = x.CategoryID,
                ImageUrl = baseUrl + x.Image,
                isVisible = x.Visibility,
                Price = float.Parse(x.Price?.ToString()),
                Quantity = x.Quantity.Value
            }).ToList();
        }

        public int getAllProductsCount(string role)
        {
           return productRepository.GetAll(generateQuery(role)).Count();
        }

        public int getSearchedProductCount(string searchWord, string role)
        {
            return productRepository.GetAll(generateSearchQuery(role, searchWord)).Count();
        }

      
    
    

        public List<ProductDomainModel> getAllPrices(int pageNo, int displayLength, string role)
        {
            return this.productRepository.GetAll(generateQuery(role))
                 .OrderBy(x => x.ID)
                            .Skip((pageNo - 1) * displayLength)
                            .Take(displayLength)
                .Select(x => new ProductDomainModel
            {
                Id = x.ID,
                Name = x.Name,
                Price = float.Parse(x.Price.ToString())
            }).ToList();
         }

        public ResultDomainModel updatePrices(List<ProductDomainModel> list)
        {
            if (list != null || list.Count > 0)
            {
                foreach (var item in list)
                {
                    Product p = productRepository.SingleOrDefault(x => x.ID == item.Id);
                    if (p != null)
                    {
                        try
                        {
                            p.Price = float.Parse(item.Price.ToString());
                            this.productRepository.Update(p); 
                        }
                        catch (Exception)
                        {
                            return initResultModel(false,"There is proplem in product called: " + item.Name);
                        }
                    }else
                    {
                        return initResultModel(false, "There is proplem in product called: " + item.Name);
                    }
                }
                return initResultModel(true, "Products price updated successfully!");

            }
            return initResultModel(false, "There is proplem in products are empty!");

        }

        public List<ProductDomainModel> getProductsByCategoryID(int id, string role)
        {
            List<ProductDomainModel> products = this.productRepository.GetAll(generateQueryByCategoryID(role,id),"Category")
                .Select(x => new ProductDomainModel()
                {
                    Id = x.ID,
                    Name = x.Name,
                    CategoryID=x.CategoryID,
                    CategoryName=x.Category.Name,
                    Definition = x.Definition,
                    Description = x.Description,
                    ImageUrl = baseUrl + x.Image,
                    isVisible = x.Visibility,
                    Price = float.Parse(x.Price?.ToString()),
                    Quantity = x.Quantity.Value
                }).ToList();


            return products;
        }

        public List<PriceDomainModel> getProductsPrices(string role)
        {
            return this.productRepository.GetAll(generateQuery(role), "Category")
                                    .Select(x => new PriceDomainModel
                                    {
                                        Id = x.ID,
                                        Name = x.Name,
                                        ImageURL = baseUrl + x.Image,
                                        CategoryName=x.Category?.Name,
                                        CategoryID = x.CategoryID.Value,
                                        Price = x.Price.Value
                                    }).ToList();
        }

     
     
        public List<ProductDomainModel> getAllProducts(string role)
        {

            return this.productRepository.GetAll(generateQuery(role), "Category")
                           .Select(x => new ProductDomainModel
                           {
                               Id = x.ID,
                               Name = x.Name,
                               CategoryName=x.Category.Name,
                               CategoryID=x.CategoryID,
                               Definition = x.Definition,
                               Description = x.Description,
                               ImageUrl = baseUrl + x.Image,
                               isVisible = x.Visibility,
                               Price = float.Parse(x.Price?.ToString()),
                               Quantity = x.Quantity.Value
                           }).ToList();
        }

        public List<ProductDomainModel> SearchInAllProducts(string searchWord, string role)
        {
            return productRepository.GetAll(generateSearchQuery(role,searchWord))
                         .OrderBy(x => x.ID)
                         .Select(x => new ProductDomainModel()
                         {
                             Id = x.ID,
                             CategoryName = x.Category?.Name,
                             Name = x.Name,
                             Definition = x.Definition,
                             Description = x.Description,
                             CategoryID = x.CategoryID,
                             ImageUrl = baseUrl + x.Image,
                             isVisible = x.Visibility,
                             Price = float.Parse(x.Price?.ToString()),
                             Quantity = x.Quantity.Value
                         }).ToList();
        }

        public ResultDomainModel isProductExists(string name)
        {
            var ifCategoryExists = this.productRepository.SingleOrDefault(m => m.Name.Trim().ToLower() == name.Trim().ToLower() && m.IsDeleted == false&&m.Category.IsDeleted==false,"Category");

            if (ifCategoryExists == null)
            {
                return new ResultDomainModel(false, "Product not exists");
            }
            else
            {
                return new ResultDomainModel(true, "Product is exists");

            }
        }
    }
}
