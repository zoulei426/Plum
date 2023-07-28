using Plum.Object;
using Refit;
using System;
using System.Text;

namespace Plum
{
    public static class ExceptionExtensions
    {
        public static string ToDetailString(this Exception @this)
        {
            var result = new StringBuilder();
            result.AppendLine($"异常信息：{@this}");

            if (@this.InnerException is not null)
            {
                var inner = @this.InnerException;
                while (inner.InnerException is not null)
                {
                    inner = inner.InnerException;
                }
                result.AppendLine($"内部信息：{inner}");

                var apiException = inner as ApiException;
                if (apiException is not null)
                {
                    result.AppendLine($"响应信息：{apiException.Content.ToObject<ErrorResponse>()}");
                }
            }
            return result.ToString();
        }
    }
}