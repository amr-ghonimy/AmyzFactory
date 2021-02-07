using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
    public class ResultDomainModel
    {
        public Boolean IsSuccess { get; set; }
        public string  Message{ get; set; }
        public int modelID{ get; set; }
        public Object Data { get; set; }
        public ResultDomainModel() { }
        public ResultDomainModel(bool isSuccess,string message,int modelID=0,Object Data=null)
        {
            this.modelID = modelID;
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Data = Data;
        }
    }
}