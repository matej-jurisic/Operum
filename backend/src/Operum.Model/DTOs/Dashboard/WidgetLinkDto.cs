namespace Operum.Model.DTOs.Dashboard
{
    // Names one Analytic/Entries widget a filter widget narrows: the followed widget, the
    // tracker on it this link reads from, and which of that tracker's fields each clause
    // runs against.
    //
    // FieldByQuery is keyed by clause: on the wire (SaveFilterItemDto) by the clause's index
    // in Clauses -- the client has no stable id until the save resolves one -- and in the
    // stored FilterWidgetConfigDto by the clause's SlotId, which DashboardService rewrites
    // those indices to on save.
    public class WidgetLinkDto
    {
        public string ItemId { get; set; } = string.Empty;
        public string TrackerId { get; set; } = string.Empty;
        public Dictionary<string, string> FieldByQuery { get; set; } = [];
    }
}
