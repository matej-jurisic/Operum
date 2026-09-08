using FluentValidation;
using Operum.Model.Constants;

namespace Operum.Model.DTOs.Dashboard.Requests
{
    // Sets a DashboardWidgetTypes.TabsContainer widget's panel title and its full tab list
    // in one call, the way SaveFilterItemDto stands for a whole filter widget.
    //
    // Tabs is the ordered set the container should end up with. A tab whose Id matches one
    // the container already has is kept (rename in place); a tab with no Id, or an unknown
    // one, is created. A tab the container had that is absent here is removed, and its
    // children are repointed to the first tab that survives -- see DashboardService.
    // SaveTabsContainer. There must be between one and DataLimits.MaxDashboardTabCount tabs.
    public class SaveTabsContainerDto
    {
        public string? Title { get; set; }
        public List<SaveTabDto> Tabs { get; set; } = [];
    }

    public class SaveTabDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SaveTabsContainerDtoValidator : AbstractValidator<SaveTabsContainerDto>
    {
        public SaveTabsContainerDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(DataLimits.MaxHeaderTextLength)
                .WithMessage($"Title cannot exceed {DataLimits.MaxHeaderTextLength} characters.");

            RuleFor(x => x.Tabs)
                .Must(t => t.Count is >= 1 and <= DataLimits.MaxDashboardTabCount)
                .WithMessage($"A tabs container must have between 1 and {DataLimits.MaxDashboardTabCount} tabs.");

            RuleForEach(x => x.Tabs).ChildRules(tab =>
            {
                tab.RuleFor(t => t.Name)
                    .NotEmpty().WithMessage(x => Messages.Required("tab name"))
                    .MaximumLength(DataLimits.MaxTabNameLength)
                    .WithMessage($"A tab name cannot exceed {DataLimits.MaxTabNameLength} characters.");
            });
        }
    }
}
