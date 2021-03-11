using AmyzFeed.Business.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmyzFactory.Models;
using AmyzFeed.Domain;
using AmyzFeed.Repository.Infrastructure.Contract;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Data;

namespace AmyzFeed.Business
{
   public class FeedsProgramBusiness : IFeedsProgramBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FeedsProgramRepository repo;
 
        private ResultDomainModel resultModel;

        public FeedsProgramBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.repo = new FeedsProgramRepository(this.unitOfWork);
        }



        private ResultDomainModel initResultModel(bool isSuccess, string message, int modelID = 0)
        {
            resultModel.IsSuccess = isSuccess;
            resultModel.Message = message;
            resultModel.modelID = modelID;
            return resultModel;
        }

        public ResultDomainModel createFeedProgram(FeedsProgramDomainModel model)
        {
            try
            {
                var feed = new FeedsProgram()
                {
                    Id = model.Id,
                    productType = model.ProductType.Trim(),
                    ProtienPercentage = model.ProtienPercentage,
                    DayFrom = model.DayFrom,
                    DayTo= model.DayTo,
                    CreationDate = DateTime.Now,
                    Quantity=model.Quantity
                };

                var ifFeedExists = this.repo.SingleOrDefault(m => m.productType.Trim().ToLower() == model.ProductType.Trim().ToLower());

                if (ifFeedExists != null)
                {

                    return initResultModel(false, "Feed Already Exists Enter Another Name!");
                }

                this.repo.Insert(feed);
                return initResultModel(true, "Insertion Success",feed.Id);
            }
            catch (Exception e)
            {

                return initResultModel(false, "Insertion failed error = "+e.Message);
            }

        }

        public ResultDomainModel deleteFeedProgram(int id)
        {
            try
            {
                this.repo.Delete(x => x.Id == id);
                return initResultModel(true,"Feed Deleted",id);
            }
            catch (Exception e)
            {
                return initResultModel(false, "Feed Deleting failed "+e.Message);
            }
        }

        public List<FeedsProgramDomainModel> GetAllFeedsProgram()
        {
            return repo.GetAll().Select(x => new FeedsProgramDomainModel()
            {
                ProductType = x.productType,
                ProtienPercentage = x.ProtienPercentage,
                DayFrom = x.DayFrom,
                DayTo = x.DayTo,
                UpdatedDate=x.UpdatedDate,
                Id=x.Id,
                CreationDate=x.CreationDate,
                Quantity=x.Quantity
            }).ToList();
        }

        public ResultDomainModel updateFeedProgram(FeedsProgramDomainModel model)
        {
            var oldFeed = this.repo.SingleOrDefault(x => x.Id == model.Id);

            if (oldFeed == null)
            {
                return initResultModel(false, "model with his id = " + model.Id + " not found!", model.Id);
            }

            try
            {
                bool ifNameExists = this.repo.Exists(x => x.productType == model.ProductType.Trim() && x.Id != model.Id);

                if (ifNameExists)
                {
                    return initResultModel(false, "The name already exists!");
                }

                oldFeed.productType = model.ProductType.Trim();
                oldFeed.ProtienPercentage = model.ProtienPercentage;
                oldFeed.UpdatedDate = DateTime.Now;
                oldFeed.Quantity = model.Quantity;
                oldFeed.DayFrom = model.DayFrom;
                oldFeed.DayTo = model.DayTo;
 
                this.repo.Update(oldFeed);
                return initResultModel(true, "Updated Success Feed Program has updated");

            }
            catch (Exception e)
            {
                return initResultModel(false, e.Message.ToString());
            }
        }
    }
}
