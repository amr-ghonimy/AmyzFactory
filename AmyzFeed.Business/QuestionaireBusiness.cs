using AmyzFeed.Business.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmyzFactory.Models;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Infrastructure.Contract;
using AmyzFeed.Repository.Data;

namespace AmyzFeed.Business
{
    public class QuestionaireBusiness : IQuestionaireBusiness
    {
        private QuestionaireRepository repository;
        private ResultDomainModel resultModel;
        private IUnitOfWork unitOfWork;
        public QuestionaireBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.repository = new QuestionaireRepository(this.unitOfWork);
        }



        private ResultDomainModel initResult(bool result,string message)
        {
            return new ResultDomainModel() {
                IsSuccess=result,Message=message
            };
        }

        public ResultDomainModel createQuestionaire(QuestionaireDomainModel question)
        {

            if (question != null)
            {
                Questionnaire quest = new Questionnaire()
                {
                    Email=question.Email,
                    Question=question.Question,
                    UserName=question.UserName
                };

                try
                {
                    this.repository.Insert(quest);
                    return initResult(true, "تم ارسال استفسارك بنجاح سيتم الرد على بريدك الالكترونى");
                }
                catch (Exception)
                {
                    return initResult(false, "لمم تتم العملية بنجاح حاول مرة أخرى");
                }
            }

            return initResult(false, "لمم تتم العملية بنجاح حاول مرة أخرى");


        }
    }
}
