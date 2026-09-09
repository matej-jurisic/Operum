namespace Operum.Model.Constants.Analytics
{
    // Before grouping and aggregation were separate fields, a Line or Bar chart's whole
    // calculation was a single fused Code ("Daily", "Count Bar Chart", ...). This maps each
    // retired code to the (Grouping, Code) pair that replaces it, for the data migration
    // (AddWidgetGrouping) and for shared Explore URLs bookmarked under the old shape.
    public static class LegacyLineBarCodes
    {
        public static readonly Dictionary<string, (string Grouping, string Code)> Map = new()
        {
            // Line
            ["Line Chart"] = (AnalyticGroupings.None, AnalyticCodes.RawValues),
            ["Aggregated Sum"] = (AnalyticGroupings.Exact, AnalyticCodes.Sum),
            ["Cumulative Sum"] = (AnalyticGroupings.Exact, AnalyticCodes.CumulativeSum),
            ["Daily"] = (AnalyticGroupings.Daily, AnalyticCodes.Sum),
            ["Weekly"] = (AnalyticGroupings.Weekly, AnalyticCodes.Sum),
            ["Monthly"] = (AnalyticGroupings.Monthly, AnalyticCodes.Sum),
            ["Yearly"] = (AnalyticGroupings.Yearly, AnalyticCodes.Sum),

            // Bar
            ["Count Bar Chart"] = (AnalyticGroupings.Exact, AnalyticCodes.Count),
            ["Sum Bar Chart"] = (AnalyticGroupings.Exact, AnalyticCodes.Sum),
            ["Average Bar Chart"] = (AnalyticGroupings.Exact, AnalyticCodes.Average),
            ["Daily Bar Chart"] = (AnalyticGroupings.Daily, AnalyticCodes.Sum),
            ["Weekly Bar Chart"] = (AnalyticGroupings.Weekly, AnalyticCodes.Sum),
            ["Monthly Bar Chart"] = (AnalyticGroupings.Monthly, AnalyticCodes.Sum),
            ["Yearly Bar Chart"] = (AnalyticGroupings.Yearly, AnalyticCodes.Sum),
        };

        // Resolves a possibly-legacy (grouping, code) request to the current shape. A request
        // that already carries a grouping is returned untouched; a bare legacy code is
        // rewritten; anything unrecognised is left alone for validation to reject.
        public static (string? Grouping, string Code) Resolve(string? grouping, string code)
        {
            if (!string.IsNullOrEmpty(grouping))
                return (grouping, code);

            return Map.TryGetValue(code, out var pair) ? (pair.Grouping, pair.Code) : (grouping, code);
        }
    }
}
