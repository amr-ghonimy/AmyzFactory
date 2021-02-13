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
using System.Web;

namespace AmyzFeed.Business
{
    public class ProductsBusiness:IProductsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ProductRepository productRepository;
        private ResultDomainModel resultModel;
        

        public ProductsBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
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


        private ResultDomainModel uplaodImage(HttpPostedFileWrapper image, string savedFilePath)
        {
 
            var imageExtention = Path.GetExtension(image.FileName);
            string title = Guid.NewGuid().ToString() + imageExtention;

            try
            {
                image.SaveAs(savedFilePath + title);
                return initResultModel(true, title);
            }
            catch (Exception e)
            {
                return initResultModel(false, e.Message.ToString());
            }
            
        }

        public ResultDomainModel createProduct(ProductDomainModel product, HttpPostedFileWrapper productImage, string serverPathToUploadImage)
        {
            // check if product is already exists
            var ifProducExists = this.productRepository.SingleOrDefault(m => m.Name.ToLower().Trim() == product.Name.ToLower().Trim());

            if (ifProducExists!=null)
            {
                return initResultModel(false, "The Product is Already Exists please enter another name");
            }
            else
            {

                // mapping ProductDomainModel to Product

                var prd = new Product()
                {
                    Name = product.Name,
                    CreationDate = DateTime.Now,
                    Definition = product.Definition,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    Price = float.Parse(product.Price.ToString()),
                    Visibility = product.isVisible,
                    CategoryID = product.CategoryId
                };


                if (productImage != null)
                {
                    // uplaod Image
                    var uploadImgResult = this.uplaodImage(productImage, serverPathToUploadImage);

                    bool isSuccess = uploadImgResult.IsSuccess;

                    if (isSuccess)
                    {
                        // here title of image not message
                        prd.Image = uploadImgResult.Message;
                    }
                }

                return confirmUploadProduct(prd);
            }

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
                oldProduct.CategoryID= newProduct.CategoryId;
                oldProduct.Price = float.Parse(newProduct.Price.ToString());
                oldProduct.Image = newProduct.ImageURL;

                this.productRepository.Update(oldProduct);
                return initResultModel(true, "Product updated successfully");
            }
            catch (Exception e)
            {
                return initResultModel(true, "failed to update product :" + e.Message.ToString());
            }
        }

        public List<ProductDomainModel> getAllProducts(int pageNo, int displayLength)
        {
            return productRepository.GetAll(p => p.CategoryID > 0 && p.IsDeleted==false, "Category")
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
                CategoryId = x.CategoryID,
                ImageURL = x.Image != null ? Constans.ServerFile + x.Image : Constans.LogoPath,
                isVisible = x.Visibility,
                Price = float.Parse(x.Price?.ToString()),
                Quantity = x.Quantity.Value
            }).ToList();
        }

     
        public ProductDomainModel getProductByID(int id)
        {
           Product product= this.productRepository.SingleOrDefault(x => x.ID == id);

            ProductDomainModel prdDm = new ProductDomainModel()
            {
                Id = product.ID,
                CategoryName = product.Category?.Name,
                Name = product.Name,
                CategoryId = product.CategoryID,
                Definition = product.Definition,
                Description = product.Description,
                ImageURL = product.Image != null ? Constans.ServerFile + product.Image : Constans.LogoPath,
                isVisible = product.Visibility,
                Price = Double.Parse( product.Price?.ToString()),
                Quantity=product.Quantity.Value
            };

            return prdDm;
        }

        public List<ProductDomainModel> SearchInAllProducts(string searchWord, int pageNo, int displayLength)
        {
 
            return productRepository.GetAll(p => p.CategoryID > 0 && p.IsDeleted == false && p.Name.Contains(searchWord)||p.Category.Name.Contains(searchWord))
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
                CategoryId = x.CategoryID,
                ImageURL = x.Image != null ? Constans.ServerFile + x.Image : Constans.LogoPath,
                isVisible = x.Visibility,
                Price = float.Parse(x.Price?.ToString()),
                Quantity = x.Quantity.Value
            }).ToList();
        }

        public int getAllProductsCount()
        {
           return productRepository.GetAll(x=>x.CategoryID > 0 && x.IsDeleted == false).Count();
        }

        public int getSearchedProductCount(string searchWord)
        {
            return productRepository.GetAll(p => p.CategoryID >0 && p.IsDeleted == false && p.Name.Contains(searchWord) || p.Category.Name.Contains(searchWord)).Count();
         }

        public List<ProductDomainModel> getAllMaterials(int pageNo, int displayLength)
        {
            return productRepository.GetAll(p => p.CategoryID == 0 && p.IsDeleted == false)
                          .OrderBy(x => x.CreationDate)
                            .Skip((pageNo - 1) * displayLength)
                            .Take(displayLength)
                    .Select(x => new ProductDomainModel()
                    {
                        Id = x.ID,
                        Name = x.Name,
                        Definition = x.Definition,
                        Description = x.Description,
                        ImageURL = x.Image != null ? Constans.ServerFile + x.Image : Constans.LogoPath,
                        isVisible = x.Visibility,
                        Price = float.Parse(x.Price?.ToString()),
                        Quantity = x.Quantity.Value
                    }).ToList();
        }

        public List<ProductDomainModel> SearchInAllMaterials(string searchWord, int pageNo, int displayLength)
        {
            return productRepository.GetAll(p => p.CategoryID == 0 && p.IsDeleted == false && p.Name.Contains(searchWord) || p.Category.Name.Contains(searchWord))
                         .OrderBy(x => x.ID)
                             .Skip((pageNo - 1) * displayLength)
                             .Take(displayLength)
                         .Select(x => new ProductDomainModel()
                         {
                             Id = x.ID,
                              Name = x.Name,
                             Definition = x.Definition,
                             Description = x.Description,
                              ImageURL = x.Image != null ? Constans.ServerFile + x.Image : Constans.LogoPath,
                             isVisible = x.Visibility,
                             Price = float.Parse(x.Price?.ToString()),
                             Quantity = x.Quantity.Value
                         }).ToList();
        }

        public int getAllMaterialsCount()
        {
            return productRepository.GetAll(x => x.CategoryID == 0 && x.IsDeleted == false).Count();
        }

        public int getSearchedMaterialsCount(string searchWord)
        {
            return productRepository.GetAll(p => p.CategoryID == 0 && p.IsDeleted == false && p.Name.Contains(searchWord) || p.Category.Name.Contains(searchWord)).Count();
        }

        public List<ProductDomainModel> getAllPrices(int pageNo, int displayLength)
        {
            return this.productRepository.GetAll(x=> x.IsDeleted == false)
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

        public List<ProductDomainModel> getProductsByCategoryID(int id)
        {
            List<ProductDomainModel> products = this.productRepository.GetAll(x => x.CategoryID == id)
                .Select(x => new ProductDomainModel()
                {
                    Id = x.ID,
                    Name = x.Name,
                    Definition = x.Definition,
                    Description = x.Description,
                    ImageURL = x.Image != null ? Constans.ServerFile + x.Image : Constans.LogoPath,
                    isVisible = x.Visibility,
                    Price = float.Parse(x.Price?.ToString()),
                    Quantity = x.Quantity.Value
                }).ToList();


            return products;
        }

        public List<PriceDomainModel> getProductsPrices()
        {
            return this.productRepository.GetAll(z => z.Visibility == true && z.Price > 0)
                                    .Select(x => new PriceDomainModel
                                    {
                                        Id = x.ID,
                                        Name = x.Name,
                                        CategoryID = x.CategoryID.Value,
                                        Price = x.Price.Value
                                    }).ToList();
        }

        public List<PriceDomainModel> getMaterialsPrices()
        {
            return this.productRepository.GetAll(z => z.Visibility == true && z.Price > 0 && z.CategoryID == null)
                                          .Select(x => new PriceDomainModel
                                          {
                                              Id = x.ID,
                                              Name = x.Name,
                                              Price = x.Price.Value
                                          }).ToList();
        }

        public List<ProductDomainModel> getAllMaterials()
        {
            return this.productRepository.GetAll(x=>x.CategoryID==null)
                .Select(x=> new ProductDomainModel {
                    Id = x.ID,
                    Name = x.Name,
                    Definition = x.Definition,
                    Description = x.Description,
                    ImageURL = x.Image != null ? Constans.ServerFile + x.Image : Constans.LogoPath,
                    isVisible = x.Visibility,
                    Price = float.Parse(x.Price?.ToString()),
                    Quantity = x.Quantity.Value
                }).ToList();
        }

        public List<ProductDomainModel> getAllProducts()
        {
            return this.productRepository.GetAll()
                           .Select(x => new ProductDomainModel
                           {
                               Id = x.ID,
                               Name = x.Name,
                               CategoryId=x.CategoryID,
                               Definition = x.Definition,
                               Description = x.Description,
                               ImageURL = x.Image != null ? Constans.ServerFile + x.Image : Constans.LogoPath,
                               isVisible = x.Visibility,
                               Price = float.Parse(x.Price?.ToString()),
                               Quantity = x.Quantity.Value
                           }).ToList();
        }
    }
}
