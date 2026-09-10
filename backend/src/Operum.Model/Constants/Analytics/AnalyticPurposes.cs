namespace Operum.Model.Constants.Analytics
{
    public static class AnalyticPurposes
    {
        public const string Xaxis = "X-axis";
        public const string Yaxis = "Y-axis";
        public const string Value = "Value";
        public const string What = "What";
        public const string When = "When";
        public const string Name = "Name";

        // Min/Max only, and optional: the field whose value the widget shows. The
        // calculation still picks the entry by the Value field; this only changes what is
        // displayed once that entry is found.
        public const string Display = "Display";

        // The field two correlation-scatter sources are joined on: a point pairs the two
        // trackers' values for each match key they share.
        public const string Match = "Match";

        public static readonly HashSet<string> All =
        [
            Xaxis, Yaxis, Value, When, What, Name, Match, Display
        ];

        public static bool IsValid(string op) => All.Contains(op);
    }
}
