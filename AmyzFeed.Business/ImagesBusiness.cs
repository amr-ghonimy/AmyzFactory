using AmyzFactory.Models;
using AmyzFeed.Business.helpers;
using AmyzFeed.Business.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace AmyzFeed.Business
{
    public class ImagesBusiness:IImageBusiness
    {
        private ResultDomainModel resultModel;
        private string baseUrl;

        public ImagesBusiness(ResultDomainModel _resultModel)
        {
            this.resultModel = _resultModel;
            this.baseUrl = WebConfigurationManager.AppSettings["baseUrl"];
        }


        private ResultDomainModel initResultModel(bool isSuccess,string message,object data=null)
        {
            this.resultModel.IsSuccess = isSuccess;
            this.resultModel.Message = message;
            this.resultModel.Data = data;
            return resultModel;
        }

        public ResultDomainModel deleteImage(string imagePath)
        {
            try
            {
                File.Delete(imagePath);
                return initResultModel(true, "Image Deleted Successfully");
                
            }
            catch (Exception e)
            {
                return initResultModel(false, e.Message);
            }
        }


        // folderPath is for access folder of images on server
        // responsePath is for preview image in site 
        public List<ImageDomainModel> getImages(string folderPath, string responsePath)
        {

            string[] allfiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Select(Path.GetFileName).ToArray();
            var listOfImages = new List<ImageDomainModel>();
            int idCounter = 1;

           



 

            if (allfiles.Count() > 0)
            {
                foreach (var item in allfiles)
                {
                    listOfImages.Add(new ImageDomainModel()
                    {
                        Id = idCounter,
                        Title = item,
                        ImageUrl = baseUrl + responsePath + item
                    });
                    idCounter++;
                }
            }

            return listOfImages;

        }


        private ResultDomainModel CancelUploadImageResponse( int max)
        {
            return new ResultDomainModel(false, "Max Images You Can Add: " + max);
        }

        private Boolean imagesCountValidation(string serverPath, int max)
        {

            DirectoryInfo di = new DirectoryInfo(serverPath);

            int count = di.GetFiles().Length;
            if (count < max)
            {
                return true;
            }
            return false;
        }


        public ResultDomainModel uploadImage(HttpRequest httpRequest, string fullPath,string responsePath, int maxImages)
        {

            var validationResult = imagesCountValidation(fullPath,  maxImages);
            if (validationResult == false)
            {
                return CancelUploadImageResponse(maxImages);
            }

            try
            {

                string imageExt = Path.GetExtension(httpRequest.Files[0].FileName);
                if (imageExt == "")
                {
                    return initResultModel(false, "Please select image with english litters only");

                }
                string uniqueKey = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssf"), Guid.NewGuid().ToString())+ imageExt;
                string filePath = "";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    filePath = fullPath+ uniqueKey;
                    postedFile.SaveAs(filePath);
                }

              

                ImageDomainModel dataDm = new ImageDomainModel()
                {
                    Id = new Random().Next(1, 120),
                    Title = uniqueKey,
                    ImageUrl= responsePath+'/'+uniqueKey
                };



                return initResultModel(true, "Image Uploaded Successfully",data:dataDm);
                
            }
            catch (Exception e)
            {
                return initResultModel(false, "Image not Uploaded Error is:" + e.Message);
            }

            
        }

    }
}
