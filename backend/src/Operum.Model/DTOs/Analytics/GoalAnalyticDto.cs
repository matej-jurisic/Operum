using Operum.Model.Constants.Analytics;
using Operum.Model.DTOs.Fields;

namespace Operum.Model.DTOs.Analytics
{
    // A Single Value calculation shown as progress toward a target. Value and Target are
    // both strings in ValueField's format (an invariant number, or hh:mm:ss for a
    // duration); Progress is their ratio, already computed so the card doesn't have to
    // parse either. Progress can exceed 1 when the target is met -- the card caps the bar
    // but still shows the real percentage.
    public class GoalAnalyticDto : AnalyticDto
    {
        public string Value { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        // Null when there's nothing to show progress against: no data, or a target that
        // isn't a positive number.
        public double? Progress { get; set; }
        public FieldDto ValueField { get; set; } = null!;

        public GoalAnalyticDto()
        {
            ResultType = AnalyticTypes.Goal;
        }
    }
}
