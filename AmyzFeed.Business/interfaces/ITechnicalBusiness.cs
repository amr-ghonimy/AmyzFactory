using AmyzFactory.Models;
using AmyzFeed.Domain;
using System.Collections.Generic;

namespace AmyzFeed.Business.interfaces
{
    public interface ITechnicalBusiness
    {
        ResultDomainModel upsertTechTexts(TechnicalTextDomainModel text, string serverPath);
        ResultDomainModel deleteTechTexts(int id, string folderPath);
        List<TechnicalTextDomainModel> findTextxByTechID(int techId, string filePath);
        List<TechnicalSupportDomainModel> getTechTextsList(string folderPath);
    }
}
