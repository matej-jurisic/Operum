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
    }
}
