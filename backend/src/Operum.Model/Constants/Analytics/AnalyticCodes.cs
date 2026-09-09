namespace Operum.Model.Constants.Analytics
{
    public static class AnalyticCodes
    {
        public const string Count = "Count";
        public const string Min = "Min";
        public const string Max = "Max";
        public const string Average = "Average";
        public const string Sum = "Sum";
        public const string StdDev = "Standard Deviation";
        public const string CountDistinct = "Count Distinct";
        public const string MostCommon = "Most Common";
        public const string LeastCommon = "Least Common";

        public const string TrueCount = "True Count";
        public const string FalseCount = "False Count";
        public const string TruePercentage = "True Percentage";

        // Line/Bar aggregations. Sum/Average/Count/Min/Max above are shared with Single
        // Value; these two are chart-only. The grouping a chart pairs one of these with is
        // AnalyticGroupings.
        public const string RawValues = "Raw Values";
        public const string CumulativeSum = "Cumulative Sum";

        public const string ScatterChart = "Scatter Chart";
        public const string CorrelationScatter = "Correlation Scatter";
        public const string Calendar = "Calendar";
        public const string DonutChart = "Donut Chart";

        public static readonly HashSet<string> All =
        [
            Count, Min, Max, Average, Sum, StdDev,
            CountDistinct, MostCommon, LeastCommon,
            TrueCount, FalseCount, TruePercentage,
            RawValues, CumulativeSum,
            Calendar, ScatterChart, CorrelationScatter, DonutChart
        ];

        public static bool IsValid(string op) => All.Contains(op);
    }
}
