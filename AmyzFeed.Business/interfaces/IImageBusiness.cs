using AmyzFactory.Models;
using System.Collections.Generic;
using System.Web;

namespace AmyzFeed.Business.interfaces
{
  public  interface IImageBusiness
    {
        ResultDomainModel uploadImage(HttpPostedFile image, string serverPath, string folderResponse, int maxImages);
        ResultDomainModel deleteImage(string imagePath);

        List<ImageDomainModel> getImages(string folderPath, string folderResponse);
    }
}
