namespace Operum.Model.Constants.Analytics
{
    // How a Line or Bar chart derives its category / x-axis key from the mapped field
    // before the aggregation (AnalyticCodes) runs over each bucket. Only Line and Bar carry
    // a grouping; every other result type's calculation is the aggregation code alone.
    public static class AnalyticGroupings
    {
        // Plot every entry as its own mark, no bucketing and no aggregation: a point on a
        // Line, a bar on a Bar chart.
        public const string None = "None";

        // One bucket per distinct field value.
        public const string Exact = "Exact";

        // Date/datetime fields only: one bucket per calendar day / week (Monday start) /
        // month / year.
        public const string Daily = "Daily";
        public const string Weekly = "Weekly";
        public const string Monthly = "Monthly";
        public const string Yearly = "Yearly";

        public static readonly HashSet<string> All =
        [
            None, Exact, Daily, Weekly, Monthly, Yearly
        ];

        // The groupings that bucket a date field into calendar periods.
        public static readonly HashSet<string> DateBuckets =
        [
            Daily, Weekly, Monthly, Yearly
        ];

        public static bool IsValid(string op) => All.Contains(op);
    }
}
