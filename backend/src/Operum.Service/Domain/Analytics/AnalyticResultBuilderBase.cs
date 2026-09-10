using Operum.Model.Common;
using Operum.Model.Constants.Analytics.Definitions;
using Operum.Model.DTOs.Analytics;
using Operum.Model.Enums;

namespace Operum.Service.Domain.Analytics
{
    public abstract class AnalyticResultBuilderBase : IAnalyticResultBuilder
    {
        public abstract string SupportedType { get; }

        public Result<AnalyticDto> Build(AnalyticResultBuilderRequest request)
        {
            var validationResult = ValidateRequest(request);
            if (!validationResult.IsSuccess)
                return Result.Success((AnalyticDto)new SingleValueAnalyticDto()
                {
                    Value = validationResult.Messages.FirstOrDefault() ?? "Error"
                });

            return BuildResult(request);
        }

        protected virtual Result ValidateRequest(AnalyticResultBuilderRequest request)
        {
            var resultType = request.Analytic.ResultType;
            var code = request.Analytic.Code;
            var grouping = request.Analytic.Grouping;

            if (!AnalyticDefinitionList.IsValidForType(resultType, code, grouping))
                return Result.Failure(ResultStatusCodes.BadRequest,
                    $"Calculation '{grouping}/{code}' not allowed for {resultType}");

            var allowed = AnalyticDefinitionList.GetAllowedPurposes(resultType, code, grouping).ToHashSet();

            foreach (var (purpose, field) in request.FieldMap)
            {
                if (!allowed.Contains(purpose))
                    return Result.Failure(ResultStatusCodes.BadRequest,
                        $"Purpose '{purpose}' is not part of this calculation for {resultType}");

                if (!AnalyticDefinitionList.IsValidDataType(resultType, code, purpose, field.Type, grouping))
                    return Result.Failure(ResultStatusCodes.BadRequest,
                        $"Field '{field.Name}' of type '{field.Type}' is not allowed for purpose '{purpose}'");
            }

            return Result.Success();
        }

        protected abstract Result<AnalyticDto> BuildResult(AnalyticResultBuilderRequest request);
    }
}
