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
    }

    public class AnalyticConfigCode
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<AnalyticConfigPurpose> Purposes { get; set; } = [];
    }

    public class AnalyticConfigPurpose
    {
        public string Name { get; set; } = default!;
        public List<string> AllowedDataTypes { get; set; } = [];
    }
}
