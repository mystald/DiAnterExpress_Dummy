using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiAnterExpressDummy.Dtos
{
    public class DtoReturnSuccess<T>
    {
        public string status { get; set; } = RequestStatus.success.ToString();
        public T data { get; set; }
    }

    public class DtoReturnError
    {
        public string status { get; set; } = RequestStatus.error.ToString();
        public string message { get; set; }
    }


    public enum RequestStatus
    {
        success,
        error,
    }
}