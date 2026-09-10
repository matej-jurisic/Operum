using Operum.Model.Constants.Analytics;
using Operum.Model.DTOs.Fields;

namespace Operum.Model.DTOs.Analytics
{
    public class SingleValueAnalyticDto : AnalyticDto
    {
        public string Value { get; set; } = string.Empty;
        public string? EntryId { get; set; }
        public FieldDto ValueField { get; set; } = null!;

        // Min/Max with a Display field: the compared value, shown smaller next to the
        // displayed label. Null for every other calculation.
        public string? SecondaryValue { get; set; }
        public FieldDto? SecondaryValueField { get; set; }

        public SingleValueAnalyticDto()
        {
            ResultType = AnalyticTypes.SingleValue;
        }
    }
}
