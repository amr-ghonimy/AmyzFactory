using AmyzFactory.Models;
using AmyzFeed.Business.helpers;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AmyzFeed.Business
{
   public class AddressesBusiness: IAddressesBusiness
    {

        private ResultDomainModel resultModel;
        private string baseUrl;



    

        public AddressesBusiness(ResultDomainModel _resultModel)
        {
            this.resultModel = _resultModel;
            this.baseUrl = WebConfigurationManager.AppSettings["baseUrl"];
         }

        private ResultDomainModel initResultModel(bool isSuccess,string message,int modelID=0,object data=null)
        {
            this.resultModel.IsSuccess = isSuccess;
            this.resultModel.Message = message;
            this.resultModel.Data = data;
            this.resultModel.modelID = modelID;
            return resultModel;
        }

        public ResultDomainModel createContact(ContactDomainModel model, string filePath,int maxNumber)
        {

            var list = new List<ContactDomainModel>();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<ContactDomainModel>>(textJson);
                model.Id = list.Last().Id+ 1;
            }

            if (list.Count == maxNumber)
            {
                return initResultModel(false, "Insertion Failed Maximum Records Are: " + maxNumber);
            }
            list.Add(model);
            var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            try
            {
                File.WriteAllText(filePath, convertedJson);
                return initResultModel(true, "Insertion Success!!",modelID:model.Id);
            }
            catch (Exception)
            {

                return initResultModel(false, "Insertion Failed");
            }
        }

        public List<ContactDomainModel> getContacts(string filePath)
        {
            var list = new List<ContactDomainModel>();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<ContactDomainModel>>(textJson);
            }

            return list;
        }

        public TextsDomainModel getTexts(string filePath)
        {
            var model = new TextsDomainModel();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                model = JsonConvert.DeserializeObject<TextsDomainModel>(textJson);
            }

            return model;
        }

        public ResultDomainModel deleteContact(int elementIndexInList, string filePath)
        {
            var list = new List<TextsDomainModel>();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<TextsDomainModel>>(textJson);
                var itemToRemove = list.Single(r => r.Id == elementIndexInList);
                bool result = list.Remove(itemToRemove);
                if (result)
                {
                    if (list.Count > 0)
                    {
                        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
                        File.WriteAllText(filePath, convertedJson);
                    }
                    else
                    {
                        File.WriteAllText(filePath, "");
                    }
                    return initResultModel(true, "Item Deleted Successfully!");
                }

            }
            return initResultModel(false, "Item Deleted failed!");
        }
      
        public ResultDomainModel createOrUpdateFile(TextsDomainModel model, string filePath)
        {
 
            // to update 
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var convertedJson = JsonConvert.SerializeObject(model, Formatting.Indented);


            try
            {

                File.WriteAllText(filePath, convertedJson);
                return initResultModel(true, "Process Success!!");

            }
            catch (Exception)
            {

                return initResultModel(false, "Process Failed!!");

            }
        }

     
        public ResultDomainModel createArticle(TextsDomainModel model, string filePath)
        {
            var list = new List<TextsDomainModel>();

            model.SubTitle= DateTime.Now.ToString("dd dddd , MMMM, yyyy", new CultureInfo("ar-AE"));


            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<TextsDomainModel>>(textJson);
                model.Id = list.Last().Id + 1;
            }

            list.Add(model);
            var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            try
            {
                File.WriteAllText(filePath, convertedJson);
                return initResultModel(true, "Insertion Success!!");
            }
            catch (Exception)
            {

                return initResultModel(false, "Insertion Failed");
            }
        }

        public List<TextsDomainModel> getArticles(string filePath)
        {

            List<TextsDomainModel> list = new List<TextsDomainModel>();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<TextsDomainModel>>(textJson);
            }

            foreach (var item in list)
            {
                item.ImageUrl = baseUrl + item.ImageUrl;
            }

            return list;
        }

        public ResultDomainModel UpdateArticle(TextsDomainModel model, string filePath)
        {
            var list = new List<TextsDomainModel>();

            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<TextsDomainModel>>(textJson);
            }

            foreach (var item in list)
            {
                if (item.Id == model.Id)
                {
                    item.Title = model.Title;
                    item.Description = model.Description;
                    break;
                }
            }

            File.WriteAllText(filePath, String.Empty);

           
            var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            try
            {
                File.WriteAllText(filePath, convertedJson);
                return initResultModel(true, "Insertion Success!!");
            }
            catch (Exception)
            {

                return initResultModel(false, "Insertion Failed");
            }
        }

        public ResultDomainModel getArticleById(int id,string filePath)
        {
            var list = new List<TextsDomainModel>();
            
            string textJson = File.ReadAllText(filePath);

            if (textJson.Length > 0)
            {
                list = JsonConvert.DeserializeObject<List<TextsDomainModel>>(textJson);


                TextsDomainModel item = list.Single(r => r.Id == id);


                if (item == null)
                {
                    return initResultModel(false, "Article Not Exists!!");
                }

                item.ImageUrl = baseUrl + item.ImageUrl;

                return initResultModel(true, "Article is Exists!!", data:item);

            }
 
            return initResultModel(false, "Article Not Exists!!");

         }
    }
}
