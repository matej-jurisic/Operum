namespace Operum.Model.DTOs.Dashboard
{
    // The Config payload for a DashboardWidgetTypes.Filter item. Serialized camelCase
    // like every other hand-serialized dashboard Config, since this one is written by hand
    // rather than through the controller's JSON formatting.
    //
    // A filter widget owns an ordered set of filter clauses whose values are typed on the
    // board, narrowing every followed widget in Links:
    //
    // Slots is the widget's own ordered clause set. Each slot has a stable, widget-local
    // SlotId and the pooled Query it currently resolves to (QueryId, see QueryPool). Two
    // slots may resolve to the same QueryId -- two "date on or before" clauses mapped to
    // different fields -- so the slot, not the pooled query, is what a value and a follower's
    // field mapping key off. SlotIds survive an edit that leaves a clause's shape unchanged.
    //
    // ValueBySlot is the persisted current value per clause, keyed by SlotId -- changing it
    // (see SetFilterValues) re-filters every follower for every future load. A slot with no
    // entry here (or an empty one) is left unapplied. Links carries, per followed
    // Analytic/Entries widget and per tracker it reads from, which field each slot runs
    // against (WidgetLinkDto.FieldByQuery, keyed by SlotId in the stored config).
    //
    // PresetIds names the board's DashboardViews this widget offers as presets. A preset is
    // a named set of values whose clause shape (data type + operator, in order) matches this
    // widget's clauses exactly; picking one on the board just writes ValueBySlot, so a
    // preset needs no separate resolution or follower list of its own.
    //
    // Pre-slot configs stored a parallel QueryIds list with ValueByQuery / FieldByQuery
    // keyed by pooled query id. DashboardService.TryParseFilterConfig folds those into slots
    // on read (a clause whose shape is unique keeps its query id as its slot id, so an
    // existing goal conditional target keyed off it still resolves); the next save rewrites
    // the config into this shape.
    public class FilterWidgetConfigDto
    {
        public List<FilterClauseSlotDto> Slots { get; set; } = [];
        public Dictionary<string, string?> ValueBySlot { get; set; } = [];
        public List<WidgetLinkDto> Links { get; set; } = [];

        public List<string> PresetIds { get; set; } = [];
    }

    // One clause of a filter widget: a stable widget-local id and the pooled Query it
    // currently resolves to.
    public class FilterClauseSlotDto
    {
        public string SlotId { get; set; } = string.Empty;
        public string QueryId { get; set; } = string.Empty;
    }
}
