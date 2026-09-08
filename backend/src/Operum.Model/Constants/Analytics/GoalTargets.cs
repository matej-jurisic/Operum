using System.Globalization;
using Operum.Model.Constants.Fields;

namespace Operum.Model.Constants.Analytics
{
    // Shared rules for a goal widget's target values -- used both when a Widget's default
    // target is set (WidgetsService) and when a placement's conditional targets are set
    // (DashboardService). A target is stored as its raw string and read back as a magnitude
    // by GoalAnalyticBuilder.
    public static class GoalTargets
    {
        // Sum/Average/Min/Max over a duration field produce a duration, so their target must
        // parse as one. Every other goal calculation (the counts included) produces a plain
        // number.
        private static readonly HashSet<string> DurationCarryingCodes =
            [AnalyticCodes.Sum, AnalyticCodes.Average, AnalyticCodes.Min, AnalyticCodes.Max];

        // Something downstream has to be able to read it as a magnitude: a number or an
        // hh:mm:ss duration.
        public static bool IsParseable(string? target) =>
            !string.IsNullOrWhiteSpace(target) &&
            (double.TryParse(target, NumberStyles.Any, CultureInfo.InvariantCulture, out _) ||
             TimeSpan.TryParse(target, CultureInfo.InvariantCulture, out _));

        // The target has to be the same kind of magnitude the calculation produces.
        public static bool MatchesFieldType(string code, string? valueFieldType, string target) =>
            valueFieldType == DataTypes.TimeSpan && DurationCarryingCodes.Contains(code)
                ? TimeSpan.TryParse(target, CultureInfo.InvariantCulture, out _)
                : double.TryParse(target, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }
}
