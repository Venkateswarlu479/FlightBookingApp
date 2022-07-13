using AuthenticationService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class ResponseModel
    {
        public ResponseModel(ResponseCode responseCode, string responseMessage, object result)
        {
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
            Result = result;
        }
        public ResponseCode ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Result { get; set; }
    }
}
