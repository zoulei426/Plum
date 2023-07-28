using Plum.Enums;
using System;

namespace Plum.Windows.Convertors
{
    public class CompletedStatusToIconConverter : ValueConverterBase<CompletedStatus, string>
    {
        protected override string Convert(CompletedStatus value) =>
           value == CompletedStatus.Completed ? "CheckCircle" : "AlertCircle";

        protected override CompletedStatus ConvertBack(string value) => throw new NotImplementedException();
    }
}