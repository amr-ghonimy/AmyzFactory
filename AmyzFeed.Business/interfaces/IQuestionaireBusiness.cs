using AmyzFactory.Models;
using System;
using System.Collections.Generic;
 

namespace AmyzFeed.Business.interfaces
{
  public  interface IQuestionaireBusiness
    {
        ResultDomainModel createQuestionaire(QuestionaireDomainModel question);
          
     }
}
