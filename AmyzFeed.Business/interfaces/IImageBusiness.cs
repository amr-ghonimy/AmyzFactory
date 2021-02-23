using AmyzFactory.Models;
using System.Collections.Generic;
using System.Web;

namespace AmyzFeed.Business.interfaces
{
  public  interface IImageBusiness
    {
        ResultDomainModel uploadImage(HttpRequest image, string fullPath,string responsePath, int maxImages);
        ResultDomainModel deleteImage(string imagePath);

        List<ImageDomainModel> getImages(string folderPath, string folderResponse);
    }
}
