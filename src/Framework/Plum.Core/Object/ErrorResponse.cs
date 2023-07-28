using System;
using System.Text;

namespace Plum.Object
{
    [Serializable]
    public class ErrorResponse
    {
        public ErrorInfo Error { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine(Error.Message);

            if (!Error.Code.IsNullOrEmpty())
            {
                result.AppendLine($"编码：{Error.Code}");
            }

            if (!Error.Details.IsNullOrEmpty())
            {
                result.AppendLine($"细节信息：{Error.Details}");
            }

            if (Error.ValidationErrors is not null && Error.ValidationErrors.Count > 0)
            {
                result.AppendLine($"验证错误：");
                foreach (var validtion in Error.ValidationErrors)
                {
                    result.AppendLine($"{validtion.Message}：{validtion.Members.StringJoin(",")}");
                }
            }

            return result.ToString();
        }
    }
}