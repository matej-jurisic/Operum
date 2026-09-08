namespace Operum.Model.DTOs.Dashboard
{
    // The Config payload for a DashboardWidgetTypes.TabsContainer widget: the panel's
    // optional title and its ordered set of tabs. Serialized camelCase like every other
    // hand-serialized dashboard Config, since this one is written by hand rather than
    // through the controller's JSON formatting.
    //
    // Each tab carries an opaque id its children reference through DashboardItem.ParentTabId;
    // the list order is the tab order. There is always at least one tab.
    public class TabsContainerConfigDto
    {
        public string? Title { get; set; }
        public List<TabDefDto> Tabs { get; set; } = [];
    }

    public class TabDefDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
