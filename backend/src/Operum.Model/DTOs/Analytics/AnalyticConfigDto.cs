namespace Operum.Model.DTOs.Analytics
{
    public class AnalyticConfigDto
    {
        public List<AnalyticConfigType> ResultTypes { get; set; } = [];
    }

    public class AnalyticConfigType
    {
        public string Name { get; set; } = default!;
        // True for result types only offered when building a saved widget (a Goal), never in
        // Explore or a notification condition.
        public bool WidgetOnly { get; set; }
        public List<AnalyticConfigCode> Codes { get; set; } = [];

        // Line/Bar only: the calculation is a (Grouping, Code) pair. Groupings lists the
        // grouping options and which Codes each one allows; GroupingPurpose names the
        // purpose the grouping constrains (its field types come from the chosen grouping,
        // not from the code). Empty for every other result type.
        public List<AnalyticConfigGrouping> Groupings { get; set; } = [];
        public string GroupingPurpose { get; set; } = string.Empty;
    }

    public class AnalyticConfigCode
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<AnalyticConfigPurpose> Purposes { get; set; } = [];
    }

    public class AnalyticConfigGrouping
    {
        public string Grouping { get; set; } = default!;
        public string Name { get; set; } = default!;
        // Field types the grouping purpose accepts under this grouping.
        public List<string> AllowedDataTypes { get; set; } = [];
        // Aggregation codes valid with this grouping.
        public List<string> AllowedCodes { get; set; } = [];
    }

    public class AnalyticConfigPurpose
    {
        public string Name { get; set; } = default!;
        public List<string> AllowedDataTypes { get; set; } = [];
        // True for a purpose the calculation runs fine without (Min/Max's Display field).
        public bool Optional { get; set; }
    }
}
