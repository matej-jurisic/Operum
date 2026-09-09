namespace Operum.Model.Constants.Analytics.Definitions
{
    public class AnalyticDefinition
    {
        public HashSet<string> Purposes { get; init; } = [];
        public Dictionary<string, AnalyticPurposeDataTypes> Codes { get; init; } = [];

        // True for result types that only make sense as a saved Widget Library entry, not as
        // an ad hoc Explore calculation or a notification condition: a Goal needs a target,
        // which is stored on the Widget. Consumers of the analytics config filter these out.
        public bool WidgetOnly { get; init; }

        // Line/Bar only: the calculation is a (Grouping, Code) pair rather than a bare Code.
        // The grouping decides how the GroupingPurpose field is bucketed and which field
        // types that purpose accepts; the Code (a key of Codes) is the within-bucket
        // aggregation. Empty for every other result type, whose calculation is the Code alone.
        public Dictionary<string, AnalyticGrouping> Groupings { get; init; } = [];

        // Line/Bar only: the purpose the grouping buckets -- X-axis for a line, Name for a
        // bar. Its allowed field types come from the chosen grouping, not from Codes.
        public string GroupingPurpose { get; init; } = string.Empty;

        // Line/Bar only: how the per-value grouping and the composed labels read for this
        // type ("value" for a line, "category" for a bar).
        public string AxisNoun { get; init; } = string.Empty;

        public bool UsesGrouping => Groupings.Count > 0;
    }

    // One grouping option for a Line or Bar chart: how the axis key is bucketed before the
    // aggregation runs.
    public class AnalyticGrouping
    {
        public string Label { get; init; } = string.Empty;

        // Field types the grouping purpose accepts under this grouping (all types for
        // Exact/None, date/datetime for the calendar-period buckets).
        public HashSet<string> AllowedAxisTypes { get; init; } = [];

        // Aggregation codes valid with this grouping.
        public HashSet<string> AllowedCodes { get; init; } = [];
    }
}
