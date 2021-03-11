using AmyzFactory.Models;
using AmyzFeed.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Business.interfaces
{
   public interface IFeedsProgramBusiness
    {
        ResultDomainModel createFeedProgram(FeedsProgramDomainModel model);
        ResultDomainModel updateFeedProgram(FeedsProgramDomainModel model);
        ResultDomainModel deleteFeedProgram(int id);
        List<FeedsProgramDomainModel> GetAllFeedsProgram();

    }
}
