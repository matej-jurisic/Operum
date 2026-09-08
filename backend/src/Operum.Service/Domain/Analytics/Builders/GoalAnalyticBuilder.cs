using System.Globalization;
using Operum.Model.Common;
using Operum.Model.Constants.Analytics;
using Operum.Model.Constants.Analytics.Definitions;
using Operum.Model.Constants.Fields;
using Operum.Model.DTOs.Analytics;
using Operum.Model.Enums;
using Operum.Model.Models;

namespace Operum.Service.Domain.Analytics.Builders
{
    // A Goal is a Single Value calculation drawn as progress toward a target. It delegates
    // the calculation itself to SingleValueAnalyticBuilder -- same codes, same field, same
    // result -- then attaches the target (carried on the transient Analytic, sourced from
    // Widget.GoalTarget) and the ratio between them.
    public class GoalAnalyticBuilder : AnalyticResultBuilderBase
    {
        private readonly SingleValueAnalyticBuilder _singleValue = new();

        public override string SupportedType => AnalyticTypes.Goal;

        protected override Result<AnalyticDto> BuildResult(AnalyticResultBuilderRequest request)
        {
            var inner = _singleValue.Build(new AnalyticResultBuilderRequest
            {
                Analytic = new Analytic
                {
                    Id = request.Analytic.Id,
                    Name = request.Analytic.Name,
                    Description = request.Analytic.Description,
                    Code = request.Analytic.Code,
                    ResultType = AnalyticTypes.SingleValue
                },
                Entries = request.Entries,
                FieldMap = request.FieldMap
            });

            if (!inner.IsSuccess)
                return inner;

            if (inner.Data is not SingleValueAnalyticDto single)
                return Result.Failure(ResultStatusCodes.BadRequest, "Goal calculation did not produce a single value.");

            var target = request.Analytic.GoalTarget;

            return Result.Success<AnalyticDto>(new GoalAnalyticDto
            {
                Id = request.Analytic.Id,
                Name = AnalyticDefinitionList.GetLabel(SupportedType, request.Analytic.Code),
                Description = request.Analytic.Description,
                Value = single.Value,
                Target = target ?? string.Empty,
                ValueField = single.ValueField,
                Progress = ComputeProgress(single.ValueField?.Type, single.Value, target)
            });
        }

        // The value / target ratio, or null when there's nothing to show: no calculated
        // value, or a target that isn't a positive magnitude.
        private static double? ComputeProgress(string? type, string? value, string? target)
        {
            if (!TryParseMagnitude(type, value, out var current) ||
                !TryParseMagnitude(type, target, out var goal) ||
                goal <= 0)
                return null;

            return current / goal;
        }

        private static bool TryParseMagnitude(string? type, string? raw, out double magnitude)
        {
            magnitude = 0;
            if (string.IsNullOrWhiteSpace(raw))
                return false;

            if (type == DataTypes.TimeSpan)
            {
                if (TimeSpan.TryParse(raw, CultureInfo.InvariantCulture, out var ts))
                {
                    magnitude = ts.TotalSeconds;
                    return true;
                }
                return false;
            }

            return double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out magnitude);
        }
    }
}
