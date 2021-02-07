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


        ResultDomainModel createContact(ContactDomainModel model, string filePath, int maxNumber);
        
        ResultDomainModel deleteContact(int elementIndexInList, string filePath);
      
        ResultDomainModel createOrUpdateFile(TextsDomainModel model, string filePath);
    }
}
