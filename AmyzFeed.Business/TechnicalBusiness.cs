using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AmyzFeed.Domain;
using Newtonsoft.Json;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Infrastructure.Contract;
using AmyzFeed.Repository.Data;

namespace AmyzFeed.Business
{
    public class TechnicalBusiness : ITechnicalBusiness
    {
        private ResultDomainModel resultModel;

        private readonly TechnicalRepository techRepository;
        private IUnitOfWork unitOfWork;

        public TechnicalBusiness(IUnitOfWork _unitOfWork, ResultDomainModel _resultModel)
        {
            this.unitOfWork = _unitOfWork;
            this.resultModel = _resultModel;
            this.techRepository = new TechnicalRepository(this.unitOfWork);
        }

       
        public List<TechnicalSupportDomainModel> getTechTextsList(string folderPath)
        {
            // all technicals support from db
            List<Technical> techs = this.techRepository.GetAll().ToList();

            List<TechnicalSupportDomainModel> techsDm = new List<TechnicalSupportDomainModel>();

            if (techs.Count > 0)
            {
                foreach (var item in techs)
                {
                    TechnicalSupportDomainModel techDm = new TechnicalSupportDomainModel()
                    {
                        Id=item.ID,
                        Name=item.Name,
                        TechTextsList=findTextxByTechID(item.ID,folderPath)
                    };

                    techsDm.Add(techDm);
                }
            }

            return techsDm;

         }



        public ResultDomainModel deleteTechTexts(int id, string folderPath)
        {
            var tempList = new List<TechnicalTextDomainModel>();
            string textJson = File.ReadAllText(folderPath);

            if (textJson.Length > 0)
            {
                tempList = JsonConvert.DeserializeObject<List<TechnicalTextDomainModel>>(textJson);

                foreach (var item in tempList)
                {
                    if (item.Id == id)
                    {
                        tempList.Remove(item);
                        // clear file
                        File.WriteAllText(folderPath, String.Empty);
                        break;
                    }
                }

                var convertedJson = JsonConvert.SerializeObject(tempList, Formatting.Indented);
                try
                {
                    File.WriteAllText(folderPath, convertedJson);
                    return initResultModel(true, "Process Success!!");
                }
                catch (Exception)
                {
                    return initResultModel(false, "Process Failed");
                }

            }
            return initResultModel(false, "Process Failed");
        }


        public List<TechnicalTextDomainModel> findTextxByTechID(int techId,string filePath)
        {
            var tempList = new List<TechnicalTextDomainModel>();
            var resultList = new List<TechnicalTextDomainModel>();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                tempList = JsonConvert.DeserializeObject<List<TechnicalTextDomainModel>>(textJson);

                foreach (var item in tempList)
                {
                    if (item.TechID == techId)
                    {
                        resultList.Add(item);
                    }
                }
            }
            return resultList;
        }

        public ResultDomainModel upsertTechTexts(TechnicalTextDomainModel text, string filePath)
        {
            var list = new List<TechnicalTextDomainModel>();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<TechnicalTextDomainModel>>(textJson);

                // to perform update
                if (text.Id > 0)
                {
                    foreach (var item in list)
                    {
                        if (item.Id == text.Id)
                        {
                            item.Title = text.Title;
                            item.Description = text.Description;
                        }
                    }
                }else
                {
                    text.Id = list.Last().Id + 1;
                    list.Add(text);
                }

                // first i will clear the file
                File.WriteAllText(filePath, String.Empty);
            }else // text file is empty
            {
                text.Id = 1;
                list.Add(text);
            }

           
            var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            try
            {
                File.WriteAllText(filePath, convertedJson);
                return initResultModel(true, "Process Success!!");
            }
            catch (Exception)
            {
                return initResultModel(false, "Process Failed");
            }
        }

        private ResultDomainModel initResultModel(bool isSuccess,string message)
        {
            this.resultModel.IsSuccess = isSuccess;
            this.resultModel.Message = message;
            return resultModel;
        }

       
    }
}
