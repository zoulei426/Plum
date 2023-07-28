using System;
using System.Collections.Generic;

namespace Plum.Object
{
    [Serializable]
    public class ValidationErrorInfo
    {
        public string Message { get; set; }

        public List<string> Members { get; set; }
    }
}