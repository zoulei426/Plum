using System;
using System.Collections.Generic;

namespace Plum.Object
{
    [Serializable]
    public class ErrorInfo
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public object Data { get; set; }

        public List<ValidationErrorInfo> ValidationErrors { get; set; }
    }
}