using Operum.Model.Constants.Analytics;
using Operum.Model.DTOs.Analytics;

namespace Operum.Service.Domain.Analytics.Processors
{
    // Buckets a line chart's points by the chosen grouping (exact X value, or a calendar
    // period) and reduces each bucket to one point with the chosen aggregation. Replaces
    // the old per-combination processors (AggregatedSum / Cumulative / Daily / Weekly /
    // ...). Raw values (no grouping) stay on LineChartProcessor.
    public class GroupedLineChartProcessor(string grouping, string aggregation) : ILineChartProcessor
    {
        public List<LineChartPointDto> Process(List<LineChartPointDto> dataPoints)
        {
            var isDateBucket = AnalyticGroupings.DateBuckets.Contains(grouping);

            var keyed = dataPoints
                .Select((p, i) =>
                {
                    if (isDateBucket)
                    {
                        var dk = ChartBuckets.DateKey(p.X, grouping);
                        return dk == null ? null : new Keyed(dk.Value.Key, dk.Value.Instant.Ticks, p);
                    }

                    return p.X == null ? null : new Keyed(p.X, i, p);
                })
                .OfType<Keyed>()
                .ToList();

            // Points arrive already ordered along the x-axis (LineChartAnalyticBuilder), so
            // ordering the groups by their first member keeps that order for both the exact
            // and the date-bucket cases without a type-aware re-sort here.
            var groups = keyed
                .GroupBy(k => k.Key)
                .OrderBy(g => g.Min(k => k.Sort))
                .ToList();

            if (aggregation == AnalyticCodes.CumulativeSum)
            {
                var running = 0.0;
                var points = new List<LineChartPointDto>();
                foreach (var g in groups)
                {
                    running += g.Sum(k => k.Point.Y ?? 0);
                    points.Add(new LineChartPointDto { X = g.Key, Y = Math.Round(running, 2) });
                }
                return points;
            }

            return [.. groups.Select(g => new LineChartPointDto
            {
                X = g.Key,
                Y = ChartBuckets.Reduce(aggregation, [.. g.Select(k => k.Point.Y ?? 0)])
            })];
        }

        private sealed record Keyed(string Key, long Sort, LineChartPointDto Point);
    }
}
