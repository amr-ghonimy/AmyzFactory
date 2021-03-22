using AmyzFactory.Models;
using AmyzFeed.Domain;
using AmyzFeed.Repository;
using System.Collections.Generic;
using System.Web;


namespace AmyzFeed.Business.interfaces
{
  public  interface IProductsBusiness
    {
        ResultDomainModel createProduct(ProductDomainModel product);
        ResultDomainModel deleteProduct(int productID);

        ResultDomainModel uplaodImage(HttpRequest image, string savedFilePath,string folderResponse);
        List<ProductDomainModel> getAllProducts(int pageNo, int displayLength,string role);
        List<ProductDomainModel> getAllProducts(string role);
        List<ProductDomainModel> SearchInAllProducts(string searchWord, int pageNo, int displayLength, string role);
        List<ProductDomainModel> SearchInAllProducts(string searchWord, string role);

        ResultDomainModel isProductExists(string name,string role);

        ProductDomainModel getProductByID(int id,string role);
 
        int getAllProductsCount(string role);
        int getSearchedProductCount(string searchWord, string role);
 
        // for admin panel table
 
         List<ProductDomainModel> getProductsByCategoryID(int id, string role);

 
        ResultDomainModel editProduct(ProductDomainModel newProduct);
        List<ProductDomainModel> getAllPrices(int pageNo, int displayLength, string role);
  
        ResultDomainModel updatePrices(List<ProductDomainModel> list);

         List<PriceDomainModel> getProductsPrices(string role);

    }
}
