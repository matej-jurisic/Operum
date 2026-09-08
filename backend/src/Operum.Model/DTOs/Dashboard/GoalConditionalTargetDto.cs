namespace Operum.Model.DTOs.Dashboard
{
    // One row of a goal placement's conditional-target list (DashboardItem.GoalConditionalTargets).
    // Serialized camelCase like every other hand-serialized dashboard Config.
    //
    // Conditions maps a followed filter clause (its pooled query id) to the value that clause
    // must currently be set to on the board for this row to apply. A clause the row does not
    // mention is a wildcard. A row with no conditions never applies. Rows are evaluated in
    // order; the first whose every condition matches a *connected* clause's current value
    // wins, and its Target replaces the widget's default. A condition naming a clause this
    // placement no longer follows makes the whole row inert.
    public class GoalConditionalTargetDto
    {
        public Dictionary<string, string> Conditions { get; set; } = [];
        public string Target { get; set; } = string.Empty;
    }
}
