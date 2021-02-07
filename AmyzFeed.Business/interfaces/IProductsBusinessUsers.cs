using AmyzFactory.Models;
using AmyzFeed.Repository;
using System.Collections.Generic; 
using System.Web;


namespace AmyzFeed.Business.interfaces
{
  public  interface IProductsBusinessUsers
    {
      
        List<ProductDomainModel> getAllProducts();
       
        ProductDomainModel getProductByID(int id);
        List<ProductDomainModel> getProductsByCategory(int categoryId);

        int getAllProductsCount();
          int getAllMaterialsCount();
        
        List<ProductDomainModel> getMaterials();

         List<ProductDomainModel> getProductsPrices( );
        List<ProductDomainModel> getMaterialsPrices();
 

    }
}
