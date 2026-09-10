using Operum.Model.Common;
using Operum.Model.Constants.Analytics;
using Operum.Model.Constants.Analytics.Definitions;
using Operum.Model.Constants.Fields;
using Operum.Model.DTOs.Analytics;
using Operum.Model.DTOs.Fields;
using Operum.Model.Enums;
using Operum.Model.Extensions;
using Operum.Model.Models;
using Operum.Service.Domain.Analytics.Calculators;

namespace Operum.Service.Domain.Analytics.Builders
{
    public class SingleValueAnalyticBuilder : AnalyticResultBuilderBase
    {
        private readonly Dictionary<string, ISingleValueCalculator> _calculators;

        public override string SupportedType => AnalyticTypes.SingleValue;

        public SingleValueAnalyticBuilder()
        {
            _calculators = new Dictionary<string, ISingleValueCalculator>
            {
                [AnalyticCodes.Count] = new CountCalculator(),
                [AnalyticCodes.TrueCount] = new TrueCountCalculator(),
                [AnalyticCodes.FalseCount] = new FalseCountCalculator(),
                [AnalyticCodes.TruePercentage] = new TruePercentageCalculator(),
                [AnalyticCodes.Min] = new MinCalculator(),
                [AnalyticCodes.Max] = new MaxCalculator(),
                [AnalyticCodes.Average] = new AverageCalculator(),
                [AnalyticCodes.Sum] = new SumCalculator(),
                [AnalyticCodes.StdDev] = new StdDevCalculator(),
                [AnalyticCodes.CountDistinct] = new CountDistinctCalculator(),
                [AnalyticCodes.MostCommon] = new MostCommonCalculator(),
                [AnalyticCodes.LeastCommon] = new LeastCommonCalculator()
            };
        }

        protected override Result<AnalyticDto> BuildResult(AnalyticResultBuilderRequest request)
        {
            var result = new SingleValueAnalyticDto
            {
                Name = AnalyticDefinitionList.GetLabel(SupportedType, request.Analytic.Code),
                Description = request.Analytic.Description,
                Id = request.Analytic.Id
            };

            var valueField = request.FieldMap.GetValueOrDefault(AnalyticPurposes.Value);
            if (valueField == null)
                return Result.Success<AnalyticDto>(result);

            var allValues = request.Entries
                .SelectMany(e => e.FieldValues.Where(fv => fv.FieldId == valueField.Id))
                .ToList();

            if (!_calculators.TryGetValue(request.Analytic.Code, out var calculator))
                return Result.Failure(ResultStatusCodes.BadRequest,
                    $"Unsupported analytic code: {request.Analytic.Code}");

            var opResult = calculator.Calculate(allValues);

            if (!opResult.IsSuccess)
                return Result.Failure(opResult.StatusCode, opResult.Messages);

            result.Value = opResult.Data.Value;
            result.EntryId = opResult.Data.EntryId;

            // Min/Max pick an entry, so they can show a different field than the one they
            // compared: the value stays what the winning entry holds in the Display field.
            var displayField = request.FieldMap.GetValueOrDefault(AnalyticPurposes.Display);
            if (displayField != null && result.EntryId != null)
            {
                var displayValue = request.Entries
                    .FirstOrDefault(e => e.Id == result.EntryId)?.FieldValues
                    .FirstOrDefault(fv => fv.FieldId == displayField.Id)
                    ?.GetValueAsString();

                // Keep the compared value around so the card can show it under the label.
                result.SecondaryValue = result.Value;
                result.SecondaryValueField = MapField(valueField, valueField.Type);

                result.Value = displayValue ?? string.Empty;
                result.ValueField = MapField(displayField, displayField.Type);

                return Result.Success<AnalyticDto>(result);
            }

            // CountDistinct returns a plain integer count — override type so the
            // frontend doesn't try to format it as the original field type (e.g. timespan).
            var syntheticNumberCodes = new HashSet<string>
            {
                AnalyticCodes.Count,
                AnalyticCodes.CountDistinct,
                AnalyticCodes.TrueCount,
                AnalyticCodes.FalseCount,
                AnalyticCodes.TruePercentage,
            };
            var displayType = syntheticNumberCodes.Contains(request.Analytic.Code)
                ? DataTypes.Number
                : valueField.Type;

            result.ValueField = MapField(valueField, displayType);

            return Result.Success<AnalyticDto>(result);
        }

        // The field the frontend formats the value with. Type is passed in because it isn't
        // always the field's own: a count of anything reads as a plain number.
        private static FieldDto MapField(Field field, string type) => new()
        {
            Description = field.Description,
            Id = field.Id,
            Name = field.Name,
            Required = field.Required,
            Type = type,
        };
    }
}
