using AmyzFactory.Models;
using AmyzFeed.Domain;
using System;
using System.Collections.Generic;


namespace AmyzFeed.Business.interfaces
{
  public  interface IAddressesBusiness
    {

        List<ContactDomainModel> getContacts(string filePath);
        TextsDomainModel getTexts(string filePath);
        List<TextsDomainModel> getArticles(string filePath);

        ResultDomainModel getArticleById(int  id, string filePath);


        ResultDomainModel createContact(ContactDomainModel model, string filePath, int maxNumber);
        
        ResultDomainModel deleteContact(int elementIndexInList, string filePath);
      
        ResultDomainModel createOrUpdateFile(TextsDomainModel model, string filePath);

        ResultDomainModel createArticle(TextsDomainModel model, string filePath);
        ResultDomainModel UpdateArticle(TextsDomainModel model, string filePath);

        
    }
}
