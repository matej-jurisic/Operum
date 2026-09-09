using Operum.Model.DTOs.Analytics;

namespace Operum.Service.Domain.Analytics.Processors
{
    // Raw values (the None grouping): one bar per entry, no bucketing and no aggregation.
    // The builder has already mapped and filtered the points, so this is a pass-through,
    // mirroring LineChartProcessor. Grouped bars stay on GroupedBarChartProcessor.
    public class BarChartProcessor : IBarChartProcessor
    {
        public List<DonutChartPointDto> Process(List<DonutChartPointDto> dataPoints)
        {
            return dataPoints;
        }
    }
}
