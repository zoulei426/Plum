using Plum.Enums;
using System;

namespace Plum.Windows.Convertors
{
    public class CompletedStatusToColorConverter : ValueConverterBase<CompletedStatus, string>
    {
        protected override string Convert(CompletedStatus value)
        {
            var result = "#616161";
            switch (value)
            {
                case CompletedStatus.ToBeCompleted:
                    result = "#ef3f23";
                    break;

                case CompletedStatus.Completed:
                    result = "#4caf50";
                    break;
            }
            return result;
        }

        protected override CompletedStatus ConvertBack(string value) => throw new NotImplementedException();
    }
}