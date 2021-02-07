﻿using AmyzFactory.Models;
using AmyzFeed.Repository;
using System.Collections.Generic; 
using System.Web;


namespace AmyzFeed.Business.interfaces
{
  public  interface IProductsBusiness
    {
        ResultDomainModel createProduct(ProductDomainModel product, HttpPostedFileWrapper productImage, string serverPathToUploadImage);
        ResultDomainModel deleteProduct(int productID);

        List<ProductDomainModel> getAllProducts(int pageNo, int displayLength);
        List<ProductDomainModel> SearchInAllProducts(string searchWord, int pageNo, int displayLength);

        ProductDomainModel getProductByID(int id);
 
        int getAllProductsCount();
        int getSearchedProductCount(string searchWord);
 
        // for admin panel table
        List<ProductDomainModel> getAllMaterials(int pageNo, int displayLength);
        List<ProductDomainModel> SearchInAllMaterials(string searchWord, int pageNo, int displayLength);

        int getAllMaterialsCount();
        int getSearchedMaterialsCount(string searchWord);

        ResultDomainModel editProduct(ProductDomainModel newProduct);
        List<ProductDomainModel> getAllPrices(int pageNo, int displayLength);
  
        ResultDomainModel updatePrices(List<ProductDomainModel> list);

    }
}
