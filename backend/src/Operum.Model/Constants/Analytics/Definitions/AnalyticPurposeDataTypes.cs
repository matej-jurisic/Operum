namespace Operum.Model.Constants.Analytics.Definitions
{
    public class AnalyticPurposeDataTypes
    {
        public string Label { get; init; } = string.Empty;
        public Dictionary<string, HashSet<string>> AllowedDataTypes { get; init; } = [];

        // Purposes a caller may leave unmapped (Min/Max's Display field). Every other key
        // of AllowedDataTypes has to be supplied.
        public HashSet<string> OptionalPurposes { get; init; } = [];
    }
}
