namespace Operum.Model.Constants
{
    // What a dashboard item renders. An analytic chart is the original kind; the
    // discriminator exists so a widget that isn't a chart can share the same grid, the
    // same placement columns and the same endpoints instead of needing a table of its own.
    public static class DashboardWidgetTypes
    {
        public const string Analytic = "analytic";

        // A button that opens a tracker's quick-add entry dialog from the board. Carries no
        // analytic definition — just Config, a QuickAddWidgetConfigDto naming the tracker.
        public const string QuickAdd = "quickAdd";

        // A read-only table of one tracker's entries. Carries no analytic definition either
        // — just Config, an EntriesWidgetConfigDto naming the fields it shows as columns. How
        // it's filtered comes only from the filter widgets it's linked to.
        public const string Entries = "entries";

        // A board filter widget with two independent facets, both narrowing whichever
        // Analytic/Entries widgets it's linked to. First, it owns a set of filter clauses
        // with a value typed directly on the board (its own QueryIds/ValueByQuery/Links).
        // Second, it can offer a dropdown of the board's DashboardViews as quick-apply
        // presets (PresetIds/SelectedPresetId/PresetLinks) — picking one applies that view's
        // whole clause set (filters AND sorts) to its followers, same as the old standalone
        // "view selector" widget did before being folded in here. Carries no analytic
        // definition — just Config, a FilterWidgetConfigDto. A typed clause left blank is
        // simply not applied; a preset left unselected contributes nothing.
        public const string Filter = "filter";

        // A short line of user-entered text that reads as a section title rather than a
        // chart. Carries no tracker or analytic — just Config, a TextWidgetConfigDto.
        public const string Header = "header";

        // A bare visual rule with no config at all. The grid already lets widgets leave
        // deliberate empty space; this is what turns a gap into something that reads as a
        // dividing line instead of unfinished layout.
        public const string Divider = "divider";

        // A free-form block of user-entered text, for context that isn't any tracker's
        // data. Shares its Config shape (TextWidgetConfigDto) with Header.
        public const string Note = "note";

        // A panel that holds a sub-grid of other widgets, so a group of them can be moved,
        // resized and titled as one. Carries no tracker or analytic. Its Config is optional:
        // a TextWidgetConfigDto holding the title once one is set, otherwise null (the card
        // falls back to a default label). The rest of its state is which items name it as
        // their parent (DashboardItem.ParentItemId) and their placement within it. A
        // Container can never sit inside another Container.
        public const string Container = "container";

        // A Container whose body is split into named tabs: every child names both this item
        // (DashboardItem.ParentItemId) and one of its tabs (DashboardItem.ParentTabId), and
        // only the active tab's children render when the board is read. The tab set and the
        // optional panel title live in Config as a TabsContainerConfigDto. Like a plain
        // Container it can never sit inside another container of either kind, and a phone
        // flattens it away the same way.
        public const string TabsContainer = "tabsContainer";

        public static readonly HashSet<string> All =
            [Analytic, QuickAdd, Entries, Filter, Header, Divider, Note, Container, TabsContainer];

        public static bool IsValid(string type) => All.Contains(type);

        // Whether a widget of this type is a panel that holds a sub-grid of other widgets:
        // the plain Container and the TabsContainer both are. Used wherever the "can't be
        // nested, and its children reparent when it's removed" rules apply to either.
        public static bool IsContainer(string type) => type is Container or TabsContainer;
    }
}
